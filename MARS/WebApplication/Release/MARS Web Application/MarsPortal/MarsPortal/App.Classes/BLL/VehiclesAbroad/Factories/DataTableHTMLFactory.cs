using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; // added
using App.Entities.VehiclesAbroad;
using App.BLL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Factories {

    public class DataTableHTMLFactory : IDataTable {
        // A class that builds a datatable from an entity {header as string, rowDefinition as string, sum as int}
        // Uses DataTableEntity class in FleetEntity to describe the entities within the lists
        //                   |  |GE|FR|UK|BE|....| <----[header]
        //		             ---------------------
        //    		         |GE|  |1 |2 |4 |....|
        //                   ---------------------
        // [rowDefinition]-->|FR|1 |  |2 |3 |....|
        //                   ---------------------
        //                   |UK|  |1 |  |2 |....|
        //                   ---------------------
        // The list is {header, rowDefinition, sum}
        //            {GE     , GE           ,    },
        //            {GE     , FR           ,  1 },
        //            {GE     , UK           ,    },
        //            {FR     , UK           ,  1 },
        //            {UK     , GE           ,  2 },
        //            {UK,    , FR,          ,  2 }

        #region fields
        IList<IDataTableEntity> _list;
        #endregion

        #region constructor
        public DataTableHTMLFactory(IList<IDataTableEntity> list) {
            _list = list;
        }
        #endregion

        #region public methods

        public string getDataTableAsString() {

            return getDataTableAsString(" ");
        }
        /// <summary>
        /// Returns the List as HTML content also carries a function at the bottom that carries out a postback using __doPostBack
        /// coded as "vehiclesAbroadTable" and args - where the args are 'duewwd, ownwwd, val' 
        /// </summary>
        /// <param name="firstColumn">The name of the first column</param>
        /// <returns>string</returns>
        public string getDataTableAsString(string firstColumn, bool clickable = true) {

            Dictionary<string, string> totals = new Dictionary<string, string>(); // keeps the totals in order for the bottom of the table

            StringBuilder html = new StringBuilder("<table cellspacing='1' cellpadding='1' border='1' style='background-color:White;border-color:#999999;border-width:1px;border-style:Solid;border-collapse:collapse;width:98%'>");
            html.Append("<tr  style='color:White;background-color:Black;font-size:Small;font-weight:bold;'>");
            html.Append("<th>" + firstColumn + "</th>");
            var cols = _list.Select(p => p.header).Distinct();
            int colSize = 100 / (cols.Count() + 1); // work out the size in % of the columns for uniformity

            foreach (var col in cols) {

                if (col != null) { // check the column is not null
                    html.Append(@"<th style='width:" + colSize.ToString() + @"%'>" + col + "</th>"); // column size tag added
                    totals.Add(col, getTotal(col).ToString()); // add the totals in order for the bottom of the table
                }
            }
            html.Append("</tr>");
            var rows = _list.Select(p => p.rowDefinition).Distinct();

            foreach (var row in rows) {

                html.Append("<tr style='color:Black;background-color:#EEEEEE;font-size:Small;'>");
                html.Append("<td>" + row + "</td>");
                foreach (var col in cols) {
                    var grp = from p in _list where p.rowDefinition == row && p.header == col select p;
                    html.Append("<td>");
                    foreach (var item in grp) html.Append(constructTd(row, col, string.IsNullOrEmpty(item.theValue) ? "" : item.theValue, clickable));
                    html.Append("</td>");
                }
                html.Append("</tr>");
            } // end of the foreach
            html.Append("<tr  style='color:White;background-color:Black;font-size:Small;font-weight:bold;'>");

            // loop through the totals
            html.Append("<td>Total</td>");
            for (int index = 0; index < totals.Count; index++) {
                html.Append("<td>");

                // Contruct the clickable event with the destination being all countries
                html.Append(constructTd("***All***", totals.ElementAt(index).Key, totals.ElementAt(index).Value, clickable));
                html.Append("</td>");
            }
            html.Append("</tr>");
            html.Append("</table>");
            return html.ToString();
        }
        #endregion

        #region helper methods

        private string constructTd(string duewwd, string ownwwd, string val, bool clickable) {
            // _doPostBack method is constructed to invoke the creation of the VehicleDetails which has four arguments:
            // duewwd As String which is the destination country
            // ownwwd As String which is the owning country
            // val which is the value in the cell
            StringBuilder stb = new StringBuilder();
            if (clickable) {
                stb.Append(@"<div onclick=""__doPostBack('DataTableOverview', '" + duewwd + "," + ownwwd + "'");
                stb.Append(@");$get('UpdateProgress1').style.display = 'block';");
                stb.Append(@""" class='GridViewCell'>" + (string.IsNullOrEmpty(val) ? "" : val) + "</div>");
            }
            else {
                stb.Append(@"<div class='GridViewCell'>" + (string.IsNullOrEmpty(val) ? "" : val) + "</div>");
            }
            return stb.ToString();
        }

        private int getTotal(string ownCountry) { // Get the total for each OwnCountry          
            return _list.Where(p => p.header.Equals(ownCountry)).Sum(w => Convert.ToInt32(w.theValue == "" ? "0" : w.theValue));
        }
        #endregion
    }
}