<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualMovementGridView.ascx.cs"
    Inherits="App.UserControls.ManualMovementGridView" %>
<%@ Import Namespace="App.Entities" %>
<asp:UpdatePanel ID="upMovement" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="overflow: scroll; overflow-x: hidden; height: 265px;">
            <asp:GridView ID="grdManualMovement" AutoGenerateColumns="False" runat="server" AllowPaging="True"
                AllowSorting="True" ShowHeaderWhenEmpty="True" EmptyDataText="Please select a country"
                ShowFooter="True" CellPadding="0" BorderWidth="0px" HorizontalAlign="Center"
                GridLines="Horizontal" OnPageIndexChanging="grdManualMovement_PageIndexChanging"
                OnRowCommand="grdManualMovement_RowCommand" OnRowEditing="grdManualMovement_RowEditing"
                OnRowDataBound="grdManualMovement_RowDataBound" OnSorting="grdManualMovement_Sorting"
                CssClass="mgt-GridView" OnDataBound="grdManualMovement_DataBound" OnPreRender="grdManualMovement_PreRender"
                meta:resourcekey="grdManualMovementResource1">
                <Columns>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:ToggleGridCheckBoxes(this.checked);"
                                meta:resourcekey="chkSelectAllResource" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:HighlightRow(this);"
                                meta:resourcekey="chkDeleteResource1" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click"
                                CssClass="chartbuttonoptions" OnClientClick="return DeleteConfirmation();" meta:resourcekey="btnDeleteResource1" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandName="Edit" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                meta:resourcekey="lnkEditResource1"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="FleetPlanEntryID">
                        <HeaderTemplate>
                            FleetPlanEntryID</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFleetPlanEntryID" runat="server" Text='<%# Eval("FleetPlanEntryID") %>'
                                meta:resourcekey="lblFleetPlanEntryIDResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="FleetPlanDetailID">
                        <HeaderTemplate>
                            FleetPlanDetailID</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFleetPlanDetailID" runat="server" Text='<%# Eval("FleetPlanDetailID") %>'
                                meta:resourcekey="lblFleetPlanDetailIDResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="ScenarioID">
                        <HeaderTemplate>
                            ScenarioID</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblScenarioID" runat="server" Text='<%# Eval("ScenarioID") %>' meta:resourcekey="lblScenarioIDResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="LocationGroupID">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkLocationGroupID" runat="server" Text="Location Group" CommandArgument="LocationGroup.LocationGroupID"
                                CommandName="sort"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLocationGroupID" runat="server" Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupID %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="LocationGroup.LocationGroupName" meta:resourcekey="TemplateFieldResource6">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkLocationGroup" runat="server" Text="Location Group" CommandArgument="LocationGroup.LocationGroupName"
                                CommandName="sort"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLocationGroup" runat="server" Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupName %>'
                                meta:resourcekey="lblFromResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="CarGroup.CarGroupDescription" meta:resourcekey="TemplateFieldResource8">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkCarGroup" runat="server" Text="Car Class" CommandArgument="CarGroup.CarGroupDescription"
                                CommandName="sort" meta:resourcekey="lnkCarGroupResource1"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCarClass" runat="server" Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupDescription %>'
                                meta:resourcekey="lblCarClassResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="CarGroupID">
                        <HeaderTemplate>
                            Car Group ID</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCarGroupID" runat="server" Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupID %>'
                                meta:resourcekey="lblCarGroupIDResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="DateOfMovement" meta:resourcekey="TemplateFieldResource10">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkDateOfMovement" runat="server" Text="Date of Movement" CommandArgument="DateOfMovement"
                                CommandName="sort" meta:resourcekey="lnkDateOfMovementResource1"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMovementDate" runat="server" Text='<%# ((DateTime)Eval("DateOfMovement")).ToShortDateString() %>'
                                meta:resourcekey="lblMovementDateResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Addition">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkAddition" runat="server" Text="Additions" CommandArgument="Addition"
                                CommandName="sort"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAddition" runat="server" Text='<%# Eval("Addition").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Deletion">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkDeletion" runat="server" Text="Deletions" CommandArgument="Deletion"
                                CommandName="sort"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDeletion" runat="server" Text='<%# Eval("Deletion").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Amount" AccessibleHeaderText="Amount">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkAmount" runat="server" Text="Count" CommandArgument="Amount"
                                CommandName="sort" meta:resourcekey="lnkMovementCountResource1"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Amount").ToString() %>' meta:resourcekey="lblCountResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle BackColor="LightGray" Height="25px" VerticalAlign="Top" HorizontalAlign="Center"
                    CssClass="Dummy" />
                <PagerTemplate>
                    <div class="Dummy">
                        <asp:DropDownList ID="ddlPageSelect" runat="server" OnSelectedIndexChanged="ddlPageSelect_SelectedIndexChanged"
                            AutoPostBack="True" meta:resourcekey="ddlPageSelectResource1">
                        </asp:DropDownList>
                        [Page
                        <%=grdManualMovement.PageIndex + 1%>
                        of
                        <%= grdManualMovement.PageCount%>] &nbsp; &nbsp;
                        <asp:Image ID="imgsep1" runat="server" ImageUrl="~/App.Images/sort-blank.gif" meta:resourcekey="imgsep1Resource1" />
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="imgButtonFirst" runat="server" CommandArgument="First" CommandName="Page"
                            ImageUrl="~/App.Images/pager-first.png" ImageAlign="Bottom" meta:resourcekey="imgButtonFirstResource1" />
                        <asp:ImageButton ID="imgButtonPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                            ImageUrl="~/App.Images/pager-previous.png" ImageAlign="Bottom" meta:resourcekey="imgButtonPreviousResource1" />
                        [Records
                        <%= grdManualMovement.PageIndex * grdManualMovement.PageSize + 1%>
                        -
                        <%= grdManualMovement.PageIndex * grdManualMovement.PageSize + grdManualMovement.Rows.Count%>]
                        <asp:ImageButton ID="imgButtonNext" runat="server" CommandArgument="Next" CommandName="Page"
                            ImageUrl="~/App.Images/pager-next.png" ImageAlign="Bottom" meta:resourcekey="imgButtonNextResource1" />
                        <asp:ImageButton ID="imgButtonLast" runat="server" CommandArgument="Last" CommandName="Page"
                            ImageUrl="~/App.Images/pager-last.png" ImageAlign="Bottom" meta:resourcekey="imgButtonLastResource1" />
                        &nbsp; &nbsp;
                        <asp:Image ID="imgsep2" runat="server" ImageUrl="~/App.Images/sort-blank.gif" meta:resourcekey="imgsep2Resource1" />
                        &nbsp; &nbsp;
                        <asp:Label ID="lblitemsPerPage" runat="server" Text="Items per Page:" meta:resourcekey="lblitemsPerPageResource1"></asp:Label>
                        <asp:DropDownList ID="ddlItemsPerPageSelector" runat="server" OnSelectedIndexChanged="ddlItemsPerPageSelector_SelectedIndexChanged"
                            AutoPostBack="True" meta:resourcekey="ddlItemsPerPageSelectorResource1">
                            <asp:ListItem Text="3" Value="3" meta:resourcekey="ListItemResource1"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5" meta:resourcekey="ListItemResource2"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10" Selected="True" meta:resourcekey="ListItemResource3"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15" meta:resourcekey="ListItemResource4"></asp:ListItem>
                        </asp:DropDownList>
                </PagerTemplate>
            </asp:GridView>
        </div>
        <script type="text/javascript" language="javascript">

            function DeleteConfirmation() {
                if (confirm("Permanently delete selected Movements?") == true)
                    return true;
                else
                    return false;
            }

            function ClearHiddenFields() {
                $('.fpd-wrap input[type=hidden]').val('');
                $(".ddlTarget").removeAttr('disabled');
                $(".ddlAddDelCarClass").removeAttr('disabled');
                $(".txtAddDelDate").removeAttr('disabled');

            }

            function ToggleGridCheckBoxes(checked) {
                $('.mgt-GridView tr').find('input:checkbox').attr('checked', checked);

                if (checked) {
                    $('.mgt-GridView >tbody > tr').not(':last').each
            (
                function () {
                    $(this).css('background-color', 'white');
                    $(this).css('color', '#5CACEE');
                }
            );
                    $('.Dummy').css('background-color', 'LightGray');
                    $('.Dummy').css('color', 'black');
                }
                else {
                    $('.mgt-GridView :gt(0)tr').each
            (
                function () {
                    $(this).css('background-color', 'white');
                    $(this).css('color', 'black');
                }
            );
                }
            }

            function HighlightRow(chkB) {
                var IsChecked = chkB.checked;
                if (IsChecked) {
                    chkB.parentElement.parentElement.style.backgroundColor = 'white';
                    chkB.parentElement.parentElement.style.color = '#5CACEE';
                }
                else {
                    chkB.parentElement.parentElement.style.backgroundColor = 'white';
                    chkB.parentElement.parentElement.style.color = 'black';
                }
            }        
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
<div>
    <asp:Button ID="btnNew" runat="server" Text="New Movement" CssClass="chartbuttonoptions"
        meta:resourcekey="btnNewResource1" />
    &nbsp;
    <asp:Button ID="btnNewAddDel" runat="server" Text="New Addition\Deletion" CssClass="chartbuttonoptions"
        meta:resourcekey="btnNewAddDelResource1" />
