using Bookworms_Online.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookworms_Online.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<IdentityUserStaff> signInManager;
		public LogoutModel(SignInManager<IdentityUserStaff> signInManager)
		{
			this.signInManager = signInManager;
		}
		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostLogoutAsync()
		{
			await signInManager.SignOutAsync();
			//session management
			HttpContext.Session.Clear();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
