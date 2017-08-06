<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonthlyLimitDetails.ascx.cs" 
        Inherits="Mars.FleetAllocation.UserControls.DistributionLimits.MonthlyLimitDetails" %>

<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>



<table>
    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucMonthlyLimit" AutoGridWidth="400" />            
        </td>
    </tr>

</table>