</div>
<br />
<asp:Panel ID="pnlNewMovement" runat="server" CssClass="modalPopup" meta:resourcekey="pnlNewMovementResource1">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblLocationGroupFrom" runat="server" Text="From:" meta:resourcekey="lblLocationGroupFromResource1"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLocationGroupTo" runat="server" Text="To:" meta:resourcekey="lblLocationGroupToResource1"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblCarclass" runat="server" Text="CarClass:" meta:resourcekey="lblCarclassResource2"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMovementAmount" runat="server" Text="Amount:" meta:resourcekey="lblMovementAmountResource1"></asp:Label>
            </td>
            <td>
                <asp:Label ID="dtMovementDate" runat="server" Text="MovementDate:" meta:resourcekey="dtMovementDateResource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlLocationGroupFrom" runat="server" meta:resourcekey="ddlLocationGroupFromResource1">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlLocationGroupTo" runat="server" meta:resourcekey="ddlLocationGroupToResource1">
                </asp:DropDownList>
                <asp:CompareValidator ID="compvMovementFrom" runat="server" ValidationGroup="Movement"
                    ControlToValidate="ddlLocationGroupTo" ControlToCompare="ddlLocationGroupFrom"
                    ErrorMessage="Matching From and To Locations not valid" Text="*" Operator="NotEqual"
                    meta:resourcekey="compvMovementFromResource1" />
            </td>
            <td>
                <asp:DropDownList ID="ddlCarClass" runat="server" meta:resourcekey="ddlCarClassResource1">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtAmount" runat="server" Width="60px" meta:resourcekey="txtAmountResource1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rvMovementAmt" ControlToValidate="txtAmount" ErrorMessage="Count Required"
                    Text="*" ValidationGroup="Movement" runat="server" meta:resourcekey="rvMovementAmtResource1" />
                <asp:CompareValidator ID="compvMovementAmt" runat="server" Type="Integer" Text="*"
                    Operator="GreaterThanEqual" ValueToCompare="1" ValidationGroup="Movement" ControlToValidate="txtAmount"
                    ErrorMessage="count must be greater than 0" meta:resourcekey="compvMovementAmtResource1" />
            </td>
            <td>
                <asp:TextBox ID="txtMovementDate" runat="server" Width="90px" meta:resourcekey="txtMovementDateResource1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfMovementDate" ControlToValidate="txtMovementDate"
                    ErrorMessage="Action Date Required" Text="*" ValidationGroup="Movement" runat="server"
                    meta:resourcekey="rfMovementDateResource1" />
                <asp:CompareValidator ID="cvMovementDate" runat="server" ControlToValidate="txtMovementDate"
                    ErrorMessage="Invalid Action Date Format" Text="*" Operator="DataTypeCheck" Type="Date"
                    ValidationGroup="Movement" meta:resourcekey="cvMovementDateResource1" />
                <asp:RangeValidator ID="rvMovementDate" runat="server" Type="Date" ControlToValidate="txtMovementDate"
                    ErrorMessage="Future Dates only" Text="*" ValidationGroup="Movement"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:ValidationSummary ID="vsMovement" runat="server" HeaderText="You must enter valid data in the following fields:"
                    ValidationGroup="Movement" meta:resourcekey="vsMovementResource1" />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnAccept" runat="server" Text="Save" OnClick="btnAccept_Click" ValidationGroup="Movement"
        CssClass="chartbuttonoptions" meta:resourcekey="btnAcceptResource1" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
        OnClientClick="return ClearHiddenFields();" CssClass="chartbuttonoptions" meta:resourcekey="btnCancelResource1" />
    <asp:CalendarExtender ID="ceMovementDate" runat="server" TargetControlID="txtMovementDate"
        Format="dd/MM/yyyy" Enabled="True">
    </asp:CalendarExtender>
