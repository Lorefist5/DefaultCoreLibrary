using System.Net;

namespace DefaultCoreLibrary.Core;

public sealed record class HttpResultError(string Code, string? Description = null, HttpStatusCode StatusCode = HttpStatusCode.InternalServerError)
    : Error(Code, Description)
{
    public HttpStatusCode StatusCode { get; } = StatusCode;
}