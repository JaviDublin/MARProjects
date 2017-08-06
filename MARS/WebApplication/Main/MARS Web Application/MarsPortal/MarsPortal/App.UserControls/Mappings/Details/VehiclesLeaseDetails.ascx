<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehiclesLeaseDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.VehiclesLeaseDetails" %>
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
            <td valign="top">
                <asp:Label ID="LabelSerials" runat="server" Text="Serials"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="TextBoxSerials" CssClass="TextBoxForm" Width="250px"
                                    TextMode="MultiLine" Wrap="true" Height="150px"></asp:TextBox>
            </td>
            <td valign="top" align="left">
                <table style="height:150px;">
                    <tr>
                        <td><asp:Label runat="server" ID="LabelCountryOwner" Text="Country Owner"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="DropDownListCountriesOwner" runat="server" DataValueField="country" Width="100px" DataTextField="country">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="LabelCountryRent" Text="Country Rent"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="DropDownListCountriesRent" runat="server" DataValueField="country" Width="100px" DataTextField="country">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="LabelStartDate" Text="Start Date"></asp:Label></td>
                        <td>
                            <uc:DatePickerTextBox ID="dptbStartDate" runat="server" Visible="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label runat="server" ID="LabelMessage" Text=""></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="SerialsInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />

            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>