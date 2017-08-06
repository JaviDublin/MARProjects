<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiSelectDropDownList.ascx.cs" Inherits="App.UserControls.DropDownList.MultiSelectDropDownList" %>
<script type="text/javascript" language="javascript">

    Sys.Application.add_load(ApplicationLoadHandler)
    function ApplicationLoadHandler(sender, args) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (!prm.get_isInAsyncPostBack()) {
            var listBox = document.getElementById('<%=ListBoxMultiSelect.ClientID%>');
            $(listBox).dropdownchecklist("destroy");
            if (listBox.length >= 2) {
                $(listBox).dropdownchecklist({
                    firstItemChecksAll: true,
                    maxDropHeight: 200,
                    width: 190
                });
            }
            else {
                $(listBox).dropdownchecklist({
                    width: 190
                });
            }
        }
    }

    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
    function pageLoaded(sender, args) {
        //get the update panels updated
        var updatedPanels = args.get_panelsUpdated();
        // check if report Settings Panel was updated
        for (idx = 0; idx < updatedPanels.length; idx++) {
            if (updatedPanels[idx].id == 'ctl00_ContentPlaceHolderMain_UserControlReportSettings_UpdatePanelReportSettings') {
                var listBox = document.getElementById('<%=ListBoxMultiSelect.ClientID%>');
                $(listBox).dropdownchecklist("destroy");
                if (listBox.length >= 2) {
                    $(listBox).dropdownchecklist({
                        firstItemChecksAll: true,
                        maxDropHeight: 200,
                        width: 190
                    });
                }

                else {
                    $(listBox).dropdownchecklist({
                        width: 190
                    });
                }
            }
            else if (updatedPanels[idx].id == 'ctl00_ContentPlaceHolderMain_UpdatePanelMaintenanceUsers') {
                var listBox = document.getElementById('<%=ListBoxMultiSelect.ClientID%>');
                $(listBox).dropdownchecklist("destroy");
                if (listBox.length >= 2) {
                    $(listBox).dropdownchecklist({
                        firstItemChecksAll: true,
                        maxDropHeight: 100,
                        width: 190
                    });
                }

                else {
                    $(listBox).dropdownchecklist({
                        width: 190
                    });
                }
            }
        }
    } 

</script>
<%--<asp:ListBox ID="ListBoxMultiSelect" runat="server" SelectionMode="Multiple" Width="120px" Style=""></asp:ListBox>--%>
<DIV style="OVERFLOW-Y:scroll; WIDTH:300px; HEIGHT:150px">

<asp:CheckBoxList ID="ListBoxMultiSelect" runat="server" Width="280px">
    <asp:ListItem runat="server"></asp:ListItem>
 </asp:CheckBoxList>
 </DIV>
