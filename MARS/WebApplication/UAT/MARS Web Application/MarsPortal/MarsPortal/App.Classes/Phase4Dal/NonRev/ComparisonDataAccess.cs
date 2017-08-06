using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using App.BLL.ExtensionMethods;
using App.Entities.Graphing.Parameters;
using Castle.Core.Internal;
using Castle.Windsor.Installer;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class ComparisonDataAccess : NonRevBaseDataAccess
    {
        private bool _siteComparison;

        public ComparisonDataAccess(Dictionary<DictionaryParameter, string> parameters) : base(parameters)
        {
            
        }


        public List<ComparisonRow> GetComparisonByStatusEntries()
        {
            DictionaryParameter groupingType;
            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatusGrouping)
                && Parameters[DictionaryParameter.OperationalStatusGrouping] == "True")
            {
                groupingType = DictionaryParameter.OperationalStatusGrouping;
            }
            else if (Parameters.ContainsKey(DictionaryParameter.KciGrouping) &&
                Parameters[DictionaryParameter.KciGrouping] == "True")
            {
                groupingType = DictionaryParameter.KciGrouping;
            }
            else throw new Exception("Invalid Grouping Type passed to GetComparisonByStatusEntries()");


            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var comparisonData = startDate == DateTime.Now.Date
                ? GetCurrentComparisonData(groupingType)
                : GetHistoryComparisonData(groupingType);


            //comparisonData.Add(BuildTotalRow(comparisonData));
            return comparisonData;
        }

        public List<ComparisonRow> GetComparisonEntries(bool siteComparison = true)
        {
            _siteComparison = siteComparison;
            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            List<ComparisonRow> comparisonData;
            if (startDate == DateTime.Now.Date)
            {
                comparisonData = GetCurrentComparisonData();
                var unmappedRow = BuildUnmappedVehicleRow(_siteComparison);
                if (unmappedRow != null)
                {
                    if (_siteComparison && Parameters.ContainsKey(DictionaryParameter.LocationCountry) && Parameters[DictionaryParameter.LocationCountry] == string.Empty)
                    {
                        comparisonData.Add(unmappedRow); 
                    }
                    if (!_siteComparison && Parameters.ContainsKey(DictionaryParameter.OwningCountry) && Parameters[DictionaryParameter.OwningCountry] == string.Empty)
                    {
                        comparisonData.Add(unmappedRow);
                    }
                }
            }
            else
            {
                comparisonData = GetHistoryComparisonData();
                var unmappedRow = BuildUnmappedVehicleHistoryRow(_siteComparison);
                if (unmappedRow != null)
                {
                    if (_siteComparison && Parameters.ContainsKey(DictionaryParameter.LocationCountry) && Parameters[DictionaryParameter.LocationCountry] == string.Empty)
                    {
                        comparisonData.Add(unmappedRow);
                    }
                    if (!_siteComparison && Parameters.ContainsKey(DictionaryParameter.OwningCountry) && Parameters[DictionaryParameter.OwningCountry] == string.Empty)
                    {
                        comparisonData.Add(unmappedRow);
                    }
                }

                
            }


            //comparisonData.Add(BuildTotalRow(comparisonData));
            comparisonData = comparisonData.OrderBy(d => d.Key).ToList();



            return comparisonData;
        }


        private List<ComparisonRow> GetCurrentComparisonData()
        {
            var comparisonType = _siteComparison ? ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters)
                                    : ComparisonLevelLookup.GetFleetComparisonTypeFromParameters(Parameters);
            var returned = GetCurrentComparisonData(comparisonType);

            return returned;
        }

        private List<ComparisonRow> GetHistoryComparisonData()
        {
            var comparisonType = _siteComparison ? ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters)
                : ComparisonLevelLookup.GetFleetComparisonTypeFromParameters(Parameters);

            var returned = GetHistoryComparisonData(comparisonType);
            
            return returned;

        }



    }
}
