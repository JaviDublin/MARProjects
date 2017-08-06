<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommunicationsPanel.ascx.cs"
    Inherits="Mars.App.UserControls.Panel.CommunicationsPanel" %>

<table style="height: 400px; text-align: center; margin-left: auto; margin-right: auto;">
    <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
        <td>
            <div id="tabbedPanel" style="width: 1050px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                <ul>
                    <li style="width: 100%; text-align: center; height: 35px;">
                        <h1>Home Page</h1>
                    </li>
                </ul>
            </div>
            <div id="tabs-1">
                <table style="width: 1050px;">
                    <tr>
                        <td colspan="2" style="text-align: left; font-size: 12px;">For any support issues, contact the <a target="_blank" href="http://intranet.hertz.com/hit/service-desk/default.aspx">Hertz Information Techonology Support Desk</a>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="width: 85%; text-align: center;">
                            <table style="width: 100%;">
                                <tr style="height: 300px; vertical-align: top; overflow-y: scroll;">
                                    <td>
                                        <asp:Repeater runat="server" ID="rptComm">
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlNews" runat="server" ScrollBars="Auto" Width="100%"
                                                    GroupingText='<%# DataBinder.Eval(Container.DataItem, "CommDate", "{0:D}")%>'
                                                    HorizontalAlign="Left">
                                                    <div class="divHeading" style="font-size: medium; font-weight: bold; color: #808080;">
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Heading")%>' />
                                                    </div>
                                                    <br />
                                                    <div class="divDetails" style="font-size: small">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Details")%>' />
                                                    </div>
                                                    <br />
                                                </asp:Panel>
                                                <br />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr style="text-align: left;">
                                    <td>
                                        <fieldset>
                                            <legend style="font-weight: bold; font-size: 12px;">Country Administrators</legend>
                                            <table style="border-spacing: 10px; font-size: 13px">
                                                <tr>
                                                    <td>
                                                        <asp:GridView runat="server" ID="gvCountryAdmins" AllowSorting="False" AutoGenerateColumns="False"
                                                            Width="600px">
                                                            <HeaderStyle CssClass="StandardDataGridHeaderStyle" HorizontalAlign="Center" />
                                                            <RowStyle CssClass="StandardDataGrid" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Country" HeaderText="Country">
                                                                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Name" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Email" HeaderText="E-Mail Address">
                                                                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td style="vertical-align: top;">Country Administrators are responsible for:
                                                        <br />
                                                        - Assigning additional access within MARS
                                                        <br />
                                                        - Updating Pools, Location Groups, Areas and Regions
                                                        <br />
                                                        - Managing Licensee Companies and Fleets<br />
                                                    </td>
                                                </tr>
                                            </table>

                                        </fieldset>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="float: right;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Repeater ID="rptDocs" OnItemCommand="rptDocs_Command" runat="server">
                                            <HeaderTemplate>
                                                <fieldset>
                                                    <legend style="font-size: medium; font-weight: bold; color: #808080;">Documentation
                                                    </legend>
                                                    <table style="text-align: left;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="color: #808080; font-size: 12px;">
                                                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>" target="#">
                                                            <%# DataBinder.Eval(Container.DataItem, "DocumentName") %>
                                                        </a>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>  
                                                </fieldset>                  
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Repeater ID="rptTraining"  runat="server">
                                            <HeaderTemplate>
                                                <fieldset>
                                                    <legend style="font-size: medium; font-weight: bold; color: #808080;">Training
                                                    </legend>
                                                    <table style="text-align: left;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="color: #808080; font-size: 12px;">
                                                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>" target="_blank">
                                                            <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                                        </a>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>  
                                                </fieldset>                  
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>

            </div>

        </td>
    </tr>
</table>

<script type="text/javascript">
    $(function () {
        $("#tabbedPanel").tabs();
    });
</script>

<%--<asp:Table ID="Table1" runat="server">
    <asp:TableRow runat="server" HorizontalAlign="Center">
        <asp:TableCell runat="server" Width="200px" />
        <asp:TableCell runat="server" Width="1000px">
            
        </asp:TableCell>
        <asp:TableCell runat="server" Width="50px">
            
            &nbsp;
        </asp:TableCell>
        <asp:TableCell runat="server" Width="200px" VerticalAlign="Top">
            
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>--%>
