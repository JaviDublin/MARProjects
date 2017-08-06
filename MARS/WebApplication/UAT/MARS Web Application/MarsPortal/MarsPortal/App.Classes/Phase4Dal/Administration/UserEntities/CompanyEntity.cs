using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class CompanyEntity
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTypeName { get; set; }
        public int CompanyTypeId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string EditCommand { get { return EditUserCommand; } }
        public const string EditUserCommand = "EditUserCommand";

        public string ViewCommand { get { return ViewUserCommand; } }
        public const string ViewUserCommand = "ViewUserCommand";
    }
}