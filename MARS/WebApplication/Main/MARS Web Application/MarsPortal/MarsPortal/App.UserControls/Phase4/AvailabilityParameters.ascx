<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailabilityParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.AvailabilityParameters" %>

<%@ Register TagPrefix="uc" TagName="VehicleParameters" Src="~/App.UserControls/Phase4/VehicleParameters.ascx" %>
<%@ Register TagPrefix="uc" TagName="MultiVehicleParameters" Src="~/App.UserControls/Phase4/MultiSelectVehicleParameters.ascx" %>

<style type="text/css">
    .DateTypeSelection {
    }
</style>

<asp:HiddenField runat="server" ID="hfMultiParameterOption" Value="False" />


<table>
    <tr>
        <td>Select Criteria:
        </td>
        <td style="float: right;">Basic Parameters                
                    <input type="checkbox" id="BasicParamSelection" checked="checked" title="Uncheck for Advanced Parameters" />
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
                    <uc:MultiVehicleParameters runat="server" ID="vhMultiParams" Visible="False" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td>
            <table style="width: 523px; text-align: left;" >
                <tr style="vertical-align: top;">
                    <td>
                        <table >
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    Fleet:
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbFleet" Width="95px" SelectionMode="Multiple" 
                                        CssClass="MultiDropDownListShowSelected" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan ="2" style="padding-top: 5px;">
                                    <asp:CheckBox runat="server" ID="cbExcludeLongtermForOffAirport" CssClass="SingleDropDownList"
                                        Visible="False" Text="Exclude Longterm for off-airport" TextAlign="Left" Width="180px"/>
                                </td>
                            </tr>
                            <tr class="separate-below" style="vertical-align: top;">

                                <td>
                                    <asp:Label runat="server" ID="lblPercentOrValues" Text="Figures:" Visible="False" />
                                    <asp:Label runat="server" ID="lblArea" Text="Owning Area:" Visible="False" />
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblPercentOrValues" Visible="False"
                                        RepeatDirection="Horizontal" />
                                    <div style="width: 160px;">
                                    <rad:AutoComplete ID="acOwningArea" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False" Visible="False"
                                        WebServiceUrl="~/AutoComplete.asmx/OwningAreaAutoComplete"
                                        Width="140px" />
                                    </div>
                                </td>

                            </tr>
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label runat="server" ID="lblShowValuesAs" CssClass="DayGrouping" Text="Show Values As:" />
                                    <asp:Label ID="lblRevenueStatus" runat="server" Text="Revenue Status:" Visible="False" />
                                    
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblShowValuesAs" CssClass="DayGrouping" RepeatDirection="Horizontal" />
                                    <asp:DropDownList runat="server" ID="ddlRevenueStatus" SelectionMode="Single" Visible="False"
                                        CssClass="SingleDropDownList">
                                        <asp:ListItem Text="All" Value="1" />
                                        <asp:ListItem Text="Non Rev" Value="" Selected="True" />
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="vertical-align: top; border-collapse: separate; ">
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label ID="lblOperationalStatuses" runat="server" Text="Operational Status:" Visible="False" />
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbOperationalStatus" Width="95px" Visible="False"
                                        SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                                </td>
                            </tr>

                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label ID="lblMovementTypes" runat="server" Text="Movement Type:" Visible="False" />
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbMovementType" Width="95px" Visible="False"
                                        SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                                </td>
                            </tr>
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label runat="server" ID="lblMinDaysNonRev" Text="Min Days Non Rev:" Visible="False" />
                                    <asp:Label runat="server" ID="lblMinDaysInCountry" Text="Min Days in Country:" Visible="False" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbMinDaysNonRev" CssClass="AutoCompleteTextBox" Width="155px" Visible="False" />
                                    <asp:TextBox runat="server" ID="tbMinDaysInCountry" CssClass="AutoCompleteTextBox" Width="155px" Text="0" Visible="False" />
                                </td>
                            </tr>
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label runat="server" ID="lblOverdue" Text="Exclude Overdue:" Visible="False" />

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlOverdue" Visible="False" CssClass="AutoCompleteTextBox" Width="160px">
                                        <asp:ListItem Text="Yes" Value="1" />
                                        <asp:ListItem Text="No" Value="" Selected="True" />
                                        <asp:ListItem Text="Exclusive" Value="-1" />
                                    </asp:DropDownList>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr class="separate-below" style="vertical-align: top;">
                                <td>
                                    <asp:Label runat="server" ID="lblForeignPredicament" Text="Predicament:" Visible="False" />    
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlForeignPredicament" Visible="False" Width="260px"
                                            CssClass="AutoCompleteTextBox" />    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CompareValidator runat="server" ControlToValidate="tbMinDaysNonRev" Type="Integer"
                            Operator="DataTypeCheck" ForeColor="Red" ErrorMessage="Min Days Non Rev must be numeric" />
                        <asp:CompareValidator runat="server" ControlToValidate="tbMinDaysInCountry" Type="Integer"
                            Operator="DataTypeCheck" ForeColor="Red" ErrorMessage="Min Days In Country must be numeric" />
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

            <asp:Panel runat="server" ID="pnlSingleDate" Visible="False">
                <table style="margin-left: 0; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">
                    <tr>
                        <td>Date:
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox SingleDateBox"
                                ID="tbSingleDate"
                                Text="" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDateSelectorFields">
                <table style="margin-left: 0; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">

                    <tr>
                        <td colspan="4">

                            <div class="ReportDateRange">
                                <table>
                                    <tr>
                                        <td>Duration:
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="AutoCompleteTextBox DateRangeCheck"
                                                ID="ddlDateRangeDuration"
                                                Width="120px">
                                                <asp:ListItem Text="Between" Value="" Selected="True" />
                                                <asp:ListItem Text="Today" Value="0" />
                                                <asp:ListItem Text="7 Days Ending" Value="7" />
                                                <asp:ListItem Text="30 Days Ending" Value="30" />
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <div class="FromDateSelection">
                                                <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox CustomDate FromDateBox"
                                                    ID="tbFromDate"
                                                    Text="" />

                                            </div>
                                        </td>

                                        <td>
                                            <label class="DateRangeLabel">to</label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox CustomDate ToDateBox"
                                                ID="tbToDate"
                                                Text="" />

                                        </td>
                                        <td>
                                            <label class="DayOfWeek">Day of Week</label>

                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlDayOfWeek" CssClass="AutoCompleteTextBox DayOfWeek" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblTopics" Text="Topic:" Visible="False" />
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTopics" Width="200px" Visible="False" CssClass="AutoCompleteTextBox" />
                                        </td>
                                    </tr>
                                </table>
                            </div>


                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="divDayGrouping">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" CssClass="dayGroupingLbl" ID="lblDayGrouping" Text="Overall:" />
                                        </td>
                                        <td>

                                            <asp:RadioButtonList runat="server" ID="rblDayGrouping" RepeatDirection="Horizontal" CssClass="dayGroupingRbl" />

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
            <div id="dvVehicleFieldsHr" runat="server">
                <hr />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">

            <asp:Panel ID="pnlIndividualVehicleFields" runat="server">
                <asp:UpdatePanel ID="upIndividualVehicleFields" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="margin-left: 0; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">
                            <tr>
                                <td>VIN:
                                </td>
                                <td>
                                    <rad:AutoComplete ID="acVin" runat="server" CssClass="AutoCompleteTextBox"
                                        AutoPostBack="False"
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteVinAll"
                                        Width="160px" />

                                </td>
                                <td>Licence:
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
                                <td>Customer:
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
                                        WebServiceUrl="~/AutoComplete.asmx/VehcileAutoCompleteColour"
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


    function QuickSelectMultiple() {
        var evt = window.event;
        var updatePanelId = "<%= upnlParameters.ClientID %>";

        //If Mousedown or enter or tab pressed
        if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
            __doPostBack(updatePanelId, "LocationMultiple CarGroupMultiple");
        }
    }


