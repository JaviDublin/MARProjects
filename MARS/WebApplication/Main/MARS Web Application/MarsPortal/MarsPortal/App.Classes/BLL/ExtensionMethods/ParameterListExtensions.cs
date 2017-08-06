using System.Collections.Generic;
using System.Linq;
using App.Entities.Graphing.Parameters;

namespace App.BLL.ExtensionMethods
{
    public static class ParameterListExtension
    {
        internal static ReportParameter GetRootParameter(this List<ReportParameter> reportParameters)
        {
            return reportParameters.FirstOrDefault(p => p.OptionIndex == 0);
        }

        internal static ReportParameter GetFirstParameterInBranch(this List<ReportParameter> reportParameters, int branchIndex)
        {
            return reportParameters.FirstOrDefault(p => p.OptionIndex == 1 && p.BranchIndex == branchIndex);
        }

        internal static void ClearAllAboveRoot(this List<ReportParameter> reportParameters)
        {
            var belowList = reportParameters.Where(p => p.OptionIndex != 0).ToList();

            if (belowList.Count > 0)
            {
                belowList.ForEach(p => p.ClearDropdownList());
            }
        }

        internal static void ShowAllParamsInBranch(this List<ReportParameter> reportParameters, int branchId, bool show)
        {
            reportParameters.Where(p => p.BranchIndex == branchId).ToList().ForEach(p => p.IsVisible = show);
        }

        internal static void ShowAllParamsNotInRoot(this List<ReportParameter> reportParameters, bool show)
        {
            reportParameters.Where(p => p.BranchIndex != 0).ToList().ForEach(p => p.IsVisible = show);
        }

        /// <summary>
        /// Clears all drop down lists below the parameter closest to the root for the specified branch
        /// </summary>
        /// <param name="reportParameters"></param>
        /// <param name="branchIndex"></param>
        internal static void ClearBelowHighestChangedParameter(this List<ReportParameter> reportParameters, int branchIndex)
        {
            var branchList = reportParameters.Where(p => p.BranchIndex == branchIndex && p.HasValueChanged).ToList();
            if (branchList.Count == 0) return;

            var lowestChangedIndex = branchList.Min(p => p.OptionIndex);
            var listToClear = reportParameters.Where(p => p.BranchIndex == branchIndex && p.OptionIndex > lowestChangedIndex).ToList();

            if (listToClear.Count == 0) return;

            listToClear.ForEach(p => p.ClearDropdownList());
        }

        internal static bool HasHigherParameterchanged(this List<ReportParameter> reportParameters, ReportParameter reportParameter)
        {
            var branchList = reportParameters.Where(p => p.BranchIndex == reportParameter.BranchIndex
                                                        && p.OptionIndex > reportParameter.OptionIndex
                                                        && p.HasValueChanged
                                                    ).ToList();

            return branchList.Count != 0;
        }

        internal static bool DoesParameterNeedUpdating(this List<ReportParameter> reportParameters, ReportParameter reportParameter)
        {
            if (reportParameter.OptionIndex == 0) return true;

            var aboveParameter = reportParameter.OptionIndex == 1 
                            ? reportParameters.GetRootParameter() 
                            : reportParameters.GetNextLowestParameter(reportParameter);
            return (aboveParameter.HasValueChanged) ? true : false;
        }

        internal static bool HasAnythingInBranchChanged(this List<ReportParameter> reportParameters, int branchIndex)
        {
            var changedParameters = reportParameters.Where(p => p.HasValueChanged && p.BranchIndex == branchIndex).ToList();
            return changedParameters.Count != 0 ? true : false;
        }

        internal static bool HasAnyParameterChanged(this List<ReportParameter> reportParameters)
        {
            var changedParameters = reportParameters.Where(p => p.HasValueChanged).ToList();
            return changedParameters.Count != 0 ? true : false;
        }

        internal static ReportParameter GetNextHighestParameter(this List<ReportParameter> reportParameters, ReportParameter reportParameter)
        {
            return reportParameters.Where(p => p.BranchIndex == reportParameter.BranchIndex
                                                  && p.OptionIndex == reportParameter.OptionIndex + 1).FirstOrDefault();
        }

        internal static ReportParameter GetNextLowestParameter(this List<ReportParameter> reportParameters, ReportParameter reportParameter)
        {
            return reportParameters.FirstOrDefault(p => p.BranchIndex == reportParameter.BranchIndex
                                                        && p.OptionIndex == reportParameter.OptionIndex - 1);
        }
    }
}
