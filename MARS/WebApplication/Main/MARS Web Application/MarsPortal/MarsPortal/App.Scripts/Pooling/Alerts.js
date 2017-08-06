var RESPONSETAG = 'divTable', HIDDENTAG = 'hdIsPostback';
var _TRUE = 'true', _FALSE = 'false', NOTTRUE = 'nottrue', NOTFALSE = 'notfalse';
var WEBSERVICEPATH = "../../App.Webservices/Pooling/AlertsService.aspx", POSTMETHOD = 'GET';
var ZERO = 0, ONE = 1, INTERVALLENGTH = 500, MAXCALLBACKS = 20;
var _callbackInterval, _callbackCount = ZERO;

function CallbackReturn(o) {
    _callbackCount += ONE;
    var hdIsPostback = document.getElementById(HIDDENTAG);
    if (hdIsPostback.value == _FALSE) {
        _callbackInterval = setInterval(CallbackReturn, INTERVALLENGTH);
        hdIsPostback.value = NOTFALSE;
    }
    if (hdIsPostback.value == _TRUE || _callbackCount >= MAXCALLBACKS) {
        hdIsPostback.value = _FALSE
        window.clearInterval(_callbackInterval);
        _callbackCount = ZERO;
        LoadXMLDoc(retData, "", WEBSERVICEPATH, POSTMETHOD);
    }
}
POPUPTOPOFFSET = 260, POPUPLEFTOFFSET = 50;
function MouseOver(o, e, t) {
    d = document.getElementById('AlertCarList');
    var rect = document.getElementById('divTable').getBoundingClientRect();
    d.style.top = e.clientY - rect.top + POPUPTOPOFFSET + 'px';
    d.style.left = e.clientX - rect.left + POPUPLEFTOFFSET + 'px';
    d.style.display = 'inline';
    LoadXMLDoc(PopPopup, '?mouseOverNode=' + t, WEBSERVICEPATH, POSTMETHOD);
}
function MouseOut() {
    document.getElementById('AlertCarList').style.display = 'none';
    document.getElementById('AlertCarList').innerHTML = 'Processing...';
}
function PopPopup(mess) {
    document.getElementById('AlertCarList').innerHTML = mess;
}