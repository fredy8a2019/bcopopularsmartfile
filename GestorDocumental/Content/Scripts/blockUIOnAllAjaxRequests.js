
function Prueba() {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onRequestEnd)
}


function onRequestStart() {
    $.blockUI();
}

function onRequestEnd() {
    $.unblockUI();
} 
