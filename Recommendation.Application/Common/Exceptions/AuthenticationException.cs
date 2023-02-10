﻿using Microsoft.AspNetCore.Identity;

namespace Recommendation.Application.Common.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException(string message)
        : base(message)
    {
    }
}