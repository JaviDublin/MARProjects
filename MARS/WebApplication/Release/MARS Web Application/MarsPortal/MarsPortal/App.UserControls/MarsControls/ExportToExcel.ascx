<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportToExcel.ascx.cs"
    Inherits="App.UserControls.ExportToExcel" %>

<asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divExport" runat="server" style="float: left;" >              
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlExportType"
                        GroupingText="Select Export Type:" Visible="false" Width="150px">
                            <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbTextExport" runat="server" AutoPostBack="true" 
                                        Text="Text Export" GroupName="Export" 
                                        oncheckedchanged="TextExport_CheckedChanged"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbExcelExport" runat="server" AutoPostBack="true" 
                                        Checked="true" Text="Excel Export"
                                        GroupName="Export" oncheckedchanged="ExcelExport_CheckedChanged"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <button runat="server" clientidmode="Static" class="DataExportButton" ID="mmsBtn" onclick='fseButtonClick(this,event); return false;' title='Select to change the commands in the text download.'>Command Text</button>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel> 
                </td> 
                <td> 
                    <asp:Panel ID="pnlExportParameters" runat="server"
                        GroupingText="Select Parameters" Visible="false" Width="150px" >
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlAdditionDeletion" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlScenarioType" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                                           
                </td>
                <td>  
                    <asp:Panel runat="server" ID="pnlExportGrouping" 
                        GroupingText="Select Level of detail:" Width="200px" 
                        meta:resourcekey="pnlExportGroupingResource2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGroupBySite" runat="server" Text="Site:" 
                                        meta:resourcekey="lblGroupBySiteResource2"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroupBySite" runat="server" 
                                        meta:resourcekey="ddlGroupBySiteResource2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGroupByFleet" runat="server" Text="Fleet:" 
                                        meta:resourcekey="lblGroupByFleetResource2" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroupByFleet" runat="server"  
                                        meta:resourcekey="ddlGroupByFleetResource2" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnLoadExcel" runat="server" CssClass="DataExportButton"
                                        OnClick="btnLoadExcel_click"  
                                        ValidationGroup="Dates" />       
                                </td>
                            </tr>
                        </table>                      
                    </asp:Panel> 
                </td>                    
            </tr>
        </table>
        </div>
    </ContentTemplate>

</asp:UpdatePanel>

