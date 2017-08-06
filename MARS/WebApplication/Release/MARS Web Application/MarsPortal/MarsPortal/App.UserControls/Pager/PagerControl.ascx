<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerControl.ascx.cs" Inherits="App.UserControls.Pager.PagerControl" %>
<asp:UpdatePanel ID="UpdatePanelPager" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0" class="tablePager">
            <tr>
                <td>
                    <asp:Label ID="LabelPagerRowsPerPage" runat="server" Text="<%$ Resources:lang,LabelPagerRowsPerPage %>"></asp:Label>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListRows_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="10" Text="10"/>
                        <asp:ListItem Value="20" Text="20"/>
                        <asp:ListItem Value="30" Text="30"/>
                        <asp:ListItem Value="40" Text="40"/>
                        <asp:ListItem Value="50" Text="50"/>
                    </asp:DropDownList>
                </td>
                <td style="width: 12px">
                </td>
                <td>
                    <asp:Button ID="ButtonFirst" ToolTip="<%$ Resources:lang,PagerFirst %>" Text="   " CommandName="First" runat="server" OnCommand="GetPageIndex" CssClass="PagerFirstInactive"></asp:Button>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:Button ID="ButtonPrevious" ToolTip="<%$ Resources:lang,PagerPrevious %>" Text="   " 
                        CommandName="Previous" runat="server" OnCommand="GetPageIndex" CssClass="PagerPreviousInactive"></asp:Button>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:Label ID="LabelPagerPage" runat="server" Text="<%$ Resources:lang,LabelPagerPage %>"></asp:Label>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListPage" runat="server" AutoPostBack="True" 
                        OnSelectedIndexChanged="DropDownListPage_SelectedIndexChanged"/>
                    
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:Label ID="LabelPagerOf" runat="server" Text="<%$ Resources:lang,LabelPagerOf %>"/>
                    <asp:Label ID="LabelTotalPages" runat="server"/>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:Button ID="ButtonNext" ToolTip="<%$ Resources:lang,PagerNext %>" Text="   " runat="server" 
                                CommandName="Next" OnCommand="GetPageIndex" CssClass="PagerNextInactive"/>
                </td>
                <td style="width: 6px">
                </td>
                <td>
                    <asp:Button ID="ButtonLast" ToolTip="<%$ Resources:lang,PagerLast %>" Text="   " runat="server" 
                                CommandName="Last" OnCommand="GetPageIndex" CssClass="PagerLastInactive"/>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
