using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.IO
{
    public class ConsoleReader : IInputReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
