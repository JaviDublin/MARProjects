<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetStatusGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Availability.FleetStatusGrid" %>

<%@ Register TagPrefix="uc" TagName="ExportToExcel" Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" %>
<style type="text/css">
    .NumberCell  {
         display: inline-block;
         width: 60px;
         text-align: right;
     }
    .WrapText {
        word-wrap: normal;
    }
</style>

<table  style="width: 100%;" >
    <tr >
        <td >
                <table runat="server" id="tblExportTable" style="text-align: left; width: 1090px; background-color: white; padding: 8px;"
                    class="StandardBorder">
                    <tr style="vertical-align: top;">
                        <td>
                            Location Country:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblLocationCountry" Font-Bold="True"/>
                        </td>
                        <td>
                            Owning Country:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblOwningCountry" Font-Bold="True" />
                        </td>
                        <td>
                            Fleet:
                        </td>
                        <td rowspan="8">
                            <asp:Label runat="server" ID="lblFleetTypes" Font-Bold="True" Width="100px" CssClass="WrapText"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pool / Region:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPoolRegion" Font-Bold="True" />
                        </td>
                        <td>
                            Car Segment:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCarSegment" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Location Group / Area:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblLocationGroupArea" Font-Bold="True" />
                        </td>
                        <td>
                            Car Class:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCarClass" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Location:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblLocation" Font-Bold="True" />
                        </td>
                        <td>
                            Car Group:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCarGroup" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDate" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Values:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblValues" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>Display:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDisplay" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>Day of Week:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDayOfWeek" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Fleet:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblTotalFleet" Font-Bold="True" CssClass="NumberCell" />
                            
                        </td>
                        <td>
                            BD:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblBd" CssClass="NumberCell" />
                            
                        </td>
                        <td>WS:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblWs" CssClass="NumberCell" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td>Operational Fleet:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblOperationalFleet" Font-Bold="True" CssClass="NumberCell" />
                            
                        </td>
                        <td>MM:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblMm" CssClass="NumberCell" />
                            
                        </td>
                        <td>TB:
                        </td>
                        <td>
                            
                            <asp:Label runat="server" ID="lblTb" CssClass="NumberCell" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td>AvailableFleet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblAvailableFleet" Font-Bold="True" CssClass="NumberCell" />
                        </td>
                        <td>TW:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTw" CssClass="NumberCell" />
                        </td>
                        <td>FS:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblFs" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            Shop Total:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblShop" CssClass="NumberCell" />
                        </td>
                        <td>
                            RL:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRl" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Overdue:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblOverdue" CssClass="NumberCell" />
                        </td>
                        <td>
                            Shop Total / Oper. Fleet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblShopTotalOverOperFleet" Font-Bold="True" CssClass="NumberCell" />
                        </td>
                        <td>
                            RP:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRp" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>Overdue / Available Fleet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblOverdueOverAvailable" Font-Bold="True" CssClass="NumberCell" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            TN:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTn" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            Idle:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblIdle" CssClass="NumberCell" />
                        </td>
                        <td>
                            SV:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSv" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            On Rent:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblOnRent" CssClass="NumberCell" />
                        </td>
                        <td>
                            SU:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSu" CssClass="NumberCell" />
                        </td>
                        <td>
                            TC:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTc" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Uti On Rent / Oper. Fleet incl. Overdue:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblUtiOnRentOperFleetInclOverdue" CssClass="NumberCell" />
                        </td>
                        <td>
                            Idle + SU:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblIdleAndSu" CssClass="NumberCell" />
                        </td>
                        <td>
                            HL:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblHl" CssClass="NumberCell" />
                        </td>
                    </tr>
                    <tr>
                        <td>Uti On Rent / Oper. Fleet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblUtiOnRentOperFleet" Font-Bold="True" CssClass="NumberCell" />
                        </td>
                        <td>
                            Idle + SU / AvailableFleet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblIdleAndSuOverAvailableFleet" Font-Bold="True" CssClass="NumberCell" />
                        </td>
                        <td>
                            HA:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblHa" CssClass="NumberCell" />
                        </td>
                    </tr>

                </table>
            
            
        </td>
    </tr>
    <tr>
        <td>
            <uc:ExportToExcel ID="ucExportToExcel" runat="server"  Visible="True"/>
        </td>
    </tr>
    
</table>






