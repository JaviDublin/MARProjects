using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Bll.Administration;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class UserEntity
    {
        public int MarsUserId { get; set; }
        public string RacfId { get; set; }
        public string UserName { get; set; }

        public string JoinedRoleIds { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyCountryId { get; set; }
        public string CompanyType { get; set; }
        public int CompanyTypeId { get; set; }
        public string CompanyCountryName { get; set; }
        public string CompanyName { get; set; }

        public string EmployeeId { get; set; }
        
        public string ShowPriviliges { get { return ShowPriviligesForUserCommand; } }

        public string EditCommand { get { return EditUserCommand; } }
        public const string EditUserCommand = "EditUserCommand";
        public const string ShowPriviligesForUserCommand = "ShowPriviligesForUserCommand";


    }
}