namespace DeveloperHomeworkAssignment.API.Utilities
{
    public class Result<T, TError> : Result<TError>
    {
        private T? _value;

        public T Value
        {
            get => IsSuccess ? _value! : throw new InvalidOperationException("Result was not successful!");
            private set => _value = value;
        }

        private Result(bool isSuccess, T? value, TError? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T, TError> Success(T value)
        {
            return new Result<T, TError>(true, value, default);
        }

        public static new Result<T, TError> Failure(TError error)
        {
            return new Result<T, TError>(false, default, error);
        }
    }

    public class Result<TError>
    {
        private TError? _error;
        public bool IsSuccess { get; private set; }

        public TError Error
        {
            get => !IsSuccess ? _error! : throw new InvalidOperationException("Result was successful!");
            private set => _error = value;
        }

        protected Result(bool isSuccess, TError? error)
        {
            IsSuccess = isSuccess;
            _error = error;
        }

        public static Result<TError> Success()
        {
            return new Result<TError>(true, default);
        }

        public static Result<TError> Failure(TError error)
        {
            return new Result<TError>(false, error);
        }
    }
}