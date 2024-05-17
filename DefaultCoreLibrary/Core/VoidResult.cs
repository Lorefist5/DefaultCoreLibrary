namespace DefaultCoreLibrary.Core;

public class VoidResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

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
    }
    public static VoidResult Success()
    {
        return new VoidResult(true, Error.None);
    }
    public static VoidResult Failure(Error error)
    {
        return new VoidResult(false, error);
    }
    public static implicit operator VoidResult(Error error) => VoidResult.Failure(error);
    public static implicit operator VoidResult(bool result) => VoidResult.Success();
}
