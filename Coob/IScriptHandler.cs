using System;

namespace Coob
{
    interface IScriptHandler
    {
        void SetFunction(string name, Delegate function);
        void SetParameter(string name, object value);
        void Initialize();
        void Load(string sourceFile);
        void Run();
        T CallFunction<T>(string functionName, params object[] arguments);
        void CallFunction(string functionName, params object[] arguments);
    }
}
