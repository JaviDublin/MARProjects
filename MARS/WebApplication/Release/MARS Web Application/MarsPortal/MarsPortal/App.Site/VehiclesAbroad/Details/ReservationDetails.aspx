<%@ Page Title="Reservation Details" Theme="MarsV3"  Language="C#" MasterPageFile="~/App.Masterpages/Application.master"
    AutoEventWireup="true" CodeBehind="ReservationDetails.aspx.cs" Inherits="App.VehiclesAbroad.RerservationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>

                <table width="98%" cellpadding="10">
                    <tr>
                        <td colspan="4"style="padding:10px; font-size: large; font-weight: bold;">Vehicles Abroad - Reservation Details</td>
                    </tr>
                    <tr><td colspan="10"><hr /></td></tr>
                    <tr>
                        <td>Return Country : </td>
                        <td><asp:Label ID="labelRtrnCountry" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Start Country : </td>
                        <td><asp:Label ID="labelStartCountry" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Car Segment : </td>
                        <td><asp:Label ID="labelCarSegment" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Start Date : </td>
                        <td><asp:Label ID="labelStartDate" Font-Bold="true" ForeColor="Blue" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Return Pool : </td>
                        <td><asp:Label ID="labelRtrnPool" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Start Pool : </td>
                        <td><asp:Label ID="labelStartPool" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Car Class : </td>
                        <td><asp:Label ID="labelCarClass" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>End Date : </td>
                        <td><asp:Label ID="labelEndDate" Font-Bold="true" ForeColor="Blue" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Return Location Group : </td>
                        <td><asp:Label ID="labelRtrnLocationGrp" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Start Location Group : </td>
                        <td><asp:Label ID="labelStartLocGrp" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td>Car Group : </td>
                        <td><asp:Label ID="labelCarGroup" Font-Bold="true" ForeColor="Blue" runat="server">***All***</asp:Label></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <asp:GridView ID="GridViewReservationDetails" runat="server" SelectedIndex="0" EmptyDataText="No data available"
                    AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="ObjectDataSourceReservationDetails"
                    OnSelectedIndexChanged="GridViewReservationDetails_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="ResLocation" HeaderText="Location" SortExpression="ResLocation" />
                        <asp:BoundField DataField="ResGroup" HeaderText="Group" SortExpression="ResGroup" />
                        <asp:BoundField DataField="ResCheckoutDate" HeaderText="Checkout Date" SortExpression="ResCheckoutDate" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="ResCheckinLoc" HeaderText="Checkin Location" SortExpression="ResCheckinLoc" />
                        <asp:BoundField DataField="ResId" HeaderText="ResId" SortExpression="ResId" />
                        <asp:BoundField DataField="ResNoDaysUntilCheckout" HeaderText="No Days Until Checkout"
                            SortExpression="ResNoDaysUntilCheckout" />
                        <asp:BoundField DataField="ResNoDaysReserved" HeaderText="Number Days Reserved" SortExpression="ResNoDaysReserved" />
                        <asp:BoundField DataField="ResDriverName" HeaderText="Driver Name" SortExpression="ResDriverName" />
                    </Columns>
                    <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" LastPageImageUrl="~/App.Images/pager-last.png"
                        Mode="NextPreviousFirstLast" NextPageImageUrl="~/App.Images/pager-next.png" PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                    <PagerStyle HorizontalAlign="Right" />
                </asp:GridView>
                <i>You are viewing page&nbsp;
                    <%=GridViewReservationDetails.PageIndex + 1%>
                    &nbsp;of&nbsp;
                    <%=GridViewReservationDetails.PageCount%>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Maximum number of rows:&nbsp;
                    <asp:DropDownList ID="DropDownListPagerMaxRows" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DropDownListPagerMaxRows_SelectedIndexChanged">
                        <asp:ListItem Value="10">10 Rows</asp:ListItem>
                        <asp:ListItem Value="20">20 Rows</asp:ListItem>
                        <asp:ListItem Value="30">30 Rows</asp:ListItem>
                        <asp:ListItem Value="40">40 Rows</asp:ListItem>
                        <asp:ListItem Value="50">50 Rows</asp:ListItem>
                    </asp:DropDownList>
                </i>
                <asp:ObjectDataSource ID="ObjectDataSourceReservationDetails" runat="server" SelectMethod="getVehicleDetails"
                    SortParameterName="sortExpression" TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel"
                    OnSelecting="ObjectDataSourceReservationDetails_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownListDueCountry" Name="dueCountry" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:Parameter Name="vehiclePredicament" Type="Int32" />
                        <asp:ControlParameter ControlID="DropDownListOwnCountry" Name="ownCountry" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="DropDownListPool" Name="pool" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="DropDownListLocationGroup" Name="locationGroup" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="DropDownListCarSegment" Name="carSegment" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="DropDownListCarClass" Name="carClass" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="DropDownListCarGroup" Name="carGroup" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="TextBoxUnit" Name="unit" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="TextBoxLicense" Name="license" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="model" Type="String" />
                        <asp:Parameter Name="modelDescription" Type="String" DefaultValue="ResDetails" />
                        <asp:Parameter Name="vin" Type="String" />
                        <asp:Parameter Name="customerName" Type="String" />
                        <asp:Parameter Name="colour" Type="String" />
                        <asp:Parameter Name="mileage" Type="String" />
                        <asp:ControlParameter ControlID="DropDownListDestinationPool" ConvertEmptyStringToNull="False"
                            Name="destinationPool" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="DropDownListDestinationLocationGroup" ConvertEmptyStringToNull="False"
                            Name="destinationLocationGroup" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="TextBoxReservationStartDate" ConvertEmptyStringToNull="False"
                            Name="reservationStartdate" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="TextBoxReservationEndDate" ConvertEmptyStringToNull="False"
                            Name="reservationEnddate" PropertyName="Text" Type="String" />
                        <asp:Parameter Name="sortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            
                <%--Container table--%>
                <table width="98%" cellpadding="10">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Reservation Return Filters:</legend>
                                <table width="98%" cellpadding="10">
                                    <tr>
                                        <td>Country</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListDueCountry" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceDueCountry">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceDueCountry" runat="server" SelectMethod="getReservationReturnCountry"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Pool</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListDestinationPool" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceDestinationPool" OnDataBound="DropDownListDestinationPool_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceDestinationPool" runat="server" SelectMethod="getPoolList"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListDueCountry" Name="country" ConvertEmptyStringToNull="False"
                                                        PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Location Group</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListDestinationLocationGroup" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceDestinationLocationGroup" OnDataBound="DropDownListDestinationLocationGroup_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceDestinationLocationGroup" runat="server"
                                                SelectMethod="getLocationList" TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListDestinationPool" ConvertEmptyStringToNull="False"
                                                        Name="poolId" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="DropDownListDueCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>Reservation Start Filters:</legend>
                                <table width="98%" cellpadding="10">
                                    <tr>
                                        <td>Country</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListOwnCountry" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceOwnCountry">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceOwnCountry" runat="server" SelectMethod="getReservationLocationCountries"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel"></asp:ObjectDataSource>
                                        </td>
                                        <td>Car Segment</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarSegment" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceCarSegment" OnDataBound="DropDownListCarSegment_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceCarSegment" runat="server" SelectMethod="getCarSegment"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>Start Date</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxReservationStartDate" runat="server" AutoPostBack="true"
                                                OnTextChanged="TextBoxReservationStartDate_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButtonStartCalendar" runat="server" ImageUrl="~/App.Images/calendar.png"
                                                AlternateText="Calendar" ToolTip="Click to show calendar" />
                                            <asp:CalendarExtender ID="TextBoxReservationStartDate_CalendarExtender" PopupButtonID="ImageButtonStartCalendar"
                                                PopupPosition="Right" runat="server" Enabled="True" TargetControlID="TextBoxReservationStartDate" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="LabelReservsationStartDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Pool</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListPool" runat="server" DataSourceID="ObjectDataSourcePool"
                                                AutoPostBack="True" OnDataBound="DropDownListPool_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourcePool" runat="server" SelectMethod="getPoolList"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>Car Class</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarClass" runat="server" AutoPostBack="True" DataSourceID="ObjectDataSourceCarClass"
                                                OnDataBound="DropDownListCarClass_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceCarClass" runat="server" SelectMethod="getCarClass"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="DropDownListCarSegment" ConvertEmptyStringToNull="False"
                                                        Name="carSegment" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>End Date</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxReservationEndDate" runat="server" AutoPostBack="true" OnTextChanged="TextBoxReservationEndDate_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButtonEndDate" runat="server" ImageUrl="~/App.Images/calendar.png"
                                                AlternateText="Calendar" ToolTip="Click to show calendar" />
                                            <asp:CalendarExtender ID="TextBoxReservationEndDate_CalendarExtender" PopupButtonID="ImageButtonEndDate"
                                                runat="server" Enabled="True" TargetControlID="TextBoxReservationEndDate" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="LabelReservationEndDate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Location Group</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListLocationGroup" runat="server" AutoPostBack="True"
                                                DataSourceID="ObjectDataSourceLocationGroup" OnDataBound="DropDownListLocationGroup_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceLocationGroup" runat="server" SelectMethod="getLocationList"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListPool" ConvertEmptyStringToNull="False"
                                                        Name="poolId" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>Car Group</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarGroup" runat="server" AutoPostBack="True" DataSourceID="ObjectDataSourceCarGroup"
                                                OnDataBound="DropDownListCarGroup_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceCarGroup" runat="server" SelectMethod="getCarGroup"
                                                TypeName="App.BLL.VehiclesAbroad.ReservationDetailsModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" ConvertEmptyStringToNull="False"
                                                        Name="country" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="DropDownListCarSegment" ConvertEmptyStringToNull="False"
                                                        Name="carSegment" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="DropDownListCarClass" ConvertEmptyStringToNull="False"
                                                        Name="carClass" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <%--end of containing table --%>
                <br />
                <hr />
                <asp:Button ID="buttonSave" runat="server" Text="Download CSV" ToolTip="Click to download file"
                    OnClick="buttonSave_Click" ClientIDMode="Static" />
                <%-- filters are hidden, needed for the objectDataSource --%>
                <div id="carFilters" class="divReportSettingsWrapper" style="display: none">
                    <div style="text-align: left;">
                        Vehicle Filters:</div>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                Unit:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxUnit" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                License:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxLicense" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Model:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxModel" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Model Description:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxModelDescription" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                VIN:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxVin" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Customer Name:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxCustomerName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Colour:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxColour" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Mileage:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxMileage" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <asp:Button ID="ButtonFilterGrid" runat="server" Text="Filter Grid" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonClearFilters" runat="server" Text="Clear Filters" />
                </div>
            </center>
            <%-- Modal popup with vehicle details --%>
            <uc:VehiclesAbroadReservations ID="VehiclesAbroadReservations1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" SkinID="backgroundCover">
            </asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" SkinID="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp; Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" SkinID="loadDataImage" />
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        //prm.add_endRequest(EndRequest);
        var postBackElement;

        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack()) {

                args.set_cancel(true);
            }
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'buttonSave') {

                //alert("postBackElement.id " + postBackElement.id);
                // This stops the progress panel showing
                // Infact it can be set to anythning and it still works!!!!!!
                $get('upChartProgress').style.display = 'none';
            }
        }
    </script>
</asp:Content>
