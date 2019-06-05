using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Database;
using ConsoleForum.App.IO;

namespace ConsoleForum.App.Core
{
    public class ConsoleForumEngine : IEngine
    {
        private readonly ICommandManager consoleForumCommandManager;

        public ConsoleForumEngine(ICommandManager consoleForumCommandManager)
        {
            this.consoleForumCommandManager = consoleForumCommandManager;
        }

        public void Run()
        {
            EngineState.IsRunning = true;

            DbContext.Initialize();

            this.consoleForumCommandManager.Reset();

            while (EngineState.IsRunning)
            {
                this.consoleForumCommandManager.HandleInput();
            }
        }
    }
}
