<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarFilter.ascx.cs" Inherits="App.AvailabilityTool.CarSearch.Filters.CarFilter" %>
  
<asp:UpdatePanel ID="UpdatePanelFilter" runat="server">
    <ContentTemplate>
        <asp:Panel ID="PanelCarFilter" runat="server"  CssClass="divReportSettingsWrapper">
            <div class="labelReportSettings">Car Filters</div>
            <hr />
            <table  class="tableReportSettingsSearchCriteria">
                <tr>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityUnit" runat="server" CssClass="labelReportSettings"
                            Text="Unit:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxUnit" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityLic" runat="server" CssClass="labelReportSettings"
                            Text="License:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxLicense" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityModel" runat="server" CssClass="labelReportSettings"
                            Text="Model:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxModel" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                        <td class="labelColumnReportSettings">
                            <asp:Label ID="LabelModDesc" runat="server" CssClass="labelReportSettings"
                                Text="Model Description:" />
                        </td>
                    <td class="dataColumnReportSettings">
                            <asp:TextBox ID="TextBoxModDesc" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                </tr>
                <tr>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityVIN" runat="server" CssClass="labelReportSettings"
                            Text="VIN:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxVin" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityCustomer" runat="server" CssClass="labelReportSettings"
                            Text="Customer Name:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxName" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityColour" runat="server" CssClass="labelReportSettings"
                            Text="Colour:" />
                    </td>
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxColour" runat="server" Width="120px" CssClass="dropDownListReportSettings" />
                    </td>
                    <td class="labelColumnReportSettings">
                        <asp:Label ID="LabelAvailabilityMileage" runat="server" CssClass="labelReportSettings"
                            Text="Mileage:" />
                    </td> 
                    <td class="dataColumnReportSettings">
                        <asp:TextBox ID="TextBoxMiles" runat="server" ToolTip="Vehicle mileage is greater than" 
                            Width="120px" CssClass="dropDownListReportSettings"/>
                    </td>
                </tr>
            </table>
            <hr />
            <table class="tableReportSettingsFooter">
                <tr>
                    <td class="buttonsColumnReportSettingsFooter">
                        <asp:Button ID="ButtonCarFilter" runat="server" Text="Filter Grid" CssClass="MarsV1GenerateReportButton"
                            ToolTip="Click to filter on criteria" onclick="ButtonCarFilter_Click" Width="100px" />
                    </td>
                    <td class="buttonsColumnReportSettingsFooter">
                        <asp:Button ID="ButtonClear" runat="server" Text="Clear Filters" CssClass="MarsV1GenerateReportButton"
                            ToolTip="Click to clear filter criteria"  onclick="ButtonClear_Click" Width="100px" />
                    </td>
                    <td class="buttonsColumnReportSettingsFooter">
                        <asp:Button ID="ButtonDownload" runat="server" Text="CSV Download" ToolTip="Download to Excel" 
                            onclick="ButtonDownload_Click" ClientIDMode="Static" CssClass="MarsV1GenerateReportButton" Width="100px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
<!--
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    //prm.add_endRequest(EndRequest);
    var postBackElement;
    function InitializeRequest(sender, args) {
        if (prm.get_isInAsyncPostBack()) {
            args.set_cancel(true);
        }
        postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'ButtonDownload') {
            //alert("postBackElement.id " + postBackElement.id);
            // This stops the progress panel showing
            // Infact it can be set to anythning and it still works!!!!!!
            $get('upChartProgress').style.display = 'none';
        }
    }
// -->
</script>



