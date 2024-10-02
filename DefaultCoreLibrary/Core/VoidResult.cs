namespace DefaultCoreLibrary.Core;

public class VoidResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }

    public Error Error => Errors.FirstOrDefault() ?? Error.None;

    private VoidResult(bool isSuccess, IEnumerable<Error> errors)
    {
        if (isSuccess && errors.Any(e => e != Error.None))
        {
            throw new ArgumentException("Success result must not have any errors.", nameof(errors));
        }
        if (!isSuccess && !errors.Any())
        {
            throw new ArgumentException("Failure result must have at least one error.", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = errors.ToList();
    }

    public static VoidResult Success()
    {
        return new VoidResult(true, Enumerable.Empty<Error>());
    }

    public static VoidResult Failure(params Error[] errors)
    {
        return new VoidResult(false, errors);
    }

    public static VoidResult Failure(IEnumerable<Error> errors)
    {
        return new VoidResult(false, errors);
    }

    public static implicit operator VoidResult(Error error) => Failure(error);
    public static implicit operator VoidResult(bool result) => result ? Success() : Failure(Error.None);
    public static implicit operator VoidResult(Exception ex) => Failure(new Error(ex.GetType().Name, ex.Message));

    // Helper method to convert Exception to Error
    private static Error ExceptionToError(Exception ex) => new Error(ex.GetType().Name, ex.Message);
}
