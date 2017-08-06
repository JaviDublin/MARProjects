<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarGroupGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CarGroupGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCarGroupDetails ID="MappingCarGroupDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCarGroups %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCarGroups" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCarGroups" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="car_group_id" onrowcommand="GridviewCarGroups_RowCommand" onrowcreated="GridviewCarGroups_RowCreated" onrowdatabound="GridviewCarGroups_RowDataBound" onsorting="GridviewCarGroups_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="car_group" HeaderText="CAR GROUP" ReadOnly="True" SortExpression="car_group">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_group_gold" HeaderText="CAR GROUP GOLD" ReadOnly="True" SortExpression="car_group_gold">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_group_fivestar" HeaderText="CAR GROUP FIVE STAR" ReadOnly="True" SortExpression="car_group_fivestar">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_group_presidentCircle" HeaderText="CAR GROUP PRESIDENT CIRCLE" ReadOnly="True" SortExpression="car_group_presidentCircle">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_group_platinum" HeaderText="CAR GROUP PLATINUM" ReadOnly="True" SortExpression="car_group_platinum">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="sort_car_group" HeaderText="SORT" ReadOnly="True" SortExpression="sort_car_group">
                                    <ItemStyle Width="75px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_class" HeaderText="CAR CLASS" ReadOnly="True" SortExpression="car_class" />
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" ReadOnly="True" SortExpression="country">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditCarGroup" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteCarGroup" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCarGroups" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCarGroup" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCarGroup" runat="server" Text="<%$ Resources:lang, CarGroupDetailsAdd %>" OnClick="ButtonAddCarGroup_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
