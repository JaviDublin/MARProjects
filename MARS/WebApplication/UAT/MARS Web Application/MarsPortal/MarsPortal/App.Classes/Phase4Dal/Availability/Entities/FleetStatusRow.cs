using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.Availability.Entities
{
    public class FleetStatusRow
    {

        public FleetStatusRow(PercentageDivisorType type, bool useDatabaseFields)
        {
            _divisorType = type;
            UseDatabaseCalculatedFields = useDatabaseFields;
        }

        private PercentageDivisorType _divisorType;

        public void SetDivisorType(PercentageDivisorType dt)
        {
            _divisorType = dt;
        }

        public double PercentOfValue
        {
            get
            {
                switch (_divisorType)
                {
                    case PercentageDivisorType.Values:
                        return 1;
                    case PercentageDivisorType.TotalFleet:
                        return TotalFleet;
                    case PercentageDivisorType.OperationalFleet:
                        return OperationalFleet;
                    case PercentageDivisorType.AvailableFleet:
                        return AvailableFleet;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public DateTime Day { get; set; }
        public string Key { get; set; }

        public bool UseDatabaseCalculatedFields;

        public double TotalFleet;
        public double OperationalFleet 
        {
            get { return UseDatabaseCalculatedFields ? OperationalFleetDatabaseValue : TotalFleet - Cu - Ha - Hl - Ll - Nc - Pl - Tc - Sv - Ws; }
        }

        public double OperationalFleetPercent
        {
            get { return (double)OperationalFleet / PercentOfValue; }
        }

        public double AvailableFleet
        {
            get { return UseDatabaseCalculatedFields ? AvailableFleetDatabaseValue : OperationalFleet - Bd - Mm - Tw - Tb - Fs - Rl - Rp - Tn; }
        }
        public double AvailableFleetPercent
        {
            get { return (double)AvailableFleet / PercentOfValue; }
        }

        public double OnRent
        {
            get { return UseDatabaseCalculatedFields ? OnRentDatabaseValue : AvailableFleet - Idle - Su - Overdue; }
        }

        public double OnRentPercent
        {
            get { return (double)OnRent / PercentOfValue; }
        }

        public double InShop
        {
            get { return (Bd + Mm + Tw) ; }
        }

        public double InShopPercent
        {
            get { return ((double)InShop) / PercentOfValue; }
        }

        public double OverduePercent
        {
            get { return ((double)Overdue) / PercentOfValue; }
        }

        public double UtilInclOverdue
        {
            get { return (OnRent + Overdue); }
        }

        public double UtilInclOverduePercent
        {
            get { return ((double)UtilInclOverdue) / PercentOfValue; }
        }

        public double Utilization
        {
            get { return OnRent; }
        }

        public double UtilizationPercent
        {
            get { return (double)OnRent / PercentOfValue; }
        }


        public double Cu;
        public double Ha;
        public double Hl;
        public double Ll;
        public double Nc;
        public double Pl;
        public double Tc;
        public double Sv;
        public double Ws;
        public double Bd;
        public double Mm;
        public double Tw;
        public double Tb;
        public double Fs;
        public double Rl;
        public double Rp;
        public double Tn;
        public double Idle;
        public double Su;
        public double Overdue;

        public double OperationalFleetDatabaseValue;
        public double AvailableFleetDatabaseValue;
        public double OnRentDatabaseValue;

        public double TotalFleetPercent { get { return (double)TotalFleet / PercentOfValue; } }
        public double CuPercent { get { return (double)Cu / PercentOfValue; } }
        public double HaPercent { get { return (double)Ha / PercentOfValue; } }
        public double HlPercent { get { return (double)Hl / PercentOfValue; } }
        public double LlPercent { get { return (double)Ll / PercentOfValue; } }
        public double NcPercent { get { return (double)Nc / PercentOfValue; } }
        public double PlPercent { get { return (double)Pl / PercentOfValue; } }
        public double TcPercent { get { return (double)Tc / PercentOfValue; } }
        public double SvPercent { get { return (double)Sv / PercentOfValue; } }
        public double WsPercent { get { return (double)Ws / PercentOfValue; } }
        public double BdPercent { get { return (double)Bd / PercentOfValue; } }
        public double MmPercent { get { return (double)Mm / PercentOfValue; } }
        public double TwPercent { get { return (double)Tw / PercentOfValue; } }
        public double TbPercent { get { return (double)Tb / PercentOfValue; } }
        public double FsPercent { get { return (double)Fs / PercentOfValue; } }
        public double RlPercent { get { return (double)Rl / PercentOfValue; } }
        public double RpPercent { get { return (double)Rp / PercentOfValue; } }
        public double TnPercent { get { return (double)Tn / PercentOfValue; } }
        public double IdlePercent { get { return (double)Idle / PercentOfValue; } }
        public double SuPercent { get { return (double)Su / PercentOfValue; } }

        public double GetValue(AvailabilityTopic topic)
        {
            switch (topic)
            {
                case AvailabilityTopic.TotalFleet:
                    return TotalFleet;                
                case AvailabilityTopic.Cu:
                    return Cu;
                case AvailabilityTopic.Ha:
                    return Ha;
                case AvailabilityTopic.Hl:
                    return Hl;
                case AvailabilityTopic.Ll:
                    return Ll;
                case AvailabilityTopic.Nc:
                    return Nc;
                case AvailabilityTopic.Pl:
                    return Pl;
                case AvailabilityTopic.Tc:
                    return Tc;
                case AvailabilityTopic.Sv:
                    return Sv;
                case AvailabilityTopic.Ws:
                    return Ws;
                case AvailabilityTopic.OperationalFleet:
                    return OperationalFleet;
                case AvailabilityTopic.Bd:
                    return Bd;
                case AvailabilityTopic.Mm:
                    return Mm;
                case AvailabilityTopic.Tw:
                    return Tw;
                case AvailabilityTopic.Tb:
                    return Tb;
                case AvailabilityTopic.Fs:
                    return Fs;
                case AvailabilityTopic.Rl:
                    return Rl;
                case AvailabilityTopic.Rp:
                    return Rp;
                case AvailabilityTopic.Tn:
                    return Tn;
                case AvailabilityTopic.AvailableFleet:
                    return AvailableFleet;
                case AvailabilityTopic.Idle:
                    return Idle;
                case AvailabilityTopic.Su:
                    return Su;
                case AvailabilityTopic.Overdue:
                    return Overdue;
                case AvailabilityTopic.OnRent:
                    return OnRent;
                case AvailabilityTopic.Utilization:
                    return Utilization;
                case AvailabilityTopic.UtilizationInclOverdue:
                    return UtilInclOverdue;
                default:
                    throw new ArgumentOutOfRangeException("selectedTopic");
            }
        }

        public double GetValuePercent(AvailabilityTopic topic)
        {
            switch (topic)
            {
                case AvailabilityTopic.TotalFleet:
                    return TotalFleetPercent;
                case AvailabilityTopic.Cu:
                    return CuPercent;
                case AvailabilityTopic.Ha:
                    return HaPercent;
                case AvailabilityTopic.Hl:
                    return HlPercent;
                case AvailabilityTopic.Ll:
                    return LlPercent;
                case AvailabilityTopic.Nc:
                    return NcPercent;
                case AvailabilityTopic.Pl:
                    return PlPercent;
                case AvailabilityTopic.Tc:
                    return TcPercent;
                case AvailabilityTopic.Sv:
                    return SvPercent;
                case AvailabilityTopic.Ws:
                    return WsPercent;
                case AvailabilityTopic.OperationalFleet:
                    return OperationalFleetPercent;
                case AvailabilityTopic.Bd:
                    return BdPercent;
                case AvailabilityTopic.Mm:
                    return MmPercent;
                case AvailabilityTopic.Tw:
                    return TwPercent;
                case AvailabilityTopic.Tb:
                    return TbPercent;
                case AvailabilityTopic.Fs:
                    return FsPercent;
                case AvailabilityTopic.Rl:
                    return RlPercent;
                case AvailabilityTopic.Rp:
                    return RpPercent;
                case AvailabilityTopic.Tn:
                    return TnPercent;
                case AvailabilityTopic.AvailableFleet:
                    return AvailableFleetPercent;
                case AvailabilityTopic.Idle:
                    return IdlePercent;
                case AvailabilityTopic.Su:
                    return SuPercent;
                case AvailabilityTopic.Overdue:
                    return OverduePercent;
                case AvailabilityTopic.OnRent:
                    return OnRentPercent;
                case AvailabilityTopic.Utilization:
                    return UtilizationPercent;
                case AvailabilityTopic.UtilizationInclOverdue:
                    return UtilInclOverduePercent;
                default:
                    throw new ArgumentOutOfRangeException("selectedTopic");
            }

        }
    }
}