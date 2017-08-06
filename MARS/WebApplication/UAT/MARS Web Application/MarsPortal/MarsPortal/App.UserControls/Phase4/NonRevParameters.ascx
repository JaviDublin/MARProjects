<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonRevParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRevParameters" %>

<%@ Register Src="~/App.UserControls/Phase4/VehicleParameters.ascx" TagName="VehicleParameters" TagPrefix="uc" %>

<style type="text/css">
    .MinDaysSelection
    {
    }

    .KciGroupingSelection
    {
    }

    .SingleReportDate
    {
    }

    .ReportDateRange
    {
    }

    .FromDateSelection
    {
    }

    .DateRangeCheck
    {
    }

    .DateRangeLabel
    {
    }

    .ReportDateBox
    {
    }

    .FromDateBox
    {
    }

    .ToDateBox
    {
    }
</style>


<asp:HiddenField runat="server" ID="hfShowReasonStatus" Value="False" />

<table >
    <tr>
        <td colspan="2">Select Criteria:
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr />
        </td>
    </tr>
    <tr style="vertical-align: top;">
        <td>
            <asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:VehicleParameters runat="server" ID="vhParams" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </td>
        <td>
            <table style="width: 523px; margin-left: auto; margin-right: auto; margin-bottom: 10px; border-collapse: separate; 
                            border-spacing: 6px; text-align: left;">
                <tr style="vertical-align: top;">
                    <td>Fleet:
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lbFleet" Width="95px" SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                    </td>
                    <td>
                        <asp:Label ID="lblRevenueStatus" runat="server" Text="Revenue Status:" />
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlRevenueStatus" SelectionMode="Single"
                                    OnSelectedIndexChanged="ddlRevenueStatus_SelectionChanged"
                                    CssClass="SingleDropDownList" AutoPostBack="True">
                                    <asp:ListItem Text="All" Value="1" />
                                    <asp:ListItem Text="Non Rev" Value="" Selected="True" />
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                </tr>
                <tr style="vertical-align: top;">
                    <td>Owning Area:
                    </td>
                    <td>
                        <rad:AutoComplete ID="acOwningArea" runat="server" CssClass="AutoCompleteTextBox"
                            AutoPostBack="False"
                            WebServiceUrl="~/AutoComplete.asmx/OwningAreaAutoComplete"
                            Width="95px" />

                        <%--<asp:ListBox runat="server" ID="lbOwningArea" Width="95px" SelectionMode="Multiple" CssClass="NoSelectAllMultiDdlList" />--%>
                    </td>

                    <td>
                        <div class="MinDaysSelection">
                            <asp:Label runat="server" ID="lblMinDaysNonRev" Text="Min Days Non Rev:" />
                        </div>
                    </td>
                    <td>
                        <div class="MinDaysSelection">
                            <asp:TextBox runat="server" ID="tbMinDaysNonRev" CssClass="AutoCompleteTextBox" Text="0" Width="155px" />
                        </div>
                        <asp:CompareValidator runat="server" ControlToValidate="tbMinDaysNonRev" Type="Integer"
                            Operator="DataTypeCheck" ForeColor="Red" ErrorMessage="Min Days Non Rev must be numeric" />
                    </td>

                </tr>
                <tr style="vertical-align: top;">
                    <td>Vehicle Status:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="lbFleetStatus" SelectionMode="Single" CssClass="SingleDropDownList">
                            <asp:ListItem Text="Active" Value="1" Selected="True" />
                            <asp:ListItem Text="Active and Defleeted" Value="" />
                        </asp:DropDownList>
                    </td>
                    <td>Operational Status:
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lbOperationalStatus" Width="95px" SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                    </td>

                </tr>
                <tr style="vertical-align: top;">
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Text="Reason Status:" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="lbReasonStatus" SelectionMode="Single" CssClass="SingleDropDownList">
                            <asp:ListItem Text="All" Value="1" Selected="True" />
                            <asp:ListItem Text="No Reasons only" Value="0" />
                        </asp:DropDownList>
                    </td>

                    <td>Movement Type:
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lbMovementType" Width="95px" 
                            SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                    </td>

                </tr>

            </table>

        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div id="dvAdditionalFieldsLabel" runat="server">
                Additional Fields:    
            </div>
            
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div id="dvAdditionalFieldsHr" runat="server">
                <hr />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel runat="server" ID="pnlDateSelectorFields">
    <table style="margin-left: 0; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">
        <tr>
            <td colspan="4">
                <table>
                    <tr>
                        <td>
                            <div class="KciGroupingSelection">
                                <asp:Label runat="server" ID="lblKciOperstatGrouping" Text="Grouping:" />
                            </div>
                        </td>
                        <td>
                            <div class="KciGroupingSelection">
                                <asp:RadioButtonList runat="server" ID="rblKciOperstatGrouping" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Kci" Text="KCI" Selected="True" />
                                    <asp:ListItem Value="OperStat" Text="OperStat" />
                                </asp:RadioButtonList>

                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="SingleReportDate">
                    <table>
                        <tr>
                            <td>
                                <label id="lblDate">Report Date:</label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox ReportDateBox" ID="tbSingleReportDate"
                                    Text="<%# DateTime.Now.Date.ToShortDateString() %>" />
                                <%--<asp:CalendarExtender runat="server" TargetControlID="tbSingleReportDate" Format="dd/MM/yyyy" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="ReportDateRange">
                    <table>
                        <tr>
                            <td>Duration:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" CssClass="AutoCompleteTextBox DateRangeCheck" ID="ddlDateRangeDuration"
                                    Width="120px">
                                    <asp:ListItem Text="1 Day Previous" Value="1" Selected="True" />
                                    <asp:ListItem Text="7 Days Previous" Value="7" />
                                    <asp:ListItem Text="30 Days Previous" Value="30" />
                                    <asp:ListItem Text="Between" Value="" />
                                    <%--<asp:ListItem Text="Previous 90 Days" Value="90" />--%>
                                </asp:DropDownList>
                            </td>

                            <td>
                                <div class="FromDateSelection">
                                    <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox FromDateBox" ID="tbFromDate"
                                        Text="" />
                                    <%--<asp:CalendarExtender runat="server" TargetControlID="tbFromDate" Format="dd/MM/yyyy" />--%>
                                </div>
                            </td>

                            <td>
                                <label class="DateRangeLabel">to</label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox ToDateBox" ID="tbToDate"
                                    Text="" />
                                <%--<asp:CalendarExtender runat="server" TargetControlID="tbToDate" Format="dd/MM/yyyy" />--%>
                            </td>
                        </tr>
                    </table>
                </div>


            </td>
        </tr>
        <tr>
            <td colspan="4">

                <asp:CompareValidator ID="cvToDate" runat="server" ControlToValidate="tbToDate"
                    ToolTip="The date to achieve the objective must be in the future"
                    ErrorMessage="To Date can not be in the future"
                    Operator="LessThanEqual" Type="Date" ForeColor="Red"
                    ValueToCompare='<%# DateTime.Now.ToShortDateString() %>' />
                <asp:CompareValidator ID="cvFromDate" runat="server" ControlToValidate="tbFromDate"
                    ToolTip="The date to achieve the objective must be in the future"
                    ErrorMessage="From Date can not be in the future"
                    Operator="LessThanEqual" Type="Date" ForeColor="Red"
                    ValueToCompare='<%# DateTime.Now.ToShortDateString() %>' />
            </td>
        </tr>
    </table>
