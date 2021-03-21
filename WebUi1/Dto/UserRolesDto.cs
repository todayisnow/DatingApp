using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUi1.Dto
{
    public class UserRolesDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
