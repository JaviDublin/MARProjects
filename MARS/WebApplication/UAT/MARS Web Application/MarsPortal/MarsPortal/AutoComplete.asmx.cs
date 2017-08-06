using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using App.DAL.Data;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using Mars.App.Classes.DAL.Data;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration;
using Rad.Controls;
using Rad.Security;

namespace MarsV2
{
    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : WebService
    {
        private static List<LocationGroupHolder> _locationData;
        private static List<BranchHolder> _branchData;
        private static List<CarGroupHolder> _carGroupData;

        private static List<CmsPoolHolder> _cmsPool;
        private static List<CmsLocationGroupHolder> _cmsLocGroup;
        private static List<CountryHolder> _ctryData;
        private const string SearchAll = ",all";

        [WebMethod]
        public List<string> GetLocationPoolList(string prefixText, int count)
        {
            if (_locationData == null)
                _locationData = ParameterDataAccess.GetAllLocationPools();

            //Only return those that begin with the prefixText passed in
            var res = (from lg in _locationData
                       where lg.LocationGroupName.ToLower().StartsWith(prefixText.ToLower())
                       select lg.LocationGroupName);
            return res.Take(count).ToList();
        }

        [WebMethod]
        public List<string> GetBranchList(string prefixText, int count)
        {
            if (_branchData == null)
                _branchData = ParameterDataAccess.GetAllBranches();

            //Only return those that begin with the prefixText passed in
            var res = (from lg in _branchData
                       where lg.BranchCode.ToLower().StartsWith(prefixText.ToLower())
                       select lg.BranchCode);
            return res.Take(count).ToList();
        }

        [WebMethod]
        [ScriptMethod]
        public List<string> SearchCarGroup(string prefixText, int count, string contextKey)
        {
            if (_carGroupData == null)
                _carGroupData = ParameterDataAccess.GetCarGroups();

            List<string> returned;
            if (contextKey.Contains(","))
            {
                var splitCountries = contextKey.Split(',');
                returned = (from lg in _carGroupData
                            where lg.CarGroup.ToLower().StartsWith(prefixText.ToLower())
                             && splitCountries.Contains(lg.Country)
                            select lg.Country + "-" + lg.CarGroup).Take(count).ToList();
            }
            else
            {
                //Only return those that begin with the prefixText passed in
                returned = (from lg in _carGroupData
                            where lg.CarGroup.ToLower().StartsWith(prefixText.ToLower())
                             && lg.Country == contextKey
                            select lg.CarGroup).Take(count).ToList();
            }


            return returned;
        }

        [WebMethod]
        [ScriptMethod]
        public List<string> SearchLocationText(string prefixText, int count, string contextKey)
        {
            List<string> returned;
            using(var dataContext = new MappingSingleSelect())
            {
                returned = dataContext.GetAutoCompletedLocations(prefixText, count, contextKey);
            }
            return returned;
        }

        [WebMethod]
        [ScriptMethod]
        public List<string> SearchMembershipDbForRacf(string prefixText, int count)
        {
            var returned = new List<string>();

            var principalContext = new PrincipalContext(ContextType.Domain,
                Mars.Properties.Settings.Default.ActiveDirectoryDomain,
                Mars.Properties.Settings.Default.ADContainer);
            // var userPrincipal = UserPrincipal.FindByIdentity(principalContext, "irop");

            var qbeUser = new UserPrincipal(principalContext);


            var srch = new PrincipalSearcher(qbeUser);
            srch.QueryFilter.SamAccountName = prefixText + "*";
            

            int foundResults = 0;
            foreach (var found in srch.FindAll())
            {
                var de = (DirectoryEntry)found.GetUnderlyingObject();
                var emplyoeeId = de.Properties["employeeID"].Value;
                returned.Add(string.Format("{0} - {1} - {2}", found.SamAccountName, emplyoeeId, found.Name));
                foundResults++;
                if (foundResults == count) break;
            }
            if (foundResults < count)
            {
                var qbeUser2 = new UserPrincipal(principalContext);
                var srch2 = new PrincipalSearcher(qbeUser2)
                            {
                                QueryFilter = {Name = "*" + prefixText + "*"}
                            };
                foreach (Principal found in srch2.FindAll())
                {
                    
                    var de = (DirectoryEntry)found.GetUnderlyingObject();
                    var emplyoeeId = de.Properties["employeeID"].Value;
                    var stringToAdd = string.Format("{0} - {1} - {2}", found.SamAccountName, emplyoeeId, found.Name);
                    if (returned.Contains(stringToAdd)) continue;
                    returned.Add(stringToAdd);
                    foundResults++;
                    if (foundResults == count) break;
                }

            }
            return returned;
        }

        [WebMethod]
        public List<AutoCompleteItems> ReservationAutoCompleteExternalId(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.ReservationAutoCompleteExternalId, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> AutoCompleteLocationWwdCode(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.AutoCompleteLocationWwdCode, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> ReservationAutoCompleteCustomerName(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.ReservationAutoCompleteCustomerName, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> ReservationAutoCompleteFlightNumber(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.ReservationAutoCompleteFlightNumber, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteModelDescription(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteModelDescription, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteModelDescriptionAll(string searchTerm)
        {
            return VehcileAutoCompleteModelDescription(searchTerm + SearchAll);
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteDriverName(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteDriverName, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> OwningAreaAutoComplete(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteAreaCode, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteDriverNameAll(string searchTerm)
        {
            return VehcileAutoCompleteDriverName(searchTerm + SearchAll);
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteVin(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteVin, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteVinAll(string searchTerm)
        {
            return VehcileAutoCompleteVin(searchTerm + SearchAll);
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteLicencePlate(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteLicencePlate, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteLicencePlateAll(string searchTerm)
        {
            return VehcileAutoCompleteLicencePlate(searchTerm + SearchAll);
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteUnitNumber(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteUnitNumber, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteUnitNumberAll(string searchTerm)
        {
            return VehcileAutoCompleteUnitNumber(searchTerm + SearchAll);
        }

        [WebMethod]
        public List<AutoCompleteItems> VehcileAutoCompleteColour(string searchTerm)
        {
            return AutoCompleteItems.SelectItems(StoredProcedures.VehcileAutoCompleteColour, searchTerm).ToList();
        }

        [WebMethod]
        public List<string> GetCountryCode(string prefixText, int count)
        {
            if (_ctryData == null)
                _ctryData = ParameterDataAccess.GetAllCountries();

            //Only return those that begin with the prefixText passed in
            var res = (from cp in _ctryData
                       where cp.CountryCode.ToLower().StartsWith(prefixText.ToLower())
                       select cp.CountryCode);
            return res.Take(count).ToList();
        }


        [WebMethod]
        public List<string> GetCountryNames(string prefixText, int count)
        {
            if (_ctryData == null)
                _ctryData = ParameterDataAccess.GetAllCountries();

            //Only return those that begin with the prefixText passed in
            var res = (from cp in _ctryData
                       where cp.CountryDesc.ToLower().StartsWith(prefixText.ToLower())
                       select cp.CountryDesc);
            return res.Take(count).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> CmsPoolbyCountry(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.CMSPoolAutoComplete, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> CmsLocGroupByCmsPool(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.CMSLocGroupAutoComplete, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> OpsAreabyOpsRegion(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.OpsAreaAutoComplete, searchTerm).ToList();
        }

        [WebMethod]
        public List<AutoCompleteItems> OpsRegionbyCountry(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.OpsRegionAutoComplete, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> CarSegmentbyCountry(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.CarSegmentsAutoComplete, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> CarClassbyCarSegment(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.CarClassesAutoComplete, searchTerm).ToList();
        }


        [WebMethod]
        public List<AutoCompleteItems> CarGroupByCarClass(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.CarGroupAutoComplete, searchTerm).ToList();
        }

        /// <summary>
        /// Get all location names.  The search term will have have the textbox text and
        /// a value for either the CMS Loc Group or OPS Area. The present
        /// value is extracted from searchterm and the appropriate  SPROC is called
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        [WebMethod]
        public List<AutoCompleteItems> LocationsSearch(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");

            var arr = searchTerm.Split(new[] { '|' });

            if (arr[1].Length > 0)
                return AutoCompleteItems.SelectItemsExtra(StoredProcedures.LocationinCMSLocGrpAutoComplete, searchTerm).ToList();
            else
                return AutoCompleteItems.SelectItemsExtra(StoredProcedures.LocationinOPSAreaAutoComplete, searchTerm.Replace("||", "|n|")).ToList();
        }



        /// <summary>
        /// Get all location codes.  The search term will have the textbox text and a value for 
        /// either the CMS Loc Group or OPS Area.   The present value is extracted 
        /// from searchterm and the appropriate SPROC is called.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        [WebMethod]
        public List<AutoCompleteItems> LocationsCodeSearch(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");

            var arr = searchTerm.Split(new[] { '|' });


            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.LocationCodeinCMSLocGrpAutoComplete, searchTerm).ToList();


            //if (arr[1].Length > 0)
            //    return AutoCompleteItems.SelectItemsExtra(StoredProcedures.LocationCodeinCMSLocGrpAutoComplete, searchTerm).ToList();
            //else
            //    return AutoCompleteItems.SelectItemsExtra(StoredProcedures.LocationCodeinOPSAreaAutoComplete, searchTerm.Replace("||", "|n|")).ToList();
        }



        /// <summary>
        ///Search for users that have this racf identifier.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        [WebMethod]
        public List<AutoCompleteItems> UserSearchRacfId(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.UsersbyRacfIdAutoComplete, searchTerm).ToList();
        }

        /// <summary>
        /// Search for users that have this name.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<AutoCompleteItems> UserSearchName(string searchTerm)
        {
            searchTerm = searchTerm.Replace("'", "''");
            return AutoCompleteItems.SelectItemsExtra(StoredProcedures.UsersbyNameAutoComplete, searchTerm).ToList();
        }


    }
}

