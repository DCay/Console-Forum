using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using ConsoleForum.App.IO;
using ConsoleForum.App.Models;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Database
{
    public static class DbContext
    {
        private static readonly List<User> users = new List<User>();

        private static readonly List<Post> posts = new List<Post>();

        private static readonly List<Comment> comments = new List<Comment>();

        private static readonly List<PostRating> ratings = new List<PostRating>();

        private static void ClearFiles()
        {
            File.WriteAllText("../../../db/users.db", "");
            File.WriteAllText("../../../db/posts.db", "");
            File.WriteAllText("../../../db/comments.db", "");
            File.WriteAllText("../../../db/ratings.db", "");
        }

        private static void InitializeUsers(List<string> unparsedUsers)
        {
            foreach (var unparsedUser in unparsedUsers)
            {
                string[] unparsedParams = unparsedUser.Split(",");

                int id = int.Parse(unparsedParams[0]);
                string username = unparsedParams[1];
                string password = unparsedParams[2];
                bool isEnabled = bool.Parse(unparsedParams[3]);
                UserRole role = Enum.Parse<UserRole>(unparsedParams[4]);
                bool isDeleted = bool.Parse(unparsedParams[5]);

                if (isDeleted) continue;

                users.Add(new User
                {
                    Id = id,
                    Username = username,
                    Password = password,
                    IsEnabled = isEnabled,
                    Role = role,
                });
            }
        }

        private static void InitializePosts(List<string> unparsedPosts)
        {
            foreach (var unparsedPost in unparsedPosts)
            {
                string[] unparsedParams = unparsedPost.Split(",");

                int id = int.Parse(unparsedParams[0]);
                string title = unparsedParams[1];
                string content = unparsedParams[2];
                User postUser = users.FirstOrDefault(user => user.Id == int.Parse(unparsedParams[3]));
                bool isDeleted = bool.Parse(unparsedParams[4]);

                if (isDeleted) continue;

                Post postForDb = new Post
                {
                    Id = id,
                    Title = title,
                    Content = content,
                    User = postUser
                };

                posts.Add(postForDb);
                postUser?.Posts.Add(postForDb);
            }
        }

        private static void InitializeComments(List<string> unparsedComments)
        {
            foreach (var unparsedComment in unparsedComments)
            {
                string[] unparsedParams = unparsedComment.Split(",");

                int id = int.Parse(unparsedParams[0]);
                string content = unparsedParams[1];
                User commentUser = users.FirstOrDefault(user => user.Id == int.Parse(unparsedParams[2]));
                Post commentPost = posts.FirstOrDefault(post => post.Id == int.Parse(unparsedParams[3]));
                bool isDeleted = bool.Parse(unparsedParams[4]);

                if (isDeleted) continue;

                Comment commentForDb = new Comment
                {
                    Id = id,
                    Content = content,
                    User = commentUser,
                    Post = commentPost
                };

                comments.Add(commentForDb);
                commentUser?.Comments.Add(commentForDb);
                commentPost?.Comments.Add(commentForDb);
            }
        }

        private static void InitializeRatings(List<string> unparsedRatings)
        {
            foreach (var unparsedRating in unparsedRatings)
            {
                string[] unparsedParams = unparsedRating.Split(",");

                int id = int.Parse(unparsedParams[0]);
                PostRatingChoice choice = Enum.Parse<PostRatingChoice>(unparsedParams[1]);
                User ratingUser = users.FirstOrDefault(user => user.Id == int.Parse(unparsedParams[2]));
                Post ratingPost = posts.FirstOrDefault(post => post.Id == int.Parse(unparsedParams[3]));
                bool isDeleted = bool.Parse(unparsedParams[4]);

                if (isDeleted) continue;

                PostRating ratingForDb = new PostRating(choice)
                {
                    Id = id,
                    User = ratingUser,
                    Post = ratingPost
                };

                ratings.Add(ratingForDb);
                ratingUser?.Ratings.Add(ratingForDb);
                ratingPost?.Ratings.Add(ratingForDb);
            }
        }

        public static ImmutableList<User> Users => users.ToImmutableList();

        public static ImmutableList<Post> Posts => posts.ToImmutableList();

        public static ImmutableList<Comment> Comments => comments.ToImmutableList();

        public static ImmutableList<PostRating> Ratings => ratings.ToImmutableList();

        public static User Add(User user)
        {
            user.Id = users.Count;
            users.Add(user);
            return user;
        }

        public static Post Add(Post post)
        {
            post.Id = posts.Count;
            posts.Add(post);
            return post;
        }

        public static Comment Add(Comment comment)
        {
            comment.Id = comments.Count;
            comments.Add(comment);
            return comment;
        }

        public static PostRating Add(PostRating postRating)
        {
            postRating.Id = ratings.Count;
            ratings.Add(postRating);
            return postRating;
        }

        public static void Initialize()
        {
            try
            {
                IInputReader usersReader = new FileReader("../../../db/users.db");
                IInputReader postsReader = new FileReader("../../../db/posts.db");
                IInputReader commentsReader = new FileReader("../../../db/comments.db");
                IInputReader ratingsReader = new FileReader("../../../db/ratings.db");

                List<string> unparsedUsers = new List<string>();
                List<string> unparsedPosts = new List<string>();
                List<string> unparsedComments = new List<string>();
                List<string> unparsedRatings = new List<string>();

                string line = null;

                while ((line = usersReader.ReadLine()) != null) unparsedUsers.Add(line);
                while ((line = postsReader.ReadLine()) != null) unparsedPosts.Add(line);
                while ((line = commentsReader.ReadLine()) != null) unparsedComments.Add(line);
                while ((line = ratingsReader.ReadLine()) != null) unparsedRatings.Add(line);

                InitializeUsers(unparsedUsers);
                InitializePosts(unparsedPosts);
                InitializeComments(unparsedComments);
                InitializeRatings(unparsedRatings);
            }
            catch(Exception)
            {
                return;
            }         
        }

        public static void Store()
        {
            if(!Directory.Exists("../../../db")) Directory.CreateDirectory("../../../db");
            if(!File.Exists("../../../db/users.db")) File.Create("../../../db/users.db");
            if(!File.Exists("../../../db/posts.db")) File.Create("../../../db/posts.db");
            if(!File.Exists("../../../db/comments.db")) File.Create("../../../db/comments.db");
            if(!File.Exists("../../../db/ratings.db")) File.Create("../../../db/ratings.db");

            ClearFiles();

            IOutputWriter usersWriter = new FileWriter("../../../db/users.db");
            IOutputWriter postsWriter = new FileWriter("../../../db/posts.db");
            IOutputWriter commentsWriter = new FileWriter("../../../db/comments.db");
            IOutputWriter ratingsWriter = new FileWriter("../../../db/ratings.db");

            users.ForEach(usersWriter.WriteLine);
            posts.ForEach(postsWriter.WriteLine);
            comments.ForEach(commentsWriter.WriteLine);
            ratings.ForEach(ratingsWriter.WriteLine);
        }
    }
}
