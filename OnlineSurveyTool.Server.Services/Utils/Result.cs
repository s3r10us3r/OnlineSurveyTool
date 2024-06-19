using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.Utils
{
    public class Result<T> : IResult<T>
    {
        public bool IsSuccess => _isSuccess;

        public string Message => _message ?? throw new InvalidOperationException("IsSuccess == true, this object has no Message!");

        public T Value => _value! ?? throw new InvalidOperationException("IsSuccess == false, this object has no Value!");

        private readonly bool _isSuccess;
        private readonly string? _message;
        private readonly T? _value;
        
        protected Result(bool isSuccess, string? message, T? value)
        {
            _isSuccess = isSuccess;
            _message = message;
            _value = value;
        }
        
        public static Result<T> Failure(string message) => new(false, message, default);

        public static Result<T> Success(T value) => new(true, default, value);
    }

    public class Result<T, TE> : Result<T>, IResult<T, TE> where TE : Enum
    {
        private readonly TE? _reason;

        private Result(bool isSuccess, string? message, T? value, TE? reason) : base(isSuccess, message, value)
        {
            _reason = reason;
        }

        public TE Reason => _reason ?? throw new InvalidOperationException("IsSuccess == false, this object has no Reason!");

        public static Result<T, TE> Failure(string message, TE reason) => new(false, message, default, reason);
        public new static Result<T, TE> Success(T value) => new(true, default, value, default);
    }
}
