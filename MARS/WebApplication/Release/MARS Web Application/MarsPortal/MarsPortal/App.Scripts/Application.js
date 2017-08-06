//===================================================================
//  Purpose:    Check Session Timeout
//===================================================================
var sessionTimeoutTimer;
var keepSessionAliveTimer;



function CheckSessionTimeout(milliSecondsTimeOut) {
	clearTimeout(sessionTimeoutTimer);
	var sessionTimeout = milliSecondsTimeOut;
	sessionTimeoutTimer = setTimeout(RefreshPageSessionTimeout, sessionTimeout);
};

function RefreshPageSessionTimeout() {
	window.location.href = window.location.href;
};


function validateCountry(source, args) {
    if (args.Value.length >= 1 && args.Value.length <= 10)
        args.IsValid = true;
    else
        args.IsValid = false;
}

function validateOwnArea(source, args) {
    if (args.Value.length >= 1 && args.Value.length <= 5)
        args.IsValid = true;
    else
        args.IsValid = false;
}

function validateSortOrder(source, args) {
    if (args.Value >= 0)
        args.IsValid = true;
    else
        args.IsValid = false;
}

function validateCMSLocationGroup(source, args) {
    if (args.Value.length >= 1 && args.Value.length <= 10)
        args.IsValid = true;
    else
        args.IsValid = false;
}

function validateLocation(source, args) {
    if (args.Value.length >= 1 && args.Value.length <= 50)
        args.IsValid = true;
    else
        args.IsValid = false;
}

function validateTurnaroundHours(source, args) {
    if (args.Value >= 0)
        args.IsValid = true;
    else
        args.IsValid = false;
}

// JScript File for TextArea
// Keep user from entering more than maxLength characters
function doKeypress(control) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength && value.length > maxLength - 1) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
    }
}
// Cancel default behavior
function doBeforePaste(control) {
    maxLength = control.attributes["maxLength"].value;
    if (maxLength) {
        event.returnValue = false;
    }
}
// Cancel default behavior and create a new paste routine
function doPaste(control) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
        var oTR = control.document.selection.createRange();
        var iInsertLength = maxLength - value.length + oTR.text.length;
        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
        oTR.text = sData;
    }
}

function printChart() {
    window.print();
}
//function pageLoad(sender, args) {
//    ResetPinControl();
//}


function Check_Click(objRef) {
    //Get the Row based on checkbox
    var row = objRef.parentNode.parentNode;
    if (objRef.checked) {
        //If checked change color to Aqua
        row.style.backgroundColor = "lightsteelblue";
    }
    else {
        //If not checked change back to original color
        if (row.rowIndex % 2 == 0) {
            //Alternating Row Color
            row.style.backgroundColor = "#C2D69B";
        }
        else {
            row.style.backgroundColor = "white";
        }
    }

    //Get the reference of GridView
    var GridView = row.parentNode;

    //Get all input elements in Gridview
    var inputList = GridView.getElementsByTagName("input");

    for (var i = 0; i < inputList.length; i++) {
        //The First element is the Header Checkbox
        var headerCheckBox = inputList[0];

        //Based on all or none checkboxes
        //are checked check/uncheck Header Checkbox
        var checked = true;
        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
            if (!inputList[i].checked) {
                checked = false;
                break;
            }
        }
    }
    headerCheckBox.checked = checked;

}


function checkAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        //Get the Cell To find out ColumnIndex
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                //If the header checkbox is checked
                //check all checkboxes
                //and highlight all rows
                row.style.backgroundColor = "lightsteelblue";
                inputList[i].checked = true;
            }
            else {
                //If the header checkbox is checked
                //uncheck all checkboxes
                //and change rowcolor back to original 
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "#C2D69B";
                }
                else {
                    row.style.backgroundColor = "white";
                }
                inputList[i].checked = false;
            }
        }
    }
}

function checkAllNoStyle(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        //Get the Cell To find out ColumnIndex
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                //If the header checkbox is checked
                //check all checkboxes
                //and highlight all rows
                inputList[i].checked = true;
            }
            else {
                //If the header checkbox is checked
                //uncheck all checkboxes
                //and change rowcolor back to original 
                inputList[i].checked = false;
            }
        }
    }
}

function MouseEvents(objRef, evt) {
    var checkbox = objRef.getElementsByTagName("input")[0];
    if (evt.type == "mouseover") {
        objRef.style.backgroundColor = "orange";
    }
    else {
        if (checkbox.checked) {
            objRef.style.backgroundColor = "lightsteelblue";
        }
        else if (evt.type == "mouseout") {
            if (objRef.rowIndex % 2 == 0) {
                //Alternating Row Color
                objRef.style.backgroundColor = "#C2D69B";
            }
            else {
                objRef.style.backgroundColor = "white";
            }

        }
    }
}
