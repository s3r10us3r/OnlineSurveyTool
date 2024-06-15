using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Extensions
{
    public static class DependencyInjection
    {
        public static void AddRepos(this IServiceCollection services)
        {
            services.AddDbContext<OSTDbContext>();
            services.AddScoped<IAnswerOptionRepo, AnswerOptionRepo>();
            services.AddScoped<IAnswerRepo, AnswerRepo>();
            services.AddScoped<IChoiceOptionRepo, ChoiceOptionRepo>();
            services.AddScoped<IQuestionRepo, QuestionRepo>();
            services.AddScoped<ISurveyRepo, SurveyRepo>();
            services.AddScoped<ISurveyResultRepo, SurveyResultRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
        }
    }
}
