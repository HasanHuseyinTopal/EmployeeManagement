using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        IHttpContextAccessor _httpContextAccessor;
        public CanEditOnlyOtherAdminRolesAndClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            
            string loggedInAdmin = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEdited = _httpContextAccessor.HttpContext.Request.Query["userId"];
            adminIdBeingEdited = adminIdBeingEdited ?? "";


            if (context.User.IsInRole("Admin") && context.User.HasClaim(x => x.Type == "Edit Role" && x.Value.ToString() == "True") && adminIdBeingEdited.ToLower() != loggedInAdmin.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
