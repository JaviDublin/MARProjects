<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.Management.Users.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <%-- Update Panel --%>
    <asp:UpdatePanel ID="UpdatePanelMaintenanceUsers" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%--User Details --%>
            <uc:UserDetails ID="UserDetails" runat="server" OnSaveUserDetails="UserSave_Click" />
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server" />
                <asp:Panel ID="PanelUsers" runat="server" Visible="false" CssClass="panelUsersMaintenance" DefaultButton="ButtonAddUser">
                    <div class="divMappingSelection">
                        <table class="tableMappingSelection">
                            <tr>
                                <td class="columnLabelMapping">
                                    <asp:Label ID="lbMapType" runat="server" Text="Filter by name or racfId:" CssClass="labelMappingSelection"></asp:Label>
                                </td>
                                <td>
                                   &nbsp; <asp:TextBox runat="server" ID="tbSearchBox"></asp:TextBox>  
                                </td>
                                <td>&nbsp;
                                    <asp:Button runat="server" ID="bnSearchUser" Text="Search" OnClick="BnSearchUserClick"/>
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="GridviewUsers" runat="server" CssClass="GridViewStyle" AutoGenerateColumns="False" DataKeyNames="name" AllowSorting="True" OnRowCommand="GridviewUsers_RowCommand" OnRowCreated="GridviewUsers_RowCreated" OnRowDataBound="GridviewUsers_RowDataBound" OnSorting="GridviewUsers_Sorting">
                                    <HeaderStyle CssClass="GridHeaderStyle" />
                                    <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <EditRowStyle CssClass="GridEditRowStyle" />
                                    <Columns>
                                        <asp:BoundField DataField="racfid" HeaderText="RACFID" ReadOnly="True" SortExpression="racfid">
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="name" HeaderText="NAME" ReadOnly="True" SortExpression="name">
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="roles" HeaderText="USER ROLES" ReadOnly="True" SortExpression="roles">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButtonUserEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditUser" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                                /
                                                <asp:LinkButton ID="LinkButtonUserDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteUser" CommandArgument='<%#Container.DataItemIndex %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowPager" colspan="2">
                                <div class="divPagerRecordsSelected">
                                    <asp:Label ID="LabelTotalRecords" runat="server" Text="<%$ Resources:lang, LabelTotalRecords %>"></asp:Label>
                                    <asp:Label ID="LabelTotalRecordsDisplay" runat="server" CssClass="labelRecordsSelected"></asp:Label>
                                </div>
                                <div class="divPagerControl">
                                    <uc:PagerControl ID="PagerControlUsers" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                            </td>
                            <td class="columnButtonAdd">
                                <asp:Button ID="ButtonAddUser" runat="server" Text="<%$ Resources:lang, UsersDetailsAdd %>" OnClick="ButtonAddUser_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- No Data --%>
                <uc:EmptyDataTemplate ID="EmptyDataTemplateUsers" runat="server" Visible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- Delete Confirm --%>
    <uc:DeleteConfirm ID="DeleteConfirm" runat="server" />
</asp:Content>
