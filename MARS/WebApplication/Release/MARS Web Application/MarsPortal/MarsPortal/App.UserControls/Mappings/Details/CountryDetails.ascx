<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.CountryDetails" %>
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
                <asp:Label ID="LabelCountry" runat="server" Text="<%$ Resources:lang, MappingDetailsCountry %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCountry" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorCountry" runat="server" ControlToValidate="TextBoxCountry" ClientValidationFunction="validateCountry" OnServerValidate="Country_Validate" ValidateEmptyText="true" Text="<%$ Resources:lang, MappingDetailsCountry %>" CssClass="ValidatorText" ValidationGroup="CountryInsert"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCountryDW" runat="server" Text="<%$ Resources:lang, MappingDetailsCountryDw %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCountryDW" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountryDW" runat="server" ControlToValidate="TextBoxCountryDW" ValidationGroup="CountryInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountryDW %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCountryDescription" runat="server" Text="<%$ Resources:lang, MappingDetailsCountryDescription %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCountryDescription" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountryDescription" runat="server" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountryDescription %>" ControlToValidate="TextBoxCountryDescription" ValidationGroup="CountryInsert" CssClass="ValidatorText"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelIsActive" runat="server" Text="<%$ Resources:lang, MappingDetailsActive %>"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxIsActive" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="CountryInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
