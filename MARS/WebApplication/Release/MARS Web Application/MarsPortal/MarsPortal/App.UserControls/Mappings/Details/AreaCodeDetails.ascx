<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaCodeDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.AreaCodeDetails" %>
<asp:Panel runat="server" CssClass="modalPopupMappingDetailsMedium" ID="PanelPopupMappingDetails" Style="display: none;">
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
                <asp:Label ID="LabelOwnArea" runat="server" Text="<%$ Resources:lang, MappingDetailsOwnArea %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxOwnArea" runat="server" MaxLength="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorOwnArea" runat="server" ValidateEmptyText="true" CssClass="ValidatorText" OnServerValidate="OwnArea_Validate" ValidationGroup="AreaCodeInsert" ClientValidationFunction="validateOwnArea" ControlToValidate="TextBoxOwnArea" Text="<%$ Resources:lang, MappingDetailsErrorMessageOwnArea %>"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCountry" runat="server" Text="<%$ Resources:lang, MappingDetailsCountry %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCountries" runat="server" DataTextField="country" Width="120px" DataValueField="country">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="DropDownListCountries" CssClass="ValidatorText" ValidationGroup="AreaCodeInsert" InitialValue="-1" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountry %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelAreaName" runat="server" Text="<%$ Resources:lang, MappingDetailsAreaName %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TexBoxAreaName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAreaName" runat="server" ValidationGroup="AreaCodeInsert" CssClass="ValidatorText" ControlToValidate="TexBoxAreaName" Text="<%$ Resources:lang, MappingDetailsErrorMessageAreaName %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelOPCO" runat="server" Text="<%$ Resources:lang, MappingDetailsOPCO %>"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxOPCO" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelFleetCo" runat="server" Text="<%$ Resources:lang, MappingDetailsFleetCo %>"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxFleetCo" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarSales" runat="server" Text="<%$ Resources:lang, MappingDetailsCarSales %>"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxCarSales" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelLicensee" runat="server" Text="<%$ Resources:lang, MappingDetailsLicensee %>"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxLicensee" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="AreaCodeInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
