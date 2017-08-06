<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarClassGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CarClassGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCarClassDetails ID="MappingCarClassDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCarClasses %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCarClasses" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCarClasses" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="car_class_id, country" onrowcommand="GridviewCarClasses_RowCommand" onrowcreated="GridviewCarClasses_RowCreated" onrowdatabound="GridviewCarClasses_RowDataBound" onsorting="GridviewCarClasses_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="car_class" HeaderText="CAR CLASS" ReadOnly="True" SortExpression="car_class" />
                                <asp:BoundField DataField="sort_car_class" HeaderText="SORT" ReadOnly="True" SortExpression="sort_car_class">
                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="car_segment" HeaderText="CAR SEGMENT" ReadOnly="True" SortExpression="car_segment">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" ReadOnly="True" SortExpression="country">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="CAR GROUPS">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectCarGroups" runat="server" Text="<%$ Resources:lang, Select %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="SelectCarGroups" ToolTip="<%$ Resources:lang, SelectToolTipCarGroups %>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditCarClass"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteCarClass"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCarClasses" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCarClass" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCarClass" runat="server" Text="<%$ Resources:lang, CarClassDetailsAdd %>" OnClick="ButtonAddCarClass_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
