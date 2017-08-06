using System;
using Mars.Entities.Sizing.Abstract;
using System.Web.UI;


namespace Mars.DAL.Sizing.Abstract {
    public interface IFleetPlanWebServiceRepository {
        IFleetPlanEntity GetMessage();
        void SetMessage(String message);
        void SetMessage(String message, FleetPlanOptions Status);
        void StartFleetSizeGenerateStoredProcedure();
    }
}
