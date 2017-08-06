<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FleetSizeExportWebService.aspx.cs" Inherits="Mars.Webservices.Sizing.FleetSizeExportWebService" %>

<div style='padding:5px;border-bottom:2px solid #99CCFF'>Choose delete text values.</div>
<div style='padding:5px;margin-top:5px;text-align:right;'>Add table cmd: <input style='width:420px' id="TxtAddTbl" type="text" value="<%=AddTable %>" onkeyup='document.getElementById("SpAddTbl").innerHTML="*";' <%=Disabled %> /><span id='SpAddTbl' style='color:Red'></span></div>
<div style='padding:5px;text-align:right;'>Delete table cmd: <input style='width:420px' id="TxtDelTbl" type="text" value="<%=DelTable %>" onkeyup='document.getElementById("SpDelTbl").innerHTML="*";' <%=Disabled %> /><span id='SpDelTbl' style='color:Red'></span></div>
<div style='padding:5px;text-align:right;'>Insert cmd: <input style='width:420px' id="TxtInsert" type="text" value="<%=InsertCmd %>" onkeyup='document.getElementById("SpInsert").innerHTML="*";' <%=Disabled %> /><span id='SpInsert' style='color:Red'></span></div>
<div style='padding:5px;text-align:right;'>Add commit cmd: <input style='width:420px' id="TxtAddCmt" type="text" value="<%=AddCommit %>" onkeyup='document.getElementById("SpAddCmt").innerHTML="*";' <%=Disabled %> /><span id='SpAddCmt' style='color:Red'></span></div>
<div style='padding:5px;text-align:right;'>Delete commit cmd: <input style='width:420px' id="TxtDelCmt" type="text" value="<%=DelCommit %>" onkeyup='document.getElementById("SpDelCmt").innerHTML="*";' <%=Disabled %> /><span id='SpDelCmt' style='color:Red'></span></div>
<% if(Disabled=="disabled") Response.Write("<div style='padding:5px;text-align:right;'>You do not have admistration rights to alter these settings.</div>");  %>
<div style='padding:5px;float:right'>
    <Button onclick='fseSaveAni();' style='background-color:Green;color:White' id='btnSave' <%=Disabled %>>Save</Button>
    <Button onclick='fseCloseAni();' style='background-color:Red;color:White' id="btnClose">Close</Button>
</div>
