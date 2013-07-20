using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob
{
    class HookHandler
    {
        private Dictionary<string, List<string>> hooks;
        public List<IScriptHandler> ScriptHandlers;

        public HookHandler()
        {
            hooks = new Dictionary<string, List<string>>();
            ScriptHandlers = new List<IScriptHandler>();
        }

        public void Add(string eventName, string functionName)
        {
            if (!hooks.ContainsKey(eventName))
                hooks[eventName] = new List<string>();

            if (!hooks[eventName].Contains(functionName))
                hooks[eventName].Add(functionName);
        }

        public void Delete(string eventName, string functionName)
        {
            if (hooks.ContainsKey(eventName) && hooks[eventName].Contains(functionName))
                hooks[eventName].Remove(functionName);
        }

        public void Call(string eventName, params object[] args)
        {
            if (!hooks.ContainsKey(eventName)) return;

            foreach (var functionName in hooks[eventName])
            {
                ScriptHandlers.ForEach(sh => sh.CallMethod(functionName, args));
            }
        }
    }
}
