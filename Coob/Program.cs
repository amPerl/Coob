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
        public static HookHandler Hooks;

        static void Main(string[] args)
        {
            Console.Title = "Coob";
            Console.TreatControlCAsInput = true;
            Log.Info("Starting Coob.");

            Scripting = new JavascriptEngine();

            Hooks = new HookHandler();
            Hooks.ScriptHandlers.Add(Scripting);

            Scripting.Initialize();
            Scripting.Load("Coob.js");

            Scripting.SetFunction("LogInfo", (Action<string>)Log.Info);
            Scripting.SetFunction("LogWarning", (Action<string>)Log.Warning);
            Scripting.SetFunction("LogError", (Action<string>)Log.Error);

            // Can't set functions within objects, so it'll have to be in the global namescape. D:
            Scripting.SetFunction("AddHook", (Action<string, object>)((eventName, function) =>
            {
                Hooks.Add(eventName, function);
            }));

            Scripting.SetFunction("RemoveHook", (Action<string, object>)((eventName, function) =>
            {
                Hooks.Delete(eventName, function);
            }));

            //Scripting = new CSharpEngine();
            //Scripting.Initialize();
            //Scripting.Load("CoobPlugin.cs");

            Scripting.Run();

            Coob = new Coob(new CoobOptions
                            {
                                Port = 12345,
                            });

            Scripting.SetParameter("coob", Root.Coob);

            var initializeEventArgs = new InitializeEventArgs(0);
            if (Hooks.Call("OnInitialize", initializeEventArgs).Canceled)
                return;

            Coob.Options.WorldSeed = initializeEventArgs.WorldSeed; // Not sure if this is the best way to do this

            Coob.StartMessageHandler();

            while(Coob.Running)
            {
                var input = Console.ReadLine().ToLower();

                if (input == "exit") // Temporary way to quit server properly. Seems to fuck up because the console hates life.
                    Coob.StopMessageHandler();
            }

            Log.Info("Stopping server...");
            //Scripting.CallFunction("onQuit");
            Hooks.Call("OnQuit", new QuitEventArgs(null));

            Log.Display(); // "Needs" to be called here since it normally gets called in the message handler (which isn't called anymore since server stopped).
        }
    }
}
