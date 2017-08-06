<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OPSAreaDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.OPSAreaDetails" %>
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
                <asp:Label ID="LabelOPSAreaId" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LabelOPSArea" runat="server" Text="<%$ Resources:lang, MappingDetailsOPSArea %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxOPSArea" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorOPSArea" runat="server" ValidationGroup="OPSAreaInsert" Text="<%$ Resources:lang, MappingDetailsErrorMessageOPSArea %>" CssClass="ValidatorText" ControlToValidate="TextBoxOPSArea"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelOPSRegion" runat="server" Text="<%$ Resources:lang, MappingDetailsOPSRegion %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListOPSRegions" runat="server" DataTextField="ops_region" DataValueField="ops_region_id" CssClass="selectLong">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorOPSRegion" runat="server" ControlToValidate="DropDownListOPSRegions" InitialValue="-1" ValidationGroup="OPSAreaInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageOPSRegion %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="OPSAreaInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
