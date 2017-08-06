<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateProgress.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Common.UpdateProgress" %>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="1000">
    <ProgressTemplate>
        <asp:Panel ID="PanelBackgroundCover" runat="server" CssClass="backgroundCover">
        </asp:Panel>
        <asp:Panel ID="PanelLoadData" runat="server" CssClass="loadData">
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; Loading Data.....
                <br />
            <asp:Image ID="Image1" runat="server" CssClass="loadDataImage" ImageUrl="~/App.Images/ajax-loader.gif"
                AlternateText="Please wait..." />
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
