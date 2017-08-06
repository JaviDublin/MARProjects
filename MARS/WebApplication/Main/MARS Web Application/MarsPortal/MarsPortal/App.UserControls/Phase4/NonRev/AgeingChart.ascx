<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgeingChart.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.AgeingChart" %>

<table>
    <tr>
        <td style="text-align: center;">Legend:
             <asp:CheckBox runat="server" ID="cbLegend" Checked="True" AutoPostBack="True" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Chart ID="chrtAge" CssClass="insetBorders" runat="server" Width="950" Height="355" ImageType="Png" OnClick="chrtAge_Click">
                <Legends>
                    <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
                    <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
                    <asp:Legend Name="LeftLegend" Docking="Left" Enabled="True" Alignment="Center" />
                </Legends>
                <ChartAreas>
                </ChartAreas>

            </asp:Chart>
        </td>
    </tr>
</table>


