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
        private JintEngine engine;
        private string source;

        public JavascriptEngine()
        {
            engine = new JintEngine();
        }

        public void Initialize()
        {
            engine.AddPermission(new UIPermission(PermissionState.Unrestricted));
            engine.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
        }

        public void Load(string SourceFile)
        {
            source = File.ReadAllText(SourceFile);
        }

        public void SetParameter(string name, object value)
        {
            engine.SetParameter(name, value);
        }

        public void SetFunction(string name, Delegate function)
        {
            engine.SetFunction(name, function);
        }

        public T CallFunction<T>(string functionName, params object[] arguments)
        {
            return (T)engine.CallFunction(functionName, arguments);
        }

        public void CallMethod(string functionName, params object[] arguments)
        {
            engine.CallFunction(functionName, arguments);
        }

        public void Run()
        {
            try
            {
                engine.Run(source);
            }
            catch (JintException ex)
            {
                Log.Error(ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
