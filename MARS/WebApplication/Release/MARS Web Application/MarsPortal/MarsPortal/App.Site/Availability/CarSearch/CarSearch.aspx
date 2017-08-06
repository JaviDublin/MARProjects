<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true" CodeBehind="CarSearch.aspx.cs" Inherits="Mars.App.Site.Availability.CarSearch.CarSearch" %>

<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewGrid.ascx" TagName="NonRevOverviewGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewVehicle.ascx" TagName="NonRevOverviewVehicle" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/AvailabilityParameters.ascx" TagName="AvailabilityParameters" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">



    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;"  >
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="float: right !important;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                    Last Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        
                        
                        <li style="text-align: center !important; width: 80%;">
                            <h1>Car Search</h1>
                        </li>
                    </ul>
                    
                    <div id="tabs-1" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>

                                    <asp:UpdatePanel ID="upnlMultiview" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <uc:NonRevOverviewGrid runat="server" ID="ucOverviewGrid" ShowMultiSelectTickBoxes="False"
                                                ShowAvailabilityFields="True" ShowApproveButton="False" />

                                            <asp:Panel ID="pnlCarSearch" runat="server" CssClass="Phase4ModalPopup">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="float: right;">
                                                                <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                                                            </div>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <uc:NonRevOverviewVehicle runat="server" ID="ucOverviewVehicle"
                                                                ShowHistory="False" ShowReasonEntry="False" />
                                                        </td>
                                                    </tr>
                                                </table>



                                            </asp:Panel>
                                            <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
                                            <asp:ModalPopupExtender
                                                ID="mpeCarSearch"
                                                runat="server"
                                                PopupControlID="pnlCarSearch"
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

            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <uc:AvailabilityParameters runat="server" ID="generalParams" ShowDayGrouping="False" ShowValuesAs="False" ShowLocationLogic="True"
                    ShowVehicleFields="True" AllowMultiSelect="True"
                    ShowAdditionalFields="False" />

            </td>
        </tr>
    </table>

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

    <script type="text/javascript">


        $(function () {
            $("#tabbedPanel").tabs();


            $(".SingleReportDate").show();
            $(".ReportDateRange").hide();
            $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");
            $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());

            if ($(".ReportDateBox").val() == "") {
                $(".ReportDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());
            }

        });


    </script>


</asp:Content>
