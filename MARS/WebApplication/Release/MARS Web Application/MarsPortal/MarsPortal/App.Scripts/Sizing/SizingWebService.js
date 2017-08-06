var _sizingService = new SizingStateService();

function Work(s) {
    var obj = eval("(" + s + ")");
    if (obj.Action == 'Running') {
        $get('lblUpdateStatus').innerHTML = obj.Message;
        $get('lblUpdateStatus').style.color = '#FF0000';
        $get('btnUpload').disabled = 'disabled';
        $get('divStatus').innerHTML = 'MoveState3';
        $get('divUpdateBtn').innerHTML = 'NoClick';
    } else {
        $get('lblUpdateStatus').innerHTML = obj.Message;
        $get('lblUpdateStatus').style.color = '#008080';
        $get('btnUpload').disabled = '';
        $get('divStatus').innerHTML = 'MoveState1';
    }
}
function StoredProc(ans) {
    $get('divStatus').innerHTML = ans;
    storedProcRunning = 0;
}
count = 0;
storedProcRunning = 0;
if ($get('divStateMachine').innerHTML == '') $get('divStateMachine').innerHTML = '0';
monitorStatus();
var UpdateInterval = this.setInterval(function () { monitorStatus(); }, 2000);

function monitorStatus() {
    StateMachine = $get('divStateMachine').innerHTML;
    $get('divCountStatus').innerHTML = ' the count = ' + count;
    count = count + 1;
    switch (StateMachine) {
        case '0': // State 0 check server for state
            if ($get('divStatus').innerHTML == 'MoveState1') $get('divStateMachine').innerHTML = '1';
            else _sizingService.GetUpdate(Work);
            if ($get('divStatus').innerHTML == 'MoveState3') $get('divStateMachine').innerHTML = '3';            
            break;
        case '1': //State1 button upload pushed
            if ($get('divUpdateBtn').innerHTML == 'Clicked') { BtnUploadClick(); break; }
            if ($get('divStatus').innerHTML == 'MoveState2') $get('divStateMachine').innerHTML = '2';
            else ($get('divStatus').innerHTML = 'MoveState0');
            if ($get('divStatus').innerHTML == 'MoveState0') $get('divStateMachine').innerHTML = '0';
            break;
        case '2': //State2 Server returns from postback
            if ($get('divStatus').innerHTML == 'MoveState3') { $get('divStateMachine').innerHTML = '3'; break; }
            if (storedProcRunning == 0) _sizingService.RunStoredProcedure(StoredProc);
            storedProcRunning = 1;
            break;
        case '3': // State3 
            $get('divStatus').innerHTML = 'MoveState0'
            if ($get('divStatus').innerHTML == 'MoveState0') $get('divStateMachine').innerHTML = '0';
            break;
//        default:
//            $get('divStateMachine').innerHTML = '0';
//            $get('divStatus').innerHTML = 'MoveState1';
    }
}
function BtnUploadClick() {
    $get('divUpdateBtn').innerHTML = 'Clicked';
    $get('btnUpload').disabled = 'disabled';
    if ($get('divStateMachine').innerHTML != '1') return;
    $get('divUpdateBtn').innerHTML = 'NoClick';

    if ($get('fupMovementUpload').value == '') {
        $get('lblStatus').innerHTML = 'Please select a file to upload.';
        return false;
    }
    if ($get('ddlCountryList').value == 'dummy') {
        $get('lblStatus').innerHTML = 'Please select a country.';
        return false;
    }
    $get('lblStatus').innerHTML = 'Starting Upload......';

    __doPostBack('UploadFile', $get('divStateMachine').innerHTML);
}
