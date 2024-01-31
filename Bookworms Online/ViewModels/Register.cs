using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bookworms_Online.ViewModels
{
	public class Register
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?)$", ErrorMessage = "Invalid credit card format")] // Example for Visa

		public string CreditCardNo { get; set; }
		[Required]
		[RegularExpression(@"^\+[1-9]{1}[0-9]{3,14}$", ErrorMessage = "Invalid phone number format")]
		public string MobileNo { get; set; }
		[Required]
		public string BillingAddress { get; set; }
		[Required]
		public string ShippingAddress { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email format")]

		public string Email { get; set; }
		[Required]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Password must contain a combination of lowercase, uppercase, numbers, and special characters.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password not match")]
		public string ConfirmPassword { get; set; }
	}
}
