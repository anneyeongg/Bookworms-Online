using Bookworms_Online.Model;
using Bookworms_Online.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookworms_Online.Pages
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

        private readonly UserManager<IdentityUserStaff> _userManager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<IdentityUserStaff> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public string DecryptedCreditCardNo { get; set; }
        //session management: displaying the session id
        public string SessionId { get; set; }   

        /*public void OnGet(string encryptedData)
		{
			if (!string.IsNullOrEmpty(encryptedData))
			{
				// Decrypt the encryptedData using the static method of EncryptionService
				DecryptedData = EncryptionService.DecryptString(encryptedData);
			}
		}*/


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // Decrypt the credit card number
                DecryptedCreditCardNo = EncryptionService.DecryptString(user.CreditCardNo);
            }
            //session management
            SessionId = HttpContext.Session.Id;

            return Page();
        }


    }
}
