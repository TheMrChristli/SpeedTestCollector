using System;

namespace SpeedTestCollector.util
{
    public class Logger
    {
        private const string _template = "[{0} [{1}] | {2}";
        private static string convert(string type, string message)
        {
            string now = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            return String.Format(_template, now, type, message);
        }
        public static void Print(string message)
        {
            Console.WriteLine(convert("INFO", message));
        }

        public static void Error(string message)
        {
            Console.WriteLine(convert("ERROR", message));
        }
    }
}