using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface IChartModel {
        Chart _Chart { get; set; }
        void bind();
    }
}
