using System;
using Coob.CoobEventArgs;
using Coob.ScriptEngines;

namespace Coob
{
    public class Program
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

            var initArgs = (InitializeEventArgs)ScriptManager.CallEvent("OnInitialize", new InitializeEventArgs());

            Coob = new Coob(new CoobOptions
            {
                Port = initArgs.Port,
                WorldSeed = initArgs.WorldSeed,
            });

            Scripting.SetParameter("coob", Coob);
            Scripting.SetParameter("world", Coob.World);

            Log.Info("World seed: " + Coob.World.Seed);

            Coob.StartServer();

            while (Coob.Running)
            {
                var input = Console.ReadLine() ?? "";

                if (input.ToLower() == "exit") // Temporary way to quit server properly. Seems to fuck up because the console hates life.
                    Coob.StopServer();
            }

            Log.Info("Stopping server...");
            //Scripting.CallFunction("onQuit");
            ScriptManager.CallEvent("OnQuit", new QuitEventArgs());

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }
    }
}
