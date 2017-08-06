<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarClassPopup.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups.CarClassPopup" %>


<asp:Panel ID="pnlCarClass" runat="server" Width="400px" Height="280px" CssClass="Phase4ModalPopup">
    
    <asp:HiddenField runat="server" ID="hfCarClassId"/>

    <table style="width: 100%;">
        <tr>
            <td style="height: 30px; text-align: center;" colspan="2">
                <h1>Car Class</h1>
                
            </td>
            <td>
                <div style="float: right; ">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
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
                    OnSelectedIndexChanged="ddlCountry_SelectionChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Car Segment:</label>
            </td>
            <td>
                <asp:Label runat="server" ID="lblCarSegment" Width="120px" Height="16px" BorderWidth="1" />
                <asp:DropDownList runat="server" ID="ddlCarSegment" CssClass="StandardAdminDropdown"/>
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Car Class:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbCarClass" Width="120px" Height="16px" MaxLength="30" />
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
                 <asp:RequiredFieldValidator runat="server" ID="rfvClassName" ControlToValidate="tbCarClass" ForeColor="Red"
                    Text="Car Class can not be emtpty" ValidationGroup="CheckControls" Enabled="False"/>
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
                                ConfirmText="Are you sure you wish to delete this Car Class?" />            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpCarClass"
    runat="server"
    PopupControlID="pnlCarClass"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />