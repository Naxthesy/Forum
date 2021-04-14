using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Topic Topic { get; set; }
        public int TopicId { get; set; }
        public int Views { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<SavedQuestion> UsersSaved { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Question()
        {
            Comments = new List<Comment>();
            UsersSaved = new List<SavedQuestion>();
        }


    }
}