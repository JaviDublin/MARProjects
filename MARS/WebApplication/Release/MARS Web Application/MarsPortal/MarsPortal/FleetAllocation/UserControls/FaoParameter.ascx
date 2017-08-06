<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FaoParameter.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.FaoParameter" %>

<%@ Register TagPrefix="uc" TagName="VehicleParameters" Src="~/App.UserControls/Phase4/VehicleParameters.ascx" %>
<%@ Register TagPrefix="uc" TagName="MultiVehicleParameters" Src="~/App.UserControls/Phase4/MultiSelectVehicleParameters.ascx" %>

<asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr>         
                <td style="float: right;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>Basic Parameters                
                            <asp:CheckBox runat="server" ID="cbBasicParameters" AutoPostBack="True" Checked="True"
                                OnCheckedChanged="cbBasicParameters_CheckedChanged"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <uc:VehicleParameters runat="server" ID="vhParams" />
                    <uc:MultiVehicleParameters runat="server" ID="vhMultiParams" Visible="False" />
                </td>
                <td style="vertical-align: top; ">
                    <asp:Panel runat="server" ID="pnlCommercialCarSegment" Visible="False">
                        <table style="margin-top: 5px; width: 250px;">
                            <tr>
                                <td>
                                    Commercial Car Segment:
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbCommercialCarSegment" CssClass="MultiDropDownListShowSelected" 
                                        Width="155px" SelectionMode="Multiple" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlDayOfWeek" Visible="False">
                        <table style="margin-top: 5px; width: 250px;">
                            <tr>
                                <td>
                                    Day of Week:
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbDayOfWeek" CssClass="MultiDropDownListShowSelected" 
                                        Width="155px" SelectionMode="Multiple" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlMonthSelection" Visible="False">
                        <table style="margin-top: 5px; width: 250px;">
                            <tr>
                                <td>
                                    Month:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMonth" CssClass="SingleDropDownList" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbYear" Width="80px" CssClass="SingleDropDownList"/>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        
    </ContentTemplate>
</asp:UpdatePanel>


    
    <script type="text/javascript">
        $(document).ready(function () {

    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
            }

        }

        function applyMultiSelects() {


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


        }

        applyMultiSelects();


    });
</script>