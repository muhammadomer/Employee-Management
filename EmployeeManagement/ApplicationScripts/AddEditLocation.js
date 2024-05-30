var selectedUserData = null;
var locationInfo = null;
//var validationExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}/;
$(document).ready(function () {
    $("#offices").addClass("active");
    GetAllRegions(function () {
        if (QueryString("id") !== null) {
            $("#btn_AddEditLocation").text("Update");
            $("#addEditHeading").text("Edit Office");
            GetLocationInfoByID(QueryString("id"));
        }
        else {
            $("#btn_AddEditLocation").text("Add");
        }
    });
    // makeTextBoxAutoComplete("Nice Name", "country", false);

    //////////////// User Add Buttons Events ////////////////////
    $("#btn_AddEditLocation").on("click", function () {
        var btnUser = $("#btn_AddEditLocation").text();
        if (LocationFormValidation()) {
            if (btnUser === "Add") {
                AddLocation();
            }
            else if (btnUser === "Update") {
                EditLocation();
            }
        }
    });

    $("#ddlRegion").on("change", function () {
        var url = 'SystemSettings.aspx/GetCountriesOfRegion';
        var regionID = $(this).val();
        regionID = regionID == null ? 0 : regionID
        if (regionID == '') {
            $("#ddlCountry").empty();
            $("#ddlCountry").append("<option value=''>--- Select Country ---</option>");
            $("#ddlCountry").trigger("change");
            return;
        }

        var requestPerameters = {
            "regionID": regionID
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            $("#ddlCountry").empty();
            $("#ddlCity").empty();
            $("#ddlCountry").append("<option value=''>--- Select Country ---</option>");
            if (response != null) {
                for (var i = 0; i < response.length; i++) {
                    $("#ddlCountry").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].CountryName) + "</option>");
                }
                if (locationInfo != null) {
                    $("#ddlCountry").val(locationInfo.CountryID);                    
                }
            }
            $("#ddlCountry").trigger("change");
        });
    });
    $("#ddlCountry").on("change", function () {
        var url = 'SystemSettings.aspx/GetCitiesOfCountry';
        var countryID = $(this).val();
        countryID = countryID == null ? 0 : countryID
        console.log("countryID");
        console.log(countryID);
        if (countryID == '') {
            $("#ddlCity").empty();
            $("#ddlCity").append("<option value=''>--- Select City ---</option>");
            return;
        }

        var requestPerameters = {
            "countryID": countryID
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            $("#ddlCity").empty();
            $("#ddlCity").append("<option value=''>--- Select City ---</option>");
            if (response != null) {               

                for (var i = 0; i < response.length; i++) {
                    $("#ddlCity").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].CityName) + "</option>");
                }
                if (locationInfo != null)
                    $('#ddlCity').val(locationInfo.CityID);
            }
        });
    });
});

GetAllRegions = function (callback) {
    var url = 'SystemSettings.aspx/GetAllRegions';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {
        if (response != null) {
            $("#ddlRegion").empty().append("<option value=''>--- Select Region ---</option>");
            for (var i = 0; i < response.length; i++) {
                $("#ddlRegion").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].RegionName) + "</option>");
            }
            $("#ddlRegion").trigger("change");
        }
        if (callback != null) {
            callback();
        }
    });
};

//////////// Location CRUD Operation  ////////////////

