using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coob
{
    class Root
    {
        public static Coob Coob;
        public static IScriptHandler Scripting;

        static void Main(string[] args)
        {
            Console.Title = "Coob";
            Log.Info("Starting Coob.");

            Coob = new Coob(new CoobOptions
            {
                Port = 12345,
                WorldSeed = 26874,
            });

            Scripting = new JavascriptEngine();

            Scripting.Initialize();
            Scripting.Load("Coob.js");

            Scripting.SetParameter("coob", Root.Coob);

            Scripting.SetFunction("LogInfo", (Action<string>)Log.Info);
            Scripting.SetFunction("LogWarning", (Action<string>)Log.Warning);
            Scripting.SetFunction("LogError", (Action<string>)Log.Error);

            Scripting.Run();

            Coob.StartMessageHandler();

            while(true) Console.ReadLine();
        }
    }
}
