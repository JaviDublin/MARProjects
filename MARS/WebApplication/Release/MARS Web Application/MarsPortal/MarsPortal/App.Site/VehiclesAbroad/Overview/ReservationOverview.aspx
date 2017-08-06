<%@ Page Title="Reservation Overview" Theme="MarsV3" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="ReservationOverview.aspx.cs" Inherits="MarsV2.VehiclesAbroad.ReservationOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            <center>
                 <table width="98%" cellpadding="10">
                    <tr>
                        <td colspan="4"style="padding:10px; font-size: large; font-weight: bold;">Vehicles Abroad - Reservation Overview</td>
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

                <div id="DataTableReservationOverview" runat="server">Loading data...</div>

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
                                            <asp:DropDownList ID="DropDownListDueCountry" runat="server" 
                                                AutoPostBack="True" DataSourceID="ObjectDataSourceDueCountry">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceDueCountry" runat="server" 
                                                SelectMethod="getReservationReturnCountry" 
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>Car Segment</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarSegment" runat="server" 
                                                AutoPostBack="True" DataSourceID="ObjectDataSourceCarSegment">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceCarSegment" runat="server" 
                                                SelectMethod="getCarSegment" 
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DropDownListOwnCountry" 
                                                        ConvertEmptyStringToNull="False" Name="country" PropertyName="SelectedValue" 
                                                        Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        <td>Start Date</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxReservationStartDate" runat="server" AutoPostBack="true" 
                                                ontextchanged="TextBoxReservationStartDate_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButtonStartCalendar" runat="server" ImageUrl="~/App.Images/calendar.png" 
                                                AlternateText="Calendar" ToolTip="Click to show calendar" />
                                            <asp:CalendarExtender ID="TextBoxReservationStartDate_CalendarExtender" PopupButtonID="ImageButtonStartCalendar"
                                                PopupPosition="Right" Format="dd/MM/yyyy"
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                runat="server" Enabled="True" TargetControlID="TextBoxReservationEndDate"  Format="dd/MM/yyyy">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                                TypeName="App.BLL.VehiclesAbroad.ReservationOverviewModel">
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
                                        <td>
                                        </td>
                                    </tr>
                                </table>  
                            </fieldset>
                        </td>
                    </tr>      
                </table>
                <%--end of containing table --%>   
            </center>   
             
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static">
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
