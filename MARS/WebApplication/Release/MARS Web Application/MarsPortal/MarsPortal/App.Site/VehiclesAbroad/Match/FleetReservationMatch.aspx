<%@ Page Title="Fleet Reservation Match" Theme="MarsV3" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="FleetReservationMatch.aspx.cs" Inherits="MarsV2.VehiclesAbroad.FleetReservationMatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <center>
                <table cellspacing="10">
                    <tr>
                        <td><h2>Vehicles Abroad - Fleet/Reservation Match</h2></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Vehicles Abroad</h3>
                        </td>
                        <td>
                            <h3>Matching Reservations</h3> 
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center">
                            <asp:GridView ID="GridViewFea" runat="server" AutoGenerateColumns="False" SkinID="choseTable" 
                                DataSourceID="ObjectDataSource1" AllowPaging="True" AutoGenerateSelectButton="True" 
                                SelectedIndex="0" EmptyDataText = "No data available" DataKeyNames="License"
                                ondatabound="GridViewFea_DataBound" AllowSorting="true"  Width="98%"
                                onrowdatabound="GridViewFea_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Location" HeaderText="Location" 
                                        SortExpression="Location" />       
                                    <asp:BoundField DataField="OwnCountry" HeaderText="Owning Country" 
                                        SortExpression="OwnCountry" />
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" 
                                        SortExpression="Unit" />
                                    <asp:BoundField DataField="License" HeaderText="License" 
                                        SortExpression="License" />
                                    <asp:BoundField DataField="ModelDesc" HeaderText="Model Description" 
                                        SortExpression="ModelDesc" />
                                    <asp:BoundField DataField="Vc" HeaderText="VC" 
                                        SortExpression="Vc" />
                                    <asp:BoundField DataField="Operstat" HeaderText="Operstat" 
                                        SortExpression="Operstat" />
                                    <asp:BoundField DataField="Daysrev" HeaderText="Non-Rev" 
                                        SortExpression="Daysrev" />
                                    <asp:BoundField DataField="Matches" HeaderText="Matches" 
                                        SortExpression="Matches" />
                                </Columns>
                                <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" 
                                    LastPageImageUrl="~/App.Images/pager-last.png" Mode="NextPreviousFirstLast"
                                    NextPageImageUrl="~/App.Images/pager-next.png"  
                                    PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                                <PagerStyle  HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="Yellow" Font-Bold="true" /> 
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                SelectMethod="getFleetReservations" SortParameterName="sortExpression" 
                                TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownListDueCountry" DefaultValue="" 
                                        Name="dueCountry" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" DefaultValue="" 
                                        Name="ownCountry" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListPool" 
                                        ConvertEmptyStringToNull="False" Name="pool" PropertyName="SelectedValue" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListLocationGroup" 
                                        ConvertEmptyStringToNull="False" Name="locationGroup" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarSegment" Name="carSegment" 
                                        ConvertEmptyStringToNull="False" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarClass" Name="carClass" 
                                        ConvertEmptyStringToNull="False" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListCarGroup" Name="carGroup" 
                                        ConvertEmptyStringToNull="False" PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="DropDownListVehiclePredicament" 
                                        ConvertEmptyStringToNull="False" DefaultValue="0" Name="vehiclePredicament" 
                                        PropertyName="SelectedIndex" Type="Int32" />
                                    <asp:ControlParameter ControlID="TextBoxStartDate" 
                                        ConvertEmptyStringToNull="False" Name="startDate" PropertyName="Text" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="TextBoxEndDate" 
                                        ConvertEmptyStringToNull="False" Name="endDate" PropertyName="Text" 
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                        <td valign="top">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" SkinID="matchTable" 
                                DataSourceID="ObjectDataSource2" AllowPaging="True" EmptyDataText = "No data available"
                                AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="ResNoDaysUntilCheckout" HeaderText="No Days Until Checkout" 
                                        SortExpression="ResNoDaysUntilCheckout" />
                                    <asp:BoundField DataField="ResLocation" HeaderText="Location" 
                                        SortExpression="ResLocation" />
                                    <asp:BoundField DataField="ResGroup" HeaderText="Group" 
                                        SortExpression="ResGroup" />
                                    <asp:BoundField DataField="ResCheckoutDate" HeaderText="Checkout Date" 
                                        SortExpression="ResCheckoutDate" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="ResCheckinLoc" HeaderText="Checkin Loc" 
                                        SortExpression="ResCheckinLoc" />
                                    <asp:BoundField DataField="ResId" HeaderText="Res Id" 
                                        SortExpression="ResId" />
                                    <asp:BoundField DataField="ResDriverName" HeaderText="Driver Name"
                                        SortExpression="ResDriverName" />
                                    <asp:BoundField DataField="ResNoDaysReserved" HeaderText="No Days Reserved" 
                                        SortExpression="ResNoDaysReserved" />
                                </Columns>
                                <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" 
                                    LastPageImageUrl="~/App.Images/pager-last.png" Mode="NextPreviousFirstLast" 
                                    NextPageImageUrl="~/App.Images/pager-next.png"  
                                    PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                                <PagerStyle HorizontalAlign="Right" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                                TypeName="App.BLL.VehiclesAbroad.FleetReservationModel" 
                                SelectMethod="getReservationMatches"  SortParameterName="sortExpression">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="GridViewFea" Name="License" 
                                        PropertyName="SelectedValue" Type="String"  />
                                    <asp:ControlParameter ControlID="TextBoxStartDate" 
                                        ConvertEmptyStringToNull="False" Name="startDateString" PropertyName="Text" 
                                        Type="String" />
                                    <asp:ControlParameter ControlID="TextBoxEndDate" 
                                        ConvertEmptyStringToNull="False" Name="endDateString" PropertyName="Text" 
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>                          
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <i>
                                You are viewing page&nbsp;
                                <%=GridViewFea.PageIndex + 1 %>
                                &nbsp;of&nbsp;
                                <%=GridViewFea.PageCount %>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="DropDownListPagerMaxRows" runat="server" 
                                 AutoPostBack="True" 
                                 onselectedindexchanged="DropDownListPagerMaxRows_SelectedIndexChanged">
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
                                <%=GridView2.PageIndex + 1 %>
                                &nbsp;of&nbsp;
                                <%=GridView2.PageCount %>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="DropDownListReservationMatch" runat="server" 
                                    AutoPostBack="True" 
                                onselectedindexchanged="DropDownListReservationMatch_SelectedIndexChanged">
                                    <asp:ListItem Value="10">10 Rows</asp:ListItem>
                                    <asp:ListItem Value="20">20 Rows</asp:ListItem>
                                    <asp:ListItem Value="30">30 Rows</asp:ListItem>
                                    <asp:ListItem Value="40">40 Rows</asp:ListItem>
                                    <asp:ListItem Value="50">50 Rows</asp:ListItem>
                                </asp:DropDownList>
                            </i>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="center">
                            <%--Container table--%>
                            <table>
                                <tr>
                                    <td>
                                        <fieldset>   
                                            <legend>Destination Filters:</legend>
                                            <table>
                                                <tr>
                                                    <td>Vehicle predicament</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListVehiclePredicament" runat="server" 
                                                             AutoPostBack="True">
                                                            <asp:ListItem>*** All ***</asp:ListItem>
                                                            <asp:ListItem>On rent: Owning country to foreign country</asp:ListItem>
                                                            <asp:ListItem>Transfer: Owning country to foreign country</asp:ListItem>
                                                            <asp:ListItem>Idle in foreign country</asp:ListItem>
                                                            <asp:ListItem>On rent in or between foreign county(ies)</asp:ListItem>
                                                            <asp:ListItem>Returning to owning country</asp:ListItem>
                                                            <asp:ListItem>Transfer: Returning to owning country</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>Country</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListDueCountry" runat="server" 
                                                            AutoPostBack="True" DataSourceID="ObjectDataSourceDueCountry">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceDueCountry" runat="server" 
                                                            SelectMethod="getFeaDueCountries" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                                        </asp:ObjectDataSource>
                                                    </td>                                        
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>Pool</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListPool" runat="server" 
                                                            DataSourceID="ObjectDataSourcePool" AutoPostBack="True">
                                                        </asp:DropDownList>                                                
                                                        <asp:ObjectDataSource ID="ObjectDataSourcePool" runat="server" 
                                                            SelectMethod="getPoolList" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="DropDownListDueCountry" 
                                                                    ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                    Type="String" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>                                            
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>Location Group</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListLocationGroup" runat="server" 
                                                            AutoPostBack="True" DataSourceID="ObjectDataSourceLocationGroup">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceLocationGroup" runat="server" 
                                                            SelectMethod="getLocationList" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="DropDownListPool" 
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
                                            <legend>Owning Filters:</legend>
                                            <table>                            
                                                <tr>
                                                    <td>Country</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListOwnCountry" runat="server" 
                                                            AutoPostBack="True" DataSourceID="ObjectDataSourceOwnCountry">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceOwnCountry" runat="server" 
                                                            SelectMethod="getFeaOwnCountries" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                    <td>Car Segment</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListCarSegment" runat="server" 
                                                            AutoPostBack="True" DataSourceID="ObjectDataSourceCarSegment">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceCarSegment" runat="server" 
                                                            SelectMethod="getCarSegment" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                                    ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                                    Type="String" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>Car Class</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListCarClass" runat="server" AutoPostBack="True" 
                                                            DataSourceID="ObjectDataSourceCarClass">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceCarClass" runat="server" 
                                                            SelectMethod="getCarClass" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
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
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>Car Group</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownListCarGroup" runat="server" AutoPostBack="True" 
                                                            DataSourceID="ObjectDataSourceCarGroup">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="ObjectDataSourceCarGroup" runat="server" 
                                                            SelectMethod="getCarGroup" 
                                                            TypeName="App.BLL.VehiclesAbroad.FleetReservationModel">
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
                        <td align="center">                          
                            <table width="98%" cellpadding="10">
                                <tr>
                                    <td>
                                        <fieldset>   
                                            <legend>Reservation Start Filters:</legend>
                                            <table width="98%" cellpadding="10">
                                                <tr>
                                                    <td>Start Date</td>
                                                    <td>
                                                        <asp:TextBox ID="TextBoxStartDate" runat="server" AutoPostBack="true" 
                                                            ontextchanged="TextBoxStartDate_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="TextBoxStartDate_CalendarExtender" runat="server"
                                                            PopupPosition="Right" PopupButtonID="ImageButtonStartDate" Format="dd/MM/yyyy"
                                                            Enabled="True" TargetControlID="TextBoxStartDate">
                                                        </asp:CalendarExtender>
                                                        <asp:ImageButton ID="ImageButtonStartDate" runat="server" ImageUrl="~/App.Images/calendar.png" 
                                                                        AlternateText="Calendar" ToolTip="Click to show calendar"  />
                                                         &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>End Date</td>
                                                    <td>
                                                        <asp:TextBox ID="TextBoxEndDate" runat="server" AutoPostBack="true"
                                                            ontextchanged="TextBoxEndDate_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="TextBoxEndDate_CalendarExtender" runat="server" 
                                                            PopupPosition="Right" PopupButtonID="ImageButtonEndDate" Format="dd/MM/yyyy"
                                                            Enabled="True" TargetControlID="TextBoxEndDate">
                                                        </asp:CalendarExtender>
                                                        <asp:ImageButton ID="ImageButtonEndDate" runat="server" ImageUrl="~/App.Images/calendar.png" 
                                                                        AlternateText="Calendar" ToolTip="Click to show calendar" />
                                                         &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <td>                    
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="LabelStartDate" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="LabelEndDate" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>

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
