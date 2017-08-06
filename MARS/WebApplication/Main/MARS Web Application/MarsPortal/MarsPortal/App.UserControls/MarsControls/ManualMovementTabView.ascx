<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualMovementTabView.ascx.cs" Inherits="App.UserControls.ManualMovementTabView" %>
<%@ Register Src="~/App.UserControls/MarsControls/ManualMovementGridView.ascx" TagName="ManualMoveMentGridview" TagPrefix="mmgv" %>
<%@ Register Src="~/App.UserControls/Parameters/GeneralReportParameters.ascx" TagPrefix="uc" TagName="GeneralReportParameters" %>

<asp:UpdatePanel ID="upMovementTabs" runat="server">
    <ContentTemplate>
        
        <asp:UpdatePanel ID="upnlGeneralParameters" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc:GeneralReportParameters ID="GeneralParams" 
                    runat="server" HideReportTypeControl="True" 
                    ShowQuickLocationGroupBox="False"
                    SingleDateSelection="True"
                    DisplayExcelFilteringParameters="True"
                    RootParameterPostback="True"
                    GridviewDatePicker="True" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:HiddenField ID="hdnTabIndex" runat="server" />

        <div style="width:900px; float:left;">
        <asp:Panel ID="pnlManualMovementGrid" runat="server" GroupingText="Results" 
            meta:resourcekey="pnlManualMovementGridResource2">
        
        <table border="0">
            <tr>
                <td >
                    <asp:Menu runat="server" ID="menuManualMovementSelection" Orientation="Horizontal"
                        onmenuitemclick="menuManualMovementSelection_MenuItemClick" Width="800px" 
                        meta:resourcekey="menuManualMovementSelectionResource2" >
                        <Items>
                            <asp:MenuItem Text="Actual" Value="0" Selected="True" 
                                meta:resourcekey="MenuItemResource5"/>
                            <asp:MenuItem Text="Scenario1" Value="1" meta:resourcekey="MenuItemResource6" />
                            <asp:MenuItem Text="Scenario2" Value="2" meta:resourcekey="MenuItemResource7" />
                            <asp:MenuItem Text="Scenario3" Value="3" meta:resourcekey="MenuItemResource8" />
                        </Items>
                        <StaticMenuItemStyle BackColor="LightGray" BorderColor="AliceBlue" 
                            BorderStyle="Groove" BorderWidth="2px" CssClass="staticitem" Font-Size="Medium" 
                            ForeColor="Black" />
                        <StaticSelectedStyle BackColor="#FAFAD2" BorderStyle="Inset" 
                            CssClass="staticselected" />
                    </asp:Menu>
                    </td>

            </tr>
        </table>

        <div style="height:290px;">
            <asp:MultiView ID="mvManualMovement" runat="server">
                <asp:View ID="viewActual" runat="server"> 
                    <asp:Panel ID="pnlActual" runat="server" 
                        CssClass="mgt-movement-option-container" meta:resourcekey="pnlActualResource2">                      
                        <div>
                            <mmgv:ManualMoveMentGridview id="mmGridViewActual" runat="server" ScenarioID="1" />
                        </div>                  
                    </asp:Panel>                    
                </asp:View>


                <asp:View ID="viewScenario1" runat="server">
                    <asp:Panel ID="pnlSecnario1" runat="server" 
                        CssClass="mgt-movement-option-container" 
                        meta:resourcekey="pnlSecnario1Resource2"> 
                        <div>
                            <mmgv:ManualMoveMentGridview id="mmGridViewScenario1" runat="server" ScenarioID="2" />
                        </div>
                    </asp:Panel>
                </asp:View>


                <asp:View ID="viewScenario2" runat="server">
                    <asp:Panel ID="pnlScenario2" runat="server" 
                        CssClass="mgt-movement-option-container" 
                        meta:resourcekey="pnlScenario2Resource2"> 
                        <div>
                            <mmgv:ManualMoveMentGridview id="mmGridViewScenario2" runat="server" ScenarioID="3" />
                        </div>
                    </asp:Panel>
                </asp:View>


                <asp:View ID="viewScenario3" runat="server">
                    <asp:Panel ID="pnlScenario3" runat="server" 
                        CssClass="mgt-movement-option-container" 
                        meta:resourcekey="pnlScenario3Resource2"> 
                        <div>
                            <mmgv:ManualMoveMentGridview id="mmGridViewScenario3" runat="server" ScenarioID="4" />
                        </div>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
        </div>
        </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>