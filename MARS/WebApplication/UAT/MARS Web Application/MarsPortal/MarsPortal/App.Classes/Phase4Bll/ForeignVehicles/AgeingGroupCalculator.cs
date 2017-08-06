using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Bll.ForeignVehicles
{
    public static class AgeingGroupCalulator
    {
        public static AgeGroup CalculateAgeGroup(int? age)
        {
            if (age == null) return AgeGroup.None;
            if (age <= 1)
                return AgeGroup.Group1;
            if (age <= 3)
                return AgeGroup.Group2;
            if (age <= 5)
                return AgeGroup.Group3;
            if (age == 7)
                return AgeGroup.Group4;
            if (age <= 10)
                return AgeGroup.Group5;
            if (age <= 15)
                return AgeGroup.Group6;
            if (age <= 30)
                return AgeGroup.Group7;
            return AgeGroup.Group8;
        }
    }
}