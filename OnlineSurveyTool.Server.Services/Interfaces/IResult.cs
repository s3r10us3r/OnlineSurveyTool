namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IResult<T>
    {
        bool IsSuccess { get; }
        string Message { get; }
        T Value { get; }
    }
}
