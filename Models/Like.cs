using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Like
    {
        public int id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

        public Comment Comment { get; set; }
        public int CommentId { get; set; }
    }
}
