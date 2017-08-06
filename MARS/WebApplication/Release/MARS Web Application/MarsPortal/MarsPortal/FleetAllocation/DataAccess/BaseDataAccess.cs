using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class BaseDataAccess : IDisposable
    {
        protected FaoDataContext DataContext;
        protected Dictionary<DictionaryParameter, string> Parameters;

        public BaseDataAccess(Dictionary<DictionaryParameter, string> parameters = null)
        {
            DataContext = new FaoDataContext
                          {
                            Log = new DebugTextWriter(),
                            CommandTimeout = 180
                          };
            Parameters = parameters;
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}