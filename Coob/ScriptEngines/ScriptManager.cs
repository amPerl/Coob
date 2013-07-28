using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coob
{
    class ScriptManager
    {
        private Dictionary<string, List<object>> hooks;
        public List<IScriptHandler> ScriptHandlers;

        public ScriptManager()
        {
            hooks = new Dictionary<string, List<object>>();
            ScriptHandlers = new List<IScriptHandler>();
        }

        public void Initialize()
        {
            foreach (var scriptHandler in ScriptHandlers)
            {
                scriptHandler.Initialize();

                scriptHandler.SetFunction("LogInfo",    (Action<object>)Log.Info);
                scriptHandler.SetFunction("LogWarning", (Action<object>)Log.Warning);
                scriptHandler.SetFunction("LogError",   (Action<object>)Log.Error);

                // Can't set functions within objects, so it'll have to be in the global namespace.
                scriptHandler.SetFunction("AddHook", (Action<string, object>)((eventName, function) =>
                {
                    AddHook(eventName, function);
                }));

                scriptHandler.SetFunction("RemoveHook", (Action<string, object>)((eventName, function) =>
                {
                    RemoveHook(eventName, function);
                }));

                string pluginDirectory = Path.Combine(@"Plugins", scriptHandler.GetScriptDirectoryName());
                if (Directory.Exists(pluginDirectory))
                {
                    foreach (string directory in Directory.GetDirectories(pluginDirectory))
                    {
                        string pluginName = directory.Substring(directory.LastIndexOf('/') + 1);

                        string entryPath = Path.Combine(directory, scriptHandler.GetEntryFileName() + scriptHandler.GetScriptExtension());
                        if (File.Exists(entryPath))
                        {
                            scriptHandler.LoadPlugin(pluginName, entryPath);
                        }
                    }
                }
            }
        }

        public void Run()
        {
            foreach (var scriptHandler in ScriptHandlers)
            {
                scriptHandler.Run();
            }
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
            if (!hooks.ContainsKey(name))
            {
                args.Canceled = false;
                return args;
            }

            foreach (object functionName in hooks[name].ToArray()) // ToArray because if event removes a function from this event, it'll cause an exception. (collection modified)
            {
                ScriptHandlers.ForEach(sh => sh.CallFunction(functionName, args));
            }

            return args;
        }
    }
}
