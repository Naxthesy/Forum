using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class SavedQuestion
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<Question> Questions { get; set; }
        public SavedQuestion()
        {
            this.Questions = new List<Question>();
        }

    }
}
