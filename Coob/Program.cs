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
        public static HookHandler Hooks;

        static void Main(string[] args)
        {
            Console.Title = "Coob";
            Console.TreatControlCAsInput = true;
            Log.Info("Starting Coob.");

            Coob = new Coob(new CoobOptions
            {
                Port = 12345,
                //WorldSeed = new Random().Next(0, int.MaxValue), // I set it to random because it's easier to see if EntityUpdate is fixed or not. (fakin loading times tho)
                WorldSeed = 19025811,
            });

            //Scripting = new JavascriptEngine();

            //Scripting.Initialize();
            //Scripting.Load("Coob.js");

            //Scripting.SetParameter("coob", Root.Coob);

            //Scripting.SetFunction("LogInfo", (Action<string>)Log.Info);
            //Scripting.SetFunction("LogWarning", (Action<string>)Log.Warning);
            //Scripting.SetFunction("LogError", (Action<string>)Log.Error);

            Scripting = new CSharpEngine();
            Scripting.Initialize();
            Scripting.Load("CoobPlugin.cs");

            Hooks = new HookHandler();
            Hooks.ScriptHandlers.Add(Scripting);

            Scripting.Run();

            if (!Scripting.CallFunction<bool>("onInitialize"))
                return;

            Coob.StartMessageHandler();

            while(Coob.Running)
            {
                var input = Console.ReadLine().ToLower();

                if (input == "exit") // Temporary way to quit server properly. Seems to fuck up because the console hates threading.
                    Coob.StopMessageHandler();
            }

            Log.Info("Stopping server...");
            Scripting.CallFunction("onQuit");

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }
    }
}
