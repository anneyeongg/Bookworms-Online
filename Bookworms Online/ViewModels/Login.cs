﻿using System.ComponentModel.DataAnnotations;

namespace Bookworms_Online.ViewModels
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
		[RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email format")]
		public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
