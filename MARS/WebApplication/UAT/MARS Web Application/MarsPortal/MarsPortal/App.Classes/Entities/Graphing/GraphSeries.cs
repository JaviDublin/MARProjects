using System.Collections.Generic;
using System.Drawing;

namespace App.Entities.Graphing
{
    /// <summary>
    /// Summary description for GenericGraphDataHolder
    /// </summary>
    public class GraphSeries
    {
        public GraphSeries(string seriesName)
        {
            SeriesName = seriesName;
            Xvalue = new List<object>();
            Yvalue = new List<double>();
            Displayed = true;
        }

        public string SeriesName { get; set; }
        public Color GraphColour { get; set; }
        public bool Displayed { get; set; }
        public bool ShowLabel { get; set; }

        public List<object> Xvalue { get; set; }
        public List<double> Yvalue { get; set; }
    }
}