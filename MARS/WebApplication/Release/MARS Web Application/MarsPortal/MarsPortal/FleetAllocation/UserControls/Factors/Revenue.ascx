<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Revenue.ascx.cs" 
    Inherits="Mars.FleetAllocation.UserControls.Factors.Revenue" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>

<table style="width: 950px; text-align: center; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" ShowMonthSelector="True" ShowCommercialCarSegment="True" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnLoad" Text="Load" CssClass="StandardButton" OnClick="btnLoad_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            
        </td>
    </tr>
        <tr>
        <td>
            <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfRecordCount" Value="0" />
                    <uc:AutoGrid runat="server" ID="ucMaxFactors" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </td>
    </tr>
</table>