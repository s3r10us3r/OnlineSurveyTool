using Microsoft.Extensions.Logging;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class LoggerMock<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel > LogLevel.Information)
        {
            Console.WriteLine(formatter(state, exception));
        }
    }
}