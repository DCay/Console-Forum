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
    [CommandHelper("Edits a Comment by given Id, updating its content.")]
    [CommandExample("EditComment|1|New Content")]
    public class EditCommentCommand : ICommand
    {
        private const string NoSuchCommentMessage = "There is no Comment with the given Id.";

        private const string SuccessEditMessage = "Successfully Editted Comment - {0}!";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            int commentId = int.Parse(this.Parameters[0]);
            string newContent = this.Parameters[1];

            Comment commentFromDb = DbContext.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (commentFromDb == null || commentFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchCommentMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                commentFromDb.Content = newContent;

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessEditMessage, commentId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
