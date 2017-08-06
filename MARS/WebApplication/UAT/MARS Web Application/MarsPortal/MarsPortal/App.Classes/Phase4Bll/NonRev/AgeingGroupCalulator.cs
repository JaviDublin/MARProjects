using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Bll.NonRev
{
    public static class AgeingGroupCalulator
    {
        public static AgeGroup CalculateAgeGroup(int? age)
        {
            if (age == null) return AgeGroup.None;
            if(age <= 2)
                return AgeGroup.Group1;
            if (age == 3)
                return AgeGroup.Group2;
            if (age >= 4 && age <= 6)
                return AgeGroup.Group3;
            if (age == 7)
                return AgeGroup.Group4;
            if (age >= 8 && age <= 10)
                return AgeGroup.Group5;
            if (age >= 11 && age <= 15)
                return AgeGroup.Group6;
            if (age >= 16 && age <= 30)
                return AgeGroup.Group7;
            if (age >= 31 && age <= 60)
                return AgeGroup.Group8;
            return AgeGroup.Group9;
        }
    }
}