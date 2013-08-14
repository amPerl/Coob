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
                var input = Console.ReadLine() ?? ""; // FixMe: This will cause the console to spam HandleConsoleCommand(line) with an empty string if there's no input.

                HandleConsoleCommand(input);
            }

            Log.Info("Stopping server...");
            //Scripting.CallFunction("onQuit");
            ScriptManager.CallEvent("OnQuit", new QuitEventArgs());

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }

        private static void HandleConsoleCommand(string line)
        {
            if (line == "")
            {
                Console.CursorTop -= 1;
                return;
            }

            var parts = line.Split(new char[] {' '}, 2);
            var command = parts[0].ToLower();
            var arg = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case "exit":
                case "quit":
                case "q":
                    Coob.StopServer();
                    break;
                case "kick":
                    if (parts.Length == 2)
                        Coob.KickPlayer(arg);
                    break;
                case "say":
                    Coob.Broadcast(arg);
                    Log.Info("[SERVER] " + arg);
                    break;
                default :
                    Log.Info("Unrecognised command: {0}", command);
                    break;
                case "js":
                    var result = Scripting.RunString(arg);
                    Log.Custom(result ?? "No returned value.", "JSCRIPT");
                    break;
                case "help":
                    Log.Info("exit, quit, q: stop the server.");
                    Log.Info("kick [player name]: kick the player with the given name.");
                    Log.Info("say: broadcast a message to the server.");
                    Log.Info("js: execute as javascript.");
                    break;
            }
        }
    }
}
