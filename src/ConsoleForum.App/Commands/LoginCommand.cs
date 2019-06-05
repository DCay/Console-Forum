using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.Database;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;

namespace ConsoleForum.App.Commands
{
    [CommandHelper("Logs in a User by given username and password.")]
    [CommandExample("Login|John|123")]
    public class LoginCommand : ICommand
    {
        private const string SuccessLoginMessage = "Successfully logged in with User - {0}!";

        private const string ErrorLoginMessage = "Invalid username or password.";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }
        
        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            string username = this.Parameters[0];
            string password = this.Parameters[1];

            User userFromDb = DbContext.Users.FirstOrDefault(user => user.Username == username && user.Password == password);

            this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);

            if (userFromDb != null)
            {
                this.Principal.SignIn(userFromDb);
                this.ConsoleWriter.SuccessLine(SuccessLoginMessage, username);
            }
            else
            {
                this.ConsoleWriter.ErrorLine(ErrorLoginMessage);
            }

            Thread.Sleep(Constants.NotificationDelay);
        }
    }
}
