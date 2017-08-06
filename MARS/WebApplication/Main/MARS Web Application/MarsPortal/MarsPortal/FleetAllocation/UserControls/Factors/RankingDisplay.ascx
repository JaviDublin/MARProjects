<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RankingDisplay.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Factors.RankingDisplay" %>


<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>


<asp:UpdatePanel runat="server" ID="upnlParameters">
    <ContentTemplate>
        <table style="text-align: center; width: 1000px; margin-left: auto; margin-right: auto;">
            <tr>
                <td style="text-align: right; width: 50%;">Country:
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                        CssClass="SingleDropDownList" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnLoad" Text="Load" OnClick="btnLoad_Click"
                        CssClass="StandardButton" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="height: 450px; vertical-align: top;">
                                <table>
                                    <tr>
                                        <td>
                                            <uc:FaoParameter runat="server" ID="ucFaoParameter" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnFilter" Text="Filter" OnClick="btnFilter_Click"
                                                CssClass="StandardButton" Visible="False" />
                                        </td>

                                    </tr>
                                </table>
                            </td>
                            <td style="height: 450px; vertical-align: top;">
                                <uc:AutoGrid runat="server" ID="agLimits" AutoGridWidth="450" PageSize="30" HideLastColumn="True" />
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
        </table>




    </ContentTemplate>
</asp:UpdatePanel>



