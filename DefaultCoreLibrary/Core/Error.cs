using System.Net;

namespace DefaultCoreLibrary.Core;

public record class Error(string Code, string? Description = null)
{
    public static readonly Error None = new(string.Empty);
    
}
