using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bookworms_Online.Model
{
	public class IdentityUserStaff: IdentityUser
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string CreditCardNo { get; set; }
		[Required]
		public string MobileNo { get; set; }
		[Required]
		public string BillingAddress { get; set; }
		[Required]
		public string ShippingAddress { get; set; }	

	}
}
