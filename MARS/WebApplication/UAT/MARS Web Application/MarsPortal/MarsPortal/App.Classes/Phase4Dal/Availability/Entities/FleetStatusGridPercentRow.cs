using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Availability.Entities
{
    public class FleetStatusGridPercentRow
    {
        public string Key { get; private set; }
        public string TotalFleet { get; private set; }
        public string Cu { get; private set; }
        public string Ha { get; private set; }
        public string Hl { get; private set; }
        public string Ll { get; private set; }
        public string Nc { get; private set; }
        public string Pl { get; private set; }
        public string Tc { get; private set; }
        public string Sv { get; private set; }
        public string Ws { get; private set; }
        public string OperationalFleet { get; private set; }
        public string Bd { get; private set; }
        public string Mm { get; private set; }
        public string Tw { get; private set; }
        public string Tb { get; private set; }
        public string Fs { get; private set; }
        public string Rl { get; private set; }
        public string Rp { get; private set; }
        public string Tn { get; private set; }
        public string AvailableFleet { get; private set; }
        public string Idle { get; private set; }
        public string Su { get; private set; }
        public string Overdue { get; private set; }
        public string OnRent { get; private set; }

        

        private const string NumberFormat = "P";

        public FleetStatusGridPercentRow(FleetStatusRow fsr, bool dateAsKey = true, bool hourlyDate = false)
        {
            SetFields(fsr, dateAsKey ? hourlyDate ? fsr.Day.ToString("dd/MM/yyyy HH:mm") : fsr.Day.ToShortDateString() : fsr.Key);

        }

        public void SetFields(FleetStatusRow fsr, string key)
        {
            Key = key;
            TotalFleet = fsr.TotalFleetPercent.ToString(NumberFormat);
            Cu = fsr.CuPercent.ToString(NumberFormat);
            Ha = fsr.HaPercent.ToString(NumberFormat);
            Hl = fsr.HlPercent.ToString(NumberFormat);
            Ll = fsr.LlPercent.ToString(NumberFormat);
            Nc = fsr.NcPercent.ToString(NumberFormat);
            Pl = fsr.PlPercent.ToString(NumberFormat);
            Tc = fsr.TcPercent.ToString(NumberFormat);
            Sv = fsr.SvPercent.ToString(NumberFormat);
            Ws = fsr.WsPercent.ToString(NumberFormat);
            OperationalFleet = fsr.OperationalFleetPercent.ToString(NumberFormat);
            Bd = fsr.BdPercent.ToString(NumberFormat);
            Mm = fsr.MmPercent.ToString(NumberFormat);
            Tw = fsr.TwPercent.ToString(NumberFormat);
            Tb = fsr.TbPercent.ToString(NumberFormat);
            Fs = fsr.FsPercent.ToString(NumberFormat);
            Rl = fsr.RlPercent.ToString(NumberFormat);
            Rp = fsr.RpPercent.ToString(NumberFormat);
            Tn = fsr.TnPercent.ToString(NumberFormat);
            AvailableFleet = fsr.AvailableFleetPercent.ToString(NumberFormat);
            Idle = fsr.IdlePercent.ToString(NumberFormat);
            Su = fsr.SuPercent.ToString(NumberFormat);
            Overdue = fsr.OverduePercent.ToString(NumberFormat);
            OnRent = fsr.OnRentPercent.ToString(NumberFormat);
        }
    }
}