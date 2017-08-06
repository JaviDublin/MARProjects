using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.HTMLFactories.Abstract;
using Mars.DAL.Pooling;
using Mars.Entities.Pooling;
using System.Text;

namespace Mars.Pooling.HTMLFactories
{
    public class AlertsHtmlPopup : HtmlTable
    {

        enum _details { location, goldGrp, number, betHrs };
        const Int32 NOOFARGUMENTS = 4;
        const String NOMATCH = "No match.", RESNBR = "ResNbr: ", RTRNLOC = "Rtrn Loc: ";
        AlertsPopupRepository _repository;
        public AlertsHtmlPopup(AlertsPopupRepository r)
        {
            _repository = r;
        }
        public String[] GetArgumentArray(String s)
        {
            if (String.IsNullOrEmpty(s)) return new String[NOOFARGUMENTS] { String.Empty, String.Empty, String.Empty, String.Empty };
            try
            {
                char[] c = { ' ', '|' };
                return s.Split(c);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException("The argument hasn't spilt into the required number of arguments [" + NOOFARGUMENTS + "] separated by space or |. Message: " + ex.Message);
            }
        }
        public override string GetTable()
        {
            if (Filter == null || String.IsNullOrEmpty(Filter.LocAndGoldCar)) return NOMATCH;
            string[] s;
            try
            {
                s = GetArgumentArray(Filter.LocAndGoldCar);
            }
            catch { return NOMATCH; }
            if (s.Length < NOOFARGUMENTS) return NOMATCH;
            StringBuilder rs = new StringBuilder();
            rs.AppendLine(RESNBR + ", " + RTRNLOC);
            foreach (AlertsPopupEntity ape in _repository.GetTable(s[(Int32)_details.location], s[(Int32)_details.goldGrp], s[(Int32)_details.betHrs]))
            {
                rs.AppendLine(ape.ResNbr + ", " + ape.RtrnLoc);
            }
            return rs.ToString();
        }
    }
}