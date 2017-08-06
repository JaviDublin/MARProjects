<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverviewVehicle.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.OverviewVehicle" %>

<%@ Register Src="~/App.UserControls/Phase4/NonRev/ReasonEntryForm.ascx" TagName="ReasonEntry" TagPrefix="uc" %>


<table class="table-form-vehicledetails" style="width: 100%" >
    <tr>
        <td style="vertical-align: top;">
            <table>
                <tr>
                    <td>
                        <table style="vertical-align: top; width: 100%;" class="insetBorders">
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Group Rented</td>
                                <td class="table-cellHeader">License Plate</td>
                                <td class="table-cellHeader">Model Description</td>
                                <td class="table-cellHeader">Unit</td>
                                <td class="table-cellHeader">VIN Number</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblGroup" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblLiscencePlate" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblModelDescription" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblUnitNumber" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblVin" runat="server" />
                                </td>
                            </tr>
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Group Charged</td>
                                <td class="table-cellHeader">Owning Country</td>
                                <td class="table-cellHeader">Owning Area</td>
                                <td class="table-cellHeader">Location Country</td>
                                <td class="table-cellHeader">Days in Country</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblCarGroupCharged" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblOwningCountry" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblOwningArea" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblLocationCountry" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblDaysInCountry" runat="server" />
                                </td>
                            </tr>
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Check Out Location</td>
                                <td class="table-cellHeader">Check Out Date</td>
                                <td class="table-cellHeader">Operational Status</td>
                                <td class="table-cellHeader">Last Document</td>
                                <td class="table-cellHeader">Days in Non Rev</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblLastLocation" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblLastChangeDateTime" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblOperationalStatus" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblLastDocumentNumber" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblNonRevDays" runat="server" />
                                </td>
                            </tr>
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Check In Location</td>
                                <td class="table-cellHeader">Check In Date</td>
                                <td class="table-cellHeader">Movement Type</td>
                                <td class="table-cellHeader">Customer</td>
                                <td class="table-cellHeader">Last Mileage</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblExpectedLocation" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblExpectedDateTime" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblMovementType" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblLastDriverName" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblKilometers" runat="server" />
                                </td>
                            </tr>
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Model</td>
                                <td class="table-cellHeader">Previous Location</td>
                                <td class="table-cellHeader">Days in MM</td>
                                <td class="table-cellHeader">Days in BD</td>
                                <td class="table-cellHeader">Block Mileage</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblTasModelCode" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblPreviousLocationCode" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblDaysInMm" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblDaysInBd" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblBlockMilage" runat="server" />
                                </td>
                            </tr>
                            <tr class="table-rowHeader">
                                <td class="table-cellHeader">Depreciation Status</td>
                                <td class="table-cellHeader">Hold Flag</td>
                                <td class="table-cellHeader">Installation Date</td>
                                <td class="table-cellHeader">MSO Date</td>
                                <td class="table-cellHeader">Block Date</td>
                            </tr>
                            <tr class="table-rowData">
                                <td class="table-cellData">
                                    <asp:Label ID="lblDepreciationStatus" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblHoldFlag" runat="server" />
                                </td>
                                
                                <td class="table-cellData">
                                    <asp:Label ID="lblInstallationDate" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblInstallationMsoDate" runat="server" />
                                </td>
                                <td class="table-cellData">
                                    <asp:Label ID="lblBlockDate" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc:ReasonEntry runat="server" ID="ucReasonEntry" ShowAddButton="True" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align: top;" >
            <asp:Panel runat="server" ID="pnlHistory" Width="260px" CssClass="StandardBorder">
            <table>
                <tr>
                    <td class="table-rowHeader">Current Period Entries:
                    </td>
                </tr>
                <tr>
                    <td style="font-family: Courier New;">
                        <asp:ListBox runat="server" ID="lbCurrentPeriodEntries" CssClass="insetBorders" Height="120px" Width="240px"
                            AutoPostBack="True" OnSelectedIndexChanged="lbCurrentPeriodEntries_SelectionChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="table-rowHeader">Remarks for Selected Entry:
                    </td>
                </tr>
                <tr>
                    <td class="StandardBorder" style="background-color: white;">
                        <table style="width: 220px;">
                            <tr style="font-weight: bold; font-size: 12px;">
                                <td>User
                                </td>
                                <td>Entered   
                                </td>
                                <td>Est Resolution
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                        <div class="ReasonRepeaterHolder" style="width: 240px; height: 200px; font-size: 10px; overflow: auto;">
                            <div id="divReasonHover" style="display: none; position: absolute; float: right; background-color: white;" class="StandardBorder">
                                <table>
                                    <tr>
                                        <td style="font-weight: bold; text-align: center; font-size: 12px; width: 130px;">
                                            <label id="lblReason"></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <label id="lblRemark"></label>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <asp:Repeater ID="rptRemarks" runat="server">
                                <HeaderTemplate>
                                    <table style="width: 220px;">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "UserId") %>      
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Timestamp", "{0:dd/MM/yy HH:mm}") %>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "ExpectedResolutionDate", "{0:dd/MM/yy}") %>
                                        </td>
                                        <td>
                                            <img src="../../../App.Images/Icons/view-more.png"
                                                itemprop="<%# DataBinder.Eval(Container.DataItem, "Remark") %>"
                                                longdesc="<%# DataBinder.Eval(Container.DataItem, "ReasonText") %>"
                                                class="MoreRemarkDetails" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <%# DataBinder.Eval(Container.DataItem, "ReasonText")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <%# Substring(DataBinder.Eval(Container.DataItem, "Remark")) %>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="3">
                                            <hr />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                        </div>
                    </td>
                </tr>

            </table>
            </asp:Panel>
            <asp:Panel ID="pnlComment" runat="server" Visible="False" Width="260px" Height="100%" CssClass="StandardBorder">
                <table>
                    <tr>
                        <td class="table-cellHeader" style="font-weight: bold;">
                            Vehicle Comment:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="tbVehicleComment" TextMode="MultiLine" Height="194px" Width="240px"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnSubmitComment" CssClass="StandardButton" Text="Save" 
                                OnClick="btnSubmitComment_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>

</table>


