using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.AuthorizationOptions
{
    public  class AuthorizeOpt
    {
        public static bool authorizeAccess (AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") && (context.User.HasClaim(claim=> claim.Type=="Edit Role" && claim.Value=="True") || context.User.IsInRole("Super Admin"));
        }
    }
}
