<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForeignVehiclesParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ForeignVehiclesParameters" %>

<%@ Register Src="~/App.UserControls/Phase4/VehicleParameters.ascx" TagName="SingleVehicleParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/MultiSelectVehicleParameters.ascx" TagName="MultiVehicleParameters" TagPrefix="uc" %>

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
    <tr style="vertical-align: top;">
        <td>
            <asp:UpdatePanel runat="server" ID="upnlParams" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:SingleVehicleParameters runat="server" ID="ucSingleVehicleParams" />
                    <uc:MultiVehicleParameters runat="server" ID="ucMultiVehicleParams" Visible="False" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </td>
        <td>
            <table style="width: 523px; margin-left: auto; margin-right: auto; margin-bottom: 10px; margin-top: 0; border-collapse: separate; border-spacing: 6px; text-align: left;">
                <tr style="vertical-align: top;">
                    <td>Fleet:
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lbFleet" Width="95px" SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                    </td>
                    <td>
                        <asp:Label ID="lblOperationalStatuses" runat="server" Text="Operational Status:"  />
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lbOperationalStatus" Width="95px" 
                            SelectionMode="Multiple" CssClass="MultiDropDownListShowSelected" />
                    </td>
                </tr>
                <tr>
                    <td>
                        
                        <asp:Label runat="server" ID="lblArea" Text="Owning Area:"  />
                    </td>
                    <td>
                        
                        <rad:AutoComplete ID="acOwningArea" runat="server" CssClass="AutoCompleteTextBox"
                            AutoPostBack="False" 
                            WebServiceUrl="~/AutoComplete.asmx/OwningAreaAutoComplete"
                            Width="95px" />
                    </td>
                    <td>
                        <asp:Label ID="lblMovementTypes" runat="server" Text="Movement Type:"  />
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

<script>
    $(document).ready(function () {

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

        }

        applyMultiSelects();


    });
</script>
