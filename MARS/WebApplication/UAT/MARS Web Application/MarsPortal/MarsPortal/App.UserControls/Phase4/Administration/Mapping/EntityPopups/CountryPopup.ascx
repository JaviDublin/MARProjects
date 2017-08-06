<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryPopup.ascx.cs"
    Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups.CountryPopup" %>


<asp:Panel ID="pnlCountry" runat="server" Width="400px" Height="230px" CssClass="Phase4ModalPopup">
    <asp:HiddenField runat="server" ID="hfCountryId"/>
    <table style="width: 100%;">
        <tr>
            <td style="height: 30px; text-align: center;" colspan="2">
                <h1>Country</h1>
                
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
                <label style="font-weight: bold;">Country Code:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbCountryCode" Width="120px" Enabled="False" MaxLength="2"/>
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Country Code DW:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbCountryDw" Width="120px" MaxLength="2"  />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Name:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbCountryName" Width="120px" Height="16px" MaxLength="30"
                      ValidationGroup="CheckControls"/>
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
                 <asp:RequiredFieldValidator runat="server" ID="rfvCountryName" ControlToValidate="tbCountryName" ForeColor="Red" 
                    Text="Country Name can not be emtpty" ValidationGroup="CheckControls" Enabled="False" />
                 <asp:RequiredFieldValidator runat="server" ID="rfvCountryCode" ControlToValidate="tbCountryCode" ForeColor="Red"
                    Text="Country Code can not be emtpty" ValidationGroup="CheckControls" Enabled="False"/>
                 <asp:Label runat="server" ID="tbEmpty" Visible="False" Text=""  />
             </td>
         </tr>
        <tr>
            <td style="height: 20px;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button runat="server" ID="bnSavePopup" Text="Save" OnClick="bnSavePopup_Click"
                      ValidationGroup="CheckControls" CssClass="StandardButton"  />
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpCountry"
    runat="server"
    PopupControlID="pnlCountry"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />
