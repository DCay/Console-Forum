using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.Models
{
    public class Comment : BaseModel
    {
        public string Content { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }

        public Comment()
        {
        }

        public override string ToString()
        {
            return $"{this.Id},{this.Content},{this.User.Id},{this.Post.Id},{this.IsDeleted}";
        }
    }
}
