<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSLocationGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CMSLocationGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCMSLocationDetails ID="MappingCMSLocationDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCMSLocationGroups %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCMSLocations" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCMSLocations" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="cms_location_group_id, country" onrowcommand="GridviewCMSLocations_RowCommand" onrowcreated="GridviewCMSLocations_RowCreated" onrowdatabound="GridviewCMSLocations_RowDataBound" onsorting="GridviewCMSLocations_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField Visible="false" DataField="cms_location_group_id" HeaderText="CMS LOCATION GROUP ID" SortExpression="cms_location_group_id">
                                    <ItemStyle Width="175px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cms_location_group_code_dw" HeaderText="CMS LOCATION GROUP CODE DW" SortExpression="cms_location_group_code_dw">
                                    <ItemStyle Width="195px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cms_location_group" HeaderText="CMS LOCATION GROUP " ReadOnly="True" SortExpression="cms_location_group"></asp:BoundField>
                                <asp:BoundField DataField="cms_pool" HeaderText="CMS POOL" ReadOnly="True" SortExpression="cms_pool">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" ReadOnly="True" SortExpression="country">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="LOCATIONS">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectLocations" runat="server" Text="<%$ Resources:lang, Select %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="SelectLocations" ToolTip="<%$ Resources:lang, SelectToolTipLocations %>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditCMSLocationGroup"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteCMSLocationGroup"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCMSLocations" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCMSLocationGroups" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCMSLocation" runat="server" Text="<%$ Resources:lang, CMSLocationDetailsAdd %>" OnClick="ButtonAddCMSLocation_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
