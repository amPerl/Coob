using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jint;
using Jint.Native;
using System.IO;
using System.Security.Permissions;
using System.Net;

namespace Coob.ScriptEngines
{
    public class JavascriptEngine : IScriptHandler
    {
        private readonly JintEngine engine;
        private List<PluginSource> sources = new List<PluginSource>();

        struct PluginSource
        {
            public string Source;
            public string Path;

            public PluginSource(string source, string path) { Source = source; Path = path; }
        }

        public JavascriptEngine()
        {
            engine = new JintEngine(Options.Ecmascript5 | Options.Strict);

            engine.SetFunction("include", (Func<string, object>)Include);
        }

        public void Initialize()
        {
            engine.AddPermission(new UIPermission(PermissionState.Unrestricted));
            engine.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            engine.AddPermission(new WebPermission(PermissionState.Unrestricted));
        }

        public void LoadPlugin(string pluginName, string sourceFile)
        {
            Log.Info("Loading Javascript plugin \"{0}\"", pluginName);

            string pluginSource = File.ReadAllText(sourceFile).Replace("\r\n", "\n");
            //pluginSource = PreprocessIncludes(pluginSource, Path.GetDirectoryName(sourceFile));

            sources.Add(new PluginSource(pluginName + " = new function() {" + pluginSource + "\n}\n", Path.GetFullPath(sourceFile)));
        }

        public object Include(string path)
        {
            var code = File.ReadAllText(path);
            return RunString("return new function() {\n" + code + "\n};");
        }

        //public string PreprocessIncludes(string pluginSource, string directory)
        //{
        //    var lines = pluginSource.Split('\n');
        //
        //    foreach (var line in lines)
        //    {
        //        string trimmed = line.Trim();
        //
        //        if (!trimmed.StartsWith("#include \""))
        //            continue;
        //
        //        if (trimmed.Length < 11)
        //            continue;
        //
        //        string includeName = trimmed.Substring(10, trimmed.Length - 10 - 1);
        //
        //        string includePath = Path.Combine(directory, includeName);
        //        if (File.Exists(includePath))
        //            pluginSource = pluginSource.Replace(trimmed, PreprocessIncludes(File.ReadAllText(includePath), directory));
        //        else
        //        {
        //            Log.Error("Could not find file to include \"" + trimmed + "\" in plugin " + directory + ".");
        //            Log.Display();
        //            Environment.Exit(1);
        //        }
        //    }
        //
        //    return pluginSource;
        //}

        public void SetParameter(string name, object value)
        {
            engine.SetParameter(name, value);
        }

        public void SetFunction(string name, Delegate function)
        {
            engine.SetFunction(name, function);
        }

        public object RunString(string code)
        {
            try
            {
                return engine.Run(code);
            }
            catch (JintException ex)
            {
                Log.Error(ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                Log.Display();
                //Environment.Exit(1);
                return null;
            }
        }

        public T CallFunction<T>(object function, params object[] arguments)
        {
            try
            {
                return function is JsFunction
                           ? (T)engine.CallFunction((JsFunction)function, arguments)
                           : (T)engine.CallFunction((string)function, arguments);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            return default(T);
        }

        public void CallFunction(object function, params object[] arguments)
        {
            try
            {
                if (function is JsFunction)
                    engine.CallFunction((JsFunction)function, arguments);
                else
                    engine.CallFunction((string)function, arguments);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        public string GetScriptDirectoryName()
        {
            return "Javascript";
        }

        public string GetScriptExtension()
        {
            return ".js";
        }

        public string GetEntryFileName()
        {
            return "main";
        }

        private void WriteException(Exception exception)
        {
            var jsException = exception as JsException;

            if (jsException != null)
                Log.Error("Script error: " + jsException.Message + "\n(" + jsException.Value + ")");
            else
                Log.Error("Script error: " + exception.Message + (exception.InnerException != null ? ("(" + exception.InnerException.Message + ")") : ""));
        }

        public void Run()
        {
            if (sources.Count == 0)
            {
                Log.Error("No plugins found.");
                return;
            }

            sources.ForEach(s => RunString(s.Source));
        }
    }
}
