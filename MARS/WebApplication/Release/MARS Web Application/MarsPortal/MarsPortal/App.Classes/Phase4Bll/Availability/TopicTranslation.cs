using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Bll.Availability
{
    public static class TopicTranslation
    {
        private static Dictionary<AvailabilityTopic, string> LookupDictionary
        {
            get
            {
                var dic  = new Dictionary<AvailabilityTopic, string>
                           {
                               {AvailabilityTopic.TotalFleet, "Total Fleet"}
                               , {AvailabilityTopic.Cu, "CU - Credit Union"}
                               , {AvailabilityTopic.Ha, "HA - Hold Admin"}
                               , {AvailabilityTopic.Hl, "HL - Hold Legal"}
                               , {AvailabilityTopic.Ll, "LL - Lease Loaner"}
                               , {AvailabilityTopic.Nc, "NC - No Car Tow"}
                               , {AvailabilityTopic.Pl, "PL - Retail Pipeline"}
                               , {AvailabilityTopic.Tc, "TC - Theft Conversion"}
                               , {AvailabilityTopic.Sv, "SV - Salvage"}
                               , {AvailabilityTopic.Ws, "WS - Wholesale"}
                               , {AvailabilityTopic.OperationalFleet, "Operational Fleet"}
                               , {AvailabilityTopic.Bd, "BD - Body Damage"}
                               , {AvailabilityTopic.Mm, "MM - Maintenance"}
                               , {AvailabilityTopic.Tw, "TW - Open Tow"}
                               , {AvailabilityTopic.Tb, "TB - Turnback"}
                               , {AvailabilityTopic.Fs, "FS - Fleet Sale"}
                               , {AvailabilityTopic.Rl, "RL - Retail Lot"}
                               , {AvailabilityTopic.Rp, "RP - Retail Pending"}
                               , {AvailabilityTopic.Tn, "TN - Transfer on Site"}
                               , {AvailabilityTopic.AvailableFleet, "Available Fleet"}
                               , {AvailabilityTopic.Idle, "Idle"}
                               , {AvailabilityTopic.Su, "SU - Service Unit"}
                               , {AvailabilityTopic.Overdue, "Overdue"}
                               , {AvailabilityTopic.OnRent, "On Rent"}
                               , {AvailabilityTopic.Utilization, "Utilization"}
                               , {AvailabilityTopic.UtilizationInclOverdue, "Utilization Incl Overdue"}
                           };
                return dic;
            }
        }

        public static string GetAvailabilityTopicDescription(AvailabilityTopic topic)
        {
            var returned = LookupDictionary[topic];
            return returned;
        }

        public static AvailabilityTopic GetAvailabilityTopicFromDescription(string description)
        {
            var returned = LookupDictionary.FirstOrDefault(d=> d.Value == description).Key;
            return returned;
        }


        public static Color GetAvailabilityColour(AvailabilityTopic topic)
        {
            switch (topic)
            {
                case AvailabilityTopic.TotalFleet:
                    return Color.Black;
                case AvailabilityTopic.Cu:
                    return Color.FromArgb(255, 153, 204);
                case AvailabilityTopic.Ha:
                    return Color.FromArgb(255, 153, 0);
                case AvailabilityTopic.Hl:
                    return Color.FromArgb(153, 51, 0);
                case AvailabilityTopic.Ll:
                    return Color.FromArgb(153, 51, 0);
                case AvailabilityTopic.Nc:
                    return Color.FromArgb(255, 204, 0);
                case AvailabilityTopic.Pl:
                    return Color.FromArgb(128, 128, 0);
                case AvailabilityTopic.Tc:
                    return Color.FromArgb(255, 128, 128);
                case AvailabilityTopic.Sv:
                    return Color.FromArgb(255, 0, 255);
                case AvailabilityTopic.Ws:
                    return Color.FromArgb(128, 0, 128);
                case AvailabilityTopic.OperationalFleet:
                    return Color.FromArgb(0, 255, 0);
                case AvailabilityTopic.Bd:
                    return Color.FromArgb(0, 0, 255);
                case AvailabilityTopic.Mm:
                    return Color.FromArgb(153, 153, 255);
                case AvailabilityTopic.Tw:
                    return Color.FromArgb(0, 0, 255);
                case AvailabilityTopic.Tb:
                    return Color.FromArgb(51, 204, 204);
                case AvailabilityTopic.Fs:
                    return Color.FromArgb(0, 0, 255);
                case AvailabilityTopic.Rl:
                    return Color.FromArgb(0, 0, 128);
                case AvailabilityTopic.Rp:
                    return Color.FromArgb(51, 51, 153);
                case AvailabilityTopic.Tn:
                    return Color.OrangeRed;
                case AvailabilityTopic.AvailableFleet:
                    return Color.FromArgb(0, 170, 0);
                case AvailabilityTopic.Idle:
                    return Color.FromArgb(255, 0, 0);
                case AvailabilityTopic.Su:
                    return Color.FromArgb(192, 192, 192);
                case AvailabilityTopic.Overdue:
                    return Color.FromArgb(150, 0, 0);
                case AvailabilityTopic.OnRent:
                    return Color.DarkGreen;
                case AvailabilityTopic.Utilization:
                    return Color.Navy;
                case AvailabilityTopic.UtilizationInclOverdue:
                    return Color.PowderBlue;
                default:
                    throw new ArgumentOutOfRangeException("topic");
            }
        }

        public static string GetAvailabilityTopicShortDescription(AvailabilityTopic topic)
        {
            switch (topic)
            {
                case AvailabilityTopic.TotalFleet:
                    return "TOTAL FLEET";
                case AvailabilityTopic.Cu:
                    return "CU";
                case AvailabilityTopic.Ha:
                    return "HA";
                case AvailabilityTopic.Hl:
                    return "HL";
                case AvailabilityTopic.Ll:
                    return "LL";
                case AvailabilityTopic.Nc:
                    return "NC";
                case AvailabilityTopic.Pl:
                    return "PL";
                case AvailabilityTopic.Tc:
                    return "TC";
                case AvailabilityTopic.Sv:
                    return "SV";
                case AvailabilityTopic.Ws:
                    return "WS";
                case AvailabilityTopic.OperationalFleet:
                    return "OPERATIONAL FLEET";
                case AvailabilityTopic.Bd:
                    return "BD";
                case AvailabilityTopic.Mm:
                    return "MM";
                case AvailabilityTopic.Tw:
                    return "TW";
                case AvailabilityTopic.Tb:
                    return "TB";
                case AvailabilityTopic.Fs:
                    return "FS";
                case AvailabilityTopic.Rl:
                    return "RL";
                case AvailabilityTopic.Rp:
                    return "RP";
                case AvailabilityTopic.Tn:
                    return "TN";
                case AvailabilityTopic.AvailableFleet:
                    return "AVAILABLE FLEET";
                case AvailabilityTopic.Idle:
                    return "Idle";
                case AvailabilityTopic.Su:
                    return "SU";
                case AvailabilityTopic.Overdue:
                    return "OVERDUE";
                case AvailabilityTopic.OnRent:
                    return "ON RENT";
                case AvailabilityTopic.Utilization:
                    return "UTILIZATION";
                case AvailabilityTopic.UtilizationInclOverdue:
                    return "UTILIZATION INCL OVERDUE";
                default:
                    throw new ArgumentOutOfRangeException("topic");
            }
        }

        
    }
}