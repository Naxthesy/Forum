using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DTO
{
    public class SubCommentDTO1
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public UserDTO1 User { get; set; }
        public DateTime CreateDate { get; set; }
        public int Likes { get; set; }
    }
}
