using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Coob.CoobEventArgs;
using Jint.Native;

namespace Coob
{
    class Root
    {
        public static Coob Coob;
        public static IScriptHandler Scripting;
        public static ScriptManager ScriptManager;

        static void Main(string[] args)
        {
            Console.Title = "Coob";
            //Console.TreatControlCAsInput = true;
            Log.Info("Starting Coob.");

            Scripting = new JavascriptEngine();

            ScriptManager = new ScriptManager();
            ScriptManager.ScriptHandlers.Add(Scripting);
            ScriptManager.Initialize();

            Scripting.Run();

            var eventArgs = Root.ScriptManager.CallEvent("OnInitialize", new InitializeEventArgs()) as InitializeEventArgs;

            Coob = new Coob(new CoobOptions
                            {
                                Port = 12345,
                                WorldSeed = eventArgs.WorldSeed,
                            });

            Scripting.SetParameter("coob", Coob);
            Scripting.SetParameter("world", Coob.World);

            Log.Info("World seed: " + Coob.World.Seed);

            Coob.StartServer();

            while(Coob.Running)
            {
                var input = Console.ReadLine().ToLower();

                if (input == "exit") // Temporary way to quit server properly. Seems to fuck up because the console hates life.
                    Coob.StopServer();
            }

            Log.Info("Stopping server...");
            //Scripting.CallFunction("onQuit");
            ScriptManager.CallEvent("OnQuit", new QuitEventArgs());

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }
    }
}
