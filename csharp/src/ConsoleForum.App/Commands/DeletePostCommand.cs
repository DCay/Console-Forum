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

namespace ConsoleForum.App.Commands
{
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Deletes a Post by given Id.")]
    [CommandExample("DeletePost|1")]
    public class DeletePostCommand : ICommand
    {
        private const string NoSuchPostMessage = "There is no Post with the given Id.";

        private const string SuccessDeleteMessage = "Successfully Deleted Post - {0}!";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            int postId = int.Parse(this.Parameters[0]);

            Post postFromDb = DbContext.Posts.FirstOrDefault(post => post.Id == postId);

            if (postFromDb == null || postFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchPostMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                postFromDb.IsDeleted = true;

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessDeleteMessage, postId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
