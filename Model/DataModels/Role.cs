using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleValue RoleValue { get; set; }
        public Role(string name, RoleValue roleValue) : base(name)
        {
            RoleValue = roleValue;
        }

        public Role()
        {

        }
    }
}
