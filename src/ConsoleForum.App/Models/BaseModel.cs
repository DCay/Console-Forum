using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
