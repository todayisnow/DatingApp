using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi.Entities;

namespace WebUi.Interfaces
{
   public interface ITokenService
    {
       Task< string> CreateToken(AppUser User);

    }
}
