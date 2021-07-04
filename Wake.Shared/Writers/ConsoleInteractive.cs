using System;

namespace Wake.Shared.Writers
{
    public class ConsoleInteractive: IConsoleInteractive
    {
        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
        
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}