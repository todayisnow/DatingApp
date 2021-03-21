using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebUi1.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}