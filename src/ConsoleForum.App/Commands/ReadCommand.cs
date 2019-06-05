using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using ConsoleForum.App.Commands.Attributes;
using ConsoleForum.App.Common;
using ConsoleForum.App.Core;
using ConsoleForum.App.Database;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Commands
{
    [UserCommand]
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Reads a Post, by given Id, viewing detailed info about it.")]
    [CommandExample("Read|1")]
    public class ReadCommand : ICommand
    {
        private const string NoSuchPostMessage = "There is no Post with the given Id.";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = false;

        public void Execute()
        {
            int postId = int.Parse(this.Parameters[0]);

            Post postFromDb = DbContext.Posts.FirstOrDefault(post => post.Id == postId);

            if (postFromDb == null || postFromDb.IsDeleted)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ErrorLine(NoSuchPostMessage);
                this.ResetsConsole = true;
                Thread.Sleep(Constants.NotificationDelay);
            }
            else
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.ImportantLine("Post - " + postFromDb.Title);
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.WriteLine(postFromDb.Content);
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);

                string postIdString = postFromDb.Id.ToString();
                string postUsernameString = postFromDb.User.Username;

                string emptySpace = new string(' ', 60 - postIdString.Length - postUsernameString.Length);

                this.ConsoleWriter.WriteLine($"[{postId}]{emptySpace} by {postUsernameString}");

                // RATINGS LABEL
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.WriteLine(new string('#', 69));
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputEmptyLine);

                string likesString = postFromDb.Ratings.Count(rating => rating.IsPositive).ToString();
                string dislikesString = postFromDb.Ratings.Count(rating => rating.IsNegative).ToString();

                this.ConsoleWriter.WriteLine($"## Likes: {likesString}{new string(' ', 46 - likesString.Length - dislikesString.Length)}Dislikes: {dislikesString} ##");
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputEmptyLine);
                this.ConsoleWriter.WriteLine(new string('#', 69));
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);

                // COMMENTS LABEL
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.WriteLine(new string('#', 69));
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputEmptyLine);
                this.ConsoleWriter.WriteLine($"##{new string(' ', 28)}Comments{new string(' ', 29)}##");
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputEmptyLine);
                this.ConsoleWriter.WriteLine(new string('#', 69));
                this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);

                if (postFromDb.Comments.Count != 0)
                {
                    foreach (var comment in postFromDb.Comments.Where(comment => !comment.IsDeleted))
                    {
                        this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                        this.ConsoleWriter.WriteLine($"(&{comment.Id}) ({(comment.User.Role == UserRole.Moderator || comment.User.Role == UserRole.Admin ? "[" + comment.User.Role + "] " : "")}{comment.User.Username}): {comment.Content}");
                        this.ConsoleWriter.WriteLine(Constants.ConsoleForumOutputPrefix);
                    }
                }                
            }
        }
    }
}
