<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverviewVehicleHistory.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.OverviewVehicleHistory" %>


<div style="width: 830px; height: 390px; overflow: auto; text-align: left; background-color: white;" class="insetBorders">
    <asp:TreeView ID="tvHistory" runat="server">
        <RootNodeStyle CssClass="TreeViewHeader"></RootNodeStyle>
    </asp:TreeView>
</div>
