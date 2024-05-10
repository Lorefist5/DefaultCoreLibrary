namespace DefaultCoreLibrary.Core;

public class Result<T>
{
    private readonly T _value;
    public T Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access Value on a failed result.");
            }
            return _value;
        }
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    // Private constructor ensures instances can only be created through Success or Failure methods.
    private Result(bool isSuccess, Error error, T? value = default)
    {
        if (isSuccess && error != Error.None)
        {
            throw new ArgumentException("Success result must not have an error.", nameof(error));
        }
        if (isSuccess && value == null)
        {
            throw new ArgumentException("Success result must have a value.", nameof(error));
        }
        if (!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Failure result must have an error.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        _value = value;
    }

    public static Result<T> Success(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Success result must have a value.");
        }
        return new Result<T>(true, Error.None, value);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(false, error);
    }

    public static implicit operator Result<T>(Error error) => Result<T>.Failure(error);
    public static implicit operator Result<T>(T result) => Result<T>.Success(result);
}
