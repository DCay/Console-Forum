using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Commands
{
    [CommandHelper("Displays information about each command and its functionality.")]
    [CommandExample("Help")]
    public class HelpCommand : ICommand
    {
        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = false;

        public void Execute()
        {
            var commands = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(type => typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface)
                .ToList();

            if (this.Principal.User == null)
            {
                commands = commands
                    .Where(type =>
                        !type.GetCustomAttributes()
                            .Any(attribute => attribute.GetType().Name.Contains("CommandAttribute"))
                        || type.GetCustomAttributes()
                            .Any(attribute => attribute.GetType() == typeof(GuestCommandAttribute)))
                    .ToList();
            }
            else
            {
                commands = commands
                    .Where(type =>
                        type.GetCustomAttributes()
                            .Any(attribute => attribute.GetType().Name.Contains($"{this.Principal.User.Role.ToString()}CommandAttribute")))
                    .ToList();
            }

            commands
                .Select(type => $"[{type.Name.Replace("Command", string.Empty)}] <=> {((CommandHelperAttribute)type.GetCustomAttribute(typeof(CommandHelperAttribute))).Help} <=> Example: \"{((CommandExampleAttribute)type.GetCustomAttribute(typeof(CommandExampleAttribute))).Example}\"")
                .ToList()
                .ForEach(help =>
                {
                    this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.ConsoleWriter.WriteLine(help);
                });
        }
    }
}
