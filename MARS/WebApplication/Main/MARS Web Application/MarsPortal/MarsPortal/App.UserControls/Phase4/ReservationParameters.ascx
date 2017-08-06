<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ReservationParameters" %>

<%@ Register Src="~/App.UserControls/Phase4/Reservations/ReservationMultiVehicleParameters.ascx" TagName="MultiParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Reservations/ReservationVehicleParameters.ascx" TagName="ResVehicleParameters" TagPrefix="uc" %>

<table>
    <tr>
        <td>Select Criteria:
        </td>
        <td style="float: right;">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    Basic Parameters
                    <asp:CheckBox runat="server" ID="cbBasicParams" OnCheckedChanged="cbBasicParams_Checked"
                        Checked="True" AutoPostBack="True" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr />
        </td>
    </tr>
    <tr style="vertical-align: top;" >
        <td colspan="2">
            <asp:UpdatePanel runat="server" ID="upnlParams" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:MultiParameters runat="server" ID="ucMultiParameters" Visible="False" />
                    <uc:ResVehicleParameters runat="server" ID="ucSingleParameters"  />        
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </td>
    </tr>
    <tr>
        <td colspan="2">
                <table style="text-align: center;">
                    <tr>
                        <td colspan="6">

                                    <table style="width: 100%">
                                        <tr style="vertical-align: central;Height: 40px;" >
                                            <td >
                                                <asp:RadioButtonList runat="server" ID="rblCheckOutSelection" RepeatDirection="Horizontal" >
                                                    <asp:ListItem Value="True" Text=" Check Out" Selected="True"/>
                                                    <asp:ListItem Value="False" Text=" Check In" />
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                &nbsp; Between &nbsp;
                                                <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox FromDateBox" 
                                                    ID="tbFromDate"
                                                    Text="" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblTo" Text="And" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox ToDateBox"
                                                    ID="tbToDate"
                                                    Text="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CompareValidator ID="cvFromDate" runat="server" ControlToValidate="tbFromDate"
                                                    ToolTip="Select a From Date"
                                                    ErrorMessage="From Date must be in the future"
                                                    Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                    ValueToCompare='<%# DateTime.Now.ToShortDateString() %>' />
                                                <asp:CompareValidator ID="cvToDate" runat="server" ControlToValidate="tbToDate"
                                                    ToolTip="Select a To Date"
                                                    ErrorMessage="To Date must be in the future"
                                                    Operator="GreaterThan" Type="Date" ForeColor="Red"
                                                    ValueToCompare='<%# DateTime.Now.ToShortDateString() %>' />
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
            <asp:Panel runat="server" ID="pnlIndividualReservationParameters" >
            <table>
                <tr>

                    <td>Res ID:
                    </td>
                    <td>
                        <rad:AutoComplete ID="acExternalId" runat="server" CssClass="AutoCompleteTextBox"
                            AutoPostBack="False"
                            WebServiceUrl="~/AutoComplete.asmx/ReservationAutoCompleteExternalId"
                            Width="120px" />
                    </td>
                    <td>Customer:
                    </td>
                    <td>
                        <rad:AutoComplete ID="acCustomerName" runat="server" CssClass="AutoCompleteTextBox"
                            AutoPostBack="False"
                            WebServiceUrl="~/AutoComplete.asmx/ReservationAutoCompleteCustomerName"
                            Width="120px" />
                    </td>
                    <td>Flight Number:
                    </td>
                    <td>
                        <rad:AutoComplete ID="acFlightNumber" runat="server" CssClass="AutoCompleteTextBox"
                            AutoPostBack="False"
                            WebServiceUrl="~/AutoComplete.asmx/ReservationAutoCompleteFlightNumber"
                            Width="120px" />
                    </td>
                </tr>

            </table>
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

    function keyPressedLocation() {

        if ((event.keyCode == 13) || (event.keyCode == 9)) {

            __doPostBack("<%= upnlParams.ClientID %>", "LocationOut");

        }
    }
    function keyPressedCarGroup() {
        if ((event.keyCode == 13) || (event.keyCode == 9)) {
            __doPostBack("<%= upnlParams.ClientID %>", "CarGroupSingle");

            }
    }

    function UpdateOutLocation() {
            var evt = window.event;
            if (evt.type == "mousedown")
                __doPostBack("<%= upnlParams.ClientID %>", "LocationOut");
    }

    function UpdateInLocation() {
            var evt = window.event;
            if (evt.type == "mousedown")
                __doPostBack("<%= upnlParams.ClientID %>", "LocationIn");
    }

    function UpdateCarGroupSingle() {
            var evt = window.event;
            if (evt.type == "mousedown")
                __doPostBack("<%= upnlParams.ClientID %>", "CarGroupSingle");
    }

    function keyPressedLocationMultiple() {

        if ((event.keyCode == 13) || (event.keyCode == 9)) {
            __doPostBack("<%= upnlParams.ClientID %>", "LocationOutMultiple");
        }
    }
    function keyPressedCarGroupMultiple() {

        if ((event.keyCode == 13) || (event.keyCode == 9)) {
            __doPostBack("<%= upnlParams.ClientID %>", "CarGroupMultiple");
        }
    }

    function UpdateOutLocationMultiple() {
        var evt = window.event;
        if (evt.type == "mousedown")
            __doPostBack("<%= upnlParams.ClientID %>", "LocationOutMultiple");
    }

    function UpdateInLocationMultiple() {
        var evt = window.event;
        if (evt.type == "mousedown")
            __doPostBack("<%= upnlParams.ClientID %>", "LocationInMultiple");
    }
    function UpdateCarGroupMultiple() {
        var evt = window.event;
        if (evt.type == "mousedown")
            __doPostBack("<%= upnlParams.ClientID %>", "CarGroupSingleMultiple");
    }

</script>

<script>
    $(document).ready(function () {

        $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" });
        $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
                $(".FromDateBox").datepicker({ dateFormat: "dd/mm/yy" });
                $(".ToDateBox").datepicker({ dateFormat: "dd/mm/yy" });
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
                close: function (event, ui) {
                    var elements = $(this).find(":selected");

                    //if (elements.length != 0) {
                    __doPostBack("<%= upnlParams.ClientID %>", $(this).attr('id'));

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


    });
</script>


