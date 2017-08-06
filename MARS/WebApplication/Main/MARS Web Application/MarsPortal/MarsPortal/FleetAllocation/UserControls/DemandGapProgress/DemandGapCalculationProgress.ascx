<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DemandGapCalculationProgress.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.DemandGapProgress.DemandGapCalculationProgress" %>


<table>
    <tr>
        <td>
            <asp:Label ID="lblCalculateCurrent" runat="server" Text="Calculate Current" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgCurrent" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCalculateMinAndMax" runat="server" Text="Calculate Min and Max" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgMinMax" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblFillDemandGapOneCarGroup" runat="server" Text="Calculate Gap One - Car Group" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgDgOneCg" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblFillDemandGapOneCarClass" runat="server" Text="Calculate Gap One - Car Class" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgDgOneCc" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblFillDemandGapTwoCarGroup" runat="server" Text="Calculate Gap Two - Car Group" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgDgTwoCg" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblFillDemandGapTwoCarClass" runat="server" Text="Calculate Gap Two - Car Class" />
        </td>
        <td>
            <asp:Image runat="server" ID="imgDgTwoCc" ImageUrl="~/App.Images/Mars-Loading.gif"/>
        </td>
    </tr>

</table>