</asp:Panel>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <div id="dvVehicleFieldsLabel" runat="server">
                Vehicle Fields:
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div id="dvVehicleFieldsHr" runat="server">
                <hr />
            </div>            
        </td>
    </tr>
    <tr >
        <td colspan="2" >
            <asp:Panel ID="pnlIndividualVehicleFields" runat="server">
                <asp:UpdatePanel ID="upIndividualVehicleFields" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style=" margin-left: 0; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">
                            <tr>
                                <td>Serial:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acVin" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteVinAll"
                                        Width="160px" />

                                </td>
                                <td>Plate:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acLicencePlate" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteLicencePlateAll"
                                        Width="160px" />
                                </td>
                                <td>Model Description:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acModelDesc" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteModelDescriptionAll"
                                        Width="160px" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td>Unit:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acUnitNumber" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteUnitNumberAll"
                                        Width="160px" />
                                </td>
                                <td>Driver Name:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acDriverName" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteDriverNameAll"
                                        Width="160px" />
                                </td>
                                <td>Colour:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acColour" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteColourAll"
                                        Width="155px" />
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </td>
    </tr>
</table>



<script type="text/javascript">
    $(document).keypress(function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });

    function QuickSelectMade() {
        var evt = window.event;
        var updatePanelId = "<%= upnlParameters.ClientID %>";

        //If Mousedown or enter or tab pressed
        if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
            __doPostBack(updatePanelId, "LocationSingle CarGroupSingle");
        }
    }
</script>

<script>
    $(document).ready(function () {

        $(".FromDateSelection").hide();
        $(".ReportDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());

        $(".DateRangeCheck").change(function () {
            var selected = $(this).val();

            if (selected == "") {
                $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", -1);
                $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());
                $(".FromDateSelection").show();
                $(".DateRangeLabel").text("and");

            } else {
                $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", "");

                $(".DateRangeLabel").text("to");
                $(".FromDateSelection").hide();
            }

        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
            }
        }

        function applyMultiSelects() {

            $('.MultiDropDownList').multiselect({
                autoOpen: false,
                minWidth: 160,
                position: {
                    my: 'top',
                    at: 'top'
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

            $('.NoSelectAllMultiDdlList').multiselect({
                autoOpen: false,
                position: {
                    my: 'top',
                    at: 'top'
                },
                minWidth: 160,
                header: '<a class=""ui-multiselect-none"" href=""#""><span class=""ui-icon ui-icon-closethick""></span><span>Uncheck All</span></a></li>',
                click: function (e) {
                    if ($(this).multiselect('widget').find('input:checked').length > 50) {
                        return false;
                    }

                }
            });
        }

        applyMultiSelects();


    });
</script>
