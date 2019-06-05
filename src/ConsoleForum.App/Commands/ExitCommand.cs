using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.Database;
using ConsoleForum.App.IO;

namespace ConsoleForum.App.Commands
{
    [CommandHelper("Exits the Console Forum application.")]
    [CommandExample("Exit")]
    public class ExitCommand : ICommand
    {
        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; }

        public void Execute()
        {
            this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
            this.ConsoleWriter.WriteLine("Saving Database and Exiting Console Forum...");

            DbContext.Store();
            Thread.Sleep(Constants.ExitNotificationDelay);

            EngineState.IsRunning = false;
        }
    }
}
