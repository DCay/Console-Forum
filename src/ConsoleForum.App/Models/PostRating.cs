using System;
using System.Collections.Generic;
using System.Text;
using ConsoleForum.App.Models.Enums;

namespace ConsoleForum.App.Models
{
    public class PostRating : BaseModel
    {
        public User User { get; set; }

        public Post Post { get; set; }

        public bool IsPositive { get; set; }

        public bool IsNegative { get; set; }

        public void Toggle()
        {
            this.IsPositive = !this.IsPositive;
            this.IsNegative = !this.IsNegative;
        }

        public PostRating(PostRatingChoice ratingChoice)
        {
            this.IsPositive = ratingChoice == PostRatingChoice.Positive;
            this.IsNegative = ratingChoice == PostRatingChoice.Negative;
        }

        public override string ToString()
        {
            return $"{this.Id},{(this.IsPositive ? PostRatingChoice.Positive.ToString() : PostRatingChoice.Negative.ToString())},{this.User.Id},{this.Post.Id},{this.IsDeleted}";
        }
    }
}
