using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Extensions
{
    public static class DependencyInjection
    {
        public static void AddRepos(this IServiceCollection services)
        {
            services.AddDbContext<OstDbContext>();
            services.AddScoped<IAnswerOptionRepo, AnswerOptionRepo>();
            services.AddScoped<IAnswerRepo, AnswerRepo>();
            services.AddScoped<IChoiceOptionRepo, ChoiceOptionRepo>();
            services.AddScoped<IQuestionRepo, QuestionRepo>();
            services.AddScoped<ISurveyRepo, SurveyRepo>();
            services.AddScoped<ISurveyResultRepo, SurveyResultRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IAnswerNumericalRepo, AnswerNumericalRepo>();
            services.AddScoped<IAnswerTextualRepo, AnswerTextualRepo>();
            services.AddScoped<IAnswerSingleChoiceRepo, AnswerSingleChoiceRepo>();
            services.AddScoped<IAnswerMultipleChoiceRepo, AnswerMultipleChoiceRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
