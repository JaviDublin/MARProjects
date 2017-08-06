<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.LocationDetails" %>
<asp:Panel runat="server" CssClass="modalPopupMappingDetailsLarge" ID="PanelPopupMappingDetails" Style="display: none;">
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
                <asp:Label ID="LabelLocation" runat="server" Text="<%$ Resources:lang, MappingDetailsLocation %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxLocation" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorLocation" runat="server" ControlToValidate="TextBoxLocation" ValidateEmptyText="true" ValidationGroup="LocationInsert" ClientValidationFunction="validateLocation" OnServerValidate="Location_Validate" Text="<%$ Resources:lang, MappingDetailsErrorMessageLocation %>" CssClass="ValidatorText"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelLocationDW" runat="server" Text="<%$ Resources:lang, MappingDetailsLocationDW %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxLocationDW" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorLocationDW" runat="server" ControlToValidate="TextBoxLocationDW" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageLocationDW %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelRealLocationName" runat="server" Text="<%$ Resources:lang, MappingDetailsRealLocationName %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxRealLocationName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorRealLocationName" runat="server" ControlToValidate="TextBoxRealLocationName" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageRealLocationName %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelLocationName" runat="server" Text="<%$ Resources:lang, MappingDetailsLocationName %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxLocationName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorLocationName" runat="server" ControlToValidate="TextBoxLocationName" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageLocationName %>">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelLocationNameDW" runat="server" Text="<%$ Resources:lang, MappingDetailsLocationNameDW %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxLocationNameDW" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorLocationNameDW" runat="server" ValidationGroup="LocationInsert" ControlToValidate="TextBoxLocationNameDW" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageLocationNameDW %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelAPDTRR" runat="server" Text="<%$ Resources:lang, MappingDetailsAPDTRR %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxAPDTRR" runat="server" MaxLength="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAPDTRR" runat="server" ControlToValidate="TextBoxAPDTRR" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageAPDTRR %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCal" runat="server" Text="<%$ Resources:lang, MappingDetailsCAL %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCal" runat="server" MaxLength="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCal" runat="server" ControlToValidate="TextBoxCal" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCAL %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCMSLocationGroupCode" runat="server" Text="<%$ Resources:lang, MappingDetailsCMSLocationGroupCode %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCMSLocationGroupCode" runat="server" DataTextField="cms_location_group" DataValueField="cms_location_group_id" CssClass="selectLong">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCMSLocationGroupCode" runat="server" ControlToValidate="DropDownListCMSLocationGroupCode" ValidationGroup="LocationInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCMSLocationGroupCode %>" InitialValue="-1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelOPSArea" runat="server" Text="<%$ Resources:lang, MappingDetailsOPSArea %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListOPSAreas" runat="server" DataTextField="ops_area" DataValueField="ops_area_id" CssClass="selectLong">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorOPSAreas" runat="server" Text="<%$ Resources:lang, MappingDetailsErrorMessageOPSArea %>" ControlToValidate="DropDownListOPSAreas" CssClass="ValidatorText" InitialValue="-1" ValidationGroup="LocationInsert"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelServedByLocn" runat="server" Text="<%$ Resources:lang, MappingDetailsServedByLocn %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxServedByLocn" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorServedByLocn" runat="server" ControlToValidate="TextBoxServedByLocn" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageServedByLocn %>" ValidationGroup="LocationInsert"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelTurnaroundHours" runat="server" Text="<%$ Resources:lang, MappingDetailsTurnaroundHours %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxTurnaroundHours" runat="server" Height="18px"></asp:TextBox>
                <asp:numericupdownextender id="NumericUpDownExtenderTurnaroundHours" runat="server" targetcontrolid="TextBoxTurnaroundHours" width="60">
                </asp:numericupdownextender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorTurnaroundHours" runat="server" ControlToValidate="TextBoxTurnaroundHours" ValidateEmptyText="true" ValidationGroup="LocationInsert" ClientValidationFunction="validateTurnaroundHours" OnServerValidate="TurnaroundHours_Validate" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageTurnaroundHours %>"></asp:CustomValidator>
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
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="LocationInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
