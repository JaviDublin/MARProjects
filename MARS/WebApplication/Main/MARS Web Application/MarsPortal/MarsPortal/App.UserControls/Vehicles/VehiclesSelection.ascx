<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehiclesSelection.ascx.cs" Inherits="App.UserControls.Vehicles.VehiclesSelection" %>
<div>
    <table>
        <tr>
            <td>
                <asp:Label ID="LabelCountry" runat="server" CssClass="labelMappingSelection"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCountries" runat="server" AutoPostBack="true" DataTextField="country" Width="120px" DataValueField="country" onselectedindexchanged="DropDownListCountries_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>