<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.AvailabilityTool.CarSearch.Default" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <%-- Update Panel --%>
    <asp:UpdatePanel ID="UpdatePanelCarSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server" />
                <%-- Report Selections --%>
                <uc:ReportSelection ID="UserControlReportSelections" runat="server" />
                <%-- Car Details --%>
                <uc:CarSearchDetails ID="CarSearchDetails" runat="server" OnSaveRemarks="ButtonSaveRemarks_Click" />
                <%-- CarSearch Gridview --%>
                <div class="divNoPrint">
                    <asp:Panel ID="PanelCarSearch" runat="server" Visible="false">
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:GridView ID="GridviewCarSearch" runat="server" AllowSorting="True" AutoGenerateColumns="False" OnRowCommand="GridviewCarSearch_RowCommand" OnRowCreated="GridviewCarSearch_RowCreated" OnSorting="GridviewCarSearch_Sorting" OnRowDataBound="GridviewCarSearch_RowDataBound"  CssClass ="GridViewStyle">
                                        <HeaderStyle CssClass="GridHeaderStyle" />
                                        <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <EditRowStyle CssClass="GridEditRowStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LSTWWD" HeaderText="LSTWWD" SortExpression="LSTWWD">
                                                <HeaderStyle VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LSTDATE" HeaderText="LSTDATE" SortExpression="LSTDATE">
                                                <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VC" HeaderText="VC" SortExpression="VC">
                                                <ItemStyle HorizontalAlign="Left" Width="30px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIT" HeaderText="UNIT" SortExpression="UNIT">
                                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LICENSE" HeaderText="LICENSE" SortExpression="LICENSE">
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Model" HeaderText="MODEL" SortExpression="Model">
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ModelDesc" HeaderText="MODDESC" SortExpression="ModelDesc">
                                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DUEWWD" HeaderText="DUEWWD" SortExpression="DUEWWD">
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DUEDATE" HeaderText="DUEDATE" SortExpression="DUEDATE">
                                                <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DUETIME" HeaderText="DUETIME" SortExpression="DUETIME">
                                                <ItemStyle HorizontalAlign="Left" Width="70px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OP" HeaderText="OP" SortExpression="OP">
                                                <ItemStyle HorizontalAlign="Left" Width="30px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MT" HeaderText="MT" SortExpression="MT">
                                                <ItemStyle HorizontalAlign="Left" Width="30px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HOLD" HeaderText="HOLD" SortExpression="HOLD">
                                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NR" HeaderText="NR" SortExpression="NR">
                                                <ItemStyle HorizontalAlign="Right" Width="30px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DRIVER" HeaderText="DRIVER" SortExpression="DRIVER">
                                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DOC" HeaderText="DOC" SortExpression="DOC">
                                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LSTMLG" HeaderText="LSTMLG" SortExpression="LSTMLG">
                                                <ItemStyle HorizontalAlign="Left" Width="65px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SERIAL" HeaderText="SERIAL" SortExpression="SERIAL" Visible="False"></asp:BoundField>
                                            <asp:TemplateField HeaderText="REMARKS" SortExpression="remark">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ImageButtonRemarks" ImageUrl="~/App.Images/pin-yellow.png" CommandArgument='<%# Eval("SERIAL") %>' CommandName="SelectVehicle" AlternateText="Click to show / edit car remarks." Width="27px" Height="20px" />
                                                    <br />
                                                    <asp:Label runat="server" ID="LabelRemark" Text='<%# Eval("REMARK") %>' Width="70px"></asp:Label>
                                                    <asp:Label runat="server" ID="LabelERDPASSED" Text='<%# Eval("ERDPASSED") %>' Width="70px" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" Wrap="true" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="rowPager">
                                    <div class="divPagerRecordsSelected">
                                        <asp:Label ID="LabelTotalRecords" runat="server" Text="<%$ Resources:lang, LabelTotalRecords %>"></asp:Label>
                                        <asp:Label ID="LabelTotalRecordsDisplay" runat="server" CssClass="labelRecordsSelected"></asp:Label>
                                    </div>
                                    <div class="divPagerControl">
                                        <uc:PagerControl ID="PagerControlCarSearch" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPage_SelectedIndexChanged" OnDropDownListRowsSelectedIndexChanged="DropDownListRows_SelectedIndexChanged" OnPageIndexCommand="GetPageIndex" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <%-- No Data --%>
                    <uc:EmptyDataTemplate ID="EmptyDataTemplateCarSearch" runat="server" Visible="false" />
                </div>
            </div>

            <%-- Report Settings --%>
            <uc:ReportSettings ID="UserControlReportSettings" runat="server" OnGenerateReport="ButtonGenerateReport_Click" />
            <%-- Confirmation Modal Dialog --%>
            <uc:ModalConfirm ID="ModalConfirmCarSearch" runat="server" />

            <%-- Added by Gavin 28 Mar 12 --%>            
            <uc:CarFilter ID="CarFilter1" runat="server" On_filterClickEventHandler="OnFilterClicked" On_downloadClickEventHandler="OnDownloadClicked" />
            <%-- ------------------------ --%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress runat="server" ID="upChartProgress" AssociatedUpdatePanelID="UpdatePanelCarSearch" DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
         </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
