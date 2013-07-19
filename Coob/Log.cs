using System;
using System.Linq;

namespace Coob
{
    class Log
    {
        static Log()
        {
            Console.Clear();
        }

        static void writeTimePrefix()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(DateTime.Now.ToString(" HH:mm:ss "));
        }

        static void writeTypePrefix(string type, ConsoleColor bg, ConsoleColor fg)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(" {0,8} ", type);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }

        public static void WriteInfo(object message)
        {
            writeTimePrefix();
            writeTypePrefix("INFO", ConsoleColor.DarkGreen, ConsoleColor.Green);
            Console.WriteLine(message.ToString());
        }

        public static void WriteWarning(object message)
        {
            writeTimePrefix();
            writeTypePrefix("WARNING", ConsoleColor.DarkYellow, ConsoleColor.Yellow);
            Console.WriteLine(message.ToString());
        }

        public static void WriteError(object message)
        {
            string[] lines = message.ToString().Split('\n');
            if (lines.Length > 1)
            {
                foreach (string line in lines.Take(2))
                    WriteError(line);
            }
            else
            {
                writeTimePrefix();
                writeTypePrefix("ERROR", ConsoleColor.DarkRed, ConsoleColor.Red);
                Console.WriteLine(message.ToString());
            }
        }
    }
}
