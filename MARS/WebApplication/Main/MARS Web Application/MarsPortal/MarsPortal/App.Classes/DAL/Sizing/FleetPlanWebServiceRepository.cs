using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Sizing.Abstract;
using Mars.Entities.Sizing.Abstract;
using System.Web.UI;
using Mars.Entities.Sizing;
using Mars.App.Classes.DAL.MarsDBContext;


namespace Mars.DAL.Sizing {
    public class FleetPlanWebServiceRepository : IFleetPlanWebServiceRepository {

        static Int32 LOGTYPE = 3, DEFAULTTIMEOUT=3000, INCREASEDTIMEOUT=300000, TIMEOUT = 3;
        static String UPLOADMESSAGE="Ready for upload."
            ,LOGNAME="MarsV3",EXCEPTIONMESSAGE="Error trying to insert record in FleetPlanWebServiceRepository, error message:";

        public IFleetPlanEntity GetMessage() {
            update();
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                IQueryable<MarsLogView> q = (from p in db.MarsLogViews where p.LogType_Id == LOGTYPE select p).OrderByDescending(p => p.Id);
                return (from p in q select new FleetPlanEntity { Message = p.Message, Status = p.Status }).FirstOrDefault();
            }
        }
        void update() {
            // check if it's timed out and override
            TimeSpan ts = DateTime.Now.Subtract(GetTimeOfRunning());
            if (ts.Minutes >= TIMEOUT) SetMessage(UPLOADMESSAGE);
        }
        public void SetMessage(string message) {
            SetMessage(message, FleetPlanOptions.None);
        }
        public void SetMessage(string message, FleetPlanOptions Status) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                Log mlv = new Log {
                    DateTime = DateTime.Now,
                    Message = message,
                    Status_Id = (Int32?)Status,
                    LogType_Id = LOGTYPE // Fleetsize
                };
                db.Logs.InsertOnSubmit(mlv);
                try { db.SubmitChanges(); }
                catch (Exception e) {
                    //ILog l = log4net.LogManager.GetLogger(LOGNAME);
                    //if (l!=null) l.Error(EXCEPTIONMESSAGE + e.Message);
                }
            }
        }
        public DateTime GetTimeOfRunning() {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                MarsLogView q = (from p in db.MarsLogViews where p.LogType_Id == LOGTYPE select p).OrderByDescending(p => p.Id).FirstOrDefault();
                if(q==null) return DateTime.Now;
                if (q.Status_Id == (Int32)FleetPlanOptions.Running) { return (DateTime)q.DateTime; }
                return DateTime.Now;
            }
        }
        public void StartFleetSizeGenerateStoredProcedure() {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                try {
                    db.CommandTimeout = INCREASEDTIMEOUT;
                    db.FleetSizeForecastGenerate(); // this is a stored procedure
                    db.CommandTimeout = DEFAULTTIMEOUT;
                }
                catch (Exception e) {
                  //  ILog l = log4net.LogManager.GetLogger(LOGNAME);
                  //  if (l != null) l.Error(EXCEPTIONMESSAGE + e.Message);
                }
            }
        }
    }
}