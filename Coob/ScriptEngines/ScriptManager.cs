using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Coob.CoobEventArgs;
using Jint.Native;

namespace Coob.ScriptEngines
{
    public class ScriptManager
    {
        private readonly Dictionary<string, List<object>> hooks;
        public List<IScriptHandler> ScriptHandlers;

        private readonly Dictionary<string, object> globals;

        private readonly List<string> loadedPlugins; 

        public ScriptManager()
        {
            hooks = new Dictionary<string, List<object>>();
            ScriptHandlers = new List<IScriptHandler>();
            globals = new Dictionary<string, object>();
            loadedPlugins = new List<string>();
        }

        public void Initialize()
        {
            foreach (var scriptHandler in ScriptHandlers)
            {
                scriptHandler.Initialize();

                scriptHandler.SetFunction("LogInfo", (Action<object>)Log.Info);
                scriptHandler.SetFunction("LogWarning", (Action<object>)Log.Warning);
                scriptHandler.SetFunction("LogError", (Action<object>)Log.Error);

                // Can't set functions within objects, so it'll have to be in the global namespace.
                scriptHandler.SetFunction("AddHook", (Action<string, object>)AddHook);
                scriptHandler.SetFunction("RemoveHook", (Action<string, object>)RemoveHook);

                scriptHandler.SetFunction("SetGlobal", (Action<string, object>)SetGlobal);
                scriptHandler.SetFunction("GetGlobal", (Func<string, object>)GetGlobal);

                string pluginDirectory = Path.Combine("Plugins", scriptHandler.GetScriptDirectoryName());
                if (!Directory.Exists(pluginDirectory))
                    continue;

                foreach (string directory in Directory.GetDirectories(pluginDirectory))
                {
                    LoadPlugin(directory.Replace('\\', '/'), scriptHandler, pluginDirectory);
                }
            }
        }

        private void LoadPlugin(string directory, IScriptHandler scriptHandler, string pluginDirectory)
        {
            string pluginName = directory.Substring(directory.LastIndexOf('/') + 1);

            if (!VerifyPlugin(pluginName))
                return;

            string entryPath = Path.Combine(directory, scriptHandler.GetEntryFileName() + scriptHandler.GetScriptExtension());
            if (File.Exists(entryPath))
            {
                var lines = GetDependencies(directory);

                foreach(var line in lines)
                {
                    if (!loadedPlugins.Contains(line))
                    {
                        var dependencies = GetDependencies("Plugins/" + scriptHandler.GetScriptDirectoryName() + "/" + line);
                        if (dependencies.Contains(pluginName))
                        {
                            Log.Error("Plugin \"" + pluginName + "\" has a dependency to itself!");
                            return;
                        }

                        if (dependencies.Contains(pluginName))
                        {
                            Log.Error("Circular dependency detected between \"" + pluginName + "\" and \"" + line + "\"!");
                            return;
                        }

                        Log.Info(pluginName + " requires " + line + ". Loading " + line + ".");
                        LoadPlugin(pluginDirectory + "/" + line, scriptHandler, pluginDirectory);
                    }
                }

                scriptHandler.LoadPlugin(pluginName, entryPath);
                loadedPlugins.Add(pluginName);
            }
            else
            {
                Log.Error("Plugin \"" + pluginName + "\" could not be loaded because it does not exist!");
                return;
            }
        }

        private List<string> GetDependencies(string path)
        {
            var result = new List<string>();
            path = Path.Combine(path, "require.txt").Replace('\\', '/');

            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i].Trim().StartsWith("//"))
                        continue;

                    var line = lines[i].Replace(" ", "");
                    var commentIndex = line.IndexOf("//", StringComparison.InvariantCultureIgnoreCase);
                    line = line.Substring(0, commentIndex > 0 ? commentIndex : lines[i].Length);

                    result.Add(line);
                }
            }

            return result;
        }

        /// <summary>Returns true if the plugin can and should be loaded.</summary>
        private bool VerifyPlugin(string pluginName)
        {
            // Don't load plugin if already loaded.
            if (loadedPlugins.Contains(pluginName))
                return false;

            if (pluginName.Contains(" "))
            {
                Log.Error("Plugin \"" + pluginName + "\" contains spaces!");
                return false;
            }

            if (char.IsDigit(pluginName, 0))
            {
                Log.Error("Plugin \"" + pluginName + "\" starts with a number!");
                return false;
            }

            if (char.IsSymbol(pluginName, 0))
            {
                Log.Error("Plugin \"" + pluginName + "\" starts with a symbol!");
                return false;
            }

            if (char.IsWhiteSpace(pluginName, 0))
            {
                Log.Error("Plugin \"" + pluginName + "\" starts with whitespace!");
                return false;
            }

            return true;
        }

        private void SetGlobal(string key, object val)
        {
            if (!globals.ContainsKey(key))
                globals.Add(key, val);
            else
                globals[key] = val;
        }

        private object GetGlobal(string key)
        {
            if (globals.ContainsKey(key))
                return globals[key];

            return JsUndefined.Instance;
        }

        public void Run()
        {
            foreach (var scriptHandler in ScriptHandlers)
                scriptHandler.Run();
        }

        public void AddHook(string eventName, object function)
        {
            if (!hooks.ContainsKey(eventName))
                hooks[eventName] = new List<object>();

            if (!hooks[eventName].Contains(function))
                hooks[eventName].Add(function);
        }

        public void RemoveHook(string eventName, object functionName)
        {
            if (hooks.ContainsKey(eventName) && hooks[eventName].Contains(functionName))
                hooks[eventName].Remove(functionName);
        }

        public ScriptEventArgs CallEvent(string name, ScriptEventArgs args)
        {
            if (Globals.PrintEvents)
            {
                if (name.ToLower().Contains("update") && Globals.PrintEventUpdates)
                    Log.Custom("Event called: " + name, "ATTN", ConsoleColor.DarkCyan, ConsoleColor.Cyan);
                else if (!name.ToLower().Contains("update"))
                    Log.Custom("Event called: " + name, "ATTN", ConsoleColor.DarkCyan, ConsoleColor.Cyan);
            }

            if (!hooks.ContainsKey(name))
            {
                args.Canceled = false;
                return args;
            }

            // ToArray because if event removes a function from this event, it'll cause an exception. (collection modified)
            foreach (object function in hooks[name].ToArray())
                ScriptHandlers.ForEach(sh => sh.CallFunction(function, args));

            return args;
        }

        public T CallEvent<T>(string name, ScriptEventArgs args) where T : ScriptEventArgs
        {
            return (T)CallEvent(name, args);
        }
    }
}
