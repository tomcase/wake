using System;

namespace Wake.Shared.Writers
{
    public interface IConsoleInteractive
    {
        void Write(string message);
        void WriteLine(string message);
        string ReadLine();
        ConsoleKeyInfo ReadKey();
    }
}