using EntityLayer.DTOs;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterWievModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.email,
                    UserName = model.email,
                    city = model.city
                };
                var result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("GetAllUsers", "Administration");
                    }
                    else
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    }
                    return View("_AfterRegister");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, item.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                ViewBag.Congrats = "Your Account is";
                return View("_AfterRegister");
            }
            else
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                }
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError(String.Empty, "Invalid Attempt");
            }
            var model = new ResetPasswordViewModel()
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var reset = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (reset.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirm");
                    }
                    else
                    {
                        foreach (var Errors in reset.Errors)
                        {
                            ModelState.AddModelError(String.Empty, Errors.Description);
                        }
                    }

                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var state = await _userManager.HasPasswordAsync(user);
            if (!state)
            {
                return RedirectToAction("SetPassword", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var changePassword = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
            if (changePassword.Succeeded)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                foreach (var Errors in changePassword.Errors)
                {
                    ModelState.AddModelError(String.Empty, Errors.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var set = await _userManager.AddPasswordAsync(user, model.setPassword);
            if (set.Succeeded)
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
            else
            {
                foreach (var Errors in set.Errors)
                {
                    ModelState.AddModelError(String.Empty, Errors.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var loginViewModel = new LoginViewModel()
            {
                externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginViewModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("Login", loginViewModel);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
    
            ApplicationUser user = null;

            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(String.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: loginViewModel.rememberMe, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        };
                        await _userManager.CreateAsync(user);

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    return View("_AfterRegister");
                }
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            model.externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                if (user != null && user.EmailConfirmed == false && (await _userManager.CheckPasswordAsync(user, model.password)))
                {
                    ModelState.AddModelError(String.Empty, "Email is not confirmed");
                    return View(model);
                }
                if (user != null)
                {
                    var signInProcess = await _signInManager.PasswordSignInAsync(user, model.password, model.rememberMe, true);
                    if (signInProcess.Succeeded)
                    {
                        if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("EmployeeList", "Employee");
                    }
                    if (signInProcess.IsLockedOut)
                    {
                        return View("_AccountLocked");
                    }
                }

                ModelState.AddModelError(String.Empty, "Invalid user name or password");
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("EmployeeList", "Employee");
        }
    }
}
