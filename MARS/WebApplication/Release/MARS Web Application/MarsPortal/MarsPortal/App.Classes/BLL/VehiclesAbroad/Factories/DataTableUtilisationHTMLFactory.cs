using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using App.BLL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Factories {

    public class DataTableUtilisationHTMLFactory : IDataTable {

        IList<IDataTableEntity> _list;
        IList<string> _columns;

        public DataTableUtilisationHTMLFactory(IList<IDataTableEntity> l) {

            _list = l;
            setColumns();
        }

        public string getDataTableAsString() {

            return getDataTableAsString(" ");
        }

        /// <summary>
        /// similar to the DataTableHTMLFactory but more to return percentages
        /// </summary>
        /// <param name="firstColumn"></param>
        /// <returns></returns>
        public string getDataTableAsString(string firstColumn, bool clickable = false) {

            // no data in the list
            if (_list.Count == 0)
                return "<div>No data.</div>";


            StringBuilder html = new StringBuilder("<table cellspacing='1' cellpadding='1' border='1' style='background-color:White;border-color:#999999;border-width:1px;border-style:Solid;border-collapse:collapse;width:98%'>");
            html.Append("<tr  style='color:White;background-color:Black;font-size:Small;font-weight:bold;'>");
            html.Append("<th>" + firstColumn + "</th>");

            int colSize = 100 / (_columns.Count() + 1); // work out the size in % of the columns for uniformity

            foreach (var col in _columns) {

                if (col != null) { // check the column is not null
                    html.Append(@"<th style='width:" + colSize.ToString() + @"%'>" + col + "</th>"); // column size tag added
                }
            }
            html.Append("</tr>");
            var rows = _list.Select(p => p.rowDefinition).Distinct();

            foreach (var row in rows) {

                html.Append("<tr  style='color:Black;background-color:#EEEEEE;font-size:Small;'>");
                html.Append("<td>" + row + "</td>");
                foreach (var col in _columns) {
                    var grp = from p in _list where p.rowDefinition == row && p.header == col select p;
                    html.Append("<td>");
                    foreach (var item in grp) html.Append(item.theValue.ToString());
                    html.Append("</td>");
                }
                html.Append("</tr>");
            } // end of the foreach
            html.Append("<tr style='color:White;background-color:Black;font-size:Small;font-weight:bold;'>");

            // loop through the totals
            html.Append("<td>Total</td>");

            IDictionary<string, string> percents = getPercents();
            foreach (var col in _columns)
                html.Append("<td>" + percents[col] + "</td>");

            html.Append("</tr></table>");
            return html.ToString();
        }

        private IDictionary<string, string> getPercents() {

            IDictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var col in _columns) {

                // get the list of values (can't average straight [well I can't!] as the retruns values are of type string
                var values = _list.Where(p => p.header == col).Select(p => p.theValue);

                // work out total and assign count (only if the convert is a value is not null)
                decimal d = 0;
                decimal count = values.Count();
                foreach (var val in values)
                    if (val != null) {
                        d += Convert.ToDecimal(val);
                    }
                    else
                        count--;

                // work out the average and put into dictionary
                dic.Add(col, (d / count).ToString("0.##"));
            }
            return dic;
        }

        private void setColumns() {
            _columns = _list.Select(p => p.header).Distinct().ToList();
        }
    }
}