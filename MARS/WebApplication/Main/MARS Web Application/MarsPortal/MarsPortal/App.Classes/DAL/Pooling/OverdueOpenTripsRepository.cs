using System;
using System.Linq;
using Mars.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.DAL.Pooling.Queryables;

using System.Data.SqlClient;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.DAL.Pooling
{
    public class OverdueOpenTripsRepository : IOverdueActualRepository
    {

        FeaFilteredQueryable _feaFilterQ;
        public OverdueOpenTripsRepository()
        {
            _feaFilterQ = new FeaFilteredQueryable();
        }
        public string GetItem(IMainFilterEntity filter, Int32 NumberOfDays)
        {
            using (var db = new MarsDBDataContext())
            {
                try
                {
                    IQueryable<FLEET_EUROPE_ACTUAL> q = _feaFilterQ.GetFeaCheckOut(db, filter);
                    return (from p in q
                            where p.CI_DAYS < 0
                            && p.MOVETYPE == "T-O" || p.MOVETYPE == "L-O"
                            select p).Sum(p => p.TOTAL_FLEET).ToString();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = log4net.LogManager.GetLogger("Pooling");
                   // if (_logger != null) _logger.Error("SQL Exception thrown in OverdueOpenTripsRepository, message : " + ex.Message);
                }
                return "0";
            }
        }
    }
}