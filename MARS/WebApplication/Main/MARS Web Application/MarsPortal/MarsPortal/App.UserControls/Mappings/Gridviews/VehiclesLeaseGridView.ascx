<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehiclesLeaseGridView.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.VehiclesLeaseGridView" %>

<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingVehiclesLeaseDetails ID="MappingVehiclesLeaseDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleVehiclesLease %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelVehiclesLease" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewVehiclesLease" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="Serial" onrowcommand="GridviewVehiclesLease_RowCommand" onrowcreated="GridviewVehiclesLease_RowCreated" onrowdatabound="GridviewVehiclesLease_RowDataBound" onsorting="GridviewVehiclesLease_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="Serial" HeaderText="SERIAL" ReadOnly="True" SortExpression="Serial">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Plate" HeaderText="PLATE" ReadOnly="True" SortExpression="Plate">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Unit" HeaderText="UNIT" ReadOnly="True" SortExpression="Unit">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ModelDescription" HeaderText="MODEL" ReadOnly="True" SortExpression="ModelDescription">
                                    <ItemStyle Width="250px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Country_Owner" HeaderText="COUNTRY OWNER" ReadOnly="True" SortExpression="Country_Owner">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Country_Rent" HeaderText="COUNTRY RENT" ReadOnly="True" SortExpression="Country_Rent">
                                    <ItemStyle Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StartDate" HeaderText="START DATE" ReadOnly="True" SortExpression="StartDate" />
                               

                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditSerial" CommandArgument='<%# Eval("Serial") %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteSerial" CommandArgument='<%# Eval("Serial") %>'></asp:LinkButton>
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
                            
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateVehiclesLease" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddVehiclesLease" runat="server" Text="<%$ Resources:lang, VehiclesLeaseDetailsAdd %>" OnClick="ButtonAddVehiclesLease_Click" />
                    <asp:Button ID="ButtonDeleteVehiclesLease" runat="server" Text="<%$ Resources:lang, VehiclesLeaseDetailsDelete %>" OnClick="ButtonDeleteVehiclesLease_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>