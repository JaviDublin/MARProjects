using Dundas.Charting.WebControl;
namespace App.BLL.ChartFactory {
    /// <summary>
    /// Updated : 25-4-12
    /// Built by Gavin for MarsV3
    /// ChartFactory is a container for classes and namespaces to be used to alter Dundas Charts
    /// Tightly coupled to AvailabilityChart.Default
    /// </summary>
    class BuildLegend {

        #region fields
        private Legend _legend;
        private string _name;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name associated with Legend</param>
        public BuildLegend(string name) {
            _name = name;
            _legend = new Legend(name);
            _legend.Docking = LegendDocking.Top;
            _legend.TableStyle = LegendTableStyle.Tall;
            _legend.Alignment = System.Drawing.StringAlignment.Center;
            _legend.BorderColor = System.Drawing.Color.Black;
            _legend.BorderStyle = ChartDashStyle.Solid;
            _legend.BorderWidth = 1;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Constructs the Legend according to the arg (argument)
        /// </summary>
        /// <param name="arg">The argument, as string, to decide what type of legend to return</param>
        /// <returns>Dundas.Charting.WebControl.Legend</returns>
        public Legend getLegend(string arg, string logic) {
            if (logic == "NUMERIC") {
                switch (arg) {
                    case "OperationalUtilization":
                        makeLegend("ON_RENT", LegendSeparatorType.None, "");
                        break;
                    case "Overdues":
                        makeLegend("OVERDUE", LegendSeparatorType.None, "");
                        break;
                    case "IdleFleet":
                        makeLegend("TOTAL_FLEET - ON_RENT", LegendSeparatorType.None, "");
                        break;
                    case "RentableFleetOnPeak":
                        makeLegend("RT_MAX", LegendSeparatorType.None, "");
                        break;
                    default:
                        makeLegend("Unknown", LegendSeparatorType.None, "");
                        break;
                }
            }
            else {
                switch (arg) {
                    case "OperationalUtilization":
                        makeLegend("ON_RENT", "OPERATION_FLEET", "%");
                        break;
                    case "Overdues":
                        makeLegend("OVERDUE", "AVAILABLE_FLEET", "%");
                        break;
                    case "IdleFleet":
                        makeLegend("TOTAL_FLEET - ON_RENT", "TOTAL_FLEET", "%");
                        break;
                    case "RentableFleetOnPeak":
                        makeLegend("RT_MAX", "AVAILABLE_FLEET_MAX", "%");
                        break;
                    default:
                        makeLegend("Unknown", LegendSeparatorType.None, "");
                        break;
                }
            }
            return _legend;
        }
        /// <summary>
        /// // Finds the index of the legend in the chart (Dundas) or returns -1
        /// </summary>
        /// <param name="chart">Requires a Dundas Chart</param>
        /// <returns>The index or -1</returns>
        public int indexOf(Chart chart) {
            if (chart.Legends.Count > 0)
                for (int f = 0; f < chart.Legends.Count; f++)
                    if (chart.Legends[f].Name.Contains(_name)) return f;
            return -1;
        }
        #endregion

        #region worker methods
        /// <summary>
        /// This is a real worker and uses the overridden makeLegend to construct a specific Legend
        /// Alters the field _legend
        /// </summary>
        /// <param name="topText">The text as string at the top of the equation</param>
        /// <param name="bottomText">The text as string at the bottom of the equation</param>
        private void makeLegend(string topText, string bottomText, string topSymbol) {
            makeLegend(topText, LegendSeparatorType.Line, topSymbol);
            makeLegend(bottomText, LegendSeparatorType.None, "");
        }
        /// <summary>
        /// The second worker which creates the LegendItems for the Chart and adds symbol to the top
        /// </summary>
        /// <param name="text">The text as string to be inserted</param>
        /// <param name="sepType">The seperator type as LegendSeparatorType</param>
        /// <param name="symbol">The symbol to display at the end</param>
        private void makeLegend(string text, LegendSeparatorType sepType, string symbol) {
            LegendItem li = new LegendItem();
            li.Cells.Add(createLegendCell(text));
            li.Separator = sepType;
            if (symbol != "") li.Cells.Add(createLegendCell(symbol));
            _legend.CustomItems.Add(li);
        }
        /// <summary>
        /// Creates a LegendCell
        /// </summary>
        /// <param name="text">The text, as string in the cell </param>
        /// <returns>The LegendCell with the text</returns>
        private LegendCell createLegendCell(string text) {
            LegendCell lc = new LegendCell(text);
            System.Drawing.Font font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10F);
            lc.Font = font;
            return lc;
        }
        #endregion
    }
}
