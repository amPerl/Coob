using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob
{
    class HookHandler
    {
        private Dictionary<string, List<object>> hooks;
        public List<IScriptHandler> ScriptHandlers;

        public HookHandler()
        {
            hooks = new Dictionary<string, List<object>>();
            ScriptHandlers = new List<IScriptHandler>();
        }

        public void Add(string eventName, object function)
        {
            if (!hooks.ContainsKey(eventName))
                hooks[eventName] = new List<object>();

            if (!hooks[eventName].Contains(function))
                hooks[eventName].Add(function);
        }

        public void Delete(string eventName, object functionName)
        {
            if (hooks.ContainsKey(eventName) && hooks[eventName].Contains(functionName))
                hooks[eventName].Remove(functionName);
        }

        /// <summary>Returns true if not cancelled.</summary>
        public ScriptEventArgs Call(string eventName, ScriptEventArgs args)
        {
            if (!hooks.ContainsKey(eventName))
            {
                args.Canceled = false;
                return args;
            }

            foreach (object functionName in hooks[eventName].ToArray()) // ToArray because if event removes a function from this event, it'll cause an exception. (collection modified)
            {
                ScriptHandlers.ForEach(sh => sh.CallFunction(functionName, args));
            }

            return args;
        }
    }
}
