using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Bll.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class AgeingRow
    {
        public string Key { get; set; }
        public int FleetCount { get; set; }

        

        public int Group1 { get; set; }
        public int Group2 { get; set; }
        public int Group3 { get; set; }
        public int Group4 { get; set; }
        public int Group5 { get; set; }
        public int Group6 { get; set; }
        public int Group7 { get; set; }
        public int Group8 { get; set; }



        public double PercentOfTotalFleet { get; set; }


        public double PercentNonRevOfTotalNonRev { get; set; }



        public List<int?> Ages { get; set; }

        public void AssignGroups()
        {
            foreach (var a in Ages)
            {
                var group = AgeingGroupCalulator.CalculateAgeGroup(a);
                switch (group)
                {
                    case AgeGroup.Group1:
                        Group1++;
                        break;
                    case AgeGroup.Group2:
                        Group2++;
                        break;
                    case AgeGroup.Group3:
                        Group3++;
                        break;
                    case AgeGroup.Group4:
                        Group4++;
                        break;
                    case AgeGroup.Group5:
                        Group5++;
                        break;
                    case AgeGroup.Group6:
                        Group6++;
                        break;
                    case AgeGroup.Group7:
                        Group7++;
                        break;
                    case AgeGroup.Group8:
                        Group8++;
                        break;
                    case AgeGroup.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}