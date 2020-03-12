using System;
using Modul_1_Base_Class_Library.Interfases;

namespace Modul_1_Base_Class_Library
{
    class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}

