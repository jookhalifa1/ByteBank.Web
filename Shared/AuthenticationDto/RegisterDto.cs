using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AuthenticationDto
{
     public  class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;

        public string Name { get; set; }= default!;

        [Phone]
        public string Phone { get; set; } = default!;


        public AddressDto address { get; set; } = default!;
        

        public string Password { get; set; } = default!;
    }
}
