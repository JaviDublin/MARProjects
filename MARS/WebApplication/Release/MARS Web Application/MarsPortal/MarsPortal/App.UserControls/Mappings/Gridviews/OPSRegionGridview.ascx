<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OPSRegionGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.OPSRegionGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingOPSRegionDetails ID="MappingOPSRegionDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleOPSRegions %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelOPSRegions" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewOPSRegions" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="ops_region_id, country" onrowcommand="GridviewOPSRegions_RowCommand" onrowcreated="GridviewOPSRegions_RowCreated" onrowdatabound="GridviewOPSRegions_RowDataBound" onsorting="GridviewOPSRegions_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="ops_region" HeaderText="OPS REGION" SortExpression="ops_region" ReadOnly="True" />
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" SortExpression="country" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="OPS AREAS">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectOPSAreas" runat="server" Text="<%$ Resources:lang, Select %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="SelectOPSAreas" ToolTip="<%$ Resources:lang, SelectToolTipOPSAreas %>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditOPSRegion"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteOPSRegion"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlOPSRegions" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateOPSRegions" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddOPSRegion" runat="server" Text="<%$ Resources:lang, OPSRegionDetailsAdd %>" OnClick="ButtonAddOPSRegion_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
