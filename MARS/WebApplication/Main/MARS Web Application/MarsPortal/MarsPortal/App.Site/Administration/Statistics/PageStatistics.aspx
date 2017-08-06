<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true" CodeBehind="PageStatistics.aspx.cs" Inherits="Mars.App.Site.Administration.Statistics.PageStatistics" %>

<%@ Register Src="~/App.UserControls/Phase4/Statistics/PageUsageChart.ascx" TagPrefix="uc" TagName="PageUsageChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">


    <table style="text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-left: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; height: 430px; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Chart</a></li>
                        <li><a href="#tabs-2">Grid</a></li>
                    </ul>
                    <div id="tabs-1" style="text-align: left;">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:PageUsageChart runat="server" ID="ucPageUsageChart" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-2">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load" Width="80px" OnClick="btnLoad_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>

                <table>
                    <tr>

                        <td style="vertical-align: top;">
                            <table>

                                <tr>
                                    <td>From Date:</>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox ReportDateBox"
                                            ID="tbFromDate" Text="<%# DateTime.Now.Date.ToShortDateString() %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>To Date:</>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="80px" CssClass="AutoCompleteTextBox ReportDateBox"
                                            ID="tbToDate" Text="<%# DateTime.Now.Date.ToShortDateString() %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Results:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTakeCount" CssClass="AutoCompleteTextBox" Width="80px">
                                            <asp:ListItem Text="10" Value="10" Selected="True" />
                                            <asp:ListItem Text="20" Value="20" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <fieldset style="width: 700px; height: 80px; font-size: 12px;">
                                <legend>Page Area Selection</legend>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:ListView ID="lvPages" runat="server" OnItemCommand="RepeaterCommand" GroupItemCount="3">
                                            <LayoutTemplate>
                                                <table>

                                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>

                                                </table>
                                            </LayoutTemplate>
                                            <GroupTemplate>
                                                <tr>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                                </tr>
                                            </GroupTemplate>
                                            <ItemTemplate>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Description") %>'
                                                        ForeColor='<%# DataBinder.Eval(Container.DataItem, "ForeColour") %>' />

                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lbBranch" Text='<%# ( (bool) Eval("Selected")) ? "Unselect" : "Select" %>'
                                                        CommandName='<%# ( (bool) Eval("Selected")) ? UnSelectCommand : SelectCommand  %>'
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MenuId") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>
    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();

            $(".ReportDateBox").datepicker({ dateFormat: "dd/mm/yy" }).datepicker("setDate", new Date());

        });
    </script>


</asp:Content>
