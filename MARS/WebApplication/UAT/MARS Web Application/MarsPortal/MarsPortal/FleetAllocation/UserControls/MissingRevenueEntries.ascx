<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MissingRevenueEntries.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.MissingRevenueEntries" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <fieldset style="width: 400px; height: 120px;">
            <legend>Car Groups found in Holding Cost, not in Revenue:
            </legend>


            <asp:Label runat="server" ID="lblMissingList" />
            <br />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="ReloadMissingEntries" CssClass="StandardButton" />
        </fieldset>

    </ContentTemplate>
</asp:UpdatePanel>
