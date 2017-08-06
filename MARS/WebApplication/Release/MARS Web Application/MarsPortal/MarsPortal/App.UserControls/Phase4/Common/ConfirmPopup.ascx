<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmPopup.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Common.ConfirmPopup" %>

<script type="text/javascript" language="javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    Sys.Application.add_load(LoadConfirmDialog);

    function LoadConfirmDialog() {
        $('#dialog-confirm').dialog({
            autoOpen: false,
            resizable: false,
            bgiframe: true,
            modal: true,
            title: "Please confirm to continue",
            width: 400,
            height: 150,
            show: 'blind'
        });
    }

    function ShowConfirm(uniqueId, text) {
        var message = document.getElementById('<%=LabelMessage.ClientID%>');
       message.innerHTML = text;


        alert(uniqueId);

       $('#dialog-confirm').dialog('option', 'buttons',
               {
                   "Continue": function () { $(this).dialog("close"); __doPostBack(this, ''); },
                   "Cancel": function () { $(this).dialog("close"); }
               });

       $('#dialog-confirm').dialog('open');
       return false;
   }




</script>
<div id="dialog-confirm" >
    <asp:Label ID="LabelMessage" runat="server"></asp:Label><br />
    <asp:Panel ID="PanelMessage2" runat="server">
        <p>
           <!-- <strong>Please note: </strong>You cannot delete records if it referenced with a child record. -->
        </p>
    </asp:Panel>
</div>
