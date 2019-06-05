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
    [CommandHelper("Deletes a Comment by given Id.")]
    [CommandExample("DeleteComment|1")]
    public class DeleteCommentCommand : ICommand
    {
        private const string NoSuchCommentMessage = "There is no Comment with the given Id.";

        private const string SuccessDeleteMessage = "Successfully Deleted Comment - {0}!";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            int commentId = int.Parse(this.Parameters[0]);

            Comment commentFromDb = DbContext.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (commentFromDb == null || commentFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchCommentMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                commentFromDb.IsDeleted = true;

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessDeleteMessage, commentId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
