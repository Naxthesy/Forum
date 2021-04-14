
using Forum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<LikeSubComment> LikesSubComments { get; set; }
        public DbSet<SavedQuestion> SavedQuestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Question>()
                .HasOne(p => p.User)
                .WithMany(t => t.Questions)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<Question>()
                .HasOne(p => p.Topic)
                .WithMany(t => t.Questions)
                .HasForeignKey(p => p.TopicId);
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Question)
                .WithMany(t => t.Comments)
                .HasForeignKey(p => p.QuestionId);
            modelBuilder.Entity<SubComment>()
                .HasOne(p => p.Comment)
                .WithMany(t => t.SubComments)
                .HasForeignKey(p => p.CommentId);

            modelBuilder.Entity<Like>()
                .HasOne(p => p.User)
                .WithMany(t => t.Likes)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<Like>()
                .HasOne(p => p.Comment)
                .WithMany(t => t.Likes)
                .HasForeignKey(p => p.CommentId);
            modelBuilder.Entity<LikeSubComment>()
                .HasOne(p => p.SubComment)
                .WithMany(t => t.Likes)
                .HasForeignKey(p => p.SubCommentId);
            modelBuilder.Entity<SubComment>()
                .HasOne(p => p.User)
                .WithMany(t => t.SubComments)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Question>()
                .HasMany(p => p.UsersSaved)
                .WithMany(p => p.Questions);
            modelBuilder.Entity<SubComment>()
                .HasMany(p => p.SubComments)
                .WithOne(p => p.subComment)
                .HasForeignKey(p => p.subCommentId);
            modelBuilder.Entity<User>()
                .HasOne(p => p.SavedQuestions)
                .WithOne(p => p.User);
            modelBuilder.Entity<User>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.user);
        }
    }
}
