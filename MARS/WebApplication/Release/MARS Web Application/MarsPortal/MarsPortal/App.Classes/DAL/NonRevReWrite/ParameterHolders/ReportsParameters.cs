using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.NonRevReWrite.Enums;

namespace Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders
{
    public class ReportsParameters
    {
        internal bool RemarksReport;
        internal DateTime? SelectedDate;

        internal string Country;
        internal int? CmsPoolId;
        internal int? CmsLocationGroupId;
        internal int? OpsRegionId;
        internal int? OpsAreadId;

        internal string Branch;
        internal int? SegmentId;
        internal int? ClassId;
        internal int? GroupId;
        internal List<string> DayGroupCodes;

        internal int TotalFleetInSelectedDayGroups;

        internal FleetName FleetName;

        internal RemarksCountGroup RemarksCountType;

        internal void SetParametersFromDictionary(Dictionary<string, string> parameters)
        {   
            Country = !parameters.ContainsKey(ParameterNames.Country) || parameters[ParameterNames.Country] == string.Empty ? null : parameters[ParameterNames.Country];
            SegmentId = !parameters.ContainsKey(ParameterNames.CarSegment) || parameters[ParameterNames.CarSegment] == string.Empty
                                   ? (int?) null
                                   : int.Parse(parameters[ParameterNames.CarSegment]);

            ClassId = !parameters.ContainsKey(ParameterNames.CarClassGroup) || parameters[ParameterNames.CarClassGroup] == string.Empty
                                      ? (int?) null
                                      : int.Parse(parameters[ParameterNames.CarClassGroup]);

            GroupId = !parameters.ContainsKey(ParameterNames.CarClass) || parameters[ParameterNames.CarClass] == string.Empty
                                 ? (int?) null
                                 : int.Parse(parameters[ParameterNames.CarClass]);
            CmsPoolId = !parameters.ContainsKey(ParameterNames.Pool) || parameters[ParameterNames.Pool] == string.Empty
                            ? (int?) null
                            : int.Parse(parameters[ParameterNames.Pool]);
            CmsLocationGroupId = !parameters.ContainsKey(ParameterNames.LocationGroup) || parameters[ParameterNames.LocationGroup] == string.Empty
                                     ? (int?) null
                                     : int.Parse(parameters[ParameterNames.LocationGroup]);
            OpsAreadId = !parameters.ContainsKey(ParameterNames.Area) || parameters[ParameterNames.Area] == string.Empty
                            ? (int?)null
                            : int.Parse(parameters[ParameterNames.Area]);
            OpsRegionId = !parameters.ContainsKey(ParameterNames.Region) || parameters[ParameterNames.Region] == string.Empty
                            ? (int?)null
                            : int.Parse(parameters[ParameterNames.Region]);

            Branch = !parameters.ContainsKey(ParameterNames.Branch) || parameters[ParameterNames.Branch] == string.Empty 
                ? null : parameters[ParameterNames.Branch];
            SelectedDate = parameters.ContainsKey(ParameterNames.FromDate) && parameters[ParameterNames.FromDate] != string.Empty
                ? DateTime.Parse(parameters[ParameterNames.FromDate]) 
                : (DateTime?) null;
        }

    }
}