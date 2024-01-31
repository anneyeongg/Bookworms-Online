using Bookworms_Online.Model;
using Bookworms_Online.Services;
using Bookworms_Online.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookworms_Online.Pages
{
	public class RegisterModel : PageModel
	{
		private UserManager<IdentityUserStaff> userManager { get; }
		private SignInManager<IdentityUserStaff> signInManager { get; }
		[BindProperty]
		public Register RModel { get; set; }
		public RegisterModel(UserManager<IdentityUserStaff> userManager, SignInManager<IdentityUserStaff> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUserStaff()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
					FirstName = RModel.FirstName,
					LastName = RModel.LastName,
					CreditCardNo = EncryptionService.EncryptString(RModel.CreditCardNo),
					MobileNo = RModel.MobileNo,
					BillingAddress = RModel.BillingAddress,
					ShippingAddress = RModel.ShippingAddress,
				};

				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}

	}
}
