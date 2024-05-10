﻿namespace DefaultCoreLibrary.Core;

public sealed record class Error(string Code, string? Description = null)
{
    public static readonly Error None = new(string.Empty);
}
