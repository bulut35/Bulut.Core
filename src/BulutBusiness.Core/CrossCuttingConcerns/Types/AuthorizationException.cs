﻿namespace BulutBusiness.Core.CrossCuttingConcerns.Types;

public class AuthorizationException : System.Exception
{
    public AuthorizationException() { }

    public AuthorizationException(string? message)
        : base(message) { }

    public AuthorizationException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}
