using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.Models
{
    public class Post : BaseModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public User User { get; set; }

        public List<Comment> Comments { get; set; }

        public List<PostRating> Ratings { get; set; }

        public Post()
        {
            this.Comments = new List<Comment>();
            this.Ratings = new List<PostRating>();
        }

        public override string ToString()
        {
            return $"{this.Id},{this.Title},{this.Content},{this.User.Id},{this.IsDeleted}";
        }
    }
}
