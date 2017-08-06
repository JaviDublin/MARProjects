var RESPONSETAG = 'scdivTable', HIDDENTAG = 'hdIsPostback';
var _TRUE = 'true', _FALSE = 'false', NOTTRUE = 'nottrue', NOTFALSE = 'notfalse';
var ZERO = 0, ONE = 1, INTERVALLENGTH = 500, MAXCALLBACKS = 20, LEFTOFFSET=0;

function OnMouseOverPowerRow(o, e) {
    x = o.id.split('-')[1];
    document.getElementById('powerRow-' + x).style.backgroundColor = '#cccccc';
}
function OnMouseOutPowerRow(o,e){
    x = o.id.split('-')[1];
    document.getElementById('powerRow-' + x).style.backgroundColor = '#ffFFFF';
}
function BalanceClicked(o, e) {
    $('#UpdateProgress1').delay(2000).show('fast');
    x = o.innerHTML.split('>');
    x = x[1].split('<');
    var s = x[0];
    __doPostBack("Balance_Clicked",s);
}

