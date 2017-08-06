using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class FleetOwnerEntity
    {
        public int FleetOwnerId { get; set; }

        public string FleetOwnerCode { get; set; }
        public string FleetOwnerName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? CompanyId { get; set; }
        //public int? CompanyOwnerTypeId { get; set; }
        public string CompanyName { get; set; }

        public bool Granted { get; set; }
        public Color TextColour { get { return  Granted ? Color.Green : Color.Red; } }
        public string RoleClickCommand { get { return StaticRoleClickCommand; } }
        public string ButtonText { get { return Granted ? StaticUnassignCommand : StaticAssignCommand; } }


        public string AssignedToCompanyLabelText { get { return AssignedToCompanyName == string.Empty ? StaticUnassignedText : AssignedToCompanyName; } }
        public string AssignedToCompanyName { get; set; }
        public bool LinkButtonVisible { get { return (Granted) || AssignedToCompanyName == string.Empty; } }

        public const string StaticRoleClickCommand = "FleetOwnerClickCommand";
        public const string StaticAssignCommand = "Assign";
        
        public const string StaticUnassignCommand = "Revoke";

        public const string StaticUnassignedText = "Unassigned";

        public string EditCommand { get { return EditUserCommand; } }
        public const string EditUserCommand = "EditUserCommand";
    }
}