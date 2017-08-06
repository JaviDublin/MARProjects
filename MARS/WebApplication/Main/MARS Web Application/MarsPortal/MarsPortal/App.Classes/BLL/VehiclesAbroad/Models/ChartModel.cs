using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.DataVisualization.Charting;
using App.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Abstract;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Models {
    public class ChartModel : IChartModel {
        private IUtilisationHistoryRepository _repository;

        public ChartModel(IUtilisationHistoryRepository r) {
            _repository = r;
        }
        public Chart _Chart { get; set; }

        public void bind() {
            loadChart();
        }

        // helper to load the chart with data
        private void loadChart() {
            IList<IDataTableEntity> l = _repository.getList4Chart();
            // get the country names
            var names = (from p in l select p.header).Distinct();
            foreach (var name in names) {
                _Chart.Series.Add(name);
                _Chart.Series[name].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Spline;
                _Chart.Legends.Add(name);
                _Chart.ChartAreas["ChartArea1"].Position.Height = 90;
                _Chart.ChartAreas["ChartArea1"].Position.Width = 80;

                // get the data for that country
                var points = from p in l where p.header == name select p;
                foreach (var point in points) {
                    _Chart.Series[name].Points.AddXY(point.rowDefinition, point.theValue);
                }
            }
        }
    }
}