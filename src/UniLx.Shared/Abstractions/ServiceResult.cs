namespace UniLx.Shared.Abstractions
{
    public class ServiceResult<T>
        where T : class
    {
        public bool IsSuccess { get; }
        public bool IsError { get => IsSuccess is false && Error != Error.None; }   
        public Error Error { get; }
        public T? Content { get; }

        private ServiceResult(bool isSuccess, T? content, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Content = content;
        }

        public static ServiceResult<T> Success(T content) => new(true, content, Error.None);
        public static ServiceResult<T> Failure(Error error) => new(false, default, error);
    }

    public static class ServiceResult
    {
        public static ServiceResult<T> Success<T>(T content) where T : class => ServiceResult<T>.Success(content);
    }
}