AddLocation = function () {

    var locationName = $("#locationName").val();
    var state = $('#state').val();
    var gpsPostal = $("#gpsPostal").val();
    var mailPostal = $("#mailPostal").val();
    var addressLine1 = $("#addressLine1").val();
    var addressLine2 = $("#addressLine2").val();
    var telephone = $("#telephone").val();

    var checkLocationName = htmlTagsValidation(locationName);
    var checkState = htmlTagsValidation(state);
    var checkGPSPostal = htmlTagsValidation(gpsPostal);
    var checkMailPostal = htmlTagsValidation(mailPostal);
    var checkAddressLine1 = htmlTagsValidation(addressLine1);
    var checkAddressLine2 = htmlTagsValidation(addressLine2);
    var checkTelephone = htmlTagsValidation(telephone);

    if (checkLocationName == false && checkState == false && checkGPSPostal == false && checkMailPostal == false && checkAddressLine1 == false && checkAddressLine2 == false && checkTelephone == false) {

        var regionID = $("#ddlRegion").val();
        var countryID = $("#ddlCountry").val();
        var cityID = $('#ddlCity').val();
        var office = $("#locationName").val();
        var longitude = $("#longitude").val();
        var latitude = $("#latitude").val();


        var url = 'AddEditOffice.aspx/AddLocation';
        var locationData = {
            "Name": locationName,
            "RegionID": regionID,
            "CountryID": countryID,
            "State": state,
            "CityID": cityID,
            "Office": office,
            "GPSPostal": gpsPostal,
            "MailPostal": mailPostal,
            "AddressLine1": addressLine1,
            "AddressLine2": addressLine2,
            "Longitude": longitude,
            "Latitude": latitude,
            "Telephone": telephone
        };
        var requestPerameters = {
            "location": locationData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Office inserted successfully.') {
                toastr.success(response);
                ClearForm();
            }
            else {
                toastr.error(response);
            }
        });
    }
    else {
        toastr.error(specialCharacterError);
    }
    
};
EditLocation = function () {

    var locationName = $("#locationName").val();
    var state = $('#state').val();
    var gpsPostal = $("#gpsPostal").val();
    var mailPostal = $("#mailPostal").val();
    var addressLine1 = $("#addressLine1").val();
    var addressLine2 = $("#addressLine2").val();
    var telephone = $("#telephone").val();

    var checkLocationName = htmlTagsValidation(locationName);
    var checkState = htmlTagsValidation(state);
    var checkGPSPostal = htmlTagsValidation(gpsPostal);
    var checkMailPostal = htmlTagsValidation(mailPostal);
    var checkAddressLine1 = htmlTagsValidation(addressLine1);
    var checkAddressLine2 = htmlTagsValidation(addressLine2);
    var checkTelephone = htmlTagsValidation(telephone);

    if (checkLocationName == false && checkState == false && checkGPSPostal == false && checkMailPostal == false && checkAddressLine1 == false && checkAddressLine2 == false && checkTelephone == false) {

        var url = 'AddEditOffice.aspx/EditLocation';

        var locationID = $("#locationID").val();
        var regionID = $("#ddlRegion").val();
        var countryID = $("#ddlCountry").val();
        var cityID = $('#ddlCity').val();
        var office = $("#locationName").val();
        var longitude = $("#longitude").val();
        var latitude = $("#latitude").val();

        var locationData = {
            "ID": locationID,
            "Name": locationName,
            "RegionID": regionID,
            "CountryID": countryID,
            "State": state,
            "CityID": cityID,
            "Office": office,
            "GPSPostal": gpsPostal,
            "MailPostal": mailPostal,
            "AddressLine1": addressLine1,
            "AddressLine2": addressLine2,
            "Longitude": longitude,
            "Latitude": latitude,
            "Telephone": telephone
        };
        var requestPerameters = {
            "location": locationData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Office updated successfully.') {
                toastr.success(response);
            }
            else {
                toastr.error(response);
            }
        });

    }
    else {
        toastr.error(specialCharacterError);
    }
    
};

ClearForm = function () {
    $("#locationName").val('');
    $("#ddlRegion").val('');
    $("#ddlCountry").val('');
    $('#state').val('');
    $('#ddlCity').val('');
    $("#locationName").val('');
    $("#gpsPostal").val('');
    $("#mailPostal").val('');
    $("#addressLine1").val('');
    $("#addressLine2").val('');
    $("#longitude").val('');
    $("#latitude").val('');
    $("#telephone").val('');
};

