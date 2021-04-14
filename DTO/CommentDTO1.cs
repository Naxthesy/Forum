using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DTO
{
    public class CommentDTO1
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Changed { get; set; }
        public int Likes { get; set; }
        public UserDTO1 user;
        public ICollection<SubCommentDTO1> SubComments { get; set; }
        public bool IsAnswer { get; set; }
    }
}
