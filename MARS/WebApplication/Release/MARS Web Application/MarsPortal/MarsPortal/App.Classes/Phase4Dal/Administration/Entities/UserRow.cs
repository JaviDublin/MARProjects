using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{

    /// <summary>
    /// Author:  Damien Connaghan
    /// Date: 17/07/2014
    /// Object used by User Admin page
    /// </summary>
    public class UserRow
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRolesSummary { get; set; }

    }
}