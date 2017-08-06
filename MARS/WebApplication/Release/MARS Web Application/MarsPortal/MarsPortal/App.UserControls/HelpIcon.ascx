<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpIcon.ascx.cs" Inherits="Mars.App.UserControls.HelpIcon" %>

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                
                //$('.popupImage').hide();
                //$('.wrap').mouseover(function () {
                //    $('.popupImage').show();
                //}).mouseout(function () {
                //    $('.popupImage').hide();
                //});
            }


        });

    </script>

<div class="wrap">
    <asp:Image CssClass="StandardBorder" runat="server" ImageUrl="~/App.Images/UserInfo.gif" ID="imgHelpIcon" />
    
    <div class="popupImage" style="position:  fixed;">
        <asp:Image CssClass="StandardBorder" runat="server"  ID="imgHoverImage" />    
    </div>
</div>

