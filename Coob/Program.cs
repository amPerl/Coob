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
        public static ScriptHandler JavaScript;

        static void Main(string[] args)
        {
            Console.Title = "Coob";
            Log.WriteInfo("Starting Coob.");

            Coob = new Coob(new CoobOptions
            {
                Port = 12345,
                WorldSeed = 12345
            });
            JavaScript = new ScriptHandler("Coob.js");

            Coob.StartMessageHandler();

            while(true) Console.ReadLine();
        }
    }
}
