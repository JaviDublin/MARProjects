using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using System.Web.UI.WebControls;
using Mars.DAL.Sizing.Abstract;
using Mars.BLL.Sizing.Models;
using System.Web.UI;
using System.Threading;
using Mars.Entities.Sizing.Abstract;

namespace Mars.Sizing.Models {
    public class FleetPlanLabelModel : IFleetPlanModel {

        static String EXCEPTIONMESSAGE="The Text Label must be assigned to a Web.UI.WebControls.Label, in FleetPlanModel.";
        readonly String NOMESSAGE="No Message";

        private IFleetPlanWebServiceRepository _fleetPlanWebServiceRepository;
        public FleetPlanLabelModel(IFleetPlanWebServiceRepository r) { _fleetPlanWebServiceRepository = r; }

        public Label TextLabel { get; set; }
        public string Text {
            get {
                return TextLabel.Text;
            }
            set {
                if(TextLabel == null) { throw new NullReferenceException(EXCEPTIONMESSAGE); }
                TextLabel.Text = value;
            }
        }
        public Label ErrorLabel {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public string ErrorText {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public void Update() {
            IFleetPlanEntity ifpe=_fleetPlanWebServiceRepository.GetMessage();
            if(ifpe==null) { Text=NOMESSAGE; return; }
            Text = String.IsNullOrEmpty(ifpe.Message)?NOMESSAGE:ifpe.Message;
        }
        public void SetMessage(string message) {
            SetMessage(message, FleetPlanOptions.None);
        }
        public void SetMessage(string message, FleetPlanOptions status) {
            _fleetPlanWebServiceRepository.SetMessage(message, status);
        }
        public void UpdateTables() {
            _fleetPlanWebServiceRepository.StartFleetSizeGenerateStoredProcedure();
        }
        public string GetMessage() {
            return _fleetPlanWebServiceRepository.GetMessage().Message;
        }
        public string GetStatus() {
            return _fleetPlanWebServiceRepository.GetMessage().Status;
        }
    }
}