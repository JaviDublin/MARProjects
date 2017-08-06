<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Fao.Master" AutoEventWireup="true" CodeBehind="AdditionsLimits.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.Factors.AdditionsLimits" %>


<%@ Register Src="~/FleetAllocation/UserControls/LimitFiles/MonthlyAdditionsLimits.ascx" TagPrefix="uc" TagName="MonthlyAdditionsLimits" %>







<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    
    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;"  >
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>    
                        <li><a href="#tabs-1">Addition Limits</a></li>
                        
  
                    </ul>
                    
                    <div id="tabs-1" >
                        <table style="text-align: center; margin-left: 10px; margin-right: auto;">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <uc:MonthlyAdditionsLimits runat="server" ID="MonthlyAdditionsLimits" />            
                                            </td>
                                        </tr>

                                    </table>
                                    
                                </td>
                            </tr>
                        </table>
                    </div>


                </div>
            </td>
        </tr>
    </table>
    
    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();
        });
    </script>

</asp:Content>
