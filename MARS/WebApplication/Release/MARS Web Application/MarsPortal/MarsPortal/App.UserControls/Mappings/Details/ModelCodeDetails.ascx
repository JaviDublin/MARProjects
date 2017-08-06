<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModelCodeDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.ModelCodeDetails" %>
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
                <asp:Label ID="LabelModelId" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LabelModel" runat="server" Text="<%$ Resources:lang, MappingDetailsModel %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxModel" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorModel" runat="server" ValidationGroup="ModelInsert" Text="<%$ Resources:lang, MappingDetailsErrorMessageModel %>" CssClass="ValidatorText" ControlToValidate="TextBoxModel"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCountry" runat="server" Text="<%$ Resources:lang, MappingDetailsCountry %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCountries" runat="server" DataTextField="country" DataValueField="country">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="DropDownListCountries" InitialValue="-1" ValidationGroup="ModelInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountry %>"></asp:RequiredFieldValidator>
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
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="ModelInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
