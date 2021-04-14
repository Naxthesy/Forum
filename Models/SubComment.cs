using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class SubComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
        public SubComment subComment { get; set; }
        public int? subCommentId { get; set; }
        public Comment Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public int CommentId { get; set; }
        public ICollection<LikeSubComment> Likes { get; set; }
        public SubComment()
        {
            SubComments = new List<SubComment>();
            Likes = new List<LikeSubComment>();
        }
    }
}
