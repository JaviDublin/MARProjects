using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    /// <summary>
    /// Author:  Damien Connaghan 
    /// Date: 07/08/2014
    /// Object used by CMS Location Group Admin Mappings
    /// </summary>
    public class CmsLocationGroupRow
    {
        
        public int CmsLocationGroupId { get; set; }
        public string CmsLocationGroup { get; set; }
        public string CmsLocGroupCodeDw { get; set; }
        public string CmsPool { get; set; }
        public int? CmsPoolId { get; set; }
        public string Country { get; set; }
        public bool? IsActive { get; set; } 

        
    }
}