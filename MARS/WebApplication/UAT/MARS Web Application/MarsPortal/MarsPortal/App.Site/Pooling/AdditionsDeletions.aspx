<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="AdditionsDeletions.aspx.cs" Inherits="Mars.App.Site.Pooling.AdditionsDeletions" %>

<%@ Register Src="~/App.UserControls/DatePicker/SingleDateTimePicker.ascx" TagName="SingleDatePicker" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Pooling/AdditionsDeletions/AdditionDeletion.ascx" TagName="AdditionDeletion" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Helpicon.ascx" TagName="HelpIcon" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">


    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.SingleDatePicker').datepicker({ dateFormat: 'dd-mm-yy' });
                $('.SingleTimePicker').ptTimeSelect();
            }
        });

    </script>


    <asp:UpdatePanel runat="server" ID="upnlManipulation">
        <ContentTemplate>
            <div style="float: right; margin-right: 25px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
            </div>
            <center>
                <table>
                    <tr>
                        <td>
                            <h1>Additions &amp; Deletions</h1>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:DynamicParameters ID="DynamicParameters" runat="server" ShowSimpleDateSelector="True" SimpleDateSelected="False"
                                IsNonRevReport="True" ShowSimpleDatePicker="True" />
                        </td>
                    </tr>
                </table>
                <asp:CollapsiblePanelExtender ID="cpeBody" TargetControlID="pnlBody" runat="server" />
                <asp:Panel ID="pnlBody" runat="server">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="width: 800px">
                                    <legend>Additions &amp; Deletions - Upload For Country</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:FileUpload runat="server" ID="fuAdditionsDeletions" Height="23px"
                                                                Width="365px" />
                                                        </td>
                                                        <td>
                                                            <uc:HelpIcon runat="server" HoverImage="~/App.Images/Pooling/PoolingAdditionsDeletionsUpload.jpg" />
                                                        </td>
                                                    </tr>
                                                 </table>
                                            </td>
                                            <td style="text-align: right; width: 100%">
                                                <asp:Button runat="server" ID="btnParse" Text="Upload" OnClick="btnParse_Click" Width="130px" />
                                                <asp:Button runat="server" ID="btnSubmit" Text="Confirm" OnClick="btnSubmit_Click" Width="130px" />
                                                <asp:ConfirmButtonExtender runat="server" TargetControlID="btnSubmit"
                                                    ConfirmText="Submiting the data from this file will wipe all previous additions and deletions for the selected country.
                                                Do you wish to continue?" />
                                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" Width="130px" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblUploadMessage" runat="server" ForeColor="Red" />
                                            </td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="width: 800px">
                                    <legend>Additions &amp; Deletions - Manual Entry</legend>
                                    <uc:AdditionDeletion ID="adManualChanges" runat="server" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="width: 800px">
                                    <legend>Additions &amp; Deletions - 
                                    <asp:DropDownList runat="server" ID="ddlAddDelSelection" AutoPostBack="True">
                                        <asp:ListItem Value="Both" Text="Both" />
                                        <asp:ListItem Value="Additions" Text="Additions" />
                                        <asp:ListItem Value="Deletions" Text="Deletions" />
                                    </asp:DropDownList>

                                    </legend>

                                    <br />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptAddDel">
                                        <HeaderTemplate>
                                            <table style="width: 100%; text-align: center;" class="GridViewStyle">
                                                <tr style="font-size: 14px;">
                                                    <td>
                                                        <asp:Button runat="server" ID="btnDeleteManualEntries" Width="130px"
                                                            Text="Delete Selected" OnClick="btnDeleteManualEntries_Click" />
                                                        <asp:ConfirmButtonExtender runat="server" TargetControlID="btnDeleteManualEntries"
                                                            ConfirmText="Delete All Selected Additions and Deletions?" />
                                                    </td>
                                                    <td>
                                                        <b>WWD</b>
                                                    </td>
                                                    <td>
                                                        <b>Car Group</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Button runat="server" ID="btnSaveManualChanges" Text="Update All" Width="130px"
                                                            OnClick="btnSaveManualChanges_Click" />
                                                        <asp:ConfirmButtonExtender runat="server" TargetControlID="btnSaveManualChanges"
                                                            ConfirmText="Update all the below Additions and Deletions?" />
                                                    </td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hfAddDelId" Value='<%# DataBinder.Eval(Container.DataItem, "Identifier") %>' />
                                                    <asp:CheckBox runat="server" ID="cbDeleteRecord" Text="" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hfLocationWwdId" Value='<%# DataBinder.Eval(Container.DataItem, "LocationWwdId") %>' />
                                                    <asp:Label runat="server" ID="lblWwd" Text='<%# DataBinder.Eval(Container.DataItem, "LocationWwd") %>' />
                                                </td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hfCarGroupId" Value='<%# DataBinder.Eval(Container.DataItem, "CarGroupId") %>' />
                                                    <asp:Label runat="server" ID="lblCarGroup" Text='<%# DataBinder.Eval(Container.DataItem, "CarGroup") %>' />
                                                </td>
                                                <td>
                                                    <uc:SingleDatePicker ID="sdpRepDate" runat="server" width="80px"
                                                        SelectedDateTime='<%# DataBinder.Eval(Container.DataItem, "RepDate") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Text="Addition" ID="lblAddition"
                                                        Visible='<%# DataBinder.Eval(Container.DataItem, "Addition") %>' />
                                                    <asp:Label runat="server" Text="Deletion" ID="lblDeletion"
                                                        Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Addition")) == false %>' />
                                                    <asp:TextBox runat="server" ID="tbValue" Text='<%# DataBinder.Eval(Container.DataItem, "Value") %>'
                                                        Width="40px" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>


                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnParse" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