LocationFormValidation = function () {
    //var regForLongLat = new RegExp("^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}");
    var locationName = $("#locationName").val();
    var regionID = $("#ddlRegion").val();
    var countryID = $("#ddlCountry").val();
    var state = $('#state').val();
    var cityID = $('#ddlCity').val();
    //var office = $("#office").val();
    var gpsPostal = $("#gpsPostal").val();
    var mailPostal = $("#mailPostal").val();
    var addressLine1 = $("#addressLine1").val();
    var addressLine2 = $("#addressLine2").val();
    var longitude = $("#longitude").val();
    var latitude = $("#latitude").val();
    var telephone = $("#telephone").val();
    if (locationName === '') {
        toastr.error('Please provide office.');
        return false;
    }
    else if (regionID === '' || regionID === null) {
        toastr.error('Please provide region.');
        return false;
    }
    else if (countryID === '' || countryID === null) {
        toastr.error('Please provide country.');
        return false;
    }
    //else if (state === '') {
    //    toastr.error('Please provide state/county/province.');
    //    return false;
    //}
    else if (cityID === '' || cityID === null) {
        toastr.error('Please provide city.');
        return false;
    }
    //else if (office === '') {
    //    toastr.error('Please provide office.');
    //    return false;
    //}
    //else if (gpsPostal === '') {
    //    toastr.error('Please provide GPS postal/zip code.');
    //    return false;
    //}
    else if (mailPostal === '') {
        toastr.error('Please provide mail postal/zip code.');
        return false;
    }
    else if (addressLine1 === '') {
        toastr.error('Please provide address line 1.');
        return false;
    }
    //else if (longitude === '') {
    //    toastr.error('Please provide longitude.');
    //    return false;
    //}
    //else if (!ValidateLongLat(longitude)) {
    //    toastr.error('Invalid longitude.');
    //    return false;
    //}
    //else if (latitude === '') {
    //    toastr.error('Please provide latitude.');
    //    return false;
    //}
    //else if (!ValidateLongLat(latitude)) {
    //    toastr.error('Invalid latitude.');
    //    return false;
    //}
    //else if (addressLine2 === '') {
    //    toastr.error('Please provide address line 2.');
    //    return false;
    //}
    else if (telephone === '') {
        toastr.error('Please provide telephone number.');
        return false;

    }
    else if (telephone.length > 18) {
        toastr.error('Telephone number is too large.');
        return false;
    }
    else if (!ValidateNumber(telephone)) {
        toastr.error('The telephone number provided is not valid.');
        return false;
    }
    return true;
};
GetLocationInfoByID = function (id) {
    var url = 'AddEditOffice.aspx/GetLocationByID';
    var requestPerameters = {
        "locationID": id
    };
    AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
        locationInfo = response;
        if (response !== null) {
            $("#locationID").val(response.ID);
            $("#ddlRegion").val(response.RegionID);
            $("#ddlRegion").trigger("change");
            $("#locationName").val(response.Name);
            //$("#ddlCountry").val(response.CountryID);
            //$("#ddlCountry").trigger("change");
            $('#state').val(response.State);
            //$('#ddlCity').val(response.CityID);
            $("#office").val(response.Office);
            $("#gpsPostal").val(response.GPSPostal);
            $("#mailPostal").val(response.MailPostal);
            $("#addressLine1").val(response.AddressLine1);
            $("#addressLine2").val(response.AddressLine2);
            $("#longitude").val(response.Longitude);
            $("#latitude").val(response.Latitude);
            $("#telephone").val(response.Telephone);
        }
    });
};
QueryString = function (key) {
    key = key.replace(/[*+?^$.\[\]{}()|\\\/]/g, "\\$&"); // escape RegEx control chars
    var match = location.search.match(new RegExp("[?&]" + key + "=([^&]+)(&|$)"));
    return match && decodeURIComponent(match[1].replace(/\+/g, " "));
};