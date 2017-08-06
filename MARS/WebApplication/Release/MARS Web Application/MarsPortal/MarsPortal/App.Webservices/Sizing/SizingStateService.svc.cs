using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Mars.Webservices.Sizing.Abstract;
using System.Text;
using Mars.DAL.Sizing.Abstract;
using Mars.DAL.Sizing;
using System.Threading;

namespace Mars.Webservices.Sizing {

    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SizingStateService : ISizingStateService {
        Thread _thread;
        static String _threadCond;
        static String START="StartThread",MOVESTATE3="MoveState3";
        static Int32 SLEEPTIME=5000;

        [OperationContract]
        public String GetUpdate() {
            return "{ 'Action' : '"+_fleetPlanWebServiceRepository.GetMessage().Status+"' , 'Message' : '"+_fleetPlanWebServiceRepository.GetMessage().Message+"'}";
        }
        static IFleetPlanWebServiceRepository _flp;
        IFleetPlanWebServiceRepository _fleetPlanWebServiceRepository {
            get {
                if (_flp == null) _flp = new FleetPlanWebServiceRepository();
                return _flp;
            }
        }
        [OperationContract]
        public String RunStoredProcedure() {
            if (String.IsNullOrEmpty(_threadCond)) {
                _thread = new Thread(new ThreadStart(threadRunner));
                _threadCond = START;
                _thread.Start();
            }
            return MOVESTATE3;
        }
        void threadRunner() {
            _fleetPlanWebServiceRepository.StartFleetSizeGenerateStoredProcedure();
            Thread.Sleep(SLEEPTIME);
            _threadCond = String.Empty;
        }
    }
}
