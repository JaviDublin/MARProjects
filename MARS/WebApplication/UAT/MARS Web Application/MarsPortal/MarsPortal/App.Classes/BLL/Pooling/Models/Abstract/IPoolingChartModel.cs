using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using System.Web.UI.DataVisualization.Charting;
using App.Classes.DAL.Reservations.Abstract;
using System.Data;
using System.Web.UI.WebControls;

namespace Mars.Pooling.Models.Abstract {
    public interface IPoolingChartModel : IChartModel {
        DataTable _DataTable { get; set; }
        IMainFilterEntity Filter { get; set; }
        void ChartClick(object o,ImageMapEventArgs e);
        int NoOfPoints { get; set; }
    }
}
