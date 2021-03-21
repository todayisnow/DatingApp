using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi1.Entities;

namespace WebUi1.Interfaces
{
   public interface ITokenService
    {
       Task< string> CreateToken(AppUser User);

    }
}
