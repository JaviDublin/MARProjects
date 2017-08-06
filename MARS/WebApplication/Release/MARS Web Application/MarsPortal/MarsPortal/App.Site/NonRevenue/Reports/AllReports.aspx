<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllReports.aspx.cs"
    MasterPageFile="~/App.MasterPages/Mars.Master"
    Inherits="Mars.App.Site.NonRevenue.Reports.AllReports" %>

<%@ Register TagPrefix="uc" TagName="ComparisonGrid" Src="~/App.UserControls/Phase4/NonRev/ComparisonGrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgeingGrid" Src="~/App.UserControls/Phase4/NonRev/AgeingGrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgeingChart" Src="~/App.UserControls/Phase4/NonRev/AgeingChart.ascx" %>
<%@ Register TagPrefix="uc" TagName="NonRevParameters" Src="~/App.UserControls/Phase4/NonRevParameters.ascx" %>
<%@ Register TagPrefix="uc" TagName="VehicleParameters" Src="~/App.UserControls/Phase4/VehicleParameters.ascx" %>
<%@ Register TagPrefix="uc" TagName="ComparisonChart" Src="~/App.UserControls/Phase4/NonRev/ComparisonChart.ascx" %>
<%@ Register TagPrefix="uc" TagName="HistoricalTrendGrid" Src="~/App.UserControls/Phase4/NonRev/HistoricalTrendGrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="HistoricalTrendChart" Src="~/App.UserControls/Phase4/NonRev/HistoricalTrendChart.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReasonHistoryGrid" Src="~/App.UserControls/Phase4/NonRev/ReasonHistoryGrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server" class="">
    <style type="text/css">
        .SelectedTabHolder
        {
        }

        .SelectedSubTabHolder
        {
        }
    </style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">


    <div id="tabbedPanel" style="width: 1100px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
        <ul>
            <li style="float: right !important;">
                <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                    <ContentTemplate>
                        Fleet Update: &nbsp;
                            <asp:Label ID="lblLastUpdate" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
            <li><a href="#tabs-1">Site Comparison</a></li>
            <li><a href="#tabs-2">Fleet Comparison</a></li>
            <li><a href="#tabs-3">Status Comparison</a></li>
            <li><a href="#tabs-4">Ageing</a></li>
            <li><a href="#tabs-5">Historical Trend</a></li>
            <li><a href="#tabs-6">Reason History</a></li>
            <li style="text-align: center !important; width: 20%;">
                            <h1>Reporting</h1>
            </li>
            
        </ul>

        <input runat="server" class="SelectedTabHolder" id="hfSelectedTab" type="hidden" value="0" />
        <input runat="server" class="SelectedSubTabHolder" id="hfSelectedSubTab" type="hidden" value="0" />

        <table style="height: 400px; width: 100%; text-align: center; margin-left: auto; margin-right: auto;">
            <tr>
                <td style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
                    <center>
                        <div id="tabs-1">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab1">Grid</a></li>
                                    <li><a href="#stab2">Chart</a></li>
                                </ul>

                                <div id="stab1">
                                    <asp:UpdatePanel runat="server" ID="upSiteCompGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonGrid ID="ucSiteCompGrid" runat="server" ShowTotalColumn="True" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="stab2">
                                    <asp:UpdatePanel runat="server" ID="upSiteCompChart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonChart ID="ucSiteCompChart" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="tabs-2">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab3">Grid</a></li>
                                    <li><a href="#stab4">Chart</a></li>
                                </ul>

                                <div id="stab3">
                                    <asp:UpdatePanel runat="server" ID="upFleetCompGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonGrid ID="ucFleetCompGrid" runat="server" ShowTotalColumn="True" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="stab4">
                                    <asp:UpdatePanel runat="server" ID="upFleetCompChart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonChart ID="ucFleetCompChart" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="tabs-3">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab5">Grid</a></li>
                                    <li><a href="#stab6">Chart</a></li>
                                </ul>

                                <div id="stab5">
                                    <asp:UpdatePanel runat="server" ID="upAgeCompGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonGrid ID="ucAgeCompGrid" runat="server" ShowTotalColumn="False" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                                <div id="stab6">
                                    <asp:UpdatePanel runat="server" ID="upAgeCompChart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ComparisonChart ID="ucAgeCompChart" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                        <div id="tabs-4">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab7">Grid</a></li>
                                    <li><a href="#stab8">Chart</a></li>
                                </ul>

                                <div id="stab7">
                                    <asp:UpdatePanel runat="server" ID="upAgeingGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:AgeingGrid ID="ucAgeingGrid" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="stab8">
                                    <asp:UpdatePanel runat="server" ID="upAgeingChart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:AgeingChart ID="ucAgeingChart" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="tabs-5">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab9">Grid</a></li>
                                    <li><a href="#stab10">Chart</a></li>
                                </ul>

                                <div id="stab9">
                                    <asp:UpdatePanel runat="server" ID="upHistoricalTrendGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:HistoricalTrendGrid ID="ucHistoricalTrendGrid" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="stab10">
                                    <asp:UpdatePanel runat="server" ID="upHistoricalTrendChart" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:HistoricalTrendChart ID="ucHistoricalTrendChart" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                        <div id="tabs-6">
                            <div class="subtab">
                                <ul>
                                    <li><a href="#stab11">Grid</a></li>
                                </ul>
                                <div id="stab11">
                                    <asp:UpdatePanel runat="server" ID="upReasonHistory" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ReasonHistoryGrid ID="ucReasonHistory" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </center>
                </td>
            </tr>
        </table>



        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td style="text-align: center;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load"
                                Width="80px" OnClick="btnLoad_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td>

                    <uc:NonRevParameters runat="server" ID="nrParams"
                        SetReportView="True" />

                </td>

            </tr>
        </table>

    </div>

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
            $("#tabbedPanel").tabs({
                select: function (event, ui) { //bind click event to link
                    var selectedIndex = ui.index;
                    $(".SelectedTabHolder").val(selectedIndex);
                    if (selectedIndex == 3) {
                        $(".MinDaysSelection").hide();
                    } else {
                        $(".MinDaysSelection").show();
                    }
                    if (selectedIndex == 2 || selectedIndex == 3 || selectedIndex == 4) {
                        $(".KciGroupingSelection").show();
                    } else {
                        $(".KciGroupingSelection").hide();
                    }
                    if (selectedIndex == 4 || selectedIndex == 5) {
                        $(".SingleReportDate").hide();
                        $(".FromDateSelection").hide();

                        $(".ReportDateRange").show();
                        $(".DateRangeLabel").text("to");
                        $(".DateRangeCheck").val('0');

                        $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");
                        $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());
                        $(".ReportDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");

                    } else {
                        $(".SingleReportDate").show();
                        $(".ReportDateRange").hide();
                        $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");
                        $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");

                        if ($(".ReportDateBox").val() == "") {
                            $(".ReportDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());
                        }

                    }
                }
            });

            $(".KciGroupingSelection").hide();
            $(".SingleReportDate").show();
            $(".ReportDateRange").hide();


            $(".subtab").tabs({
                select: function (event, ui) {
                    var selectedIndex = ui.index.toString();


                    $(".SelectedSubTabHolder").val(selectedIndex);
                }
            });
            $(".ui-tabs-panel").css("background", "none");

        });

    </script>

</asp:Content>
