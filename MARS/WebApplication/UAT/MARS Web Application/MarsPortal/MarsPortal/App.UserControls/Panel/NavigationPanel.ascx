<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationPanel.ascx.cs" Inherits="App.UserControls.Panel.NavigationPanel" %>
<div class="div-TabbedMenu">
    <asp:DataList runat="server" ID="DataListNavigationMenu" RepeatDirection="Horizontal"
        OnItemCommand="DataListTabbedMenu_ItemCommand">
        <ItemTemplate>
            <asp:Panel runat="server" ID="PanelNavigationMenu" CssClass="panel-PanelMenuItem">
                <asp:HiddenField ID="HiddenFieldIndex" runat="server" Value='<%#Eval("Index")%>' />
                <asp:LinkButton ID="LinkButtonMenuItem" runat="server" CommandArgument='<%#Eval("Index") %>'
                    Text='<%#Eval("Title") %>' ToolTip='<%#Eval("ToolTip") %>' CausesValidation="false"
                    CommandName="Select"></asp:LinkButton>
            </asp:Panel>
        </ItemTemplate>
    </asp:DataList>
</div>