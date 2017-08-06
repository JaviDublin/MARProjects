using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.BLL.Pooling.Models.Abstract;
using System.Web.UI;
using Mars.DAL.Sizing.Abstract;

namespace Mars.BLL.Sizing.Models {
    public interface IFleetPlanModel:ILabelModel {
        void SetMessage(String message);
        void SetMessage(String message, FleetPlanOptions status);
        void UpdateTables();
        String GetMessage();
        String GetStatus();
    }
}
