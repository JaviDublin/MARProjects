<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportToExcel.ascx.cs" Inherits="Mars.App.UserControls.Phase4.HelpIcons.ExportToExcel" %>

    

<table>
    <tr>
        <td>
            <asp:ImageButton runat="server" ID="ibExportToExcel" ImageUrl="~/App.Images/Excel.jpg" AlternateText="Export to Excel" CssClass="PlainDataExportButton"
            OnClick="ibExportToExcel_Click" />        
        </td>
    </tr>
    <tr>
        <td>
            Export To Excel
        </td>
    </tr>
</table>