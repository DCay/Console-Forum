using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;

namespace ConsoleForum.App.Commands
{
    [UserCommand]
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Logs out the currently logged-in User.")]
    [CommandExample("Logout")]
    public class LogoutCommand : ICommand
    {
        private const string SuccessLogoutMessage = "Successfully logged out.";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            this.Principal.SignOut();

            this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
            this.ConsoleWriter.SuccessLine(SuccessLogoutMessage);

            Thread.Sleep(Constants.NotificationDelay);
        }
    }
}
