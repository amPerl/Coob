using System;
using System.Collections.Generic;
using System.Linq;

namespace Coob
{
    class Log
    {
        public class LogMessage
        {
            public string LogType;
            public string Message;
            public ConsoleColor BGColor;
            public ConsoleColor FGColor;

            public LogMessage(string logType, object message, ConsoleColor bgColor, ConsoleColor fgColor)
            {
                LogType = logType;
                Message = message.ToString();
                BGColor = bgColor;
                FGColor = fgColor;
            }
        }

        private static Queue<LogMessage> queuedMessages = new Queue<LogMessage>();

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

        public static void WriteInfo(string format, params object[] args)
        {
            queuedMessages.Enqueue(new LogMessage("INFO", string.Format(format, args), ConsoleColor.DarkGreen, ConsoleColor.Green));
        }

        public static void WriteWarning(string format, params object[] args)
        {
            queuedMessages.Enqueue(new LogMessage("WARNING", string.Format(format, args), ConsoleColor.DarkYellow, ConsoleColor.Yellow));
        }

        public static void WriteError(string format, params object[] args)
        {
            queuedMessages.Enqueue(new LogMessage("ERROR", string.Format(format, args), ConsoleColor.DarkRed, ConsoleColor.Red));
        }

        public static void Display()
        {
            while (queuedMessages.Count > 0)
            {
                LogMessage message = queuedMessages.Dequeue();

                writeTimePrefix();
                writeTypePrefix(message.LogType, message.BGColor, message.FGColor);
                Console.WriteLine(message.Message);
            }
        }
    }
}
