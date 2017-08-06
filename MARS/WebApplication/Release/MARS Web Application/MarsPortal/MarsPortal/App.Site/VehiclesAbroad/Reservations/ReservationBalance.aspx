<%@ Page Title="Reservation Balance" Theme="MarsV3" Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Application.Master" CodeBehind="ReservationBalance.aspx.cs" Inherits="App.Site.VehiclesAbroad.Reservations.ReservationBalance2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <br />
    <h2 style="text-align:left;">Vehicles Abroad - Reservation Balance</h2>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridViewReservationBalance" runat="server" 
                    onrowdatabound="GridViewReservationBalance_RowDataBound">
                </asp:GridView>
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" GroupingText="Filters " Width="300px" Height="72px">
                                Number of days:&nbsp;
                                <asp:DropDownList ID="DropDownListNoOfDays" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="DropDownListNoOfDays_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel runat="server" GroupingText="Information " Height="72px">
                                The Reservation Balance is :
                                <br />
                                The Available Fleet + Known Oneway Check-Ins 
                                by Owning Country in Destination Country
                                <br /> 
                                MINUS 
                                <br />
                                The Reservations in the next X days + Known Open Oneway Rentals
                                out of the Destination Country into the Owning Country.
                            </asp:Panel>
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

    <script type="text/javascript">
    // not sure if this is needed
        //var reloadGrid = <%--= reloadGrid --%>
        //if (reloadGrid === 1) setTimeout(function () { __doPostBack(); }, 1000);
    </script>

</asp:Content>