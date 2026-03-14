using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IEmailServices
    {
        public Task SendEmailAsync(string email, string Object, string message);
         
         
    }
}
