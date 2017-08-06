<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModelCodeGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.ModelCodeGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingModelCodeDetails ID="MappingModelCodeDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleModelCodes %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelModelCodes" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewModelCodes" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="model_id" onrowcommand="GridviewModelCodes_RowCommand" onrowcreated="GridviewOModelCodes_RowCreated" onrowdatabound="GridviewModelCodes_RowDataBound" onsorting="GridviewModelCodeSorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="model" HeaderText="MODEL" SortExpression="model" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="country" HeaderText="COUNTRY" SortExpression="country" ReadOnly="True"></asp:BoundField>
                                <asp:TemplateField HeaderText="ACTIVE" SortExpression="active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxActive" runat="server" Checked='<%# Eval("active") %>' Visible="false" />
                                        <asp:Image ID="ImageActive" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditModelCode"></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteModelCode"></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlModelCodes" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateModelCodes" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddModelCode" runat="server" Text="<%$ Resources:lang, ModelCodeDetailsAdd %>" OnClick="ButtonModelCode_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
