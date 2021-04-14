using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class LikeSubComment
    {
        public int Id { get; set; }
        public SubComment SubComment { get; set; }
        public int SubCommentId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

    }
}
