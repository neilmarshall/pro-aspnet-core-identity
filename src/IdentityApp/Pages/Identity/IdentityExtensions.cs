﻿using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityApp.Pages.Identity
{
    public static class IdentityExtensions
    {
        public static bool Process(this IdentityResult result, ModelStateDictionary modelState)
        {
            foreach (var err in result.Errors ?? Enumerable.Empty<IdentityError>())
            {
                modelState.AddModelError(string.Empty, err.Description);
            }

            return result.Succeeded;
        }
    }
}
