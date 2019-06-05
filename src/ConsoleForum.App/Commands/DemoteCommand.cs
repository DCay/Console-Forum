using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [AdminCommand]
    [CommandHelper("Demotes the User with the given Id into a lower role.")]
    [CommandExample("Demote|1")]
    public class DemoteCommand : ICommand
    {
        private const string NoSuchUserMessage = "There is no User with the given Id.";

        private const string SuccessDemoteMessage = "Successfully Demoted User - {0}";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            int userId = int.Parse(this.Parameters[0]);

            User userFromDb = DbContext.Users.FirstOrDefault(user => user.Id == userId);

            if (userFromDb == null || userFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchUserMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                if (userFromDb.Role == UserRole.Admin)
                {
                    userFromDb.Role = UserRole.Moderator;
                }
                else if (userFromDb.Role == UserRole.Moderator)
                {
                    userFromDb.Role = UserRole.User;
                }

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessDemoteMessage, userId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
