﻿using System;
using System.Collections.Generic;
using System.IO;
using Coob.CoobEventArgs;
using Jint.Native;

namespace Coob.ScriptEngines
{
    public class ScriptManager
    {
        private readonly Dictionary<string, List<object>> hooks;
        public List<IScriptHandler> ScriptHandlers;

        private readonly Dictionary<string, object> globals;

        public ScriptManager()
        {
            hooks = new Dictionary<string, List<object>>();
            ScriptHandlers = new List<IScriptHandler>();
            globals = new Dictionary<string, object>();
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

                string pluginDirectory = Path.Combine(@"Plugins", scriptHandler.GetScriptDirectoryName());
                if (!Directory.Exists(pluginDirectory))
                    continue;

                foreach (string directory in Directory.GetDirectories(pluginDirectory))
                {
                    string pluginName = directory.Replace('\\', '/');
                    pluginName = pluginName.Substring(pluginName.LastIndexOf('/') + 1);

                    string entryPath = Path.Combine(directory, scriptHandler.GetEntryFileName() + scriptHandler.GetScriptExtension());
                    if (File.Exists(entryPath))
                        scriptHandler.LoadPlugin(pluginName, entryPath);
                }
            }
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
            if (!hooks.ContainsKey(name))
            {
                args.Canceled = false;
                return args;
            }

            // ToArray because if event removes a function from this event, it'll cause an exception. (collection modified)
            foreach (object functionName in hooks[name].ToArray())
                ScriptHandlers.ForEach(sh => sh.CallFunction(functionName, args));

            return args;
        }

        public T CallEvent<T>(string name, ScriptEventArgs args) where T : ScriptEventArgs
        {
            return (T)CallEvent(name, args);
        }
    }
}
