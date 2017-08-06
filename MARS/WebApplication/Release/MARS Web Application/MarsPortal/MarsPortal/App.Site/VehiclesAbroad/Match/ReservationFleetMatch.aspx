<%@ Page Title="Reservation Fleet Match" Theme="MarsV3" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="ReservationFleetMatch.aspx.cs" Inherits="MarsV2.VehiclesAbroad.ReservationFleetMatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                 <table cellspacing="10">
                    <tr>
                        <td><h2>Vehicles Abroad - Reservation/Fleet Match</h2></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Reservations</h3>
                        </td>
                        <td>
                            <h3>Matching Fleet</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridViewReservation" runat="server" SkinID="choseTable" 
                                AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                DataSourceID="ObjectDataSourceReservation" AllowPaging="True" 
                                SelectedIndex="0" EmptyDataText="No data available" DataKeyNames="ResId" 
                                AllowSorting="True" onrowdatabound="GridViewReservation_RowDataBound" >
                                <Columns>  
                                    <asp:BoundField DataField="ResNoDaysUntilCheckout" HeaderText="No Days Until Checkout" 
                                        SortExpression="ResNoDaysUntilCheckout" />          
                                    <asp:BoundField DataField="ResLocation" HeaderText="Location" 
                                        SortExpression="ResLocation" />
                                    <asp:BoundField DataField="ResGroup" HeaderText="Group" 
                                        SortExpression="ResGroup" />
                                    <asp:BoundField DataField="ResCheckoutDate" HeaderText="Checkout Date" DataFormatString="{0:d}"
                                        SortExpression="ResCheckoutDate" />
                                    <asp:BoundField DataField="ResCheckinLoc" HeaderText="Checkin Loc" 
                                        SortExpression="ResCheckinLoc" />
                                    <asp:BoundField DataField="ResId" HeaderText="Res Id" 
                                        SortExpression="ResId" />
                                    <asp:BoundField DataField="ResNoDaysReserved" HeaderText="No Days Reserved" 
                                        SortExpression="ResNoDaysReserved" />
                                    <asp:BoundField DataField="ResDriverName" HeaderText="Driver Name" 
                                        SortExpression="ResDriverName" />
                                    <asp:BoundField DataField="Matches" HeaderText="Matches" 
                                        SortExpression="Matches" />
                                </Columns>
                                <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" 
                                    LastPageImageUrl="~/App.Images/pager-last.png" Mode="NextPreviousFirstLast"
                                    NextPageImageUrl="~/App.Images/pager-next.png"  
                                    PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                                <PagerStyle  HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                                <selectedrowstyle backcolor="LightCyan" forecolor="DarkBlue" font-bold="true" Font-Underline="False" /> 
                            </asp:GridView>
                            
                            <asp:ObjectDataSource ID="ObjectDataSourceReservation" runat="server" 
                                SelectMethod="getReservations" SortParameterName="sortExpression" 
                                TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownListDueCountry" 
                                        ConvertEmptyStringToNull="False" Name="dueCountry" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                        ConvertEmptyStringToNull="False" Name="ownCountry" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListPool" 
                                        ConvertEmptyStringToNull="False" Name="pool" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListLocationGroup" 
                                        ConvertEmptyStringToNull="False" Name="locationGroup" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarSegment" 
                                        ConvertEmptyStringToNull="False" Name="carSegment" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarClass" 
                                        ConvertEmptyStringToNull="False" Name="carClass" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarGroup" 
                                        ConvertEmptyStringToNull="False" Name="carGroup" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListDestinationPool" 
                                        ConvertEmptyStringToNull="False" Name="destinationPool" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListDestinationLocationGroup" 
                                        ConvertEmptyStringToNull="False" Name="destinationLocationGroup" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="TextBoxReservationStartDate" 
                                        Name="reservationStartdate" PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="TextBoxReservationEndDate" 
                                        Name="reservationEnddate" PropertyName="Text" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <%--Container table--%>
                        </td>
                        <td valign="top">                            
                            <asp:GridView ID="GridViewMatches" runat="server" SkinID="matchTable"
                                    AutoGenerateColumns="False" DataSourceID="ObjectDataSourceMatches" 
                                    AllowPaging="True" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="OwnCountry" HeaderText="OwnCountry" 
                                        SortExpression="OwnCountry" />
                                    <asp:BoundField DataField="Location" HeaderText="Location" 
                                        SortExpression="Location" />
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                                    <asp:BoundField DataField="License" HeaderText="License" 
                                        SortExpression="License" />
                                    <asp:BoundField DataField="ModelDesc" HeaderText="ModelDesc" 
                                        SortExpression="ModelDesc" />
                                    <asp:BoundField DataField="Vc" HeaderText="Vc" SortExpression="Vc" />
                                    <asp:BoundField DataField="Operstat" HeaderText="Operstat" 
                                        SortExpression="Operstat" />
                                    <asp:BoundField DataField="Daysrev" HeaderText="Non-rev" 
                                        SortExpression="Daysrev" />
                                </Columns>
                                <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" 
                                    LastPageImageUrl="~/App.Images/pager-last.png" Mode="NextPreviousFirstLast"
                                    NextPageImageUrl="~/App.Images/pager-next.png"  
                                    PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                                <PagerStyle  HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:GridView>

                            <asp:ObjectDataSource ID="ObjectDataSourceMatches" runat="server" 
                                SelectMethod="getFleetMatches" SortParameterName="sortExpression"
                                TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="GridViewReservation" 
                                        ConvertEmptyStringToNull="False" Name="resId" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <i>
                                You are viewing page&nbsp;
                                <%=GridViewReservation.PageIndex + 1 %>
                                &nbsp;of&nbsp;
                                <%=GridViewReservation.PageCount %>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                Maximum number of rows:&nbsp;
                                <asp:DropDownList ID="DropDownListPagerMaxRows" runat="server" 
                                    AutoPostBack="True" onselectedindexchanged="DropDownListPagerMaxRows_SelectedIndexChanged">
                                    <asp:ListItem Value="10">10 Rows</asp:ListItem>
                                    <asp:ListItem Value="20">20 Rows</asp:ListItem>
                                    <asp:ListItem Value="30">30 Rows</asp:ListItem>
                                    <asp:ListItem Value="40">40 Rows</asp:ListItem>
                                    <asp:ListItem Value="50">50 Rows</asp:ListItem>
                                </asp:DropDownList>
                            </i>
                        </td>
                        <td align="center">
                            <i>
                                You are viewing page&nbsp;
                                <%=GridViewMatches.PageIndex + 1 %>
                                &nbsp;of&nbsp;
                                <%=GridViewMatches.PageCount %>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                Maximum number of rows:&nbsp;
                                <asp:DropDownList ID="DropDownListFleetMax" runat="server" 
                                    AutoPostBack="True" onselectedindexchanged="DropDownListFleetMax_SelectedIndexChanged" >
                                    <asp:ListItem Value="10">10 Rows</asp:ListItem>
                                    <asp:ListItem Value="20">20 Rows</asp:ListItem>
                                    <asp:ListItem Value="30">30 Rows</asp:ListItem>
                                    <asp:ListItem Value="40">40 Rows</asp:ListItem>
                                    <asp:ListItem Value="50">50 Rows</asp:ListItem>
                                </asp:DropDownList>
                            </i>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        <table width="98%" cellpadding="10">
                            <tr>
                                <td>
                                    <fieldset>   
                                        <legend>Reservation Return Filters:</legend>
                                        <table width="98%" cellpadding="10">
                                            <tr>
                                                <td>Country</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListDueCountry" runat="server" 
                                                        AutoPostBack="True" DataSourceID="ObjectDataSourceDueCountry">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceDueCountry" runat="server" 
                                                        SelectMethod="getReservationReturnCountry" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                    </asp:ObjectDataSource>
                                                </td>                                                                         
                                            </tr>
                                            <tr>
                                                <td>Pool</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListDestinationPool" runat="server" 
                                                        AutoPostBack="True" DataSourceID="ObjectDataSourceDestinationPool">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceDestinationPool" runat="server" 
                                                        SelectMethod="getPoolList" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListDueCountry" Name="country" 
                                                                ConvertEmptyStringToNull="False" PropertyName="SelectedValue" Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Location Group</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListDestinationLocationGroup" runat="server" 
                                                        AutoPostBack="True" 
                                                        DataSourceID="ObjectDataSourceDestinationLocationGroup">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceDestinationLocationGroup" 
                                                        runat="server" SelectMethod="getLocationList" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListDestinationPool" 
                                                                ConvertEmptyStringToNull="False" Name="poolId" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                            <asp:ControlParameter ControlID="DropDownListDueCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
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
                                                    <asp:DropDownList ID="DropDownListOwnCountry" runat="server" 
                                                        AutoPostBack="True" DataSourceID="ObjectDataSourceOwnCountry">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceOwnCountry" runat="server" 
                                                        SelectMethod="getReservationLocationCountries" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                    </asp:ObjectDataSource>
                                                </td>
                                                <td>Car Segment</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListCarSegment" runat="server" 
                                                        AutoPostBack="True" DataSourceID="ObjectDataSourceCarSegment">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceCarSegment" runat="server" 
                                                        SelectMethod="getCarSegment" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                                <td>Start Date</td>
                                                <td>
                                                    <asp:TextBox ID="TextBoxReservationStartDate" runat="server" 
                                                        AutoPostBack="true" ontextchanged="TextBoxReservationStartDate_TextChanged"></asp:TextBox>
                                                    <asp:ImageButton ID="ImageButtonStartCalendar" runat="server" ImageUrl="~/App.Images/calendar.png" 
                                                        AlternateText="Calendar" ToolTip="Click to show calendar" />
                                                    <asp:CalendarExtender ID="TextBoxReservationStartDate_CalendarExtender" PopupButtonID="ImageButtonStartCalendar"
                                                        PopupPosition="Right"  Format="dd/MM/yyyy"
                                                        runat="server" Enabled="True" TargetControlID="TextBoxReservationStartDate">
                                                    </asp:CalendarExtender>
                                                    <asp:Label ID="LabelReservsationStartDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Pool</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListPool" runat="server" 
                                                        DataSourceID="ObjectDataSourcePool" AutoPostBack="True">
                                                    </asp:DropDownList>                                                
                                                    <asp:ObjectDataSource ID="ObjectDataSourcePool" runat="server" 
                                                        SelectMethod="getPoolList" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                                <td>Car Class</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListCarClass" runat="server" AutoPostBack="True" 
                                                        DataSourceID="ObjectDataSourceCarClass">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceCarClass" runat="server" 
                                                        SelectMethod="getCarClass" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                            <asp:ControlParameter ControlID="DropDownListCarSegment" 
                                                                ConvertEmptyStringToNull="False" Name="carSegment" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                                <td>End Date</td>
                                                <td>
                                                    <asp:TextBox ID="TextBoxReservationEndDate" runat="server" AutoPostBack="true" 
                                                        ontextchanged="TextBoxReservationEndDate_TextChanged"></asp:TextBox>
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
                                                    <asp:DropDownList ID="DropDownListLocationGroup" runat="server" 
                                                        AutoPostBack="True" DataSourceID="ObjectDataSourceLocationGroup">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceLocationGroup" runat="server" 
                                                        SelectMethod="getLocationList" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListPool" 
                                                                ConvertEmptyStringToNull="False" Name="poolId" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                            <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                                <td>Car Group</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownListCarGroup" runat="server" AutoPostBack="True" 
                                                        DataSourceID="ObjectDataSourceCarGroup">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceCarGroup" runat="server" 
                                                        SelectMethod="getCarGroup" 
                                                        TypeName="App.BLL.VehiclesAbroad.ReservationFleetModel">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                            <asp:ControlParameter ControlID="DropDownListCarSegment" 
                                                                ConvertEmptyStringToNull="False" Name="carSegment" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                            <asp:ControlParameter ControlID="DropDownListCarClass" 
                                                                ConvertEmptyStringToNull="False" Name="carClass" PropertyName="SelectedValue" 
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>  
                                    </fieldset>
                                </td>
                            </tr>      
                        </table>
                        <%--end of containing table --%> 
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" SkinID="backgroundCover"></asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" SkinID="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;
                Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" SkinID="loadDataImage" />
            </asp:Panel>
        </ProgressTemplate>        
    </asp:UpdateProgress>

</asp:Content>
