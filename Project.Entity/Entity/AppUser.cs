using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Entity
{
    public class AppUser:IdentityUser<int>
    {
        public string Address { get; set; }

    }
}
