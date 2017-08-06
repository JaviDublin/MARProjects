<%@ Page Title="Foreign Non-Revenue" Theme="MarsV3" MasterPageFile="~/App.MasterPages/Application.Master" Language="C#" AutoEventWireup="true" CodeBehind="ForeignNonRevenue.aspx.cs" Inherits="MarsV2.VehiclesAbroad.ForeignNonRevenue2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <br />
    <h2 style="text-align:left;">Vehicles Abroad - Foreign Non-Revenue</h2>
    <br />
    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="gridNonRevenue" runat="server">Loading data</div>

                <asp:Panel ID="Panel1" runat="server" GroupingText="Filters" Width="450px" Height="42px">
                    Country :&nbsp;
                    <asp:DropDownList ID="DropDownListCountries" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownListCountries_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    Number of Non Rev days :&nbsp;
                    <asp:DropDownList ID="DropDownListDays" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="DropDownListDays_SelectedIndexChanged">
                    </asp:DropDownList>
                </asp:Panel>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
