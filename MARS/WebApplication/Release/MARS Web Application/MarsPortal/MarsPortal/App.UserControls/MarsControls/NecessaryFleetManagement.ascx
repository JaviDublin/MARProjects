<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NecessaryFleetManagement.ascx.cs"
    Inherits="App.UserControls.NecessaryFleetManagement" %>

<%@ Import Namespace="App.Entities" %>
<asp:UpdatePanel ID="updFrozenZone" runat="server">
    <ContentTemplate>
        <center>
            <fieldset>
                <legend>Please select criteria:</legend>
                <div>
                    <asp:UpdatePanel ID="updDynamicParameters" runat="server">
                        <ContentTemplate>
                            <div style="width: 520px; float: left; height: 30px; vertical-align: bottom;">
                                <uc:DynamicParameters ID="DynamicParams" runat="server" FlatTable="true" 
                                    ShowOpsSelector="False"  ShowQuickLocationGroupBox="false" ShowNoDateSelector="True"
                                    HideDatePicker="True" HideGroupingText="True" />
                            </div>
                            <div style="width: 355px; float: right; height: 30px; vertical-align: bottom;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUtilisation" runat="server" Text="Util.:" meta:resourcekey="lblUtilisationResource2"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUtilisation" runat="server" ToolTip="Leave this field blank to remain unmodified"
                                                meta:resourcekey="txtUtilisationResource2"></asp:TextBox>
                                            <asp:CompareValidator ID="cvUtilisation" runat="server" ControlToValidate="txtUtilisation"
                                                ErrorMessage="Invalid Utilisation Value" Text="*" Operator="DataTypeCheck" Type="Double"
                                                ValidationGroup="NecFleet" ForeColor="Red" meta:resourcekey="cvUtilisationResource2"
                                                Enabled="False" />
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Utilisation Range must be within  0 - 100"
                                                ControlToValidate="txtUtilisation" Type="Double" MinimumValue="0" MaximumValue="100"
                                                ValidationGroup="NecFleet" ForeColor="Red" Text="*" Enabled="False"></asp:RangeValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNonRev" runat="server" Text="Non Rev:" meta:resourcekey="lblNonRevResource2"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNonRev" runat="server" ToolTip="Leave this field blank to remain unmodified"
                                                meta:resourcekey="txtNonRevResource2"></asp:TextBox>
                                            <asp:CompareValidator ID="cvNonRev" runat="server" ControlToValidate="txtNonRev"
                                                ErrorMessage="Invalid Non Rev Value" Text="*" Operator="DataTypeCheck" Type="Double"
                                                ValidationGroup="NecFleet" ForeColor="Red" meta:resourcekey="cvNonRevResource2" />
                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Non Rev Range must be within  0 - 100"
                                                ControlToValidate="txtNonRev" Type="Double" MinimumValue="0" MaximumValue="100"
                                                ValidationGroup="NecFleet" ForeColor="Red" Text="*"></asp:RangeValidator>
                                        </td>
               
                                        <td>
                                            <asp:Button ID="btnMultiUpdate" runat="server" Text="Update" ValidationGroup="NecFleet"
                                                OnClick="btnMultiUpdate_Click" CssClass="chartbuttonoptionsNarrow" meta:resourcekey="btnMultiUpdateResource2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 100%; float: left;">
                                <asp:ValidationSummary ID="vsNecessaryFleet" runat="server" ValidationGroup="NecFleet"
                                    ForeColor="Red" HeaderText="You must enter valid data in the following fields:"
                                    meta:resourcekey="vsNecessaryFleetResource2" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </div>
                <div>
                    <div style="overflow: scroll; overflow-x: hidden; height: 300px; width: 100%; float: left;">
                        <asp:GridView ID="grdNecessaryFleet" AutoGenerateColumns="False" runat="server" AllowPaging="True"
                            AllowSorting="True" ShowHeaderWhenEmpty="True" EmptyDataText="Please select a country"
                            ShowFooter="True" CellPadding="2" CellSpacing="2" HorizontalAlign="Center" GridLines="Horizontal"
                            OnPreRender="grdNecessaryFleet_PreRender" CssClass="mgt-GridView" OnPageIndexChanging="grdNecessaryFleet_PageIndexChanging"
                            OnSorting="grdNecessaryFleet_Sorting" OnRowDataBound="grdNecessaryFleet_RowDataBound"
                            OnDataBound="grdNecessaryFleet_DataBound" OnRowCommand="grdNecessaryFleet_RowCommand"
                            OnRowEditing="grdNecessaryFleet_RowEditing" meta:resourcekey="grdNecessaryFleetResource2">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="CountryID">
                                    <HeaderTemplate>
                                        CountryID</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountryID" runat="server" Text='<%# ((Country)Eval("Country")).CountryID %>'
                                            meta:resourcekey="lblCountryIDResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource11">
                                    <HeaderTemplate>
                                        Country Description</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountryDescription" runat="server" Text='<%# ((Country)Eval("Country")).CountryDescription %>'
                                            meta:resourcekey="lblCountryDescriptionResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="LocationGroupID">
                                    <HeaderTemplate>
                                        LocationGroupID</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationGroupID" runat="server" Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupID %>'
                                            meta:resourcekey="lblLocationGroupIDResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="LocationGroup.LocationGroupName" meta:resourcekey="TemplateFieldResource13">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkLocationGroupfrom" runat="server" Text="Location Group" CommandArgument="LocationGroup.LocationGroupName"
                                            CommandName="sort" meta:resourcekey="lnkLocationGroupfromResource2"></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationGroupName" runat="server" Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupName %>'
                                            meta:resourcekey="lblLocationGroupNameResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="CarGroupID">
                                    <HeaderTemplate>
                                        CarClassID</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCarClassID" runat="server" Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupID %>'
                                            meta:resourcekey="lblCarClassIDResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="CarGroup.CarGroupDescription" meta:resourcekey="TemplateFieldResource15">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkCarClass" runat="server" Text="Car Class" CommandArgument="CarGroup.CarGroupDescription"
                                            CommandName="sort" meta:resourcekey="lnkCarClassResource2"></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCarclassDescription" runat="server" Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupDescription %>'
                                            meta:resourcekey="lblCarclassDescriptionResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource16" HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUtilization" runat="server" Text='<%# Eval("Utilization") %>' meta:resourcekey="lblUtilizationResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource17" HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNonRevFleet" runat="server" Text='<%# Eval("NonRevFleet") %>' meta:resourcekey="lblNonRevFleetResource2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource18">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandName="Edit" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                            meta:resourcekey="lnkEditResource2"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="LightGray" Height="25px" VerticalAlign="Top" HorizontalAlign="Center" />
                            <PagerTemplate>
                                <div>
                                    <asp:DropDownList ID="ddlPageSelect" runat="server" OnSelectedIndexChanged="ddlPageSelect_SelectedIndexChanged"
                                        AutoPostBack="True" meta:resourcekey="ddlPageSelectResource2">
                                    </asp:DropDownList>
                                    [Page
                                    <%=grdNecessaryFleet.PageIndex + 1%>
                                    of
                                    <%= grdNecessaryFleet.PageCount%>] &nbsp; &nbsp;
                                    <asp:Image ID="imgsep1" runat="server" ImageUrl="~/App.Images/sort-blank.gif" meta:resourcekey="imgsep1Resource2" />
                                    &nbsp; &nbsp;
                                    <asp:ImageButton ID="imgButtonFirst" runat="server" CommandArgument="First" CommandName="Page"
                                        ImageUrl="~/App.Images/pager-first.png" ImageAlign="Bottom" meta:resourcekey="imgButtonFirstResource2" />
                                    <asp:ImageButton ID="imgButtonPrevious" runat="server" CommandArgument="Prev" CommandName="Page"
                                        ImageUrl="~/App.Images/pager-previous.png" ImageAlign="Bottom" meta:resourcekey="imgButtonPreviousResource2" />
                                    [Records
                                    <%= grdNecessaryFleet.PageIndex * grdNecessaryFleet.PageSize + 1%>
                                    -
                                    <%= grdNecessaryFleet.PageIndex * grdNecessaryFleet.PageSize + grdNecessaryFleet.Rows.Count%>]
                                    <asp:ImageButton ID="imgButtonNext" runat="server" CommandArgument="Next" CommandName="Page"
                                        ImageUrl="~/App.Images/pager-next.png" ImageAlign="Bottom" meta:resourcekey="imgButtonNextResource2" />
                                    <asp:ImageButton ID="imgButtonLast" runat="server" CommandArgument="Last" CommandName="Page"
                                        ImageUrl="~/App.Images/pager-last.png" ImageAlign="Bottom" meta:resourcekey="imgButtonLastResource2" />
                                    &nbsp; &nbsp;
                                    <asp:Image ID="imgsep2" runat="server" ImageUrl="~/App.Images/sort-blank.gif" meta:resourcekey="imgsep2Resource2" />
                                    &nbsp; &nbsp;
                                    <asp:Label ID="lblitemsPerPage" runat="server" Text="Items per Page:" meta:resourcekey="lblitemsPerPageResource2"></asp:Label>
                                    <asp:DropDownList ID="ddlItemsPerPageSelector" runat="server" OnSelectedIndexChanged="ddlItemsPerPageSelector_SelectedIndexChanged"
                                        AutoPostBack="True" meta:resourcekey="ddlItemsPerPageSelectorResource2">
                                        <asp:ListItem Text="3" Value="3" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10" Selected="True" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                    </asp:DropDownList>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="DataExportButton"
                            OnClick="btnExport_Click" />
                        <asp:Button ID="btnDummy" runat="server" CssClass="hidden" meta:resourcekey="btnDummyResource2" />
                    </div>
            </fieldset>
            <br />
            <asp:Panel ID="pnlNecFleetReset" runat="server" GroupingText="Reset Country Utilisation\ Non-Rev"
                meta:resourcekey="pnlNecFleetResetResource2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSelectCountryReset" runat="server" Text="Select Country:  " meta:resourcekey="lblSelectCountryResetResource2"></asp:Label>
                            <asp:DropDownList ID="ddlCountryListReset" runat="server" meta:resourcekey="ddlCountryListResetResource2">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSelectResetType" runat="server" Text="Reset criteria:  " meta:resourcekey="lblSelectResetTypeResource2"></asp:Label>
                            <asp:DropDownList ID="ddlResetType" runat="server" meta:resourcekey="ddlResetTypeResource2">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc:DateRangePicker ID="drpNecFleetReset" runat="server" HistoricalDateRange="true"
                                PopupPositionTop="true" />
                        </td>
                        <td>
                            <asp:Button ID="btnReset" runat="server" Text="Reset Country" ValidationGroup="Dates"
                                CssClass="chartbuttonoptions" OnClick="btnReset_Click" OnClientClick="return ResetConfirmation();"
                                meta:resourcekey="btnResetResource2" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </center>
        <asp:Panel ID="pnlEditNecessaryFleet" runat="server" CssClass="modalPopup" meta:resourcekey="pnlEditNecessaryFleetResource2">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" meta:resourcekey="lblCountryResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLocationGroup" runat="server" Text="Location Group:" meta:resourcekey="lblLocationGroupResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCarClass" runat="server" Text="Car Class:" meta:resourcekey="lblCarClassResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblUtilisationEdit" runat="server" Text="Utilization:" meta:resourcekey="lblUtilisationEditResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNonRevEdit" runat="server" Text="Non Rev:" meta:resourcekey="lblNonRevEditResource2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCountry" runat="server" Enabled="False" meta:resourcekey="txtCountryResource2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocationGroup" runat="server" Enabled="False" meta:resourcekey="txtLocationGroupResource2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarClass" runat="server" Enabled="False" meta:resourcekey="txtCarClassResource2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUtilisationEdit" runat="server" meta:resourcekey="txtUtilisationEditResource2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNonRevEdit" runat="server" meta:resourcekey="txtNonRevEditResource2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSaveNecessaryFleetUpdate" runat="server" Text="Save" CssClass="chartbuttonoptions"
                OnClick="btnSaveNecessaryFleetUpdate_Click" meta:resourcekey="btnSaveNecessaryFleetUpdateResource2" />
            <asp:Button ID="btnCancelNecessaryFleetUpdate" runat="server" Text="Cancel" CssClass="chartbuttonoptions"
                meta:resourcekey="btnCancelNecessaryFleetUpdateResource2" />
            <br />
            <asp:Label ID="lblNecFleetMessage" runat="server" ForeColor="Red" meta:resourcekey="lblNecFleetMessageResource2"></asp:Label>
            <asp:ModalPopupExtender ID="ajaxNecessaryFleetPopup" runat="server" PopupControlID="pnlEditNecessaryFleet"
                TargetControlID="btnDummy" DropShadow="True" CancelControlID="btnCancelNecessaryFleetUpdate"
                BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True">
            </asp:ModalPopupExtender>
            <asp:HiddenField ID="hdnNecFleetCountryID" runat="server" />
            <asp:HiddenField ID="hdnNecFleetLocationGroupID" runat="server" />
            <asp:HiddenField ID="hdnNecFleetCarClassID" runat="server" />
        </asp:Panel>
        <script type="text/javascript" language="javascript">
            function ResetConfirmation() {
                if (confirm("Are you sure you want to reset \n the values for the selected dates?") == true)
                    return true;
                else
                    return false;
            }
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
