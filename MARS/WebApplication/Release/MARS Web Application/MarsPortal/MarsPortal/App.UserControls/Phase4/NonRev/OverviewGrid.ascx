<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverviewGrid.ascx.cs" EnableViewState="true"
    Inherits="Mars.App.UserControls.Phase4.NonRev.OverviewGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/NonRev/MultiReasonEntry.ascx" TagName="MultiReasonEntry" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>


<asp:HiddenField runat="server" ID="hfEmptyGrid" Value="1" />
<asp:HiddenField runat="server" ID="hfShowApproveButton" Value="0" />

<asp:HiddenField runat="server" ID="hfShowMultiSelectTickBoxes" Value="1" />

<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />

<asp:HiddenField runat="server" ID="hfShowNonRevFields" Value="False" />
<asp:HiddenField runat="server" ID="hfShowForeignVehicleFields" Value="False" />
<asp:HiddenField runat="server" ID="hfShowAvailabilityFields" Value="False" />

<table>
    <tr>
        <td>
            <asp:GridView runat="server" ID="gvOverview" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                OnSorting="GridviewOverview_Sorting" AllowPaging="True" Width="1000px" BorderStyle="None"
                OnDataBound="GridviewOverview_DataBound">
                <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                <RowStyle CssClass="StandardDataGrid" />
                <PagerSettings Position="Bottom" />
                <PagerTemplate>
                    <table style="width: 100%; background-color: white;" >
                        <tr>
                            <td style="text-align: left; margin-top: 0;">
                                <asp:Label runat="server" ID="lblRowCount" />
                            </td>

                            <td style="float: right;">Page Size:
                                <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SizeChange">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="200" Value="200" />
                                    <asp:ListItem Text="500" Value="500" />
                                </asp:DropDownList>
                                <asp:ImageButton runat="server" ID="lbgvFirst" ImageUrl="~/App.Images/pager-first.png" OnClick="gvFirstButton_Click" />
                                <asp:ImageButton runat="server" ID="lbgvPrevious" ImageUrl="~/App.Images/pager-previous.png" OnClick="gvPreviousButton_Click" />
                                <asp:Label runat="server" ID="lblPageAt" Text="Page 1 of 2" />
                                <asp:ImageButton runat="server" ID="lbgvNext" ImageUrl="~/App.Images/pager-next.png" OnClick="gvNextButton_Click" />
                                <asp:ImageButton runat="server" ID="lbgvLast" ImageUrl="~/App.Images/pager-last.png" OnClick="gvLastButton_Click" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%" >
                        <tr style="text-align: left; margin-top: 0;">
                            <td>
                                <asp:Button runat="server" ID="btnAddGroupRemark" CssClass="StandardButton" Text="Edit Selected" Width="120px" OnClick="btnAddGroupRemark_Click" />
                                <asp:Button runat="server" ID="btnApproveAll" CssClass="StandardButton" Text="Approve All" Width="120px" OnClick="btnApproveAll_Click" />
                                <asp:ConfirmButtonExtender runat="server" TargetControlID="btnApproveAll"
                                    ConfirmText="Do you wish to confirm all the above Vehicles" />
                            </td>
                            <td style="float: right; text-align: center;">
                                <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                            </td>
                        </tr>
                    </table>
                </PagerTemplate>
                <EditRowStyle CssClass="GridEditRowStyle" />

                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="8px" HorizontalAlign="Center" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAllNoStyle(this);" />
                        </HeaderTemplate>
                        <ItemStyle Width="8px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hfVehicleId" Value='<%#Eval("VehicleId") %>' runat="server" />
                            <asp:CheckBox ID="cbVehicle" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastLocationCode" HeaderText="Check Out Location" SortExpression="LastLocationCode">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CarGroup" HeaderText="Group" SortExpression="CarGroup">
                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LicensePlate" HeaderText="License" SortExpression="LicensePlate">
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnitNumber" HeaderText="Unit" SortExpression="UnitNumber">
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ModelDescription" HeaderText="Model Description" SortExpression="ModelDescription">
                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastChangeDateTime" HeaderText="Check Out Date" SortExpression="LastChangeDateTime"
                        DataFormatString="{0:dd/MM/yyyy}">
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ExpectedLocationCode" HeaderText="Check In Location" SortExpression="ExpectedLocationCode">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ExpectedDateString" HeaderText="Check In Date" SortExpression="ExpectedDateTime">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocumentNumber" HeaderText="Last Document" SortExpression="DocumentNumber">
                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="LastDriverName" HeaderText="Customer" SortExpression="LastDriverName">
                        <ItemStyle HorizontalAlign="Left" Width="70px" /> 
                    </asp:BoundField>
                    <asp:BoundField DataField="OwningCountry" HeaderText="Owning Country" SortExpression="OwningCountry">
                        <ItemStyle HorizontalAlign="Left"  />       
                    </asp:BoundField>
                    <asp:BoundField DataField="LastMilage" HeaderText="Last Mileage" DataFormatString="{0:#,0}" SortExpression="LastMilage">
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NonRevDays" HeaderText="Days Non Rev" DataFormatString="{0:#,0}" SortExpression="NonRevDays">
                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="InCountryDays" HeaderText="Days In Country" DataFormatString="{0:#,0}" SortExpression="InCountryDays">
                        <ItemStyle HorizontalAlign="Right" Width="50px" /> 
                    </asp:BoundField>
                    <asp:BoundField DataField="OperationalStatusCode" HeaderText="OP" SortExpression="OperationalStatusCode">
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MovementTypeCode" HeaderText="MT" SortExpression="MovementTypeCode">
                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CommentShort" HeaderText="Comment" SortExpression="CommentShort">
                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField SortExpression="Processed">
                        <ItemStyle Width="40px" HorizontalAlign="Center" /> 
                        <ItemTemplate>
                            <asp:Image ID="NextRentColor" runat="server" Visible='<%#Eval("NextRentColor") != string.Empty %>' ImageUrl='<%#Eval("NextRentColor") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastReason" HeaderText="Code" SortExpression="LastReason" >
                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastRemark" HeaderText="Remark" SortExpression="LastRemark" >
                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Select" CommandName="ShowVehicle"
                                CommandArgument='<%# Eval("VehicleId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Serial" SortExpression="Serial">
                        <ItemStyle HorizontalAlign="Left" Width="8px" Font-Size="1" ForeColor="White" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>
</table>

<asp:Panel ID="pnlMultiReasonPopup" runat="server">
    <uc:MultiReasonEntry runat="server" ID="ucMultiReasonEntry" />
</asp:Panel>
<asp:Button runat="server" ID="btnDummy" Style="display: none;" />
<asp:ModalPopupExtender ID="mpeMultiReasonEntry" runat="server" TargetControlID="btnDummy" 
    PopupControlID="pnlMultiReasonPopup" BackgroundCssClass="modalBackground" />
