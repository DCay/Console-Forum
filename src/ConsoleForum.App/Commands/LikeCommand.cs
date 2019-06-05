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
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Commands
{
    [UserCommand]
    [ModeratorCommand]
    [AdminCommand]
    [CommandHelper("Likes a Post by a given Post Id.")]
    [CommandExample("Like|1")]
    public class LikeCommand : ICommand
    {
        private const string NoSuchPostMessage = "There is no Post with the given Id.";

        private const string CannotLikeAgainMessage = "You already liked this Post.";

        private const string SuccessLikeMessage = "Successfully Liked Post - {0}!";

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
            else if (postFromDb.Ratings.Any(rating => rating.User.Id == this.Principal.User.Id))
            {
                PostRating ratingFromDb = postFromDb.Ratings.First(rating => rating.User.Id == this.Principal.User.Id);

                if (ratingFromDb.IsPositive)
                {
                    this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.ConsoleWriter.ErrorLine(CannotLikeAgainMessage);
                    Thread.Sleep(Constants.NotificationDelay);
                }
                else
                {
                    ratingFromDb.Toggle();

                    this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                    this.ConsoleWriter.SuccessLine(SuccessLikeMessage, postId);
                    Thread.Sleep(Constants.NotificationDelay);
                }
            }
            else
            {
                PostRating rating = new PostRating(PostRatingChoice.Positive)
                {
                    User = this.Principal.User,
                    Post = postFromDb
                };

                DbContext.Add(rating);

                this.Principal.User.Ratings.Add(rating);
                postFromDb.Ratings.Add(rating);

                this.ConsoleWriter.Write(Constants.ConsoleForumOutputPrefix);
                this.ConsoleWriter.SuccessLine(SuccessLikeMessage, postId);
                Thread.Sleep(Constants.NotificationDelay);
            }
        }
    }
}
