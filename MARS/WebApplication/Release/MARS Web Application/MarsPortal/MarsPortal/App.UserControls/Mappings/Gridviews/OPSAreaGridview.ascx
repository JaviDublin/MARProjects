<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OPSAreaGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.OPSAreaGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingOPSAreaDetails ID="MappingOPSAreaDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleOPSAreas %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelOPSAreas" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewOPSAreas" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="ops_area_id, country" onrowcommand="GridviewOPSAreas_RowCommand" onrowcreated="GridviewOPSAreas_RowCreated" onrowdatabound="GridviewOPSAreas_RowDataBound" onsorting="GridviewOPSAreas_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="ops_area" HeaderText="OPS AREA" ReadOnly="True" SortExpression="ops_area" />
                                <asp:BoundField DataField="ops_region" HeaderText="OPS REGION" ReadOnly="True" SortExpression="ops_region">
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
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditOPSArea"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteOPSArea"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlOPSAreas" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateOPSAreas" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddOPSArea" runat="server" Text="<%$ Resources:lang, OPSAreaDetailsAdd %>" OnClick="ButtonAddOPSArea_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
