using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;

namespace ConsoleForum.App.Commands
{
    public interface ICommand
    {
        IInputReader ConsoleReader { get; set; }

        IOutputWriter ConsoleWriter { get; set; }

        List<string> Parameters { get; set; }

        IPrincipal Principal { get; set; }

        bool ResetsConsole { get; set; }

        void Execute();
    }
}
