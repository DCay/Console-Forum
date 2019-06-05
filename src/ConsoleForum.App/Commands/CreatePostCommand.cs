using System;
using System.Collections.Generic;
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
    [CommandHelper("Creates a Post by a given title and content.")]
    [CommandExample("CreatePost|SomeTitle|SomeLongContent")]
    public class CreatePostCommand : ICommand
    {
        private const string SuccessCreatePostMessage = "Successfully created Post - {0}!";
        
        private const string TitleAndContentMalformed =
            "Title and Content must not be empty.";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = true;

        public void Execute()
        {
            string title = this.Parameters[0];
            string content = this.Parameters[1];

            if (Validator.IsNullOrEmpty(title) || Validator.IsNullOrEmpty(content))
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(TitleAndContentMalformed);
            }

            Post postForDb = new Post
            {
                Title = title,
                Content = content,
                User = this.Principal.User
            };

            DbContext.Add(postForDb);
            this.Principal.User.Posts.Add(postForDb);
            
            this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
            this.ConsoleWriter.SuccessLine(SuccessCreatePostMessage, title);
            Thread.Sleep(Constants.NotificationDelay);
        }
    }
}
