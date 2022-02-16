using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;

namespace ConsoleForum.App
{
    public class Launcher
    {
        public static void Main(string[] args)
        {
            IInputReader consoleReader = new ConsoleReader();
            IOutputWriter consoleWriter = new ConsoleWriter();

            ICommandManager consoleForumCommandManager = new ConsoleForumCommandManager(consoleReader, consoleWriter);

            IEngine consoleForumEngine = new ConsoleForumEngine(consoleForumCommandManager);
            consoleForumEngine.Run();
        }
    }
}
