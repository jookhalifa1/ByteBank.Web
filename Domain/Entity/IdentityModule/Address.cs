using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.IdentityModule
{
    [Owned]
     public class Address
    {
        public string street { get; set;}


        public string city { get; set; }


        public string Country { get; set; }

    }
}
    