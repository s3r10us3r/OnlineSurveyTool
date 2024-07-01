namespace OnlineSurveyTool.Server.Services.Utils.Interfaces
{
    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
        string Message { get; }
    }
    
    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }

    public interface IResult<out T, out TE> : IResult<T> where TE : Enum
    {
        TE Reason { get; }
    }
}
