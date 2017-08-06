<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComparisonChart.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.ComparisonChart" %>

<asp:HiddenField runat="server" ID="hfSessionString" />

<table>
    <tr>
        <td style="vertical-align: top;">
            <div style="float: left;">
                <h3> </h3>
            </div>
        </td>
        <td>

            <table>
                <tr>
                    <td style="text-align: center;">Legend:
                        <asp:CheckBox runat="server" ID="cbLegend" AutoPostBack="True" Checked="True" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Chart ID="chrtComparison" CssClass="insetBorders" runat="server" Width="950" Height="355" ImageType="Png" OnClick="chrtComparison_Click">
                            <Legends>
                                <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
                                <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
                            </Legends>
                            <ChartAreas>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>


