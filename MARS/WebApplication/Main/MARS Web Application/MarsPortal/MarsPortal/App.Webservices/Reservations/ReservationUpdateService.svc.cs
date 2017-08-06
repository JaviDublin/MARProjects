using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using App.Classes.DAL.Reservations;
using App.Classes.DAL.Reservations.Abstract;
using App.Classes.Entities.Reservations.Abstract;
using App.Webservices.Reservations.Abstract;

namespace Mars.App.Webservices.Reservations {
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReservationUpdateService : IReservationUpdateService {

        [WebGet]
        [OperationContract]
        public string GetUpdate() {
            IReservationLogRepository _repository = new ReservationLogRepository();
            IReservationDBUpdateEntity x = _repository.getItem();
            return "{ '" + x.TeraDataMessage + "', ''" + x.FleetMessage + "' }";
        }
    }
}
