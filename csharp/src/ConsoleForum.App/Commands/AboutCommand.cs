using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;

namespace ConsoleForum.App.Commands
{
    [CommandHelper("Views detailed information about the Console Forum.")]
    [CommandExample("About")]
    public class AboutCommand : ICommand
    {
        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; }

        public void Execute()
        {
            this.ConsoleWriter.WriteLine(Constants.ConsoleForumAbout);
            this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
        }
    }
}
