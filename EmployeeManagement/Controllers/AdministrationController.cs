using EntityLayer.DTOs;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.roleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles", "Administration");

                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, errors.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> RoleUpdate(string roleId)
        {
    
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ModelState.AddModelError(String.Empty, "There is no role");
                return NotFound();
            }

            RoleUpdateViewModel model = new()
            {
                roleId = role.Id,
                roleName = role.Name,
                usersInRole = new List<string>()
            };
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, model.roleName))
                {
                    model.usersInRole.Add(user.Email);
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.roleId);
                role.Name = model.roleName;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles", "Administration");
                }
                foreach (var Errors in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, Errors.Description);
                }

            }
            return View(model);
        }
        public async Task<IActionResult> RoleDelete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var delete = await _roleManager.DeleteAsync(role);
                if (delete.Succeeded)
                {
                    return RedirectToAction("GetAllRoles", "Administration");
                }
                else
                {
                    foreach (var Errors in delete.Errors)
                    {
                        ModelState.AddModelError(String.Empty, Errors.Description);
                    }
                }
            }
            return RedirectToAction("GetAllRoles", "Administration");
        }
        [HttpGet]
        public async Task<IActionResult> RoleUpdateOrDeleteUsersFromRoles(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var users = await _userManager.Users.ToListAsync();
            List<RoleUserInRoleViewModel> model = new();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Add(new RoleUserInRoleViewModel() { isSelected = true, userName = user.UserName, userId = user.Id });
                }
                else
                {
                    model.Add(new RoleUserInRoleViewModel() { isSelected = false, userName = user.UserName, userId = user.Id });
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdateOrDeleteUsersFromRoles(List<RoleUserInRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            foreach (var userModel in model)
            {
                var User = await _userManager.FindByIdAsync(userModel.userId);
                if (userModel.isSelected && !(await _userManager.IsInRoleAsync(User, roleId)))
                {
                    await _userManager.AddToRoleAsync(User, role.Name);
                }
                else if (!userModel.isSelected)
                {
                    await _userManager.RemoveFromRoleAsync(User, role.Name);
                }
            }
            return RedirectToAction("RoleUpdate", "Administration", new { roleId = roleId });
        }
        [HttpGet]
        public async Task<IActionResult> UserUpdate(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            UserUpdateViewModel userModel = new()
            {
                userName = user.UserName,
                userId = user.Id,
                userEmail = user.Email,
                userCity = user.city,
                roles = userRoles.ToList(),
                claims = userClaims.Select(x => x.Type + " : " + x.Value.ToString()).ToList(),
            };


            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserUpdate(UserUpdateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);

            user.Id = model.userId;
            user.Email = model.userEmail;
            user.UserName = model.userName;
            user.city = model.userCity;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("GetAllUsers", "Administration");
            else
            {
                foreach (var Errors in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, errorMessage: Errors.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> UserUpdateOrDeleteRolesFromUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();
            List<UserRoleInUserViewModel> model = new();

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Add(new UserRoleInUserViewModel() { isSelected = true, roleId = user.Id, roleName = role.Name });
                }
                else
                {
                    model.Add(new UserRoleInUserViewModel() { isSelected = false, roleId = user.Id, roleName = role.Name });
                }
            }

            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> UserUpdateOrDeleteRolesFromUser(List<UserRoleInUserViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            foreach (var role in model)
            {
                if (role.isSelected && !await _userManager.IsInRoleAsync(user, role.roleName) && role.roleName!="Super Admin")
                {
                    await _userManager.AddToRoleAsync(user, role.roleName);
                }
                else if (!role.isSelected && await _userManager.IsInRoleAsync(user,role.roleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.roleName);
                }
            }
            return RedirectToAction("UserUpdate", "Administration", new { userId = userId });
        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> UserDelete(string userId)
        {
            await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userId));
            return RedirectToAction("GetAllUsers", "Administration", new { userId = userId });

        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var existingClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                userId = userId,
            };
            foreach (Claim claim in ClaimStore.AllClaims)
            {
                UserClaim userClaim = new()
                {
                    claimType = claim.Type
                };
                if (existingClaims.Any(x => x.Type == claim.Type && x.Value == "True"))
                {
                    userClaim.isSelected = true;
                }
                else
                {
                    userClaim.isSelected = false;
                }
                model.claims.Add(userClaim);
            }


            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);
            var exist = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user,exist);
            

            await _userManager.AddClaimsAsync(user, model.claims.Select(x => new Claim(x.claimType, x.isSelected ? "True" : "False")));
            


     

            return RedirectToAction("UserUpdate", "Administration", new { userId = model.userId });
        }
    }
}
