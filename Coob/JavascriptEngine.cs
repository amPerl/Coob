using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;
using Jint.Native;
using System.IO;
using System.Security.Permissions;

namespace Coob
{
    public class JavascriptEngine : IScriptHandler
    {
        private JintEngine _Engine;
        private string _Source;

        public JavascriptEngine()
        {
            _Engine = new JintEngine();
        }

        public void Initialize()
        {
            _Engine.AddPermission(new UIPermission(PermissionState.Unrestricted));
            _Engine.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
        }

        public void Load(string SourceFile)
        {
            _Source = File.ReadAllText(SourceFile);
        }

        public void SetParameter(string name, object value)
        {
            _Engine.SetParameter(name, value);
        }

        public void SetFunction(string name, Delegate function)
        {
            _Engine.SetFunction(name, function);
        }

        public T CallFunction<T>(string functionName, params object[] arguments)
        {
            return (T)_Engine.CallFunction(functionName, arguments);
        }

        public void CallMethod(string functionName, params object[] arguments)
        {
            _Engine.CallFunction(functionName, arguments);
        }

        public void Run()
        {
            try
            {
                _Engine.Run(_Source);
            }
            catch (JintException ex)
            {
                Log.Error(ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
