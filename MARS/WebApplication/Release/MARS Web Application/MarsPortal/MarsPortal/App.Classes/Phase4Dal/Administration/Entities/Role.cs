using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    public class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public Role(string roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
    }
}