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
    public IReadOnlyList<Error> Errors { get; }

    public Error Error => Errors.FirstOrDefault() ?? Error.None;

    // Private constructor ensures instances can only be created through Success or Failure methods.
    private Result(bool isSuccess, IEnumerable<Error> errors, T? value = default)
    {
        if (isSuccess && errors.Any(e => e != Error.None))
        {
            throw new ArgumentException("Success result must not have any errors.", nameof(errors));
        }
        if (isSuccess && value == null)
        {
            throw new ArgumentException("Success result must have a value.", nameof(errors));
        }
        if (!isSuccess && !errors.Any())
        {
            throw new ArgumentException("Failure result must have at least one error.", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = errors.ToList();
        _value = value;
    }

    public static Result<T> Success(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Success result must have a value.");
        }
        return new Result<T>(true, Enumerable.Empty<Error>(), value);
    }

    public static Result<T> Failure(params Error[] errors)
    {
        return new Result<T>(false, errors);
    }

    public static Result<T> Failure(IEnumerable<Error> errors)
    {
        return new Result<T>(false, errors);
    }

    public static implicit operator Result<T>(Error error) => Failure(error);
    public static implicit operator Result<T>(T result) => Success(result);
    public static implicit operator Result<T>(List<Error> errors) => Failure(errors);
}