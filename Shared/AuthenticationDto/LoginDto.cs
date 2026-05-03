using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AuthenticationDto
{
     public  class LoginDto
    {
        [EmailAddress(ErrorMessage ="Invalid Email ")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
ErrorMessage = "Password must be at least 8 chars, include uppercase, lowercase, number and special character Eg (P@ssw0rd)")]

        public string Password { get; set; }
    }
}
