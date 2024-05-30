
var specialCharacterError = "Special characters are not allowed.";

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlTagsValidation(string) {
    
    var regex = /(<([^>]+)>)/ig;
    
    var exist = regex.test(string);
   
    return exist;
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

// validate phone no with + sign
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 43 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function encData(data) {
    var d = new Date();
    var encString = d.getSeconds() + ">" + data + ">" + d.getTime();
    return btoa(encString);
}
