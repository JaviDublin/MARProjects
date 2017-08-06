<%@ Control Language="C#" AutoEventWireup="true" Inherits="App.UserControls.Parameters.DynamicReportParameters"
    CodeBehind="DynamicReportParameters.ascx.cs" %>
<asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <uc:QuickSelectLG runat="server" ID="quickSelectLocationGroup" />
                    
                    <uc:DateRangePicker ID="ucDateRange" runat="server" />
                    <uc:DatePicker id="ucDatePicker" runat="server" Visible="False" />
                    <asp:Panel runat="server" ID="pnlSingleDate" Visible="False">
                        
                        <fieldset style="height: 39px">
                            <legend>
                                Date Selection
                            </legend>
                            <table> 
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="tbDate" Width="75px" AutoPostBack="True" />
                                </td>
                            </tr>
                            </table>
                
                        </fieldset>
                        
                        <asp:CompareValidator ID="cvFromDate" runat="server" ControlToValidate="tbDate" ErrorMessage="Invalid Date Format"
                        Text="*" Operator="DataTypeCheck" Type="Date" ValidationGroup="Dates" ForeColor="Red"/>
                        <asp:CalendarExtender ID="ceFromDateExtender" runat="server" TargetControlID="tbDate" Format="dd/MM/yyyy" />
                    </asp:Panel>
                    
                </td>
                <td valign="top">
                    <asp:Panel ID="pnlDynamicParameters" runat="server" CssClass="dynamicFieldset">
                        <fieldset>
                            <legend><asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, DynamicParametersLegend %>" /> 
                                    <asp:RadioButtonList runat="server" ID="rblOpsSelector" OnSelectedIndexChanged="OpsLogicChanged" RepeatDirection="Horizontal" AutoPostBack="True" >
                                        <asp:ListItem Text="CMS" Selected="True" />
                                        <asp:ListItem Text="OPS" />
                                    </asp:RadioButtonList>
                            </legend>       
                            <asp:PlaceHolder runat="server" ID="phParameterTable" />
                        </fieldset>            
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
