using System;
using System.Linq;
using Coob.CoobEventArgs;
using Coob.ScriptEngines;
using Jint;

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

            var initArgs = ScriptManager.CallEvent<InitializeEventArgs>("OnInitialize", new InitializeEventArgs());

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
                var input = Console.ReadLine() ?? ""; // FixMe: This will cause the console to spam HandleConsoleCommand(line) with an empty string.

                HandleConsoleCommand(input);
            }

            Log.Info("Stopping server...");
            //Scripting.CallFunction("onQuit");
            ScriptManager.CallEvent("OnQuit", new QuitEventArgs());

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }

        private static void HandleConsoleCommand(string line)
        {
            var parts = line.Split(' ');
            var command = parts[0];

            switch (command)
            {
                case "exit":
                case "quit":
                case "q":
                    Coob.StopServer();
                    break;
                case "kick":
                    if (parts.Length == 2)
                        Coob.KickPlayer(parts[1]);
                    break;
                case "say":
                    Coob.Broadcast(line.Substring(4));
                    break;
                default :
                    Log.Info("Unrecognised command: {0}", command);
                    break;
            }
        }
    }
}
