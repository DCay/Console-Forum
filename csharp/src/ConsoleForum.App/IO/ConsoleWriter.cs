using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.IO
{
    public class ConsoleWriter : IOutputWriter
    {
        public void Write(object output)
        {
            Console.Write(output.ToString());
        }

        public void Write(object output, params object[] parameters)
        {
            Console.Write(string.Format(output.ToString(), parameters));
        }

        public void WriteLine(object output)
        {
            Console.WriteLine(output.ToString());
        }

        public void WriteLine(object output, params object[] parameters)
        {
            Console.WriteLine(string.Format(output.ToString(), parameters));
        }

        public void ErrorLine(object output, params object[] parameters)
        {
            var oldForeColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            this.WriteLine(output, parameters);

            Console.ForegroundColor = oldForeColor;
        }

        public void SuccessLine(object output, params object[] parameters)
        {
            var oldForeColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;

            this.WriteLine(output, parameters);

            Console.ForegroundColor = oldForeColor;
        }

        public void ImportantLine(object output, params object[] parameters)
        {
            var oldForeColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            this.WriteLine(output, parameters);

            Console.ForegroundColor = oldForeColor;
        }
    }
}
