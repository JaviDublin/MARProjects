using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls; // added
using System.Web.UI; // added

namespace App.Classes.BLL.Workers {

    public class VehiclesAbroadWorker {

        public static string NonRevOwnCountry = "";
        public static string NonRevDueCountry = "";

        /// <summary>
        /// Static method the add the availability css to a page
        /// </summary>
        /// <param name="page"></param>
        public static void addCSS(Page page) {

            HtmlLink myHtmlLink = new HtmlLink();           // Add the Availability.css using the HtmlLink control.
            myHtmlLink.Href = "~/App.Styles/Availability.css";
            myHtmlLink.Attributes.Add("rel", "stylesheet");
            myHtmlLink.Attributes.Add("type", "text/css");
            page.Header.Controls.Add(myHtmlLink);           // Add the HtmlLink to the Head section of the page.
        }

        /// <summary>
        /// Static method to direct to a download page
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="page"></param>
        public static void downloadFile(string fileName, string filePath, Page page) { // save fileName to filePath

            page.Response.ContentType = "application/vnd.ms-excel";
            page.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
            page.Response.Redirect(filePath + fileName, false);
            page.Response.End();
        }
    }
}