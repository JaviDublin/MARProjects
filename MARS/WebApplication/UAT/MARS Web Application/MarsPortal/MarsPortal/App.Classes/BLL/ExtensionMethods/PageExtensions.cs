using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using App.BLL.ExtensionMethods;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.Entities.Graphing.Parameters;
using App.UserControls.Parameters;
using Mars.App.Classes.DAL.Data;

//using App.Parameters;

namespace Mars.App.Classes.BLL.ExtensionMethods
{
    internal static class PageExtensions
    {
        internal static void CheckQuickLocationGroupSelected(this Page page, List<LocationGroupHolder> locationGroupData,
                                string quickSelectedValue, Dictionary<string, string> selectedParameters
                                , List<ReportParameter> reportParameters)
        {
            //Get the location Group from our collection, if there is no match, the user manually typed in one that doesn't exist, return
            var seletedLocationGroup = locationGroupData.FirstOrDefault(lg => String.Equals(lg.LocationGroupName, quickSelectedValue, StringComparison.CurrentCultureIgnoreCase));
            if (seletedLocationGroup == null) return;

            selectedParameters.AddOrUpdateInDictionary(ParameterNames.Country, seletedLocationGroup.Country);
            selectedParameters.AddOrUpdateInDictionary(ParameterNames.Pool, seletedLocationGroup.PoolId.ToString());

            //Add Country Pool and Location Group to the Dictionary, updating the parameters drop down list as you do so
            var parameter = reportParameters.First(p => p.Name == ParameterNames.Country);
            parameter.ParameterDropDownList.SelectedValue = seletedLocationGroup.Country;

            parameter = reportParameters.First(p => p.Name == ParameterNames.Pool);
            parameter.SetParameterOptions(selectedParameters);
            parameter.ParameterDropDownList.SelectedValue = seletedLocationGroup.PoolId.ToString();

            parameter = reportParameters.First(p => p.Name == ParameterNames.LocationGroup);
            parameter.SetParameterOptions(selectedParameters);
            parameter.ParameterDropDownList.SelectedValue = seletedLocationGroup.LocationGroupId.ToString();

        }

        internal static void CheckQuickBranchSelected(this Page page, List<BranchHolder> branchData,
                        string quickSelectedValue, Dictionary<string, string> selectedParameters
                        , List<ReportParameter> reportParameters, LocationLogic logic)
        {
            //Get the Branch from our collection, if there is no match, the user manually typed in one that doesn't exist, return
            var seletedBranch = branchData.FirstOrDefault(lg => lg.BranchCode.ToLower() == quickSelectedValue.ToLower());
            if (seletedBranch == null) return;

            selectedParameters.AddOrUpdateInDictionary(ParameterNames.Country, seletedBranch.Country);

            var parameter = reportParameters.First(p => p.Name == ParameterNames.Country);
            parameter.ParameterDropDownList.SelectedValue = seletedBranch.Country;
            if (logic == LocationLogic.Cms)
            {
                selectedParameters.AddOrUpdateInDictionary(ParameterNames.Pool, seletedBranch.PoolId.ToString());
                selectedParameters.AddOrUpdateInDictionary(ParameterNames.LocationGroup, seletedBranch.LocationGroupId.ToString());

                parameter = reportParameters.First(p => p.Name == ParameterNames.Pool);
                parameter.SetParameterOptions(selectedParameters);
                parameter.ParameterDropDownList.SelectedValue = seletedBranch.PoolId.ToString();

                parameter = reportParameters.First(p => p.Name == ParameterNames.LocationGroup);
                parameter.SetParameterOptions(selectedParameters);
                parameter.ParameterDropDownList.SelectedValue = seletedBranch.LocationGroupId.ToString();
            }
            else
            {
                selectedParameters.AddOrUpdateInDictionary(ParameterNames.Region, seletedBranch.RegionId.ToString());
                selectedParameters.AddOrUpdateInDictionary(ParameterNames.Area, seletedBranch.AreaId.ToString());

                parameter = reportParameters.First(p => p.Name == ParameterNames.Region);
                parameter.SetParameterOptions(selectedParameters);
                parameter.ParameterDropDownList.SelectedValue = seletedBranch.RegionId.ToString();

                parameter = reportParameters.First(p => p.Name == ParameterNames.Area);
                parameter.SetParameterOptions(selectedParameters);
                parameter.ParameterDropDownList.SelectedValue = seletedBranch.AreaId.ToString();
            }

            parameter = reportParameters.First(p => p.Name == ParameterNames.Branch);
            parameter.SetParameterOptions(selectedParameters);
            parameter.ParameterDropDownList.SelectedValue = seletedBranch.BranchId.ToString();
            
        }