</script>

<script>
    $(document).ready(function () {

        $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" });
        $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" });
        $(".SingleDateBox").datepicker({ dateFormat: "dd/mm/yy" });


        $(".CustomDate").change(function () {
            dateChanged();
        });

        function dateChanged() {

            var d = new Date();
            var dateString =
                    ("0" + d.getDate()).slice(-2) + "/" +
                    ("0" + (d.getMonth() + 1)).slice(-2) + "/" +
                    d.getFullYear();


            $("#divDayGrouping").show();
            $(".DayOfWeek").show();
            $(".DayGrouping").show();

            if ($(".FromDateBox").val() == dateString &&
                    $(".ToDateBox").val() == dateString && $(".DateRangeCheck").val() == "0") {

                $("#divDayGrouping").hide();
                $(".DayOfWeek").hide();
                $(".DayGrouping").hide();
            }
        }

        $(".DateRangeCheck").change(function () {
            var selected = $(this).val();

            $(".FromDateBox").show();
            $(".ToDateBox").show();
            $("#divDayGrouping").show();

            $(".DayOfWeek").show();
            $(".DayGrouping").show();

            if (selected == "") {
                $(".DateRangeLabel").text("and");
                dateChanged();
            } else {
                if (selected == "0") {
                    $(".DateRangeLabel").text("");
                    $(".ToDateBox").hide();
                    $(".FromDateBox").hide();
                    $("#divDayGrouping").hide();
                    $(".DayOfWeek").hide();
                    $(".DayGrouping").hide();
                } else {
                    $(".DateRangeLabel").text("to");
                    $(".DayGrouping").show();
                    $(".FromDateBox").hide();
                }
            }
        });


        $('#<%= rblShowValuesAs.ClientID %>').change(function () {
            var dayGrouping = $('#<%= rblDayGrouping.ClientID %>');
            var dgAvgButton = dayGrouping.find("input[value='Average']");
            var dgMaxButton = dayGrouping.find("input[value='Max']");
            var dgMinButton = dayGrouping.find("input[value='Min']");


            if ($(".BasicParamSelection").is(':checked')) {
                var avgButton = $('#<%= rblShowValuesAs.ClientID %>').find("input[value='Average']");
                var troughButton = $('#<%= rblShowValuesAs.ClientID %>').find("input[value='Trough']");
                var peakButton = $('#<%= rblShowValuesAs.ClientID %>').find("input[value='Peak']");



                if (avgButton.is(':checked')) {
                    dgAvgButton.attr("checked", "checked");
                }
                if (troughButton.is(':checked')) {
                    dgMinButton.attr("checked", "checked");
                }
                if (peakButton.is(':checked')) {
                    dgMaxButton.attr("checked", "checked");
                }
            }
        });

        function BasicParametersChanged() {
            var dayGrouping = $('#<%= rblDayGrouping.ClientID %>');
            var showValuesAs = $('#<%= rblShowValuesAs.ClientID %>');
            var minButton = showValuesAs.find("input[value='Min']");
            var maxButton = showValuesAs.find("input[value='Max']");

            if (this.checked) {
                minButton.hide();
                var label = $("label[for='" + minButton.attr('id') + "']");
                label.hide();
                maxButton.hide();
                label = $("label[for='" + maxButton.attr('id') + "']");
                label.hide();

                $('#<%= lblDayGrouping.ClientID %>').hide();
                dayGrouping.hide();
            } else {
                $('#<%= lblDayGrouping.ClientID %>').show();
                showValuesAs.find("input[value='Min']").show();
                showValuesAs.find("input[value='Max']").show();
                minButton.show();
                var label2 = $("label[for='" + minButton.attr('id') + "']");
                label2.show();
                maxButton.show();
                label = $("label[for='" + maxButton.attr('id') + "']");
                label.show();
                dayGrouping.show();
            }
            __doPostBack("<%= upnlParameters.ClientID %>", "BasicParametersChanged");
        }

        $("#BasicParamSelection").change(function () {

            BasicParametersChanged();

        });


        SetupBasicParamsForFirstLoad();

        function SetupBasicParamsForFirstLoad() {
            var dayGrouping = $('#<%= rblDayGrouping.ClientID %>');
            var showValuesAs = $('#<%= rblShowValuesAs.ClientID %>');
            var minButton = showValuesAs.find("input[value='Min']");
            var maxButton = showValuesAs.find("input[value='Max']");

            minButton.hide();
            var label = $("label[for='" + minButton.attr('id') + "']");
            label.hide();
            maxButton.hide();
            label = $("label[for='" + maxButton.attr('id') + "']");
            label.hide();

            $('#<%= lblDayGrouping.ClientID %>').hide();
            dayGrouping.hide();
        }


        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
            }
            $("#BasicParamSelection").change(function () {

                BasicParametersChanged();

            });
            dateChanged();
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
                close: function (event, ui) {
                    var elements = $(this).find(":selected");

                    //if (elements.length != 0) {
                    __doPostBack("<%= upnlParameters.ClientID %>", $(this).attr('id'));

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
