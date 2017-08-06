using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Dal.Administration.Membership
{
    public static class MembershipDataAccess
    {
        public static Guid? GetGuidFromMembershipDatabase(string userId)
        {
            Guid? guid;
            using(var membershipDataAccess = new RadMembershipDataContext())
            {
                var guidFromDb = membershipDataAccess.Users.FirstOrDefault(d => d.GlobalId == userId);
                if (guidFromDb == null) return null;
                
                guid = guidFromDb.UserId;
            }
            return guid;
        }
    }
}