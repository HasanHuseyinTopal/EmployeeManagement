using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeManagement.Security
{
    public class CantDeleteHimSelfHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        IHttpContextAccessor _httpContextAccessor;
        public CantDeleteHimSelfHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor=httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            string loggedInAdmin = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEdited = _httpContextAccessor.HttpContext.Request.Query["userId"];
            adminIdBeingEdited = adminIdBeingEdited ?? "";

            if (adminIdBeingEdited != loggedInAdmin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
