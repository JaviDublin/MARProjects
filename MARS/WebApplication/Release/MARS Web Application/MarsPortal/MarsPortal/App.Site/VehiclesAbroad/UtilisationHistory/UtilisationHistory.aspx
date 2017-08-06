<%@ Page Title="Utilisation History" Theme="MarsV3"  Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="UtilisationHistory.aspx.cs" Inherits="App.Site.VehiclesAbroad.UtilisationHistory.UtilisationHistory2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <br />
    <h2 style="text-align:left">Vehicles Abroad - Utilisation History</h2>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="dataTableUtilisation" runat="server" clientidmode="Static">Loading data...</div>

                <div id="chartView" style="display:none">
                    <asp:Chart ID="Chart1" runat="server" Width="800px" Height="400px">
                        <Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>

                <asp:Panel ID="panelFilters" runat="server" GroupingText="Filters:"  Width="450px" Height="112px">
                    <table>
                        <tr>
                            <td>
                                <input id="buttonGrid" type="button" disabled="disabled" value="Show Grid" onclick="showGridChart(true);" />
                            </td>
                            <td>
                                <input id="buttonChart" type="button" value="Show Chart" onclick="showGridChart(false);" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Country:
                                <br />
                                <asp:DropDownList ID="DropDownListCountry" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="UpdateView">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Start Date:
                                <br />
                                <asp:TextBox runat="server" ID="textBoxStartDate" onchange='__doPostBack("date");'></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imageStartDate"  ImageUrl="~/App.Images/calendar.png" 
                                    AlternateText="Calendar" ToolTip="Click to show calendar" />  
                                <asp:CalendarExtender ID="textBoxStartDate_CalendarExtender" runat="server" 
                                    PopupButtonID="imageStartDate" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="textBoxStartDate">
                                </asp:CalendarExtender>
                                <br />
                                <asp:Label runat="server" ID="labelErrorStartDate" ForeColor="Red"></asp:Label>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                            </td>
                            <td>
                                End Date:
                                <br />
                                <asp:TextBox runat="server" ID="textboxEndDate" onchange='__doPostBack("date");'></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imageButtonEndDate"  ImageUrl="~/App.Images/calendar.png" 
                                    AlternateText="Calendar" ToolTip="Click to show calendar" /> 
                                <asp:CalendarExtender ID="textboxEndDate_CalendarExtender" runat="server" 
                                    PopupButtonID="imageButtonEndDate" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="textboxEndDate">
                                </asp:CalendarExtender>
                                <br />  
                                <asp:Label runat="server" ID="labelErrorEndDate" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>               
                    </table>                        
                </asp:Panel>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">

        // script to handle the visibilty of the grid or chart
        // arg cond is true for Grid and false for Chart
        function showGridChart(cond) {

            document.getElementById("buttonGrid").disabled = cond;
            document.getElementById("buttonChart").disabled = !cond;

            document.getElementById("dataTableUtilisation").style.display = cond == true ? "inline" : "none";
            document.getElementById("chartView").style.display = cond == true ? "none" : "inline";

            cond != cond;
        }    
    </script>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" SkinID="backgroundCover"></asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" SkinID="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;
                Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" SkinID="loadDataImage" />
            </asp:Panel>
        </ProgressTemplate>        
    </asp:UpdateProgress>

</asp:Content>