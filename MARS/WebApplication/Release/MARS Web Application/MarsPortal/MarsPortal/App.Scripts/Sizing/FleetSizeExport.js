function fseLoadXMLDoc(retFunction, query) {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            retFunction(xmlhttp.responseText);
        }
    }
    xmlhttp.open('get', "../../../App.Webservices/Sizing/FleetSizeExportWebService.aspx"+query, true);
    xmlhttp.send();
}
function fseRetData(s) {
    fseMessage = s;
    fseGenTable(retId);
}
function fseButtonClick(o, e) {
    if (fseShowing) return;
    fseShowing = true;
    retId = o.id + '1';
    fseCoverAll();
    fseMouseTop = e.clientY + 'px';
    fseMouseLeft = e.clientX + 'px';
    fseLoadXMLDoc(fseRetData, '?docId=' + retId);
}
var fseAniTimer, fseAniCount, fseNNId, fseShowing = false, fseMessage, fseCountInc = 1,fseQuery,fseMouseTop,fseMouseLeft;
var ANICOUNT = 40, FSEWIDTH = '560px', FSEHEIGHT = '240px', FSEINTERVAL = (msieversion() > 8 || msieversion() == 0) ? 10 : 1;

function fseGenTable(nn) {
    var h = document.createElement("div");
    h.setAttribute("id", nn);
    h.style.position = 'absolute'
    h.style.top = fseMouseTop;
    h.style.left = fseMouseLeft;
    h.style.color = '#0000cc';
    h.style.backgroundColor = '#ffffff';
    h.style.borderWidth = '2px';
    h.style.borderStyle = 'solid';
    h.style.borderColor = '#99CCFF';
    h.style.width = FSEWIDTH;
    h.style.height = FSEHEIGHT;
    h.style.overflow = 'hidden';
    h.style.clip = "rect(0px,0px,0px,0px)";
    h.style.zIndex = 999;
    document.body.appendChild(h);
    fseAniCount = 0;
    fseNNId = nn;
    fseAniTimer = setInterval(function () { fseStartAni(); }, FSEINTERVAL);
}
function fseStartAni() {
    fseAniCount += fseCountInc;
    if (fseAniCount > ANICOUNT) { clearInterval(fseAniTimer); return; }
    theElewelly = document.getElementById(fseNNId);
    theElewelly.style.clip = 'rect(0px,' + fseAniCount * 15 + 'px,' + fseAniCount * 7 + 'px,0px)';
    theElewelly.innerHTML = fseMessage;//  + "<br />Ani count=" + fseAniCount;
}
function fseCloseAni() {
    document.body.removeChild(document.getElementById(fseNNId));
    document.body.removeChild(document.getElementById('fseCoverAll'));
    fseShowing = false;
}
function fseSaveAni() {
    fseQuery = "?saveAll=yes";
    if (document.getElementById('SpAddTbl').innerHTML == '*') fseQuery += '&AddTable=' + document.getElementById('TxtAddTbl').value;
    if (document.getElementById('SpDelTbl').innerHTML == '*') fseQuery += '&DelTable=' + document.getElementById('TxtDelTbl').value;
    if (document.getElementById('SpInsert').innerHTML == '*') fseQuery += '&InsertTable=' + document.getElementById('TxtInsert').value;
    if (document.getElementById('SpAddCmt').innerHTML == '*') fseQuery += '&AddCommit=' + document.getElementById('TxtAddCmt').value;
    if (document.getElementById('SpDelCmt').innerHTML == '*') fseQuery += '&DelCommit=' + document.getElementById('TxtDelCmt').value;
    if (fseQuery == "?saveAll=yes") document.getElementById(fseNNId).innerHTML = '<center><div>No changes to save.</div><div><button onclick="fseCloseAni();">Close</button></div></center>';
    else document.getElementById(fseNNId).innerHTML = '<center><div>Are you sure?</div><div><button onclick="fseLoadXMLDoc(fseSaveAll, fseQuery);">Yes</button><button onclick="fseCloseAni();">No</button></div></center>';
}
function fseSaveAll(t) {
    s = t.split(',');
    alert(s[0]);
    fseCloseAni();
}
function fseCoverAll() {
    h1 = document.createElement("div");
    h1.setAttribute('id', 'fseCoverAll');
    h1.style.position = 'fixed';
    h1.style.top = '0px'
    h1.style.left = '0px';
    h1.style.width = '100%';
    h1.style.height = '100%';
    h1.style.zIndex = 998;
    h1.style.opacity = 0.5
    h1.style.filter = 'alpha(opacity:50)';
    h1.style.backgroundColor = '#ffffff';
    document.body.appendChild(h1);
}
function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0) return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)));
    else return 0;
}
