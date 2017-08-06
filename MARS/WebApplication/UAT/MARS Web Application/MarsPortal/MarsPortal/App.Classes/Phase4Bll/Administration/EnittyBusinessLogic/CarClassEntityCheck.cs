using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class CarClassEntityCheck
    {
        internal const string ClassAlreadyExistsForCountry = "A Car Class with this name already exists in this Country";

        internal static bool DoesClassExistForCountry(MarsDBDataContext dataContext, string className, int countryId, int classId = 0)
        {
            var returned = dataContext.CAR_CLASSes.Any(d => d.CAR_SEGMENT.COUNTRy1.CountryId == countryId
                && d.car_class1 == className
                && ((d.car_class_id != classId) || classId == 0));
            return returned;
        }
    }
}