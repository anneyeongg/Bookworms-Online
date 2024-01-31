using Bookworms_Online.Model;
using Bookworms_Online.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication;
using Bookworms_Online.Services;

namespace Bookworms_Online.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<IdentityUserStaff> signInManager;
        private readonly UserManager<IdentityUserStaff> userManager;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUserStaff> signInManager, UserManager<IdentityUserStaff> userManager, ICaptchaValidator captchaValidator, IHttpContextAccessor httpContextAccessor, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _captchaValidator = captchaValidator;
            _httpContextAccessor = httpContextAccessor; // Inject IHttpContextAccessor
            _logger = logger;
        }

        public void OnGet()
        {
            CheckSessionTimeout();
        }

        public async Task<IActionResult> OnPostAsync(string captcha)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
            {
                ModelState.AddModelError("captcha", "reCAPTCHA validation failed.");
            }

            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var currentSessionId = HttpContext.Session.Id;

                // Check if there is an existing session ID stored in the user's session
                var existingSessionId = HttpContext.Session.GetString("SessionId");
                if (!string.IsNullOrEmpty(existingSessionId) && existingSessionId != currentSessionId)
                {
                    // Sign out the user
                    await signInManager.SignOutAsync();

                    // Redirect to the error page with a message about multiple logins detected
                    TempData["ErrorMessage"] = "Multiple logins detected. You have been signed out.";
                    return RedirectToPage("/ErrorPage");
                }

                // Check if the user has an active session (session ID) on the server
                if (ActiveSessions.IsSessionActive(userId, currentSessionId))
                {
                    // Redirect to the error page with a message
                    TempData["ErrorMessage"] = "You are already logged in another tab or device.";
                    return RedirectToPage("/ErrorPage");
                }

                HttpContext.Session.SetString("SessionId", currentSessionId);
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, true);

                if (identityResult.Succeeded)
                {
                    //session management
                    var user = await userManager.FindByEmailAsync(LModel.Email);
                    if (user != null)
                    {
                        HttpContext.Session.SetString("UserId", user.Id);
                        HttpContext.Session.SetString("UserName", user.UserName);
                        //anything else that needs to be set can be set here
                    }
                    //session timeout
                    HttpContext.Session.SetString("LastAccessed", DateTime.UtcNow.ToString());
                    CheckSessionTimeout();
                    return RedirectToPage("Index");
                }
                else if (identityResult.IsLockedOut)
                {
                    // Handle account lockout (e.g., display a message to the user)
                    ModelState.AddModelError("", "Account is locked. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }
            // If any of the conditions above are not met, return to the login page
            return Page();
        }





        private void CheckSessionTimeout()
        {
            // Retrieve user-specific session data
            var userId = HttpContext.Session.GetString("UserId");
            var currentSessionId = HttpContext.Session.Id;

            // Implement session timeout logic here
            var lastAccessedTime = GetLastAccessedTime(userId);

            var currentTime = DateTime.UtcNow;
            var sessionTimeoutMinutes = 30; // Set your session timeout duration in minutes

            if (currentTime.Subtract(lastAccessedTime).TotalMinutes >= sessionTimeoutMinutes)
            {
                // Session has genuinely timed out
                // Remove the active session from the list (map) of active sessions
                ActiveSessions.RemoveActiveSession(userId);

                // Clear the session data
                HttpContext.Session.Clear();

                // Sign out the user (if using Identity)
                signInManager.SignOutAsync();

                // Redirect to the login page
                Response.Redirect("/Login");
                return;
            }
        }
        private DateTime GetLastAccessedTime(string userId)
        {
            // Implement logic to retrieve the last accessed time for the user, e.g., from a database or other storage.
            // You can store the last accessed time when the user logs in or accesses a protected page.
            // This is just a placeholder method; replace it with actual logic.
            // Example:
            // var lastAccessedTimeString = RetrieveLastAccessedTimeFromDatabase(userId);
            // if (DateTime.TryParse(lastAccessedTimeString, out var lastAccessedTime))
            // {
            //     return lastAccessedTime;
            // }
            // return DateTime.UtcNow;

            // Default return value in case no other return statement is executed
            return DateTime.UtcNow;
        }


    }
}
