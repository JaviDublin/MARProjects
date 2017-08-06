<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpIcon.ascx.cs" Inherits="Mars.App.UserControls.HelpIcon" %>


<div class="wrap">
    <asp:Image CssClass="StandardBorder" runat="server" ImageUrl="~/App.Images/UserInfo.gif" ID="imgHelpIcon" />
    
    <div class="popupImage" style="position:  fixed;">
        <asp:Image CssClass="StandardBorder" runat="server"  ID="imgHoverImage" />
        <asp:Label runat="server" ID="lblTextDescription" Visible="False" />
    </div>
</div>

