<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master"
    AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="Mars.App.Site.NonRevenue.Overview.Overview" %>


<%@ Register Src="~/App.UserControls/Phase4/NonRevParameters.ascx" TagName="NonRevParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewGrid.ascx" TagName="NonRevOverviewGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewVehicle.ascx" TagName="NonRevOverviewVehicle" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewVehicleHistory.ascx" TagName="NonRevOverviewVehicleHistory" TagPrefix="uc" %>




<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">



    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="float: right !important;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                    Fleet Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li style="text-align: center !important; width: 75%;">
                            <h1>Overview</h1>
                        </li>
                    </ul>
                    <div id="Div1">
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>

                                    <asp:UpdatePanel ID="upnlMultiview" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <uc:NonRevOverviewGrid runat="server" ID="ucOverviewGrid" 
                                                ShowNonRevFields="True"
                                                ShowMultiSelectTickBoxes="True" ShowApproveButton="False" />

                                            <asp:Panel ID="pnlNonRevOverview" runat="server" CssClass="Phase4ModalPopup">

                                                <div id="NonRevOverviewTabs">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <ul style="background: white; width: 100%">
                                                                    <li><a href="#tabs-1">Details</a></li>
                                                                    <li><a href="#tabs-2">History</a></li>
                                                                </ul>

                                                            </td>
                                                            <td>
                                                                <div style="float: right;">
                                                                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                                                                </div>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div id="tabs-1">
                                                                    <uc:NonRevOverviewVehicle runat="server" ID="ucOverviewVehicle" />
                                                                </div>
                                                                <div id="tabs-2">
                                                                    <uc:NonRevOverviewVehicleHistory runat="server" ID="ucOverviewVehicleHistory" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>


                                                </div>


                                            </asp:Panel>
                                            <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
                                            <asp:ModalPopupExtender
                                                ID="mpeNonRevOverview"
                                                runat="server"
                                                PopupControlID="pnlNonRevOverview"
                                                TargetControlID="btnDummy"
                                                DropShadow="True"
                                                BackgroundCssClass="modalBackgroundGray" />

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td></td>
        </tr>
    </table>


    <table style="margin-left: auto; margin-right: auto;">
        <tr>
            <td style="text-align: center;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load" Width="80px" OnClick="btnLoad_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <uc:NonRevParameters runat="server" ID="nrParams" SetOverviewView="True" />
            </td>
        </tr>
    </table>


    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            $("#tabbedPanel").tabs();
            var mouseX;
            var mouseY;
            $(document).mousemove(function (e) {

                mouseX = e.pageX;
                mouseY = e.pageY;
            });

            function EndRequestHandler(sender, args) {
                $('#NonRevOverviewTabs').tabs();

                $(".MoreRemarkDetails").mouseover(function (e) {
                    var a = $(this).attr("longdesc");
                    var t = $(this).attr("itemprop");
                    $("#lblReason").text(a);
                    $("#lblRemark").text(t);

                    var pos = $(".ReasonRepeaterHolder").position();


                    //or $(this).offset(); if you really just want the current element's offset

                    var relX = pos.left + 230;
                    var relY = pos.top;
                    //alert(relX);
                    $('#divReasonHover').css({ 'left': relX, 'top': relY });
                    $("#divReasonHover").show();


                });
                $(".MoreRemarkDetails").mouseout(function () {
                    $("#divReasonHover").hide();
                });
            }

        });

    </script>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="2000">
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
</asp:Content>
