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

        /// <summary>Returns true if not cancelled.</summary>
        public bool Call(string eventName, params object[] args)
        {
            if (!hooks.ContainsKey(eventName)) return true;

            var eventArgs = new ScriptEventArgs();
            var list = new List<object>(args);
            list.Insert(0, eventArgs);
            args = list.ToArray();

            foreach (var functionName in hooks[eventName])
            {
                ScriptHandlers.ForEach(sh => sh.CallFunction(functionName, args));
            }

            return !eventArgs.Canceled;
        }
    }

    public class ScriptEventArgs : EventArgs
    {
        public bool Canceled = false;
    }
}
