using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        public override string Id { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<LikeSubComment> LikesSubComment { get; set; }
        [ForeignKey("SavedQuestionsId")]
        public SavedQuestion SavedQuestions { get; set; }
        public User()
        {
            Questions = new List<Question>();
            Comments = new List<Comment>();
            SubComments = new List<SubComment>();
            Likes = new List<Like>();
            LikesSubComment = new List<LikeSubComment>();
        }
    }
}
