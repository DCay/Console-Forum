using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;

namespace ConsoleForum.App.Commands
{
    [GuestCommand]
    [UserCommand]
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Clears the Console Forum's Console.")]
    [CommandExample("Clear")]
    public class ClearCommand : ICommand
    {
        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            // Do nothing;
        }
    }
}
