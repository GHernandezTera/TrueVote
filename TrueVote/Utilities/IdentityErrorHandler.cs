using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TrueVote.Utilities
{
    public class IdentityErrorHandler
    {
        public static IActionResult HandleIdentityErrors(IdentityResult result)
        {
            if (result.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
            {
                return new BadRequestObjectResult("An user with that email already exists.");
            }

            if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
            {
                return new BadRequestObjectResult("The password provided is incorrect.");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new BadRequestObjectResult($"Failed operation: {errors}");
        }
    }
}
