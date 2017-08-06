<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSPoolGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CMSPoolGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCMSPoolDetails ID="MappingCMSPoolDetails" runat="server"  OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCMSPools %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCMSPools" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCMSPools" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="cms_pool_id, country" onrowcommand="GridviewCMSPools_RowCommand" onrowcreated="GridviewCMSPools_RowCreated" onrowdatabound="GridviewCMSPools_RowDataBound" onsorting="GridviewCMSPools_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="cms_pool" HeaderText="CMS POOL" SortExpression="cms_pool" ReadOnly="True" />
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" SortExpression="country" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="CMS LOCATION GROUPS">
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectCMSLocationGroups" runat="server" Text="<%$ Resources:lang, Select %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="SelectCMSLocationGroups" ToolTip="<%$ Resources:lang, SelectToolTipCMSLocationGroups %>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditCMSPool" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteCMSPool" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCMSPools" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCMSPools" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCMSPool" runat="server" Text="<%$ Resources:lang, CMSPoolDetailsAdd %>" OnClick="ButtonAddCMSPool_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
