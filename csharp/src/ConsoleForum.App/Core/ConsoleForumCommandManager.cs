using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Windows.Input;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;
using ConsoleForum.App.Models.Enums;
using ICommand = ConsoleForum.App.Commands.ICommand;

namespace ConsoleForum.App.Core
{
    public class ConsoleForumCommandManager : ICommandManager
    {
        private readonly IInputReader consoleReader;

        private readonly IOutputWriter consoleWriter;

        private IPrincipal currentlyLoggedInUser;

        public ConsoleForumCommandManager(IInputReader consoleReader, IOutputWriter consoleWriter)
        {
            this.consoleReader = consoleReader;
            this.consoleWriter = consoleWriter;
            this.currentlyLoggedInUser = new Principal();
        }

        private bool IsLoggedIn() => currentlyLoggedInUser.User != null;

        private bool IsAdmin() => this.IsLoggedIn() && this.currentlyLoggedInUser.User.Role.Equals(UserRole.Admin);

        private bool IsModerator() => this.IsLoggedIn() && this.currentlyLoggedInUser.User.Role.Equals(UserRole.Moderator);

        private string GetAppropriateHeader()
        {
            if (this.IsAdmin()) return string.Format(Constants.AdminIntroductionMessage, this.currentlyLoggedInUser.User.Username, new string(' ', 37 - this.currentlyLoggedInUser.User.Username.Length));
            if (this.IsModerator()) return string.Format(Constants.ModeratorIntroductionMessage, this.currentlyLoggedInUser.User.Username, new string(' ', 43 - this.currentlyLoggedInUser.User.Username.Length));
            if (this.IsLoggedIn()) return string.Format(Constants.UserIntroductionMessage, this.currentlyLoggedInUser.User.Username, new string(' ', 53 - this.currentlyLoggedInUser.User.Username.Length));
            return Constants.GuestIntroductionMessage;
        }

        private void ClearConsole() => Console.Clear();

        private void PrintIntro()
        {
            this.consoleWriter.WriteLine(Constants.ConsoleForumIntroBanner);
            this.consoleWriter.WriteLine(Constants.ConsoleForumOutputEmptyLine);
            this.consoleWriter.WriteLine(this.GetAppropriateHeader());
            this.consoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
        }

        private ICommand InstanceCommand(string input)
        {
            List<string> inputParams = input.Split("|").ToList();
            string commandName = inputParams[0].ToLower();
            inputParams.RemoveAt(0);

            var commandType = Assembly.GetEntryAssembly()
                ?.GetTypes()
                .Where(type => typeof(ICommand).IsAssignableFrom(type))
                .FirstOrDefault(type => type.Name.Replace("Command", string.Empty).ToLower() == commandName);

            if (commandType != null)
            {
                ICommand command = (ICommand)Activator.CreateInstance(commandType);

                command.ConsoleReader = this.consoleReader;
                command.ConsoleWriter = this.consoleWriter;
                command.Parameters = inputParams;
                command.Principal = this.currentlyLoggedInUser;

                return command;
            }

            return null;
        }

        private bool IsAuthorized(ICommand command)
        {
            var authAttributes = command
                .GetType()
                .GetCustomAttributes()
                .Where(attribute => attribute.GetType().Name.Contains("CommandAttribute"))
                .ToList();
                
            if (!authAttributes.Any())
            {
                return true;
            }
            else
            {
                return 
                    (this.IsLoggedIn() && authAttributes
                    .Any(attribute => attribute.GetType().Name.Contains(this.currentlyLoggedInUser.User.Role.ToString())))
                    || (!this.IsLoggedIn() && authAttributes
                            .Any(attribute => attribute.GetType() == typeof(GuestCommandAttribute)));
            }
        }

        public void Reset()
        {
            this.ClearConsole();
            this.PrintIntro();
        }

        public void HandleInput()
        {
            this.consoleWriter.Write(Constants.ConsoleForumOutputPrefix);
            string input = this.consoleReader.ReadLine();

            ICommand command = this.InstanceCommand(input);

            if (command == null)
            {
                this.consoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.consoleWriter.ErrorLine("Unsupported command...");
                Thread.Sleep(Constants.NotificationDelay);
                this.Reset();
            }
            else
            {
                if (!this.IsAuthorized(command))
                {
                    this.consoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.consoleWriter.ErrorLine("You have no access to this command.");
                    Thread.Sleep(Constants.NotificationDelay);
                    this.Reset();
                }
                else
                {
                    command.Execute();
                    if (command.ResetsConsole) this.Reset();
                }
            }
        }
    }
}
