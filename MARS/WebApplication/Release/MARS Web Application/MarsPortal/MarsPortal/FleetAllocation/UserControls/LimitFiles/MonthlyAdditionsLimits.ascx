<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonthlyAdditionsLimits.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.AdditionsLimits.MonthlyAdditionsLimits" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>

<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/MonthlyLimitDetails.ascx" TagPrefix="uc" TagName="MonthlyLimitDetails" %>
<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/WeeklyLimitDetails.ascx" TagPrefix="uc" TagName="WeeklyLimitDetails" %>





<table >
    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucMonthlyAdds" />
        </td>
    </tr>
     <tr>
        <td>
            <uc:MonthlyLimitDetails runat="server" ID="MonthlyLimitDetails" />
        </td>
        <td>
            <uc:WeeklyLimitDetails runat="server" ID="WeeklyLimitDetails" />
        </td>
    </tr>
</table>