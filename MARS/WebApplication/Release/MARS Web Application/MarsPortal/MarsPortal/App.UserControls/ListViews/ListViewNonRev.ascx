<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListViewNonRev.ascx.cs" Inherits="App.UserControls.ListViews.ListViewNonRev" %>
<asp:UpdatePanel runat="server" ID="UpdatePanelListViewNonRev" UpdateMode="Conditional"
    ChildrenAsTriggers="false">
    <ContentTemplate>
        <table width="100%">
            <tr>
                <td align="left" class="contentboxForm">
                    <asp:PlaceHolder ID="PlaceHolderData" runat="server">
                        <%-- Resizable Container --%>
                        <div id="ResizableListView" class="ui-widget-content">
                            <rad:ListView ID="ListViewNonRevOverView" runat="server" ItemPlaceholderID="PlaceHolderDetailsNonRev"
                                OnItemCommand="ListViewNonRevOverview_ItemCommand" LayoutTableID="tableNonRevOverview"
                                OnSorting="ListViewNonRevOverview_Sorting" ItemTemplateRow="rowListViewDetailsNonRev"
                                DataKeyNames="VehicleId" LayoutTableClass="listviewtable" LayoutTableDefaultHeight="350"
                                LayoutTableTheme="rad__listview-table" ResizableControl="ResizableListView">
                                <LayoutTemplate>
                                    <table cellpadding="0" cellspacing="0" class="listviewtable" id="tableNonRevOverview"
                                        runat="server" clientidmode="Static">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonSerial" runat="server" Text="<%$Resources:ListViewHeaders, Serial %>"
                                                        CommandArgument="Serial" ToolTip="<%$Resources:ToolTips, Serial%>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonPlate" runat="server" Text="<%$Resources:ListViewHeaders, Plate %>"
                                                        CommandArgument="Plate" ToolTip="<%$Resources:ToolTips, Plate%>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonUnit" runat="server" Text="<%$Resources:ListViewHeaders, Unit %>"
                                                        CommandArgument="Unit" ToolTip="<%$Resources:ToolTips, FileDateUnit%>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonMSODate" runat="server" Text="<%$Resources:ListViewHeaders, MSODate%>"
                                                        CommandArgument="MSODate" ToolTip="<%$Resources:ToolTips, MSODate%>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonCountryCode" runat="server" Text="<%$Resources:ListViewHeaders, CountryCode %>"
                                                        CommandArgument="CountryCode" ToolTip="<%$Resources:ToolTips,CountryCode %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonModelCode" runat="server" Text="<%$Resources:ListViewHeaders, ModelCode%>"
                                                        CommandArgument="ModelCode" ToolTip="<%$Resources:ToolTips,ModelCode %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonModelDescription" runat="server" Text="<%$Resources:ListViewHeaders, ModelDescription %>"
                                                        CommandArgument="ModelDescription" ToolTip="<%$Resources:ToolTips,ModelDescription %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonCarGroup" runat="server" Text="<%$Resources:ListViewHeaders, CarGroup%>"
                                                        CommandArgument="CarGroup" ToolTip="<%$Resources:ToolTips,CarGroup %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonAreaCode" runat="server" Text="<%$Resources:ListViewHeaders, AreaCode %>"
                                                        CommandArgument="AreaCode" ToolTip="<%$Resources:ToolTips, AreaCode %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonERDPassed" runat="server" Text="<%$Resources:ListViewHeaders, ERDPassed %>"
                                                        CommandArgument="ERDPassed" ToolTip="<%$Resources:ToolTips, ERDPassed %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                    <rad:ListViewHeader ID="LinkButtonRemark" runat="server" Text="<%$Resources:ListViewHeaders, Remark %>"
                                                        CommandArgument="Remark" ToolTip="<%$Resources:ToolTips, Remark %>"></rad:ListViewHeader>
                                                </th>
                                                <th>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr id="PlaceholderDetailsBuyer" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="rowListViewDetailsBuyer" runat="server">
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Serial")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Plate")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Unit")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("MSODate")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("CountryCode")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("ModelCode")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("ModelDescription")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("CarGroup")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("AreaCode")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("ERDPassed")))%>
                                        </td>
                                        <td>
                                            <%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Remark")))%>
                                        </td>
                                       
                                        <td>
                                           
                                        </td>
                                      
                                    </tr>
                                </ItemTemplate>
                            </rad:ListView>
                            <%-- Pager --%>
                            <rad:ListViewPager ID="ListViewPager" runat="server" OnPagerItemCommand="OnPager_Command"
                                DefaultPageSize="Fifteen" EditButtonVisible="false" DeleteButtonVisible="false" />
                        </div>
                    </asp:PlaceHolder>
                    <%-- No Data --%>
                    <asp:PlaceHolder ID="PlaceHolderNoData" runat="server">
                        <uc:EmptyDataTemplate runat="server" ID="EmptyData" />
                    </asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>