<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarClassDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.CarClassDetails" %>
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
                <asp:Label ID="LabelCarClassId" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LabelCarClass" runat="server" Text="<%$ Resources:lang, MappingDetailsCarClass %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarClass" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCarClass" runat="server" ValidationGroup="CarClassInsert" ControlToValidate="TextBoxCarClass" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCarClass %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarSegment" runat="server" Text="<%$ Resources:lang, MappingDetailsCarSegment %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCarSegment" runat="server" DataTextField="car_segment" DataValueField="car_segment_id">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCarSegment" runat="server" InitialValue="-1" ControlToValidate="DropDownListCarSegment" ValidationGroup="CarClassInsert" Text="<%$ Resources:lang, MappingDetailsErrorMessageCarSegment %>" CssClass="ValidatorText"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarSortOrder" runat="server" Text="<%$ Resources:lang, MappingDetailsSortOrder %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxSortOrder" runat="server" Height="18px"></asp:TextBox>
                <asp:numericupdownextender id="NumericUpDownExtenderSortOrder" runat="server" targetcontrolid="TextBoxSortOrder" width="60">
                </asp:numericupdownextender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorSortOrder" runat="server" ControlToValidate="TextBoxSortOrder" CssClass="ValidatorText" ValidationGroup="CarClassInsert" ClientValidationFunction="validateSortOrder" OnServerValidate="SortOrder_Validate" Text="<%$ Resources:lang, MappingDetailsErrorMessageSortOrder %>" ValidateEmptyText="true"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="CarClassInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
