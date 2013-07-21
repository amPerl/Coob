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

        public void RunString(string code)
        {
            //throw new NotImplementedException();
        }

        public T CallFunction<T>(object function, params object[] arguments)
        {
            if (function is MethodInfo)
            {
                var output = ((MethodInfo)function).Invoke(mainClass, arguments);
                return (T)output;
            }
            else
            {
                var output = mainType.GetMethod((string)function).Invoke(mainClass, arguments);
                return (T)output;
            }
        }

        public void CallFunction(object function, params object[] arguments)
        {
            if (function is MethodInfo)
            {
                ((MethodInfo)function).Invoke(mainClass, arguments);
            }
            else
            {
                mainType.GetMethod((string)function).Invoke(mainClass, arguments);
            }
        }
    }
}
