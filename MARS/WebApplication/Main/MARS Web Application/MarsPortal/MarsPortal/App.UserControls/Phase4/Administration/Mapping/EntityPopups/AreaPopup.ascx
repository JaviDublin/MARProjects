<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaPopup.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups.AreaPopup" %>


<asp:Panel ID="pnlArea" runat="server" Width="400px" Height="280px" CssClass="Phase4ModalPopup">
    
    <asp:HiddenField runat="server" ID="hfAreaId"/>

    <table style="width: 100%;">
        <tr>
            <td style="height: 30px; text-align: center;" colspan="2">
                <h1>Area</h1>
                
            </td>
            <td>
                <div style="float: right; ">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png"  />
                </div>
            </td>
        </tr>        
        <tr>
            <td style="height: 20px;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Country:</label>
            </td>
            <td>
                <asp:Label runat="server" ID="lblCountryName" Width="120px" Height="16px" BorderWidth="1"  />
                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="StandardAdminDropdown" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCountry_SelectionChanged" Visible="False"/>
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Region:</label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlRegion" CssClass="StandardAdminDropdown"/>
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Area:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbAreaName" Width="120px" Height="16px" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Is Active:</label>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="cbActive" />
            </td>
        </tr>
        <tr>
             <td colspan="2">
                 <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
                 <asp:RequiredFieldValidator runat="server" ID="rfvAreaName" ControlToValidate="tbAreaName" ForeColor="Red"
                    Text="Area Code can not be emtpty" ValidationGroup="CheckControls" Enabled="False"/>
             </td>
         </tr>
        <tr>
            <td style="height: 20px;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnSavePopup" Text="Save" OnClick="btnSavePopup_Click"
                                ValidationGroup="CheckControls" CssClass="StandardButton" />
                        </td>
                        <td>
                            <div style="width: 40px;">&nbsp;</div>            
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnDelete" Text="Delete" ForeColor="Red" OnClick="btnDelete_Click"
                                 ValidationGroup="CheckControls" CssClass="StandardButton" />
                            <asp:ConfirmButtonExtender runat="server" TargetControlID="btnDelete" 
                                ConfirmText="Are you sure you wish to delete this Area?" />            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpRegion"
    runat="server"
    PopupControlID="pnlArea"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />