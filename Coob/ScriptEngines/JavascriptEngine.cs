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

        public void Load(string sourceFile)
        {
            source = File.ReadAllText(sourceFile);
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
            try
            {
                return (T)engine.CallFunction(functionName, arguments);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            return default(T);
        }

        public void CallFunction(string functionName, params object[] arguments)
        {
            try
            {
                engine.CallFunction(functionName, arguments);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        private void WriteException(Exception exception)
        {
            Log.Error(exception);
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
