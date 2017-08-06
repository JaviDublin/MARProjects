using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using App.DAL.Data;
using Rad.Controls;

namespace Application.Webservices
{
    /// <summary>
    /// Summary description for SearchTerm
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SearchTerm : System.Web.Services.WebService
    {

        [WebMethod]
        public List<AutoCompleteItems> SearchTermSerial(string searchTerm)
        {
            object o = AutoCompleteItems.SelectItems(StoredProcedures.SearchTermSerial, searchTerm);
            return o as List<AutoCompleteItems>;
        }

        [WebMethod]
        public List<AutoCompleteItems> SearchTermPlate(string searchTerm)
        {
            object o = AutoCompleteItems.SelectItems(StoredProcedures.SearchTermPlate, searchTerm);
            return o as List<AutoCompleteItems>;
        }

        [WebMethod]
        public List<AutoCompleteItems> SearchTermDriver(string searchTerm)
        {
            object o = AutoCompleteItems.SelectItems(StoredProcedures.SearchTermDriver, searchTerm);
            return o as List<AutoCompleteItems>;
        }
    }
}
