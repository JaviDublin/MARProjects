var MSACTIVEX = "Microsoft.XMLHTTP", READSTATE = 4, XMLSTATUS = 200;

function LoadXMLDoc(retFunction, query, _webservicePath, _postMethod) {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject(MSACTIVEX);
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == READSTATE && xmlhttp.status == XMLSTATUS) {
            retFunction(xmlhttp.responseText);
        }
    }

    xmlhttp.open(_postMethod, _webservicePath + query, true);
    xmlhttp.send();
}
function pscCoverAll() {
    h1 = document.createElement("div");
    h1.setAttribute('id', 'pscCoverAll');
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
function pscCreateLoading() {
    h1 = document.createElement("div");
    h1.setAttribute('id', 'pscLoading');
    h1.style.position = 'absolute';
    h1.style.width = '20%';
    h1.style.height = '12%';
    h1.style.left = '40%';
    h1.style.top = '44%';
    h1.style.backgroundColor = '#ffffff';
    h1.style.color = '#000000';
    h1.style.borderColor = 'red';
    h1.style.fontSize = '30px';
    h1.style.textAlign = 'center';
    h1.style.verticalAlign = 'text-bottom';
    h1.innerHTML = '--LOADING--';
    h1.style.borderColor = '#000000';
    h1.style.borderStyle = 'solid';
    h1.style.borderWidth = '2px';
    document.body.appendChild(h1);
}
function pscUncoverAll() {
    if (document.getElementById('pscCoverAll'))
        document.body.removeChild(document.getElementById('pscCoverAll'));
}
function pscRemoveLoading() {
    if (document.getElementById('pscLoading'))
        document.body.removeChild(document.getElementById('pscLoading'));
}
function BrowserDetails() {
    this.winW = 630;
    this.winH = 460;
    if (document.body && document.body.offsetWidth) {
        this.winW = document.body.offsetWidth;
        this.winH = document.body.offsetHeight;
    }
    if (document.compatMode == 'CSS1Compat' && document.documentElement && document.documentElement.offsetWidth) {
        this.winW = document.documentElement.offsetWidth;
        this.winH = document.documentElement.offsetHeight;
    }
    if (window.innerWidth && window.innerHeight) {
        this.winW = window.innerWidth;
        this.winH = window.innerHeight;
    }
}
BrowserDetails.prototype.SetIds = function (winWId, winHId) {
    document.getElementById(winWId).value = this.winW;
    document.getElementById(winHId).value = this.winH;
}
BrowserDetails.prototype.SetStyle = function (IdOfStyle, ratioW) {
    if (ratioW == null) ratioW = 0.80;
    document.getElementById(IdOfStyle).style.width = this.winW * ratioW + 'px';
}