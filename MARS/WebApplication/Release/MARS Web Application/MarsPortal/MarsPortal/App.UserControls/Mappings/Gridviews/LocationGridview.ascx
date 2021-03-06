﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationGridview.ascx.cs" Inherits="App.UserControls.Mappings.Gridviews.LocationGridview" %>
<asp:UpdatePanel ID="UpdatePanelMappingGridview" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <%-- Details --%>
        <uc:MappingLocationDetails ID="MappingLocationDetails" runat="server" OnSaveMappingDetails="MappingDetailsSave_Click" />
        <%-- Title --%>
        <div class="divMappingGridviewTitle">
            <asp:Label ID="LableTitle" runat="server" Text="<%$ Resources:lang, GridviewTitleLocations %>"></asp:Label>
        </div>
        <%-- Gridview --%>
        <asp:Panel ID="PanelLocations" runat="server" Visible="false">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridviewLocations" runat="server" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="location" onrowcommand="GridviewLocations_RowCommand" onrowcreated="GridviewLocations_RowCreated" onrowdatabound="GridviewLocations_RowDataBound" onsorting="GridviewLocations_Sorting" CssClass="GridViewStyle">
                            <HeaderStyle CssClass="GridHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                            <RowStyle CssClass="GridRowStyle" />
                            <EditRowStyle CssClass="GridEditRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="location" HeaderText="LOCN" SortExpression="location" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="location_dw" HeaderText="LOCN DW" SortExpression="location_dw" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="real_location_name" HeaderText="REAL LOCN" SortExpression="real_location_name" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="location_name" HeaderText="LOCN NAME" SortExpression="location_name" ReadOnly="True">
                                    <ItemStyle Width="130px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="location_name" HeaderText="LOCN NAME DW" SortExpression="location_name_dw" ReadOnly="True">
                                    <ItemStyle Width="130px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="ACTIVE" SortExpression="active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxActive" runat="server" Checked='<%# Eval("active") %>' Visible="false" />
                                        <asp:Image ID="ImageActive" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ap_dt_rr" HeaderText="APDTRR" SortExpression="ap_dt_rr" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cal" HeaderText="CAL" SortExpression="cal" ReadOnly="True">
                                    <ItemStyle Width="75px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ops_area" HeaderText="OPS AREA" SortExpression="ops_area" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cms_location_group_id" HeaderText="LOCN CODE" SortExpression="cms_location_group_id" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cms_location_group" HeaderText="LOCN GRP" SortExpression="cms_location_group" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="served_by_locn" HeaderText="SRVD LOCN" SortExpression="served_by_locn" ReadOnly="True">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="turnaround_hours" HeaderText="TRN HRS" SortExpression="turnaround_hours" ReadOnly="True">
                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="<%$ Resources:lang, Edit %>" CommandName="EditLocation" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        /
                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="<%$ Resources:lang, Delete %>" CommandName="DeleteLocation" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
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
                            <uc:PagerControl ID="PagerControlLocations" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- NoData --%>
        <uc:EmptyDataTemplate ID="EmptyDataTemplateLocations" runat="server" Visible="false" />
        <%-- Add Record --%>
        <table cellpadding="0" cellspacing="0" class="mappingGridviewPanelFooter">
            <tr>
                <td>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td class="columnButtonAdd">
                    <asp:Button ID="ButtonAddLocation" runat="server" Text="<%$ Resources:lang, LocationDetailsAdd %>" OnClick="ButtonAddLocation_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
