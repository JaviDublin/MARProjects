<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarSegmentGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CarSegmentGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCarSegmentDetails ID="MappingCarSegmentDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCarSegments %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCarSegments" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCarSegments" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="car_segment_id, country" onrowcommand="GridviewCarSegments_RowCommand" onrowcreated="GridviewCarSegments_RowCreated" onrowdatabound="GridviewCarSegments_RowDataBound" onsorting="GridviewCarSegments_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="car_segment" HeaderText="CAR SEGMENT" SortExpression="car_segment" ReadOnly="True" />
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" SortExpression="country" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="sort_car_segment" HeaderText="SORT" SortExpression="sort_car_segment" ReadOnly="True">
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="CAR CLASSES">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectCarClasses" runat="server" Text="<%$ Resources:lang, Select %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="SelectCarClasses" ToolTip="<%$ Resources:lang, SelectToolTipCarClasses %>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditCarSegment"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteCarSegment"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCarSegments" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCarSegment" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCarSegment" runat="server" Text="<%$ Resources:lang, CarSegmentDetailsAdd %>" OnClick="ButtonAddCarSegment_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
