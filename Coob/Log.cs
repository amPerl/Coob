using System;
using System.Collections.Generic;

namespace Coob
{
    public static class Log
    {
        public class LogMessage
        {
            public string LogType;
            public string Message;
            public ConsoleColor BgColor;
            public ConsoleColor FgColor;

            public LogMessage(string logType, object message, ConsoleColor bgColor, ConsoleColor fgColor)
            {
                LogType = logType;
                Message = message.ToString();
                BgColor = bgColor;
                FgColor = fgColor;
            }
        }

        private static readonly Queue<LogMessage> QueuedMessages = new Queue<LogMessage>();

        static Log()
        {
            Console.Clear();
        }

        static void WriteTimePrefix()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(DateTime.Now.ToString(" HH:mm:ss "));
        }

        static void WriteTypePrefix(string type, ConsoleColor bg, ConsoleColor fg)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(" {0,8} ", type);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }

        public static void Info(object message)
        {
            if (message == null)
                message = "null";

            // Can't use the method below because if the message contains { or } it will throw invalid string format exception.
            QueuedMessages.Enqueue(new LogMessage("INFO", message, ConsoleColor.DarkGreen, ConsoleColor.Green));
        }

        public static void Info(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                args = new object[] { "null" };

            QueuedMessages.Enqueue(new LogMessage("INFO", string.Format(format, args), ConsoleColor.DarkGreen, ConsoleColor.Green));
        }

        public static void Warning(object message)
        {
            if (message == null)
                message = "null";

            QueuedMessages.Enqueue(new LogMessage("WARNING", message, ConsoleColor.DarkYellow, ConsoleColor.Yellow));
        }

        public static void Warning(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                args = new object[] { "null" };

            QueuedMessages.Enqueue(new LogMessage("WARNING", string.Format(format, args), ConsoleColor.DarkYellow, ConsoleColor.Yellow));
        }

        public static void Error(object message)
        {
            if (message == null)
                message = "null";

            QueuedMessages.Enqueue(new LogMessage("ERROR", message, ConsoleColor.DarkRed, ConsoleColor.Red));
        }

        public static void Error(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                args = new object[] { "null" };

            QueuedMessages.Enqueue(new LogMessage("ERROR", string.Format(format, args), ConsoleColor.DarkRed, ConsoleColor.Red));
        }

        public static void Display()
        {
            while (QueuedMessages.Count > 0)
            {
                LogMessage message = QueuedMessages.Dequeue();

                WriteTimePrefix();
                WriteTypePrefix(message.LogType, message.BgColor, message.FgColor);
                Console.WriteLine(message.Message);
            }
        }
    }
}
