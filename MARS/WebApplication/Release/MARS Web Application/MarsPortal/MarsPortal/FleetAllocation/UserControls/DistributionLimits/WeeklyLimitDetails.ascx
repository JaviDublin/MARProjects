<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeeklyLimitDetails.ascx.cs" 
    Inherits="Mars.FleetAllocation.UserControls.DistributionLimits.WeeklyLimitDetails" %>


<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>


<table>
    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucWeeklyLimit" AutoGridWidth="400" />
        </td>
    </tr>

</table>

