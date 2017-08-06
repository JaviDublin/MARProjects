<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaCodeGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.AreaCodeGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Title --%>
        <uc:MappingAreaCodeDetails ID="MappingAreaCodeDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleAreaCodes %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelAreaCodes" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewAreaCodes" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="ownarea" onrowcommand="GridviewAreaCodes_RowCommand" onrowcreated="GridviewAreaCodes_RowCreated" onrowdatabound="GridviewAreaCodes_RowDataBound" onsorting="GridviewAreaCodes_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="ownarea" HeaderText="OWN AREA" ReadOnly="True" SortExpression="ownarea">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" ReadOnly="True" SortExpression="country">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="area_name" HeaderText="AREA NAME" ReadOnly="True" SortExpression="area_name"></asp:BoundField>
                                <asp:TemplateField HeaderText="OPCO" SortExpression="opco">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbOpco" Text=' <%# Eval("opco").ToString() == "False" ? "" : "<b>Yes</b>"%>'   ></asp:Label>
                        
                                      <!--  <asp:CheckBox ID="CheckBoxOPCO" runat="server" Checked='<%# Eval("opco") %>' Visible="false" />
                                        <asp:Image ID="ImageOPCO" runat="server" /> -->
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FLEETCO" SortExpression="fleetco">
                                    <ItemTemplate>
                                            <asp:Label runat="server" ID="lbFleetco" Text=' <%# Eval("fleetco").ToString() == "False" ? "" : "<b>Yes</b>"%>'   ></asp:Label>
                        
                                        <!--
                                        <asp:CheckBox ID="CheckBoxFleetCo" runat="server" Checked='<%# Eval("fleetco") %>' Visible="false" />
                                        <asp:Image ID="ImageFleetCo" runat="server" />
                                            -->
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CARSALES" SortExpression="carsales">
                                    <ItemTemplate>
                                            <asp:Label runat="server" ID="lbCarsales" Text=' <%# Eval("carsales").ToString() == "False" ? "" : "<b>Yes</b>"%>'   ></asp:Label>
                        
                                            <!--
                                        <asp:CheckBox ID="CheckBoxCarSales" runat="server" Checked='<%# Eval("carsales") %>' Visible="false" />
                                        <asp:Image ID="ImageCarSales" runat="server" />
                                                -->
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LICENSEE" SortExpression="licensee">
                                    <ItemTemplate>
                                        
                                            <asp:Label runat="server" ID="lbLicensee" Text=' <%# Eval("licensee").ToString() == "False" ? "" : "<b>Yes</b>"%>'   ></asp:Label>
                        
<!--
                                        <asp:CheckBox ID="CheckBoxLicensee" runat="server" Checked='<%# Eval("licensee") %>' Visible="false" />
                                        <asp:Image ID="ImageLicensee" runat="server" />
    -->
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditAreaCode" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteAreaCode" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlAreaCodes" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateAreaCodes" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddAreaCode" runat="server" Text="<%$ Resources:lang, AreaCodeDetailsAdd %>" OnClick="ButtonAddAreaCode_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
