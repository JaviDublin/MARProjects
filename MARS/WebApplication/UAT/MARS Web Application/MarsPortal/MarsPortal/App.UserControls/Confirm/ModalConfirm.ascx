<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalConfirm.ascx.cs" Inherits="App.UserControls.Confirm.ModalConfirm" %>
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
		$('#dialog-confirm').bgiframe();
	}

	function ShowConfirm(clientId, uniqueId) {
		if (IsPageValid()) {
			var ddl = document.getElementById(clientId);
			var selectedValue = ddl.options[ddl.selectedIndex].value;

			if (selectedValue == "-1") {
				$('#dialog-confirm').dialog('option', 'buttons',
				{
					"Continue": function () { $(this).dialog("close"); __doPostBack(uniqueId, ''); },
					"Cancel": function () { $(this).dialog("close"); }
				});

				$('#dialog-confirm').dialog('open');

				return false;
			}
			else {
				return true;
			}
		}
		else {
			return false;
		}
	}

	function IsPageValid() {
		var validationGroup = document.getElementById('<%=LabelValidationGroup.ClientID%>').innerText;
		if (validationGroup == 'none') {
			return true;
		}
		else {
			var vGroup = Page_ClientValidate(validationGroup);
			if (vGroup) {
				return true;
			}
			else {
				return false;
			}
		}
	}
</script>
<div id="dialog-confirm" class="divModalConfirm" style="display: none;">
	<asp:Label ID="LabelMessageLine1" runat="server" Text="<%$ Resources:lang, ConfirmReportForAllCountries %>"></asp:Label><br />
	<asp:Label ID="LabelMessageLine2" runat="server" Text="<%$ Resources:lang, AreYouSureYouWantToContinue %>"></asp:Label>
</div>
<asp:Label ID="LabelValidationGroup" runat="server" Style="display: none;"></asp:Label>
