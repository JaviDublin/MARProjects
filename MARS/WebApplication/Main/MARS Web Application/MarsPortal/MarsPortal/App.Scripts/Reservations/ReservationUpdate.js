
var service = new ReservationUpdateService();
setInterval(function () { service.GetUpdate(worked); }, 300000);

function worked(result) {
    var obj = eval("(" + result + ")");
    $get("labelDBUpdate").innerHTML = obj.Message;
    
}