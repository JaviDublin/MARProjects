<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageInformation.ascx.cs" Inherits="App.UserControls.Pages.PageInformation" %>
<div class="divPageInformation">
    <div class="divPageTitle">
        <asp:Label ID="LabelPageTitle" runat="server" CssClass="labelPageTitle"></asp:Label>
    </div>
    <div class="divPageUpdateInformation">
        <div class="divLeftPageInformation">
            <asp:Label ID="LabelLastUpdateInfo" runat="server" Text="<%$ Resources:lang, LastUpdated %>"></asp:Label>
            <br />
            <asp:Label ID="LabelNextUpdateInfo" runat="server" Visible="false" Text="<%$ Resources:lang, NextUpdateDue %>"></asp:Label>
        </div>
        <div class="divRightPageInformation">
            <asp:Label ID="LabelLastUpdate" runat="server"></asp:Label>
            <br />
            <asp:Label ID="LabelNextUpdate" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
    <div class="divPageOptions">
        <asp:Button ID="ButtonSwitch" runat="server" Visible="false" />
    </div>
    <div class="divClearFloats">
    </div>
</div>
<hr />
