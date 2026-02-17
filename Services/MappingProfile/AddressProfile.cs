using AutoMapper;
using Domain.Entity.IdentityModule;
using Shared.AuthenticationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class AddressProfile:Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>();
        }
    }
}
