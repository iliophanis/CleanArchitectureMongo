using System.Collections.Generic;
using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public static class SignInResultExtentions
    {
        public static Result ToApplicationResult(this SignInResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(new List<string> { "Login failed." });
        }
    }
}