<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverdueGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Pooling.OverdueGrid" %>


<table style="text-align: right;">
    <tr>
        <td>Overdue Collections:
        </td>
        <td style="width: 40px;">
            <asp:Label runat="server" ID="lblCollections" />
        </td>
    </tr>
    <tr>
        <td>Overdue / Open Trips Due:
        </td>
        <td>
            <asp:Label runat="server" ID="lblOpenTrips" />
        </td>
    </tr>
</table>
