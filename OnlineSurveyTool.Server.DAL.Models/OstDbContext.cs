using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class OstDbContext : DbContext
    {
        public OstDbContext(DbContextOptions options) : base(options)
        {
        }

        public OstDbContext() : base()
        {
        }

        
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ChoiceOption> ChoiceOptions { get; set; }
        public virtual DbSet<SurveyResult> SurveyResults { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerOption>()
                .HasOne(e => e.ChoiceOption)
                .WithMany(e => e.AnswerOptions)
                .HasForeignKey(e => e.ChoiceOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SurveyResult>()
                .HasOne(e => e.Survey)
                .WithMany(e => e.Results)
                .HasForeignKey(e => e.SurveyId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ChoiceOption>()
                .HasOne(e => e.Question)
                .WithMany(e => e.ChoiceOptions)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Question>()
                .HasOne(e => e.Survey)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.Surveys)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(ao => ao.Answer)
                .WithMany(a => a.AnswerOptions)
                .HasForeignKey(ao => new { ao.ResultId, ao.Number })
                .OnDelete(DeleteBehavior.Cascade);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (builder.IsConfigured) return;
            const string connectionString = @"Server=localhost;Database=OnlineSurveyTool;User=server;Password=serverPassword";
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

}
