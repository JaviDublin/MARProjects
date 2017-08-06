<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadingScreen.ascx.cs" Inherits="App.UserControls.LoadingScreen" %>

<div id="light" class="white_content">
    <br />&nbsp;&nbsp;
    <asp:Label runat="server" ID="lblLoading" Text="<%$ Resources:LocalizedChartControl, LoadingChartScreen %>" />
    <%--added by Gavin--%>
    &nbsp;&nbsp;&nbsp;&nbsp
    <asp:Image ID="Image1" runat="server" ImageUrl="~/App.Images/ajax-loader.gif" AlternateText="wait" />
    <%-- ------------ --%>
</div>
<div id="fade" class="black_overlay" />

<script type="text/javascript">
    $(document).ready(function () {
        document.getElementById('light').style.display = 'block';
        document.getElementById('fade').style.display = 'block';
    });
</script>
