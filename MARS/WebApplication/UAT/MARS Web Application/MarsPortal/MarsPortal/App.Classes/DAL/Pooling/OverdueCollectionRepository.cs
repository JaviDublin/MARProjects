using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;
using Mars.DAL.Pooling.Queryables;

using System.Data.SqlClient;

namespace Mars.DAL.Pooling
{
    public class OverdueCollectionsRepository : IOverdueActualRepository
    {

        FeaFilteredQueryable _feaFilterQ;
        public OverdueCollectionsRepository()
        {
            _feaFilterQ = new FeaFilteredQueryable();
        }
        public string GetItem(IMainFilterEntity filter, int NumberOfDays)
        {
            using (var db = new MarsDBDataContext())
            {
                try
                {
                    var q = _feaFilterQ.GetFeaCheckOut(db, filter); ;
                    return (from p in q
                            where p.CI_HOURS < 0 && p.MOVETYPE == "R-O"
                            select p).Sum(p => p.TOTAL_FLEET).ToString();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = LogManager.GetLogger("Pooling");
                    //if (_logger != null) _logger.Error(" SQL Exception thrown in OverdueCollectionsRepository, message : " + ex.Message);
                }
                return "0";
            }
        }
    }
}