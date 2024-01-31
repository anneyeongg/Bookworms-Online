using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace Bookworms_Online.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly HtmlEncoder _htmlEncoder;

        public PrivacyModel(ILogger<PrivacyModel> logger, HtmlEncoder htmlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
        }

        public void OnGet()
        {
            string userInput = "<script>alert('Hello, World!');</script>";
            string encodedUserInput = _htmlEncoder.Encode(userInput);

            // Now you can use 'encodedUserInput' and save it to the database
        }
    }
}
