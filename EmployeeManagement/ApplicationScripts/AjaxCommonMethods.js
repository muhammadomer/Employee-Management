
AjaxPostRequestWithRequestPerameters = function (url, requestPerameters, callbcak) {
    
    var dataToReturn = null;

    var token = $('[name=__RequestVerificationToken]').val();

    try {
        $.ajax({
            type: "POST",
            url: url,
            headers: { '__RequestVerificationToken': token },
            data: JSON.stringify(requestPerameters),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    if (data.d != '' && data.d != null && data.d != "null") {
                        dataToReturn = data.d;
                        /*if (data.d.length > 0) {
                            dataToReturn = data.d;
                        }*/
                    }

                }
                callbcak(dataToReturn);
            },
            error: function (xhr, textStatus, errorThrown) {

                if (xhr.getResponseHeader("X-Responded-JSON") != null
                    && JSON.parse(xhr.getResponseHeader("X-Responded-JSON")).status == "401") {
                    console.log("Error 401 found");
                    window.location.href = "WarningPage.aspx";
                    return;
                }

                console.log('Error In Ajax Call');
                console.log('Status: ' + textStatus);
                console.log('Error: ' + errorThrown);
                window.location.href = "WarningPage.aspx";
                callbcak(dataToReturn);
            }
        });
    }
    catch (e) {
        console.log('Error In AjaxPostRequestWithRequestPerameters: ' + e.message);
        $().toastmessage("showErrorToast", "Sorry, an unexpected error occurred.");
        return null;
    }
};

AjaxPostRequestWithoutRequestPerameters = function (url, callbcak) {
    
    var dataToReturn = null;

    var token = $('[name=__RequestVerificationToken]').val();

    try {
        $.ajax({
            type: "POST",
            url: url,
            headers: { '__RequestVerificationToken': token },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d != '' && data.d != null && data.d != "null") {
                    
                    //if (data.d.length > 0) {
                    
                    dataToReturn = data.d;
                    //}
                }
                
                callbcak(dataToReturn);
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.getResponseHeader("X-Responded-JSON") != null
                    && JSON.parse(xhr.getResponseHeader("X-Responded-JSON")).status == "401") {
                    console.log("Error 401 found");
                    window.location.href = "WarningPage.aspx";
                    return;
                }

                console.log('Error In Ajax Call');
                console.log('Status: ' + textStatus);
                console.log('Error: ' + errorThrown);
                window.location.href = "WarningPage.aspx";
                callbcak(dataToReturn);
            }
        });
    }
    catch (e) {
        console.log('Error In AjaxPostRequestWithoutRequestPerameters: ' + e.message);
        $().toastmessage("showErrorToast", "Sorry, an unexpected error occurred.");
        return null;
    }
}

function isUserAborted(xhr) {
    return !xhr.getAllResponseHeaders();
}


function setHeartbeat() {
    setInterval("heartbeat()", 5 * 60 * 1000); // every 5 min
}

function heartbeat() {
    $.get(
        "/SessionHeartbeat.ashx",
        null,
        function (data) {
            setHeartbeat();
        },
        "json"
    );
}