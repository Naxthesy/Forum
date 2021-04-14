using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Changed { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public ICollection<Like> Likes { get; set; }
        public User user;
        public string UserId;
        public ICollection<SubComment> SubComments { get; set; }
        public Comment()
        {
            SubComments = new List<SubComment>();
            Likes = new List<Like>();
        }
        public bool IsAnswer { get; set; }

    }
}
