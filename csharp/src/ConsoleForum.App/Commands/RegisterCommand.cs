using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.Database;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Commands
{
    [CommandHelper("Registers a User in the database by given username password and confirmPassword.")]
    [CommandExample("Register|John|123|123")]
    public class RegisterCommand : ICommand
    {
        private const string SuccessRegisterMessage = "Successfully registed User - {0}!";

        private const string PasswordsDoNotMatchMessage = "Passwords do not match.";

        private const string ExistentUsernameMessage = "Username already exists in the database.";

        private const string UsernameAndPasswordMalformed =
            "Username and password must consist only of alphanumeric characters.";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            string username = this.Parameters[0];
            string password = this.Parameters[1];
            string confirmPassword = this.Parameters[2];

            this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);

            if (Validator.IsNullOrEmpty(username) || Validator.IsNullOrEmpty(password)
                                                  || !Validator.IsAlphanumeric(username)
                                                  || !Validator.IsAlphanumeric(password))
            {
                this.ConsoleWriter.ErrorLine(UsernameAndPasswordMalformed);
            }
            else if (password != confirmPassword)
            {
                this.ConsoleWriter.ErrorLine(PasswordsDoNotMatchMessage);
            }
            else if(DbContext.Users.Any(user => user.Username == username))
            {
                this.ConsoleWriter.ErrorLine(ExistentUsernameMessage);
            }
            else
            {
                DbContext.Add(new User
                {
                    Username = username,
                    Password = password,
                    Role = UserRole.User,
                });

                this.ConsoleWriter.SuccessLine(SuccessRegisterMessage, username);
            }

            Thread.Sleep(Constants.NotificationDelay);
        }
    }
}
