<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSPoolDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.CMSPoolDetails" %>
<asp:Panel runat="server" CssClass="modalPopupMappingDetailsSmall" ID="PanelPopupMappingDetails" Style="display: none;">
    <asp:Panel ID="PanelMappingDetailsTitle" runat="server" CssClass="PanelMappingDetailsTitleSmall">
        <div class="divHertzLogoMappingDetails">
            <asp:Image ID="ImageLogo" runat="server" AlternateText="Hertz Logo" ImageUrl="~/App.Images/hertz-logo.jpg" />
        </div>
        <div class="divApplicationLogoMappingDetails">
            <asp:Image ID="ImageButtonApplicationLogo" runat="server" AlternateText="Application Logo" ImageUrl="~/App.Images/application-logo.gif" />
        </div>
    </asp:Panel>
    <div class="divClearFloats">
    </div>
    <table class="tableMappingDetails">
        <tr>
            <td>
                <asp:Label ID="LabelCMSPoolId" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LabelCMSPool" runat="server" Text="<%$ Resources:lang, MappingDetailsCMSPool %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCMSPool" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCMSPool" runat="server" ControlToValidate="TextBoxCMSPool" ValidationGroup="CMSPoolInsert" Text="<%$ Resources:lang, MappingDetailsErrorMessageCMSPool %>" CssClass="ValidatorText"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCountry" runat="server" Text="<%$ Resources:lang, MappingDetailsCountry %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCountries" runat="server" DataValueField="country" Width="120px" DataTextField="country">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server"  ControlToValidate="DropDownListCountries" ValidationGroup="CMSPoolInsert" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountry %>" CssClass="ValidatorText" InitialValue="-1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="CMSPoolInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
