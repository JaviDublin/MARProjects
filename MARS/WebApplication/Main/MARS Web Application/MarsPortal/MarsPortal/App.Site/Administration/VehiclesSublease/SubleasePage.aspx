<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubleasePage.aspx.cs" MasterPageFile="~/App.Masterpages/Mars.Master"
    Inherits="Mars.App.Site.Administration.VehiclesSublease.SubleasePage" %>

<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">


    <table style="text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-left: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; height: 430px; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Vehicle Sublease</a></li>

                    </ul>
                    <div id="tabs-1" style="text-align: left;">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr>
                                <td></td>
                            </tr>
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:AutoGrid runat="server" ID="agSubleasedVehicles" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>
            </td>
        </tr>

        <tr style="vertical-align: top;">
            <td>
                <asp:UpdatePanel ID="upnlParameters" runat="server">
                    <ContentTemplate>


                        <table>
                            <tr>

                                <td style="vertical-align: top;">
                                    <table>
                                        <tr>
                                            <td>Owning Country:</>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlOwningCountry"
                                                    CssClass="SingleDropDownList" AutoPostBack="True"
                                                    OnSelectedIndexChanged="UpdateDataGrid" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Renting Country:</>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlRentingCountry"
                                                    CssClass="SingleDropDownList" AutoPostBack="True"
                                                    OnSelectedIndexChanged="UpdateDataGrid" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Model Description
                                            </td>
                                            <td>
                                                <asp:ListBox runat="server" ID="lbModels" CssClass="MultiDropDownListShowSelected"
                                                    SelectionMode="Multiple" OnSelectedIndexChanged="UpdateDataGrid" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding-top: 15px;">
                                                <asp:Button ID="btnAddRemoveVehicles" runat="server" CssClass="StandardButton"
                                                    Width="180px"
                                                    Text="Add or Remove Vehicles" OnClick="ShowVehiclePopup" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding-top: 15px;">
                                                <asp:Button ID="btnClearAll" runat="server" CssClass="StandardButton"
                                                    Width="180px"
                                                    Text="Delete All" OnClick="ClearAllSubleasedVehicles" />
                                                <asp:ConfirmButtonExtender runat="server" TargetControlID="btnClearAll" 
                                                    ConfirmText="This will permanently delete all Subleases"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>


                        <asp:Panel runat="server" ID="pnlAddRemoveVehicles" CssClass="Phase4ModalPopup" Width="800" Height="300">
                            <table style="width: 100%; font-size: 12px; vertical-align: top;" >
                                <tr>
                                    <td colspan="2">
                                        <div style="float: right;">
                                            <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                                        </div>
                                        <div style="height: 20px;">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="font-size: 14px; font-weight: bold;">
                                        Add or Remove vehicles by VIN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <table style="width: 100%; text-align: center;">
                                            <tr>
                                                <td style="width: 100px;">&nbsp;
                                                </td>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="rbAddVehicles" Text="Add Vehicles" 
                                                        OnCheckedChanged="AddVehiclesSelected" AutoPostBack="True"
                                                        Checked="True"
                                                        GroupName="AddOrRemoveVehicles" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="rbRemoveVehicles" Text="Remove Vehicles" 
                                                        OnCheckedChanged="RemoveVehiclesSelected"  AutoPostBack="True"
                                                        GroupName="AddOrRemoveVehicles" />
                                                </td>
                                                <td style="width: 100px;">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                            <hr/>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top;">
                                    <td>
                                        <table>
                                            <tr>
                                                <td>Paste Comma separated VINs in the box below:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tbVinInput" TextMode="MultiLine" Width="220px" Height="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnParseVinField" runat="server" Text="Check"
                                                        CssClass="StandardButton" OnClick="ParseVins" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlVehiclesToModify">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField runat="server" ID="hfVinResult" Value="{0} of {1} VINS successfully Parsed"/>
                                                         &nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <asp:TextBox runat="server" ID="tbVinResults" TextMode="MultiLine" ReadOnly="True" Width="220px" Height="150px" />
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>Renting Country:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" ID="ddlEditRentingCountry"
                                                                        CssClass="SingleDropDownList" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Start Date:
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox runat="server" Width="80px" 
                                                                        CssClass="AutoCompleteTextBox SingleDateBox"
                                                                        ID="tbStartDate" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="padding-top: 30px;">
                                                                    <asp:Button runat="server" CssClass="StandardButton"
                                                                        ID="btnApplyChangesToVehicles" Text="Add Vehicles" Width="150px"
                                                                        OnClick="UpdateVehicles" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>


                        <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
                        <asp:ModalPopupExtender
                            ID="mpeEditVehicles"
                            runat="server"
                            PopupControlID="pnlAddRemoveVehicles"
                            TargetControlID="btnDummy"
                            DropShadow="True"
                            BackgroundCssClass="modalBackgroundGray" />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>




    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();

        });

        $(document).ready(function () {

            applyMultiSelects();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

            function panelLoaded(sender, args) {
                if (args.get_panelsUpdated().length > 0) {
                    applyMultiSelects();

                }
            }


            function applyMultiSelects() {

                $(".SingleDateBox").datepicker({ dateFormat: "dd/mm/yy" });

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




</asp:Content>