</asp:Panel>
<asp:Panel ID="pnlAddDel" runat="server" CssClass="modalPopup" meta:resourcekey="pnlAddDelResource1">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblTarget" runat="server" Text="Target:" meta:resourcekey="lblTargetResource1"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAddDelCarClass" runat="server" Text="CarClass:" meta:resourcekey="lblAddDelCarClassResource1"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAddition" runat="server" Text="Addition:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblDeletion" runat="server" Text="Deletion:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAddDelDate" runat="server" Text="Action Date:" meta:resourcekey="lblAddDelDateResource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTarget" CssClass="ddlTarget" runat="server" meta:resourcekey="ddlTargetResource1">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlAddDelCarClass" CssClass="ddlAddDelCarClass" runat="server"
                    meta:resourcekey="ddlAddDelCarClassResource1">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtAddition" runat="server" Width="60px" CssClass="txtAddition"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddition" ControlToValidate="txtAddition" ErrorMessage="Amount Required"
                    Text="*" ValidationGroup="AddDel" runat="server" />
                <asp:CompareValidator ID="cvAddition" runat="server" ValidationGroup="AddDel" ControlToValidate="txtAddition"
                    ErrorMessage="Numerical value required" Text="*" Operator="DataTypeCheck" Type="Integer" />
                <asp:RangeValidator ID="rvAddition" runat="server" ErrorMessage="Positive values only"
                    ControlToValidate="txtAddition" MinimumValue="0" Type="Integer" Text="*" ForeColor="Red"></asp:RangeValidator>
            </td>
            <td>
                <asp:TextBox ID="txtDeletion" runat="server" Width="60px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfDeletion" ControlToValidate="txtDeletion" ErrorMessage="Amount Required"
                    Text="*" ValidationGroup="AddDel" runat="server" />
                <asp:CompareValidator ID="cvDeletion" runat="server" ValidationGroup="AddDel" ControlToValidate="txtDeletion"
                    ErrorMessage="Numerical value required" Text="*" Operator="DataTypeCheck" Type="Integer" />
                <asp:RangeValidator ID="rvDeletion" runat="server" ErrorMessage="Positive values only"
                    ControlToValidate="txtDeletion" MinimumValue="0" Type="Integer" Text="*" ForeColor="Red"></asp:RangeValidator>
            </td>
            <td>
                <asp:TextBox ID="txtAddDelDate" runat="server" Width="90px" meta:resourcekey="txtAddDelDateResource1"
                    CssClass="txtAddDelDate"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddDelDate" ControlToValidate="txtAddDelDate"
                    ErrorMessage="Action Date Required" Text="*" ValidationGroup="AddDel" runat="server"
                    meta:resourcekey="rfvAddDelDateResource1" />
                <asp:CompareValidator ID="cvAddDelDate" runat="server" ValidationGroup="AddDel" ControlToValidate="txtAddDelDate"
                    ErrorMessage="Invalid Action Date Format" Text="*" Operator="DataTypeCheck" Type="Date"
                    meta:resourcekey="cvAddDelDateResource1" />
                <asp:RangeValidator ID="rvAddDelDate" runat="server" Type="Date" ControlToValidate="txtAddDelDate"
                    ErrorMessage="Future Dates only" Text="*" ValidationGroup="AddDel"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:ValidationSummary ID="vsAddDel" runat="server" ValidationGroup="AddDel" HeaderText="You must enter valid data in the following fields:"
                    meta:resourcekey="vsAddDelResource1" />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnActionAddDel" runat="server" Text="Save" OnClick="btnActionAddDel_Click"
        ValidationGroup="AddDel" CssClass="chartbuttonoptions" meta:resourcekey="btnActionAddDelResource1" />
    <asp:Button ID="btnCancelAddDel" runat="server" Text="Cancel" CausesValidation="False"
        OnClientClick="return ClearHiddenFields();" CssClass="chartbuttonoptions" meta:resourcekey="btnCancelAddDelResource1" />
    <asp:CalendarExtender ID="ceAddDelDate" runat="server" TargetControlID="txtAddDelDate"
        Format="dd/MM/yyyy" Enabled="True" />
</asp:Panel>
<asp:ModalPopupExtender ID="ajaxNewMovementPopup" runat="server" PopupControlID="pnlNewMovement" BackgroundCssClass="modalBackground"
    TargetControlID="btnNew" DropShadow="True" CancelControlID="btnCancel" DynamicServicePath=""
    Enabled="True" OnCancelScript="ToggleGridCheckBoxes(false)">
</asp:ModalPopupExtender>
<asp:ModalPopupExtender ID="ajaxAddDelPopup" runat="server" PopupControlID="pnlAddDel" BackgroundCssClass="modalBackground"
    TargetControlID="btnNewAddDel" DropShadow="True" CancelControlID="btnCancelAddDel"
    DynamicServicePath="" Enabled="True" OnCancelScript="ToggleGridCheckBoxes(false)">
</asp:ModalPopupExtender>
<asp:HiddenField ID="hdnFleetPlanID" runat="server" />
<div class="fpd-wrap">
    <asp:HiddenField ID="hdnFleetPlanDetailID" runat="server" />
</div>
