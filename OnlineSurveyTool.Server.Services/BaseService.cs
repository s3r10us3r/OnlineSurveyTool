using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Interfaces;

namespace OnlineSurveyTool.Server.Services
{
    public class BaseService<E, R> : IService<E, R> where E : EntityBase, new() where R : IRepo<E>
    {
        private R _repo;
        private ILogger<BaseService<E, R>> _logger;

        protected R Repo => _repo;
        protected ILogger<BaseService<E, R>> Logger => _logger;

        public BaseService(R repo, ILogger<BaseService<E, R>> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
