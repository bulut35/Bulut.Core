﻿namespace BulutBusiness.Core.CrossCuttingConcerns.Types;

public class BusinessException : System.Exception
{
    public BusinessException() { }

    public BusinessException(string? message)
        : base(message) { }

    public BusinessException(string? message, System.Exception? innerException)
        : base(message, innerException) { }
}
