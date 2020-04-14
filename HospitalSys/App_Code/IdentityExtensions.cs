using HospitalSys.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HospitalSys.App_Code
{
    public static class IPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static async Task<string> GetUserDisplayName(this ClaimsPrincipal principal, UserManager<ApplicationUser> userManager)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            var model = await userManager.FindByIdAsync(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return model.FirstName + " " + model.LastName;
        }
    }

}
