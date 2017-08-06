<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.CountryGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingCountryDetails ID="MappingCountryDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleCountries %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelCountries" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewCountries" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="country" onrowcommand="GridviewCountries_RowCommand" onrowcreated="GridviewCountries_RowCreated" onrowdatabound="GridviewCountries_RowDataBound" onsorting="GridviewCountries_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" ReadOnly="True" SortExpression="country">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country_dw" HeaderText="COUNTRY DW" ReadOnly="True" SortExpression="country_dw">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country_description" HeaderText="DESCRIPTION" ReadOnly="True" SortExpression="country_description" />
                                <asp:TemplateField HeaderText="ACTIVE" SortExpression="active">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbActive" Text=' <%# Eval("active").ToString() == "False" ? "" : "<b>Yes</b>"%>'   ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AREA CODES">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectAreaCodes" runat="server" Text="<%$ Resources:lang, Select %>" CommandName="SelectAreaCodes" ToolTip="<%$ Resources:lang, SelectToolTipAreaCodes %>" CommandArgument='<%# Eval("Country") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CMS POOLS">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectCMSPools" runat="server" Text="<%$ Resources:lang, Select %>" CommandName="SelectCMSPools" ToolTip="<%$ Resources:lang, SelectToolTipCMSPools %>" CommandArgument='<%# Eval("Country") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OPS REGIONS">
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectOPSRegions" runat="server" Text="<%$ Resources:lang, Select %>" CommandName="SelectOPSRegions" ToolTip="<%$ Resources:lang, SelectToolTipOPSRegions %>" CommandArgument='<%# Eval("Country") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CAR SEGMENTS">
                                    <ItemStyle Width="105px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectCarSegments" runat="server" Text="<%$ Resources:lang, Select %>" CommandName="SelectCarSegments" ToolTip="<%$ Resources:lang, SelectToolTipCarSegments %>" CommandArgument='<%# Eval("Country") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MODEL CODES">
                                    <ItemStyle Width="105px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonSelectModelCodes" runat="server" Text="<%$ Resources:lang, Select %>" CommandName="SelectModelCodes" ToolTip="<%$ Resources:lang, SelectToolTipModelCodes %>" CommandArgument='<%# Eval("Country") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditCountry" CommandArgument='<%# Eval("country") %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteCountry" CommandArgument='<%# Eval("country") %>'></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlCountries" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateCountries" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddCountry" runat="server" Text="<%$ Resources:lang, CountryDetailsAdd %>" OnClick="ButtonAddCountry_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
