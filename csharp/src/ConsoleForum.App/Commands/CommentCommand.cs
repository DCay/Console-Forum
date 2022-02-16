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
    [UserCommand]
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Creates a Comment to a Post, by given Post Id.")]
    [CommandExample("Comment|1|This is a Simple Comment.")]
    public class CommentCommand : ICommand
    {
        private const string NoSuchPostMessage = "There is no Post with the given Id.";

        private const string ContentMalformed = "Content must not be empty.";

        private const string SuccessCreateCommentMessage = "Successfully created Comment for Post - {0}!";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            int postId = int.Parse(this.Parameters[0]);
            string content = this.Parameters[1];

            Post postFromDb = DbContext.Posts.FirstOrDefault(post => post.Id == postId);

            if (postFromDb == null || postFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchPostMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else if (Validator.IsNullOrEmpty(content))
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchPostMessage);
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                Comment commentForDb = new Comment
                {
                    Content = content,
                    User = this.Principal.User,
                    Post = postFromDb
                };

                DbContext.Add(commentForDb);
                this.Principal.User.Comments.Add(commentForDb);
                postFromDb.Comments.Add(commentForDb);

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessCreateCommentMessage, postId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