        public static void SortListByPropertyName<T>(this Page p, List<T> list, bool isAscending, string propertyName) where T : IComparable
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            Comparison<T> asc = (t1, t2) => ((IComparable)propInfo.GetValue(t1, null)).CompareTo(propInfo.GetValue(t2, null));
            Comparison<T> desc = (t1, t2) => ((IComparable)propInfo.GetValue(t2, null)).CompareTo(propInfo.GetValue(t1, null));
            list.Sort(isAscending ? asc : desc);
        }

        internal static List<ReportParameter> GetReportParameters(this Page p, LocationLogic logic)
        {
            List<ReportParameter> returned;
            if (logic == LocationLogic.Ops)
            {
                returned = new List<ReportParameter>
                           {
                               new ReportParameter(0, 0, ParameterDataAccess.GetCountryParameterListItems,
                                                   ParameterNames.Country),
                               new ReportParameter(1, 1, ParameterDataAccess.GetOpsRegionListItems,
                                                   ParameterNames.Region),
                               new ReportParameter(1, 2, ParameterDataAccess.GetOpsAreaListItems,
                                                   ParameterNames.Area),
                               new ReportParameter(1, 3, ParameterDataAccess.GetAllBranches, ParameterNames.Branch),
                               new ReportParameter(2, 1, ParameterDataAccess.GetCarSegmentParameterListItems,
                                                   ParameterNames.CarSegment),
                               new ReportParameter(2, 2, ParameterDataAccess.GetCarClassGroupParameterListItems,
                                                   ParameterNames.CarClassGroup),
                               new ReportParameter(2, 3, ParameterDataAccess.GetCarClassParameterListItems,
                                                   ParameterNames.CarClass),
                           };
            }
            else
            {
                returned = new List<ReportParameter>
                           {
                               new ReportParameter(0, 0, ParameterDataAccess.GetCountryParameterListItems,
                                                   ParameterNames.Country),
                               new ReportParameter(1, 1, ParameterDataAccess.GetPoolParameterListItems,
                                                   ParameterNames.Pool),
                               new ReportParameter(1, 2, ParameterDataAccess.GetLocationGroupParameterListItems,
                                                   ParameterNames.LocationGroup),
                               new ReportParameter(1, 3, ParameterDataAccess.GetAllBranches, ParameterNames.Branch),
                               new ReportParameter(2, 1, ParameterDataAccess.GetCarSegmentParameterListItems,
                                                   ParameterNames.CarSegment),
                               new ReportParameter(2, 2, ParameterDataAccess.GetCarClassGroupParameterListItems,
                                                   ParameterNames.CarClassGroup),
                               new ReportParameter(2, 3, ParameterDataAccess.GetCarClassParameterListItems,
                                                   ParameterNames.CarClass),
                           };
            }
            return returned;
        }



        internal static List<ReportParameter> GetDefaultParameters(this Page page)
        {
            return new List<ReportParameter>
                       {
                           new ReportParameter(0, 0, ParameterDataAccess.GetCountryParameterListItems, ParameterNames.Country),
                           new ReportParameter(1, 1, ParameterDataAccess.GetPoolParameterListItems, ParameterNames.Pool),
                           new ReportParameter(1, 2, ParameterDataAccess.GetLocationGroupParameterListItems, ParameterNames.LocationGroup),
                           new ReportParameter(2, 1, ParameterDataAccess.GetCarSegmentParameterListItems, ParameterNames.CarSegment),
                           new ReportParameter(2, 2, ParameterDataAccess.GetCarClassGroupParameterListItems, ParameterNames.CarClassGroup),
                           new ReportParameter(2, 3, ParameterDataAccess.GetCarClassParameterListItems, ParameterNames.CarClass),
                       };
        }

        internal static bool HaveDatesChanged(this Page page, GeneralReportParameters generalReportParameters)
        {
            var fromDate = generalReportParameters.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? generalReportParameters.SelectedParameters[ParameterNames.FromDate] : null;
            var toDate = generalReportParameters.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? generalReportParameters.SelectedParameters[ParameterNames.ToDate] : null;

            if (!generalReportParameters.SelectedParameters.ContainsKey(ParameterNames.PreviousFromDate)) return false;

            return generalReportParameters.SelectedParameters[ParameterNames.PreviousFromDate] != fromDate || generalReportParameters.SelectedParameters[ParameterNames.PreviousToDate] != toDate;
        }

        internal static void SetPreviousDates(this Page page, GeneralReportParameters generalReportParameters)
        {
            var fromDate = generalReportParameters.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? generalReportParameters.SelectedParameters[ParameterNames.FromDate] : null;
            var toDate = generalReportParameters.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? generalReportParameters.SelectedParameters[ParameterNames.ToDate] : null;
            generalReportParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.PreviousFromDate, fromDate);
            generalReportParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.PreviousToDate, toDate);
        }

        internal static string RadUserId(this Page page)
        {
            return Rad.Security.ApplicationAuthentication.GetGlobalId();
            
        }

        internal static string RadUsername(this Page page)
        {
            return Rad.Security.ApplicationAuthentication.GetUserName();
        }
        
    }
}