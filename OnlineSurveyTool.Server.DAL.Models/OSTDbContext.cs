using Microsoft.EntityFrameworkCore;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class OSTDbContext : DbContext
    {
        public OSTDbContext(DbContextOptions options) : base(options)
        {
        }

        public OSTDbContext() : base()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ChoiceOption> ChoiceOptions { get; set; }
        public virtual DbSet<SurveyResult> SurveyResults { get; set; }
        public virtual DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasOne(e => e.SurveyResult)
                .WithMany(e => e.Answers)
                .HasForeignKey(e => e.SurveyResultId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Answer>()
                .HasOne(e => e.Question)
                .WithMany(e => e.Answers)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Answer>()
                .HasOne(e => e.SingleChoiceOption)
                .WithMany(e => e.Answers)
                .HasForeignKey(e => e.SingleChoiceOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(e => e.Answer)
                .WithMany(e => e.AnswerOptions)
                .HasForeignKey(e => e.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=OnlineSurveyTool";
                builder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }
    }

}
