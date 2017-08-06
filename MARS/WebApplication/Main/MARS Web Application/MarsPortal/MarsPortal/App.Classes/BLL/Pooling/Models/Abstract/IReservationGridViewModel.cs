using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using System.Web.UI.WebControls;
using App.Classes.Entities.Pooling.Abstract;
using System.Web.UI.HtmlControls;
using Mars.Entities.Reservations.Abstract;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IReservationGridViewModel : IPoolingGridViewModel {
        IReservationDetailsEntity GetModalDetails(int rowIndex);
        IReservationDetailsFilterEntity ResFilters { get; set; }
    }
}
