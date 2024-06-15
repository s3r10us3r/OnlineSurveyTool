﻿using Microsoft.Extensions.Logging;

namespace OnlineSurveyTool.Server.Services
{
    public class BaseService
    {
        private ILogger<BaseService> _logger;

        protected ILogger<BaseService> Logger => _logger;

        public BaseService(ILogger<BaseService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
