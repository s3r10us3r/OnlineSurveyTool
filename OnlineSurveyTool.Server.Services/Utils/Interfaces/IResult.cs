namespace OnlineSurveyTool.Server.Services.Utils.Interfaces
{
    public interface IResult<T>
    {
        bool IsSuccess { get; }
        string Message { get; }
        T Value { get; }
    }

    public interface IResult<T, TE> : IResult<T> where TE : Enum
    {
        TE Reason { get; }
    }
}
