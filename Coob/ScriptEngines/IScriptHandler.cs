using System;

namespace Coob.ScriptEngines
{
    public interface IScriptHandler
    {
        void SetFunction(string name, Delegate function);
        void SetParameter(string name, object value);
        void Initialize();
        void LoadPlugin(string pluginName, string entryFile);
        void Run();
        object RunString(string code);
        T CallFunction<T>(object function, params object[] arguments);
        void CallFunction(object function, params object[] arguments);
        string GetScriptDirectoryName();
        string GetScriptExtension();
        string GetEntryFileName();
    }
}
