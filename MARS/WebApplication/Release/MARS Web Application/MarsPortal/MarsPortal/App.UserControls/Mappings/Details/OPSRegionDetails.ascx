<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OPSRegionDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.OPSRegionDetails" %>
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
                <asp:Label ID="LabelOPSRegionId" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LabelOPSRegion" runat="server" Text="<%$ Resources:lang, MappingDetailsOPSRegion %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxOPSRegion" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorOPSRegion" runat="server" ControlToValidate="TextBoxOPSRegion" ValidationGroup="OPSRegionInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageOPSRegion %>"></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" Text="<%$ Resources:lang, MappingDetailsErrorMessageCountry %>" ControlToValidate="DropDownListCountries" ValidationGroup="OPSRegionInsert" InitialValue="-1" CssClass="ValidatorText"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="OPSRegionInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:ModalPopupExtender ID="ModalPopupExtenderMappingDetails" runat="server" PopupControlID="PanelPopupMappingDetails" TargetControlID="ButtonModalPopupMappingDetails" BackgroundCssClass="modalBackground" PopupDragHandleControlID="PanelMappingDetailsTitle" CancelControlID="ButtonCancel">
</asp:ModalPopupExtender>
