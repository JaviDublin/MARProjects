<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MappingSelection.ascx.cs" Inherits="App.UserControls.Mappings.Common.MappingSelection" %>
<div class="divMappingSelection">
    <table class="tableMappingSelection">
        <tr>
            <td class="columnLabelMapping">
                <asp:Label ID="LabelMappingType" runat="server" Text="<%$ Resources:lang, MappingType %>" CssClass="labelMappingSelection"></asp:Label>
            </td>
            <td style="width: 180px;">
                <asp:DropDownList ID="DropDownListMappingType" runat="server" AutoPostBack="true" Width="150px" onselectedindexchanged="DropDownListMappingType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="columnLabelMapping">
                <asp:Label ID="LabelCountry" runat="server" Text="<%$ Resources:lang, MappingCountryFilter %>" CssClass="labelMappingSelection"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCountries" runat="server" AutoPostBack="true" DataTextField="country" Width="120px" DataValueField="country" onselectedindexchanged="DropDownListCountries_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <hr />
</div>