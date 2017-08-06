<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true"
    CodeBehind="LocationSummary.aspx.cs" Inherits="Mars.App.Site.ManagerReports.LocationSummary" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <table style="height: 400px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="width: 100%; text-align: center; height: 35px;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="float: right; position: absolute; right: 10px;">
                                        <tr>
                                            <td style="text-align: right;">Fleet Update:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLastUpdate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">Reservation Update:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblReservationUpdate" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <h1>Location Summary</h1>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>

                    </ul>

                <div id="tabs-1">
                        <table width="1050px;">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 90px">Location:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbQuickLocation" Width="155px"
                                                    onkeydown="keyPressedLocation()" />
                                                <asp:AutoCompleteExtender ID="acLocation" runat="server" ServiceMethod="GetBranchList"
                                                    TargetControlID="tbQuickLocation" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                                                    CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="UpdateLocation"
                                                    UseContextKey="True" />

                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlLocationSummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlNoData" runat="server" Visible="False">
                                                <table width="700px" style="text-align: center">
                                                    <tr>
                                                        <td>
                                                            <h1>There is no Vehicle data in the selected Location</h1>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlResults" runat="server" Visible="False">
                                                <table style="text-align: left;">
                                                    <tr>
                                                        <td>
                                                            <table width="700px">
                                                                <tr style="text-align: left;">

                                                                    <td style="width: 90px; ">Location Name:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblLocationName" runat="server" />
                                                                        <asp:HiddenField runat="server" ID="hfLocationId" />
                                                                    </td>
                                                                    <td style="text-align: right;">Date:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDate" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right;">Car Segment:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlCarSegments" AutoPostBack="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <table style="width: 400px;">
                                                                            <tr style="vertical-align: top;">
                                                                                <td style="width: 90px">Fleet:
                                                                                </td>
                                                                                <td colspan="4">
                                                                                    <asp:ListBox runat="server" ID="lbFleet" Width="195px"
                                                                                        SelectionMode="Multiple" AutoPostBack="False"
                                                                                        CssClass="MultiDropDownListShowSelected" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnRefresh" runat="server" Text="Load"
                                                                                        CssClass="StandardButton" OnClick="btnRefresh_Click" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:GridView runat="server" ID="gvSummary" AutoGenerateColumns="False"
                                                                OnRowDataBound="gvSummary_RowDataBound"
                                                                ShowHeaderWhenEmpty="True" ShowFooter="True"
                                                                AllowSorting="True" OnSorting="Gridview_Sorting"
                                                                Width="1040px" CellSpacing="20" CellPadding="20">
                                                                <HeaderStyle CssClass="StandardDataGridHeaderStyle" HorizontalAlign="Center" />
                                                                <FooterStyle CssClass="StandardDataGridHeaderStyle" HorizontalAlign="Center" />
                                                                <RowStyle CssClass="StandardDataGrid" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="CarSegmentName" HeaderText="Car Segment" SortExpression="CarSegmentName"
                                                                        FooterText="Total">
                                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CarClassName" HeaderText="Car Class" SortExpression="CarClassName">
                                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityOp" HeaderText="Operational" SortExpression="AvailabilityOp">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityShop" HeaderText="Shop" SortExpression="AvailabilityShop">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityAvailable" HeaderText="Available" SortExpression="AvailabilityAvailable">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityOnRent" HeaderText="OnRent" SortExpression="AvailabilityOnRent">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityIdle" HeaderText="Idle" SortExpression="AvailabilityIdle">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityOverdue" HeaderText="Overdue" SortExpression="AvailabilityOverdue">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvailabilityUtilization" HeaderText="Utilization" SortExpression="AvailabilityUtilization"
                                                                        DataFormatString="{0:P}">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="NonRevGreaterThanThree" HeaderText="Non Rev > 3" SortExpression="NonRevGreaterThanThree">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="NonRevGreaterThanSeven" HeaderText="Non Rev > 7" SortExpression="NonRevGreaterThanSeven">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReservationCheckInToday" HeaderText="Check In Today" SortExpression="ReservationCheckInToday">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReservationCheckInRemaining" HeaderText="Check In Remaining" SortExpression="ReservationCheckInRemaining">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReservationCheckOutToday" HeaderText="Check Out Today" SortExpression="ReservationCheckOutToday">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReservationCheckOutRemaining" HeaderText="Check Out Remaining" SortExpression="ReservationCheckOutRemaining">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>

                                                            <div>&nbsp;</div>
                                                            <asp:GridView runat="server" ID="gvForeignVehicles" AutoGenerateColumns="False"
                                                                ShowHeaderWhenEmpty="True"
                                                                AllowSorting="False"
                                                                Width="350px" CellSpacing="20" CellPadding="20">
                                                                <HeaderStyle CssClass="StandardDataGridHeaderStyle" HorizontalAlign="Center" />
                                                                <RowStyle CssClass="StandardDataGrid" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="CarSegmentName" HeaderText="Car Segment" SortExpression="CarSegmentName">
                                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="VehicleCount" HeaderText="Foreign Fleet" SortExpression="VehicleCount">
                                                                        <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReservationCount" HeaderText="Foreign Reservations Remaining" SortExpression="ReservationCount">
                                                                        <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr style="float: right; text-align: center;">
                                                        <td>
                                                            <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
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
    
    <asp:UpdateProgress ID="UpdateProgressPopup" runat="server" ClientIDMode="Static" DisplayAfter="1000">
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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
            }


        }
        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        function keyPressedLocation() {
            if (((event.keyCode == 13) || (event.keyCode == 9)) && $("#<%= tbQuickLocation.ClientID %>").val().length > 6) {
                
                __doPostBack("<%= upnlLocationSummary.ClientID %>", "Location");

            }
        }

        function UpdateLocation() {
            var evt = window.event;
            
            if (evt.type == "mousedown" || (event.keyCode == 13) || (event.keyCode == 9))
                __doPostBack("<%= upnlLocationSummary.ClientID %>", "Location");
        }

        $(function () {
            $("#tabbedPanel").tabs();



        });

        function applyMultiSelects() {

            $('.MultiDropDownListShowSelected').multiselect({
                autoOpen: false,
                minWidth: 160,
                close: function (event, ui) {
                    var elements = $(this).find(":selected");

                    //if (elements.length != 0) {
                    __doPostBack("<%= pnlResults.ClientID %>", $(this).attr('id'));

                    ////} 
                },
                click: function (e) {
                    if ($(this).multiselect("widget").find("input:checked").length > 20) {
                        return false;
                    } else {

                    }
                },
                selectedList: 20,
                position: {
                    my: 'top',
                    at: 'top'
                }
            });
        }


        applyMultiSelects();

    </script>
</asp:Content>
