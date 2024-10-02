namespace DefaultCoreLibrary.Core;

public class VoidResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private VoidResult(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new ArgumentException("Success result must not have an error.", nameof(error));
        }
        if (!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Failure result must have an error.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static VoidResult Success()
    {
        return new VoidResult(true, Error.None);
    }

    public static VoidResult Failure(Error error)
    {
        return new VoidResult(false, error);
    }

    public static implicit operator VoidResult(Error error) => Failure(error);
    public static implicit operator VoidResult(bool result) => result ? Success() : Failure(Error.None);
    public static implicit operator VoidResult(Exception ex) => Failure(new Error(ex.GetType().Name, ex.Message));

    // Helper method to convert Exception to Error
    private static Error ExceptionToError(Exception ex) => new Error(ex.GetType().Name, ex.Message);
}
