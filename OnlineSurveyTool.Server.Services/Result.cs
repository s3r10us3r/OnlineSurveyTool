using OnlineSurveyTool.Server.Services.Interfaces;

namespace OnlineSurveyTool.Server.Services
{
    public sealed class Result<T> : IResult<T>
    {
        public bool IsSuccess => _isSuccess;

        public string Message
        {
           get
           {
                if (IsSuccess)
                    throw new InvalidOperationException("IsSuccess == true, this object has no Message!");
                return _message;
           }
        }

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("IsSuccess == false, this object has no Value!");
                return _value!;
            }
        }

        protected bool _isSuccess;
        protected string _message;
        protected T? _value;

        protected Result(bool isSuccess, string message, T? value)
        {
            _isSuccess = isSuccess;
            _message = message;
            _value = value;
        }

        public static Result<T> Failure(string message) => new Result<T>(false, message, default);

        public static Result<T> Success(T value) => new Result<T>(true, default, value);
    }
}
