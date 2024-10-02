using System.Net;

namespace DefaultCoreLibrary.Core;


public class HttpResult<T>
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
    public IReadOnlyList<HttpResultError> Errors { get; }
    public HttpStatusCode StatusCode { get; }

    public HttpResultError Error => Errors.FirstOrDefault() ?? new HttpResultError(string.Empty);

    // Private constructor ensures instances can only be created through Success or Failure methods.
    private HttpResult(bool isSuccess, HttpStatusCode statusCode, IEnumerable<HttpResultError> errors, T? value = default)
    {
        if (isSuccess && errors.Any(e => e != new HttpResultError(string.Empty)))
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
        StatusCode = statusCode;
        Errors = errors.ToList();
        _value = value;
    }

    public static HttpResult<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Success result must have a value.");
        }
        return new HttpResult<T>(true, statusCode, Enumerable.Empty<HttpResultError>(), value);
    }

    public static HttpResult<T> Failure(HttpStatusCode statusCode, params HttpResultError[] errors)
    {
        return new HttpResult<T>(false, statusCode, errors);
    }

    public static HttpResult<T> Failure(HttpStatusCode statusCode, IEnumerable<HttpResultError> errors)
    {
        return new HttpResult<T>(false, statusCode, errors);
    }

    public static implicit operator HttpResult<T>(HttpResultError error) => Failure(HttpStatusCode.InternalServerError, error);
    public static implicit operator HttpResult<T>(T result) => Success(result);
    public static implicit operator HttpResult<T>(Exception ex) => Failure(HttpStatusCode.InternalServerError, new HttpResultError(ex.GetType().Name, ex.Message));
    public static implicit operator HttpResult<T>(List<HttpResultError> errors) => Failure(HttpStatusCode.InternalServerError, errors);


    // Helper method to convert Exception to HttpResultError
    private static HttpResultError ExceptionToError(Exception ex) => new HttpResultError(ex.GetType().Name, ex.Message);

    // Method to handle different status codes
    public HttpResult<T> When(HttpStatusCode statusCode, Action<HttpResultError> action)
    {
        var error = Errors.FirstOrDefault(e => e.StatusCode == statusCode);
        if (error != null)
        {
            action(error);
        }
        return this;
    }
}