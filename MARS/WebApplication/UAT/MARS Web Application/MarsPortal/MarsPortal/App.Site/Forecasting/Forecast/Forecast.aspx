﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.Masterpages/Application.Master"
    Inherits="App.Reports.Forecasting.Forecast" CodeBehind="Forecast.aspx.cs" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceholderMainContent">
        <asp:UpdatePanel ID="upChart" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc:ChartControl ID="ccForecast" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upnlGeneralParameters" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc:GeneralReportParameters ID="GeneralParams" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress runat="server" ID="upChartProgress" DisplayAfter="1000">
            <ProgressTemplate>
                <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>

</asp:Content>
