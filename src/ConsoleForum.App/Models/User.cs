using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsEnabled { get; set; } = true;

        public UserRole Role { get; set; }

        public List<Post> Posts { get; set; }

        public List<Comment> Comments { get; set; }

        public List<PostRating> Ratings { get; set; }

        public User()
        {
            this.Posts = new List<Post>();
            this.Comments = new List<Comment>();
            this.Ratings = new List<PostRating>();
        }

        public override string ToString()
        {
            return $"{this.Id},{this.Username},{this.Password},{this.IsEnabled},{this.Role.ToString()},{this.IsDeleted}";
        }
    }
}
