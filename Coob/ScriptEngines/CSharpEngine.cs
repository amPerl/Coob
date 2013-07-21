using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Coob
{
    class CSharpEngine : IScriptHandler
    {
        private string source;

        public void SetFunction(string name, Delegate function)
        {
            //throw new NotImplementedException();
        }

        public void SetParameter(string name, object value)
        {
            //throw new NotImplementedException();
        }

        CSharpCodeProvider codeProvider;
        ICodeCompiler icc;
        Assembly compiledAssembly;
        Type mainType;
        Object mainClass;

        public void Initialize()
        {
            codeProvider = new CSharpCodeProvider();
        }

        public void Load(string sourceFile)
        {
            source = File.ReadAllText(sourceFile);
            var res = codeProvider.CompileAssemblyFromSource(
                new System.CodeDom.Compiler.CompilerParameters()
                {
                    GenerateInMemory = false
                },
                source
            );

            compiledAssembly = res.CompiledAssembly;

            mainType = compiledAssembly.GetType("CoobPlugin");
            mainClass = Activator.CreateInstance(mainType);
        }

        public void Run()
        {
            mainType.GetMethod("Main").Invoke(mainClass, new object[]{});
        }

        public T CallFunction<T>(string functionName, params object[] arguments)
        {
            var output = mainType.GetMethod(functionName).Invoke(mainClass, arguments);
            return (T)output;
        }

        public void CallFunction(string functionName, params object[] arguments)
        {
            mainType.GetMethod(functionName).Invoke(mainClass, arguments);
        }
    }
}
