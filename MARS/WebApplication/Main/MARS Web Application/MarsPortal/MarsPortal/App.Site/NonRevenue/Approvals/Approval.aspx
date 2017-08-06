<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approval.aspx.cs"
    MasterPageFile="~/App.MasterPages/Mars.Master"
    Inherits="Mars.App.Site.NonRevenue.Approvals.Approval" %>


<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewGrid.ascx" TagName="NonRevOverviewGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewVehicle.ascx" TagName="NonRevOverviewVehicle" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/NonRev/OverviewVehicleHistory.ascx" TagName="NonRevOverviewVehicleHistory" TagPrefix="uc" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server" class="">
    <style type="text/css">
    </style>
</asp:Content>


<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <div id="tabbedPanel" style="width: 1100px; margin-left: auto; margin-right: auto; text-align: center; background-color: transparent;">
        <ul>
            <li style="float: right !important;">
                <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                    <ContentTemplate>
                        Fleet Update: &nbsp;
                            <asp:Label ID="lblLastUpdate" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
            <li><a href="#tabs-1">Approval</a></li>
            <li><a href="#tabs-2">Approval History</a></li>
            <li style="text-align: center !important; width: 75%;">
                <h1>Approval</h1>
            </li>
        </ul>
        <table width="100%" style="text-align: center;">
            <tr>
                <td style="vertical-align: top;">
                    <div id="tabs-1">

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfApprovalRequestedDateTime" />
                                <table style="margin-left: auto; margin-right: auto; text-align: left;">
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Button runat="server" ID="btnLoad" Text="Load" Width="70px" OnClick="btnLoad_Click" CssClass="StandardButton" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table>
                                                <tr>
                                                    <td>Owning Country:</td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlOwningCountry" CssClass="SingleDropDownList"
                                                            SelectionMode="Single" />
                                                    </td>
                                                    <td>Location Country:</td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlLocationCountry" CssClass="SingleDropDownList"
                                                            SelectionMode="Single" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Operating Status:</td>
                                                    <td>
                                                        <select runat="server" id="lbOperationalStatus" class="MultiDropDownListShowSelected" multiple="True"></select>
                                                    </td>
                                                    <td>Fleet:
                                                    </td>
                                                    <td>
                                                        <select runat="server" id="lbFleet" class="MultiDropDownListShowSelected" multiple="True"></select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Minimum Days Non Rev:</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="tbMinDaysNonRev" Width="155px" CssClass="AutoCompleteTextBox" Text="30" />

                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CompareValidator runat="server" ControlToValidate="tbMinDaysNonRev" Type="Integer"
                                                            Operator="DataTypeCheck" ForeColor="Red" ErrorMessage="Value must be an integer!" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                                <table style="height: 360px;">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:NonRevOverviewGrid runat="server" ID="ucOverviewGrid" ShowApproveButton="True"
                                                ShowMultiSelectTickBoxes="False" />
                                        </td>
                                    </tr>
                                </table>

                                <asp:Label runat="server" ID="lblUploadMessage" Font-Size="14" ForeColor="Black"
                                    Text="All selected vehicles are marked Approved" />

                                <asp:Panel ID="pnlNonRevOverview" runat="server" CssClass="Phase4ModalPopup">
                                    <div id="NonRevOverviewTabs">
                                        <table>
                                            <tr>
                                                <td>
                                                    <ul>
                                                        <li><a href="#tabs-3">Details</a></li>
                                                        <li><a href="#tabs-4">History</a></li>
                                                    </ul>
                                                </td>
                                                <td>
                                                    <div style="float: right;">
                                                        <asp:ImageButton runat="server" ID="ibClose"
                                                            ImageUrl="~/App.Images/Icons/close.png" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="tabs-3">
                                                        <uc:NonRevOverviewVehicle runat="server" ID="ucOverviewVehicle"
                                                            ShowMultiSelectTickBoxes="False" />
                                                    </div>
                                                    <div id="tabs-4">
                                                        <uc:NonRevOverviewVehicleHistory runat="server"
                                                            ID="ucOverviewVehicleHistory" />
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
                        </asp:UpdatePanel>
                    </div>

                    <div id="tabs-2">

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table style="margin-left: auto; margin-right: auto;">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Button runat="server" ID="btnLoadApproved" Text="Load" Width="70px" OnClick="btnLoadApproved_Click"
                                                            CssClass="StandardButton" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; text-align: left;">
                                                        <table>
                                                            <tr>
                                                                <td>Owning Country:</td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" ID="ddlApprovedOwningCountry" CssClass="SingleDropDownList"
                                                                        SelectionMode="Single" />
                                                                </td>
                                                                <td>Location Country:</td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" ID="ddlApprovedLocationCountry" CssClass="SingleDropDownList"
                                                                        SelectionMode="Single" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>Month:
                                                                </td>
                                                                <td>
                                                                    <input runat="server" id="ipMonthSelected" style="width: 155px;"
                                                                        class="AutoCompleteMonthPicker" readonly="readonly" />
                                                                </td>
                                                                <td colspan="2">&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td>
                                            <%--                                                        <asp:ListBox runat="server" ID="lbApprovalHistory" Width="500px" Height="200px"
                                                            OnSelectedIndexChanged="lbApprovalHistory_SelectionChanged" AutoPostBack="True" />
                                            --%>
                                            <asp:GridView ID="gvApprovalHistory" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" Width="600px"
                                                OnRowCommand="gvApprovalHistory_Edit" CssClass="GridViewStyle">
                                                <HeaderStyle CssClass="GridHeaderStyle" />
                                                <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                                <RowStyle CssClass="GridRowStyle" />
                                                <EditRowStyle CssClass="GridEditRowStyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="VehicleNonRevApprovalId" Visible="False" />
                                                    <asp:BoundField DataField="UserId" HeaderText="Approved By">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ApprovedOn" HeaderText="Approved On">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LocationCountry" HeaderText="Location Country">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OwningCountry" HeaderText="Owning Country">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MinimumDaysNonRev" HeaderText="Min Days Non Rev" DataFormatString="{0:#,0}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VehiclesApproved" HeaderText="Vehicles Approved" DataFormatString="{0:#,0}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="false" CommandName="EditItem"
                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VehicleNonRevApprovalId")%>'
                                                                Text="Show" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel runat="server" ID="pnlHistory" Visible="False">
                                                <table style="text-align: center; margin-left: auto; margin-right: auto;">
                                                    <tr>

                                                        <td>
                                                            <div style="overflow-y: scroll; height: 300px; vertical-align: top;">
                                                                <asp:GridView runat="server" ID="gvHistory" Width="750px" AutoGenerateColumns="False">
                                                                    <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                                                                    <RowStyle CssClass="StandardDataGrid" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial">
                                                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CarGroup" HeaderText="Group" SortExpression="CarGroup">
                                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="LicensePlate" HeaderText="Plate" SortExpression="LicensePlate">
                                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="UnitNumber" HeaderText="Unit" SortExpression="UnitNumber">
                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ModelDescription" HeaderText="Desc" SortExpression="ModelDescription">
                                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="LastChangeDateTime" HeaderText="LstDate" SortExpression="LastChangeDateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="OperationalStatusCode" HeaderText="Stat" SortExpression="OperationalStatusCode">
                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="MovementTypeCode" HeaderText="Type" SortExpression="MovementTypeCode">
                                                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="VehicleFleetTypeName" HeaderText="Fleet Type" SortExpression="VehicleFleetTypeName">
                                                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="NonRevDays" HeaderText="Days Non Rev" SortExpression="NonRevDays">
                                                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                        <td style="vertical-align: top;">
                                                            <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>

        </table>
        <%--                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbApproval" />
                            <asp:AsyncPostBackTrigger ControlID="lbHistory" />
                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
    </div>


    <script type="text/javascript">
        
        $(function () {
            $("#tabbedPanel").tabs({ selected: <%= SelectedTab %> });
            $(".ui-tabs-panel").css("background", "none");
        });

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

            function EndRequestHandler(sender, args) {
               
                $('#NonRevOverviewTabs').tabs();

                $(".MoreRemarkDetails").mouseover(function (e) {
                    var a = $(this).attr("longdesc");
                    var t = $(this).attr("itemprop");
                    $("#lblReason").text(a);
                    $("#lblRemark").text(t);

                    var pos = $(".ReasonRepeaterHolder").position();


                    var relX = pos.left + 230;
                    var relY = pos.top;
                    $('#divReasonHover').css({ 'left': relX, 'top': relY });
                    $("#divReasonHover").show();


                });
                $(".MoreRemarkDetails").mouseout(function () {
                    $("#divReasonHover").hide();
                });
            }


            function applyMultiSelects() {
                $('.MultiDropDownList').multiselect({
                    autoOpen: false,
                    minWidth: 160,
                    position: {
                        my: 'left bottom',
                        at: 'left top'
                    }
                    
                });

                $('.MultiDropDownListShowSelected').multiselect({
                    autoOpen: false,
                    minWidth: 160,
                    selectedList: 20,
                    position: {
                        my: 'top',
                        at: 'top'
                    }
                });

                $(".AutoCompleteMonthPicker").datepicker({
                    dateFormat: 'MM yy',
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,

                    onClose: function (dateText, inst) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).val($.datepicker.formatDate('MM yy', new Date(year, month, 1)));
                    }
                });

                $(".AutoCompleteMonthPicker").focus(function () {
                    $(".ui-datepicker-calendar").hide();

                });
                
            }

            function panelLoaded(sender, args) {
                if (args.get_panelsUpdated().length > 0) {
                    applyMultiSelects();
                }
            }

            applyMultiSelects();
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
