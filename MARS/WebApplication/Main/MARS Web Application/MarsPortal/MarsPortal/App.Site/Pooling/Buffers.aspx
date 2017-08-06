<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Buffers.aspx.cs" Inherits="Mars.App.Site.Pooling.Buffers" %>

<%@ Register Src="~/App.UserControls/Helpicon.ascx" TagName="HelpIcon" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">



    <asp:UpdatePanel runat="server" ID="upnlManipulation">
        <ContentTemplate>
            <div style="float: right; margin-right: 25px;">
                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
            </div>
            <center>
                <table>
                    <tr>
                        <td>
                            <h1>Buffers</h1>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:DynamicParameters ID="DynamicParameters" runat="server"
                                IsNonRevReport="True" ShowNoDateSelector="True" />
                        </td>
                    </tr>
                </table>

                <asp:CollapsiblePanelExtender ID="cpeBody" TargetControlID="pnlBody" runat="server" />
                <asp:Panel ID="pnlBody" runat="server">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="width: 800px">
                                    <legend>Buffers - Upload for Country</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:FileUpload runat="server" ID="fuBuffers" Height="23px"
                                                                Width="365px" />
                                                        </td>
                                                        <td>
                                                            <uc:HelpIcon runat="server" HoverImage="~/App.Images/Pooling/PoolingBuffersUpload.jpg" />
                                                        </td>
                                                    </tr>
                                                </table>


                                            </td>
                                            <td style="width: 100%; text-align: right;">
                                                <asp:Button runat="server" ID="btnParse" Text="Upload" OnClick="btnParse_Click" Width="130px" />
                                                <asp:Button runat="server" ID="btnSubmit" Text="Confirm" OnClick="btnSubmit_Click" Width="130px" />
                                                <asp:ConfirmButtonExtender runat="server" TargetControlID="btnSubmit"
                                                    ConfirmText="Submiting the data from this file will wipe all previous buffers for the selected country.
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
                                    <legend>Buffers - Manual Entry</legend>
                                    <table style="text-align: center;">
                                        <tr>
                                            <td>WWD
                                            </td>
                                            <td>Car Group
                                            </td>
                                            <td>Amount
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbWwd" CssClass="QuickLocationGroupInput"
                                                    onkeydown="return (event.keyCode!=13);" Width="65px" />

                                                <asp:AutoCompleteExtender ID="acWwd" runat="server" ServiceMethod="GetBranchList"
                                                    TargetControlID="tbWwd" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                                                    CompletionInterval="500" EnableCaching="true" CompletionSetCount="8"
                                                    UseContextKey="True" />
                                            </td>
                                            <td>

                                                <asp:TextBox runat="server" ID="tbCarGroup" Width="41px" />
                                                <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                                                    TargetControlID="tbCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                                                    CompletionInterval="500" EnableCaching="true" CompletionSetCount="8"
                                                    UseContextKey="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbValue" Width="28px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                                    <div style="width: 100%; text-align: right">
                                        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" Width="130px" />
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="width: 800px">
                                    <legend>Buffers</legend>
                                    <br />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptBuffers">
                                        <HeaderTemplate>
                                            <table style="width: 590px; text-align: center;" class="GridViewStyle">
                                                <tr style="font-size: 14px;">
                                                    <td>
                                                        <asp:Button runat="server" ID="btnDeleteManualEntries" Width="130px"
                                                            Text="Delete Selected" OnClick="btnDeleteManualEntries_Click" />
                                                        <asp:ConfirmButtonExtender runat="server" TargetControlID="btnDeleteManualEntries"
                                                            ConfirmText="Delete All Selected Buffers?" />
                                                    </td>
                                                    <td>WWD
                                                    </td>
                                                    <td>Car Group
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Button runat="server" ID="btnSaveManualChanges" Text="Update All" Width="130px"
                                                            OnClick="btnSaveManualChanges_Click" />
                                                        <asp:ConfirmButtonExtender runat="server" TargetControlID="btnSaveManualChanges"
                                                            ConfirmText="Update all the below Buffers?" />
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
