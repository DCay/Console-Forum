using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [CommandHelper("Views all created Posts from the database, ordered by rating.")]
    [CommandExample("AllPosts")]
    public class AllPosts : ICommand
    {
        private const string NoPostsMessage = "There are currently no Posts.";

        private const string PostFormatOutput = "[{0}] {1}";

        public IInputReader ConsoleReader { get; set; }

        public IOutputWriter ConsoleWriter { get; set; }

        public List<string> Parameters { get; set; }

        public IPrincipal Principal { get; set; }

        public bool ResetsConsole { get; set; } = false;

        public void Execute()
        {
            List<Post> postsFromDb = DbContext.Posts
                .Where(post => !post.IsDeleted)
                .OrderByDescending(post => post.Ratings.Count(rating => rating.IsPositive))
                .ThenBy(post => post.Ratings.Count(rating => rating.IsNegative))
                .ToList();

            if (postsFromDb.Count == 0)
            {
                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.WriteLine(NoPostsMessage);
            }
            else
            {
                for (int i = 0; i < 3 && i < postsFromDb.Count; i++)
                {
                    Post post = postsFromDb[i];
                    this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.ConsoleWriter.ImportantLine(PostFormatOutput, post.Id, post.Title);
                }

                for (int i = 3; i < postsFromDb.Count; i++)
                {
                    Post post = postsFromDb[i];
                    this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.ConsoleWriter.WriteLine(PostFormatOutput, post.Id, post.Title);
                }
            }
        }
    }
}
