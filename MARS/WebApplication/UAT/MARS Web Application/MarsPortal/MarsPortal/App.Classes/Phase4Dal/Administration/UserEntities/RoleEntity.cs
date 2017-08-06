using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class RoleEntity
    {
        public int UserRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool Granted { get; set; }
        public bool Enabled { get; set; }

        public bool AdminRole { get; set; }

        public string ButtonText { get { return Granted ? "Revoke" : "Grant"; } }
    }
}