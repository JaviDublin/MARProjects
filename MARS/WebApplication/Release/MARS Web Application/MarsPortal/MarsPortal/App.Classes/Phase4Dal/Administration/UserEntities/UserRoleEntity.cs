using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class UserRoleEntity
    {
        public int MarsUserRoleId { get; set; }

        public string ButtonText { get; set; }
        public string RoleClickCommand { get { return StaticRoleClickCommand; } }

        public const string StaticRoleClickCommand = "RoleClickCommand";
    }
}