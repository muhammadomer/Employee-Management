var globalCountryID = null;
var switchState = null;
var pageLoad = null;
var regionTable = null;
var countryTable = null;
var cityTable = null;
var departmentTable = null;
var practiceGroupTable = null;
$(document).ready(function () {
    $("#settings").addClass("active");
    GetAllRegions();
    GetAllCountries();
    GetAllCountriesList();
    GetAllCities();
    GetAllDepartments();
    GetAllPracticeGroups();
    GetGeneralSettings();
    var options = {};
    options.common = {
        minChar: 8
    };
    options.rules = {
        activated: {
            wordTwoCharacterClasses: true,
            wordRepetitions: true,
            wordUppercase: true,
        },
        scores: { wordUppercase: 5 }
    }
    options.ui = {
        //showErrors: true,
        //showStatus: true
    };
    $('#newEncryptedPassword').pwstrength(options);
    $("#btnChangeEncryptedPassword").on("click", function () {
        var oldPassword = $("#oldEncryptedPassword").val();
        var newPassword = $("#newEncryptedPassword").val();
        var confirmNewPassword = $("#confirmNewEncryptedPassword").val();
        var passwordValidation = validateUserPasswordWithConfirmPassword(newPassword, confirmNewPassword, 'NULL');
        if (oldPassword == '' && !$("#oldEncryptedPasswordDiv").hasClass("hidden")) {
            toastr.error("Please provide Old Password");
            return false;
        }
        else if (newPassword == '') {
            toastr.error("Please provide New Password");
            return false;
        }
        else if (confirmNewPassword == '') {
            toastr.error("Please provide Confirm Password");
            return false;
        }
        else if (newPassword != confirmNewPassword) {
            toastr.error("New and Old Password should be same");
            return false;
        }
        else if (passwordValidation != 'true') {
            toastr.error(passwordValidation);
            return false;
        }
        var changePasswordURL = "SystemSettings.aspx/ChangeEncryptedPassword";
        var requestPerameters = {
            "oldPassword": oldPassword,
            "newPassword": newPassword,
            "confirmNewPassword": confirmNewPassword
        };
        AjaxPostRequestWithRequestPerameters(changePasswordURL, requestPerameters, function (response) {
            if (response == "1") {
                toastr.success("Password changed sucessfully");
                $("#ModalChangeEncryptedPassword").modal("hide");
            }
            else if (response == "2") {
                toastr.error("Please provide correct Old Password");
            }
            else if (response == "3") {
                toastr.error("Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.");
            }
            else if (response == "4") {
                toastr.error("The passwords you supplied do not match.");
            }
            else if (response == "5") {
                toastr.error("Please provide all manadatory fields.");
            }
            else {
                toastr.error("Failed to update settings.");
            }
        });
    });
    $('#ModalChangeEncryptedPassword').on('hidden.bs.modal', function () {
        $("#oldEncryptedPassword").val('');
        $("#newEncryptedPassword").val('');
        $("#confirmNewEncryptedPassword").val('');
    });
    // Click Event for Add Edit Region, Country, City, Department

    $("#btnAddEditRegion").on("click", function () {
        var regionName = $("#txbRegionName").val();
        if (regionName == '') {
            toastr.error("Please provide region name.");
            return false;
        }
        if ($("#btnAddEditRegion").text() == "Save") {
            AddRegion();
        }
        else if ($("#btnAddEditRegion").text() == "Update") {
            UpdateRegion();
        }
    });

    $("#ddlCountriesForAddEditCountry").change(function () {
        var countryName = $("#ddlCountriesForAddEditCountry").val();
       
        if (countryName == 'XX') {
          
            $('#txbCountryName').removeClass('hidden');

        }
        else {
            $('#txbCountryName').addClass('hidden');
        }
        
});

    $("#btnAddEditCountry").on("click", function () {
        var regionID = $("#ddlRegionsForAddEditCountry").val();


        var countryName = $("#ddlCountriesForAddEditCountry").val();

      

        //var countryName = $("#txbCountryName").val();
        if (regionID == '' || regionID ==0 ) {
            toastr.error("Please provide region name.");
            return false;
        }
        else if (countryName == '' || countryName ==0) {
            toastr.error("Please provide country name.");
            return false;
        }

      
        if ($("#btnAddEditCountry").text() == "Save") {
            AddCountry();
        }
        else if ($("#btnAddEditCountry").text() == "Update") {
            UpdateCountry();
        }
    });

    $("#btnAddEditCity").on("click", function () {
        var regionID = $("#ddlRegionsForAddEditCity").val();
        var countryID = $("#ddlCountriesForAddEditCity").val();
        var cityName = $("#txbCityName").val();
        if (regionID == '') {
            toastr.error("Please provide region name.");
            return false;
        }
        else if (countryID == '' || countryID==0) {
            toastr.error("Please provide country name.");
            return false;
        }
        else if (cityName == '') {
            toastr.error("Please provide city name.");
            return false;
        }
        if ($("#btnAddEditCity").text() == "Save") {
            AddCity();
        }
        else if ($("#btnAddEditCity").text() == "Update") {
            UpdateCity();
        }
    });

    $("#btnAddEditDepartment").on("click", function () {
        var departmentName = $("#txbDepartmentName").val();
        if (departmentName == '') {
            toastr.error("Please provide department name.");
            return false;
        }
        if ($("#btnAddEditDepartment").text() == "Save") {
            AddDepartment();
        }
        else if ($("#btnAddEditDepartment").text() == "Update") {
            UpdateDepartment();
        }
    });

    $("#btnAddEditPracticeGroup").on("click", function () {
        var practicegroupName = $("#txbPracticeGroupName").val();
        if (practicegroupName == '') {
            toastr.error("Please provide Practice Group name.");
            return false;
        }
        if ($("#btnAddEditPracticeGroup").text() == "Save") {
            AddPracticeGroup();
        }
        else if ($("#btnAddEditPracticeGroup").text() == "Update") {
            UpdatePracticeGroup();
        }
    });

    $("#btnSaveSMTPSettings").on("click", function () {

        var mailServer = $("#txtMailServer").val();
        var mailUsername = $("#txtMailUsername").val();
        var mailPassword = $("#txtMailPassword").val();

        var checkMailServer = htmlTagsValidation(mailServer);
        var checkMailUsername = validateEmail(mailUsername);
        var checkMailPassword = htmlTagsValidation(mailPassword);

        if (checkMailServer == false && checkMailUsername == true && checkMailPassword == false) {

            var smtpPort = $("#txtSMTPPort").val();

            var enableSSL = $('#ckbEnableSSL').bootstrapSwitch('state');
            if (mailServer == '') {
                toastr.error("Please provide outgoing mail server.");
                return false;
            }
            else if (smtpPort == '') {
                toastr.error("Please provide smtp port.");
                return false;
            }
            else if (mailUsername == '') {
                toastr.error("Please provide mail username.");
                return false;
            }
            else if (mailPassword == '') {
                toastr.error("Please provide mail password.");
                return false;
            }
            var url = 'SystemSettings.aspx/SaveSMTPSettings';
            var SMTPData = {
                "MailServer": mailServer,
                "SMTPPort": smtpPort,
                "MailUsername": mailUsername,
                "MailPassword": mailPassword,
                "EnableSSL": enableSSL
            };
            var requestPerameters = {
                "smtpSettings": SMTPData
            };
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("SMTP settings save successfuly");
                    GetGeneralSettings();
                }
                else {
                    toastr.error("Failed to save SMTP settings");
                }
            });

        }
        else {
            toastr.error("Invalid input.");
        }

    });

    $("#btnTestSMTPSettings").on("click", function () {

        var toEmail = $("#txbToEmail").val();

        var mailServer = $("#txtMailServer").val();
        var mailUsername = $("#txtMailUsername").val();
        var mailPassword = $("#txtMailPassword").val();

        var checkMailServer = htmlTagsValidation(mailServer);
        var checkMailUsername = validateEmail(mailUsername);
        var checkMailPassword = validateEmail(mailPassword);
        var validToEmail = validateEmail(toEmail);

        if (checkMailServer == false && checkMailUsername == true && checkMailPassword == false && validToEmail == true) {

            var smtpPort = $("#txtSMTPPort").val();

            var enableSSL = $('#ckbEnableSSL').bootstrapSwitch('state');
            if (toEmail == '') {
                toastr.error("Please provide email.");
                return false;
            }
            else if (mailServer == '') {
                toastr.error("Please provide outgoing mail server.");
                return false;
            }
            else if (smtpPort == '') {
                toastr.error("Please provide smtp port.");
                return false;
            }
            else if (mailUsername == '') {
                toastr.error("Please provide mail username.");
                return false;
            }
            else if (mailPassword == '') {
                toastr.error("Please provide mail password.");
                return false;
            }
            var url = 'SystemSettings.aspx/TestSMTPSettings';
            var SMTPData = {
                "Host": mailServer,
                "Port": smtpPort,
                "FromAddress": mailUsername,
                "Password": mailPassword,
                "EnableSSL": enableSSL
            };
            var requestPerameters = {
                "toEmail": toEmail,
                "smtpSettings": SMTPData
            };
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {

                if (response === "Sent email successfuly") {
                    toastr.success(response);
                    $("#ModalTestEmail").modal("hide");
                }
                else {
                    toastr.error(response);
                }
            });

        }
        else {
            toastr.error("Invalid input.");
        }

    });

    $("#ddlRegionsForAddEditCity").on("change", function () {

       
        var url = 'SystemSettings.aspx/GetCountriesOfRegion';
        var regionID = $(this).val();
      
        var requestPerameters = {
            "regionID": regionID
        };
        $("#ddlCountriesForAddEditCity").empty();
        $("#ddlCountriesForAddEditCity").append("<option value=0>--- Select Country ---</option>");
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response != null) {
                for (var i = 0; i < response.length; i++) {
                    $("#ddlCountriesForAddEditCity").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].CountryName) + "</option>");
                }
                if (globalCountryID != null) {
                    $("#ddlCountriesForAddEditCity").val(globalCountryID);
                    globalCountryID = null;
                }
            }
        });
    });

    $('#ckbTwoFAAuthentication').on('switchChange.bootstrapSwitch', function (event, state) {

        if (state && switchState == null && pageLoad == null) {
            ShowConfirmationDialog('Enable 2FA', 'Are you sure want to enable 2FA?', function () {
                var url = 'SystemSettings.aspx/EnableDisableTwoFA';
                var requestPerameters = {
                    "state": state
                };
                AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                    if (response) {
                        toastr.success("2FA enabled successfully.");
                        $("#btnEnableAuthentication").removeClass("hidden");
                        switchState = null;
                    }
                    else {
                        toastr.error("Failed to enable 2FA");
                        switchState = "123";
                        $("#ckbTwoFAAuthentication").bootstrapSwitch('state', false);
                    }
                });
            }, function () {
                switchState = "123";
                $("#ckbTwoFAAuthentication").bootstrapSwitch('state', false);
            }, 'Yes');
        }
        else if (!state && switchState == null && pageLoad == null) {
            ShowConfirmationDialog('Disable 2FA', 'Are you sure want to disable 2FA?', function () {
                var url = 'SystemSettings.aspx/EnableDisableTwoFA';
                var requestPerameters = {
                    "state": state
                };
                AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                    if (response) {
                        toastr.success("2FA disabled successfully.");
                        $("#btnEnableAuthentication").addClass("hidden");
                        switchState = null;
                    }
                    else {
                        toastr.error("Failed to disable 2FA");
                        switchState = "123";
                        $("#ckbTwoFAAuthentication").bootstrapSwitch('state', true);
                    }
                });
            }, function () {
                switchState = "123";
                $("#ckbTwoFAAuthentication").bootstrapSwitch('state', true);
            }, 'Yes');

        }
        else {
            switchState = null;
            pageLoad = null;
        }
    });

    // Close event from All popup

    $('#AddEditRegionModal').on('hidden.bs.modal', function () {
        $("#txbRegionName").val('');
        $("#btnAddEditRegion").text("Save");
    });

    $('#AddEditCountryModal').on('hidden.bs.modal', function () {
        $("#ddlRegionsForAddEditCountry").val(0).trigger("change");
        $("#ddlCountriesForAddEditCountry").val(0);
        //$("#txbCountryName").val('');
        $("#btnAddEditCountry").text("Save");
        $('#txbCountryName').addClass('hidden');
    });

    $('#AddEditCityModal').on('hidden.bs.modal', function () {
        $("#ddlRegionsForAddEditCity").val(0).trigger("change");
        $("#txbCityName").val('');
        $("#btnAddEditCity").text("Save");
    });

    $('#AddEditDepartmentModal').on('hidden.bs.modal', function () {
        $("#txbDepartmentName").val('');
        $("#btnAddEditDepartment").text("Save");
    });

    $('#AddEditPracticeGroupModal').on('hidden.bs.modal', function () {
        $("#txbPracticeGroupName").val('');
        $("#btnAddEditPracticeGroup").text("Save");
    });

    $('#regionsTable').on('draw.dt', function () {
        $("#regionsTable").find(".odd").removeClass();
        $("#regionsTable").find(".even").removeClass();
    });

    $('#countriesTable').on('draw.dt', function () {
        $("#countriesTable").find(".odd").removeClass();
        $("#countriesTable").find(".even").removeClass();
    });

    $('#citiesTable').on('draw.dt', function () {
        $("#citiesTable").find(".odd").removeClass();
        $("#citiesTable").find(".even").removeClass();
    });

    $('#departmentsTable').on('draw.dt', function () {
        $("#departmentsTable").find(".odd").removeClass();
        $("#departmentsTable").find(".even").removeClass();
    });

    $('#practiceGroupTable').on('draw.dt', function () {
        $("#practiceGroupTable").find(".odd").removeClass();
        $("#practiceGroupTable").find(".even").removeClass();
    });
    $("#btnImageUpload").click(function () {
        $.FileDialog({ multiple: false }).on('files.bs.filedialog', function (ev) {
            var files = ev.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
                //alert(files[i].height);
            }
            var token = $('[name=__RequestVerificationToken]').val();
            $.ajax({
                url: "ImageUploadHandler.ashx",
                headers: { '__RequestVerificationToken': token },
                type: "POST",
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger;
                    if (result == "true") {
                        location.reload();
                    }
                    else {
                        toastr.error("showErrorToast", "Please provide jpg, jpeg and png images only.");
                    }

                },
                error: function (xhr, textStatus, errorThrown) {

                    if (isUserAborted(xhr)) {
                        return;
                    }

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

                }
                //error: function (err) {
                //    alert(err.statusText)
                //}
            });
        }).on('cancel.bs.filedialog', function (ev) {
            //alert("Cancelled!");
        });
    });
    GetCompanyDetail();
});
GetCompanyDetail = function () {
    var url = 'SystemSettings.aspx/GetReportLogoPath';
    AjaxPostRequestWithoutRequestPerameters(url, function (companyLogoPath) {
        console.log("Gor Response From Ajax");
        console.log(companyLogoPath);
        logoPath = companyLogoPath[0];
        $("#txbCompanyName").val(companyLogoPath[1]);
        $("#txbCompanyAddress").val(companyLogoPath[2]);

        var option = "";

        debugger;


        if (companyLogoPath[3] == "$") {
            option += '<option value=$ selected >  Dollar  </option>';
            option += '<option value=£  >  Pound  </option>';
            option += '<option value=€  >  Euro  </option>';
           

        }

        if (companyLogoPath[3] == "£") {
            option += '<option value=$  >  Dollar  </option>';
            option += '<option value=£  selected>  Pound  </option>';
            option += '<option value=€  >  Euro  </option>';


        }
        if (companyLogoPath[3] == "€") {
            option += '<option value=$  >  Dollar  </option>';
            option += '<option value=£  >  Pound  </option>';
            option += '<option value=€  selected>  Euro  </option>';


        }

        $("#ddlCurrency").append(option);

        $('.logo-image').attr("src", "data:image/jpg;base64," + logoPath); // Set Path on System Page Image Box
    });
}

GetAllCountriesList = function () {
    var url = 'SystemSettings.aspx/GetAllCountriesList';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if (response != null) {
            $("#ddlCountriesForAddEditCountry").empty().append("<option value=0>--- Select Country ---</option>");
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                $("#ddlCountriesForAddEditCountry").append("<option value='" + response[i].Code + "'>" + htmlEncode(response[i].CountryName) + "</option>");
            }
        }
      
    });
};


GetAllRegions = function () {
    var url = 'SystemSettings.aspx/GetAllRegions';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if ($.fn.DataTable.isDataTable('#regionsTable')) {
            $('#regionsTable').DataTable().destroy();
        }
        $("#regionsTable").empty();
        $("#regionsTable").append(" <thead><tr><th class='SecondHeading'>REGION NAME</th><th class='SecondHeading' style = 'width:40px;'></th></tr></thead><tbody>");
        if (response != null) {
            $("#ddlRegionsForAddEditCountry").empty().append("<option value=0>--- Select Region ---</option>");
            $("#ddlRegionsForAddEditCity").empty().append("<option value=0>--- Select Region ---</option>");
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                $("#ddlRegionsForAddEditCountry").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].RegionName) + "</option>");
                $("#ddlRegionsForAddEditCity").append("<option value='" + response[i].ID + "'>" + htmlEncode(response[i].RegionName) + "</option>");
                if (response[i].RegionName != "Unassigned") {
                    tableHTML += "<tr><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td style='text-align: right;'><a class='edit-region' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Edit' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-region' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                }
                else {
                    tableHTML += "<tr><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td></td></tr >";
                }
            }
            tableHTML += "</tbody>";
            $("#regionsTable").append(tableHTML);
        }
        regionTable = $('#regionsTable').dataTable({
            destroy: true,
            "order": [],
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
                }
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            "pageLength": 5,
            //"bLengthChange": false,
            //"searching": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }
        });
        $("#regionsTable").find(".odd").removeClass();
        $("#regionsTable").find(".even").removeClass();
        $('[data-toggle="tooltip"]').tooltip();
        $("#regionsTable_length").empty();
        $("#regionsTable_length").append('<a class="btn purple" href="#AddEditRegionModal" data-toggle="modal" style="margin-left: 1px;" id="btnAddRegionModal">New<i class="fa fa-plus-circle" style="margin-left: 5px;"></i></a>');
    });
};

GetAllCountries = function () {
    var url = 'SystemSettings.aspx/GetAllCountries';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if ($.fn.DataTable.isDataTable('#countriesTable')) {
            $('#countriesTable').DataTable().destroy();
        }
        $("#countriesTable").empty();
        $("#countriesTable").append(" <thead><tr><th class='SecondHeading'>COUNTRY NAME</th><th class='SecondHeading'>REGION NAME</th><th class='SecondHeading' style = 'width:40px;'></th></tr></thead><tbody>");
        if (response != null) {
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                if (response[i].CountryName != "Unassigned") {
                    tableHTML += "<tr><td class='CountryName'>" + htmlEncode(response[i].CountryName) + "</td><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td style='text-align: right;'><a class='edit-country' data-id = '" + response[i].ID + "' data-reg-id = '" + response[i].RegionID + "' data-toggle='tooltip' data-placement='top' title='Edit' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-country' data-id = '" + response[i].ID + "' data-reg-id = '" + response[i].RegionID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                }
                else {
                    tableHTML += "<tr><td class='CountryName'>" + htmlEncode(response[i].CountryName) + "</td><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td></td></tr >";
                }
            }
            tableHTML += "</tbody>";
            $("#countriesTable").append(tableHTML);
        }
        countryTable = $('#countriesTable').dataTable({
            destroy: true,
            "order": [],
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
                }
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            "pageLength": 5,
            //"bLengthChange": false,
            //"searching": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }
        });
        $("#countriesTable").find(".odd").removeClass();
        $("#countriesTable").find(".even").removeClass();
        $('[data-toggle="tooltip"]').tooltip();

        $("#countriesTable_length").empty();
        $("#countriesTable_length").append('<a class="btn purple" href="#AddEditCountryModal" data-toggle="modal" style="margin-left: 1px;">New<i class="fa fa-plus-circle" style="margin-left: 5px;"></i></a>');
    });
};

GetAllCities = function () {
    var url = 'SystemSettings.aspx/GetAllCities';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if ($.fn.DataTable.isDataTable('#citiesTable')) {
            $('#citiesTable').DataTable().destroy();
        }
        $("#citiesTable").empty();
        $("#citiesTable").append(" <thead><tr><th class='SecondHeading'>CITY NAME</th><th class='SecondHeading'>COUNTRY NAME</th><th class='SecondHeading'>REGION NAME</th><th class='SecondHeading' style = 'width:40px;'></th></tr></thead><tbody>");
        if (response != null) {
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                if (response[i].CityName != "Unassigned") {
                    tableHTML += "<tr><td class='CityName'>" + htmlEncode(response[i].CityName) + "</td><td class='CountryName'>" + htmlEncode(response[i].CountryName) + "</td><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td style='text-align: right;'><a class='edit-city' data-reg-id = '" + response[i].RegionID + "' data-con-id = '" + response[i].CountryID + "' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Edit' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-city' data-reg-id = '" + response[i].RegionID + "' data-con-id = '" + response[i].CountryID + "' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                }
                else {
                    tableHTML += "<tr><td class='CityName'>" + htmlEncode(response[i].CityName) + "</td><td class='CountryName'>" + htmlEncode(response[i].CountryName) + "</td><td class='RegionName'>" + htmlEncode(response[i].RegionName) + "</td><td></td></tr >";
                }
            }
            tableHTML += "</tbody>";
            $("#citiesTable").append(tableHTML);
        }
        cityTable = $('#citiesTable').dataTable({
            destroy: true,
            "order": [],
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
                }
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            "pageLength": 5,
            //"bLengthChange": false,
            //"searching": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }
        });
        $("#citiesTable").find(".odd").removeClass();
        $("#citiesTable").find(".even").removeClass();
        $('[data-toggle="tooltip"]').tooltip();

        $("#citiesTable_length").empty();
        $("#citiesTable_length").append('<a class="btn purple" href="#AddEditCityModal" data-toggle="modal" style="margin-left: 1px;">New<i class="fa fa-plus-circle" style="margin-left: 5px;"></i></a>');
    });
};

GetAllDepartments = function () {
    var url = 'SystemSettings.aspx/GetAllDepartments';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if ($.fn.DataTable.isDataTable('#departmentsTable')) {
            $('#departmentsTable').DataTable().destroy();
        }
        $("#departmentsTable").empty();
        $("#departmentsTable").append(" <thead><tr><th class='SecondHeading'>DEPARTMENT NAME</th><th class='SecondHeading' style = 'width:40px;'></th></tr></thead><tbody>");
        if (response != null) {
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Name != "Practice Group") {
                    if (response[i].Name != "Unassigned") {
                        tableHTML += "<tr><td class='DepartmentName'>" + htmlEncode(response[i].Name) + "</td><td style='text-align: right;'><a class='edit-department' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Edit' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-department' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                    }
                    //else {
                    //    tableHTML += "<tr><td class='DepartmentName'>" + htmlEncode(response[i].Name) + "</td><td></td></tr >";
                    //}
                }
                else {
                    tableHTML += "<tr><td class='DepartmentName'>" + htmlEncode(response[i].Name) + "</td><td style='text-align: right;'><a ><i class='fa fa-pencil customIcon disabled-customIcon'></i></a></td></tr >";
                }

            }
            tableHTML += "</tbody>";
            $("#departmentsTable").append(tableHTML);
        }
        departmentTable = $('#departmentsTable').dataTable({
            destroy: true,
            "order": [],
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
                }
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            "pageLength": 5,
            //"bLengthChange": false,
            //"searching": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }
        });
        $("#departmentsTable").find(".odd").removeClass();
        $("#departmentsTable").find(".even").removeClass();
        $('[data-toggle="tooltip"]').tooltip();

        $("#departmentsTable_length").empty();
        $("#departmentsTable_length").append('<a class="btn purple" href="#AddEditDepartmentModal" data-toggle="modal" style="margin-left: 1px;">New<i class="fa fa-plus-circle" style="margin-left: 5px;"></i></a>');
    });
};

GetAllPracticeGroups = function () {
    var url = 'SystemSettings.aspx/GetAllPracticeGroups';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if ($.fn.DataTable.isDataTable('#practiceGroupTable')) {
            $('#practiceGroupTable').DataTable().destroy();
        }
        $("#practiceGroupTable").empty();
        $("#practiceGroupTable").append(" <thead><tr><th class='SecondHeading'>PRACTICE GROUP</th><th class='SecondHeading' style = 'width:40px;'></th></tr></thead><tbody>");
        if (response != null) {
            var tableHTML = '';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Name != "Unassigned") {
                    tableHTML += "<tr><td class='PracticeGroup'>" + htmlEncode(response[i].Name) + "</td><td style='text-align: right;'><a class='edit-practicegroup' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Edit' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-practicegroup' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                }
                //else {
                //    tableHTML += "<tr><td class='PracticeGroup'>" + htmlEncode(response[i].Name) + "</td><td></td></tr >";
                //}
            }
            tableHTML += "</tbody>";
            $("#practiceGroupTable").append(tableHTML);
        }
        practiceGroupTable = $('#practiceGroupTable').dataTable({
            destroy: true,
            "order": [],
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
                }
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            "pageLength": 5,
            //"bLengthChange": false,
            //"searching": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }
        });
        $("#practiceGroupTable").find(".odd").removeClass();
        $("#practiceGroupTable").find(".even").removeClass();
        $('[data-toggle="tooltip"]').tooltip();

        $("#practiceGroupTable_length").empty();
        $("#practiceGroupTable_length").append('<a class="btn purple" href="#AddEditPracticeGroupModal" data-toggle="modal" style="margin-left: 1px;">New<i class="fa fa-plus-circle" style="margin-left: 5px;"></i></a>');
    });
};

GetGeneralSettings = function () {
    var url = 'SystemSettings.aspx/GetGeneralSettings';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {
        
        $("#txtMailServer").val(response.MailServer);
        $("#txtSMTPPort").val(response.SMTPPort);
        $("#txtMailUsername").val(response.MailUsername);
        $("#txtMailPassword").val(response.MailPassword);
        $("#ckbEnableSSL").bootstrapSwitch('state', response.EnableSSL);
        if (!response.TwoFAEnable) {
            pageLoad = "No";
        }
        $("#ckbTwoFAAuthentication").bootstrapSwitch('state', response.TwoFAEnable);

        if (response.HashCode == null) {
            $("#oldEncryptedPasswordDiv").addClass("hidden");
        }
        else {
            $("#oldEncryptedPasswordDiv").removeClass("hidden");
        }
    });
};

AddRegion = function () {

    var regionName = $("#txbRegionName").val();

    var checkRegionName = htmlTagsValidation(regionName);

    if (checkRegionName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateRegion';

        var regionData = {
            "Name": regionName
        };
        var requestPerameters = {
            "region": regionData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Region inserted successfully.') {
                toastr.success(response);
                GetAllRegions();
                $("#AddEditRegionModal").modal("hide");
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

UpdateRegion = function () {

    var regionName = $("#txbRegionName").val();

    var checkRegionName = htmlTagsValidation(regionName);

    if (checkRegionName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateRegion';
        var regionID = $("#regionID").val();

        var regionData = {
            "ID": regionID,
            "Name": regionName
        };
        var requestPerameters = {
            "region": regionData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Region updated successfully.') {
                toastr.success(response);
                GetAllRegions();
                $("#AddEditRegionModal").modal("hide");
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

AddCountry = function () {

    var countryName = $("#ddlCountriesForAddEditCountry option:selected").text();
    if (countryName == 'Other Country') {
         countryName = $("#txbCountryName").val();
    }

    if (countryName == '') {
        toastr.error("Please provide country name.");
        return false;
    }


    var checkCountryName = htmlTagsValidation(countryName);

    if (checkCountryName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateCountry';
        var regionID = $("#ddlRegionsForAddEditCountry").val();
        var code = $("#ddlCountriesForAddEditCountry").val();

        var countryData = {
            "Name": countryName,
            "RegionID": regionID,
            "Code": code
        };
        var requestPerameters = {

            "country": countryData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Country inserted successfully.') {
                toastr.success(response);
                GetAllCountries();
                $("#AddEditCountryModal").modal("hide");
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

UpdateCountry = function () {

    var countryName = $("#ddlCountriesForAddEditCountry option:selected").text();
    //var countryName = $("#txbCountryName").val();

    var checkCountryName = htmlTagsValidation(countryName);

    if (checkCountryName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateCountry';
        var regionID = $("#ddlRegionsForAddEditCountry").val();
        var code = $("#ddlCountriesForAddEditCountry").val();
        var countryID = $("#countryID").val();

        var countryData = {
            "ID": countryID,
            "Name": countryName,
            "RegionID": regionID,
            "Code": code
        };
        var requestPerameters = {
            "country": countryData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Country updated successfully.') {
                toastr.success(response);
                GetAllCountries();
                $("#AddEditCountryModal").modal("hide");
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

AddCity = function () {

    var cityName = $("#txbCityName").val();

   

    var checkCityName = htmlTagsValidation(cityName);

    if (checkCityName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateCity';
        var countryID = $("#ddlCountriesForAddEditCity").val();

        var cityData = {
            "Name": cityName,
            "CountryID": countryID
        };
        var requestPerameters = {
            "city": cityData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'City inserted successfully.') {
                toastr.success(response);
                GetAllCities();
                $("#AddEditCityModal").modal("hide");
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

UpdateCity = function () {

    var cityName = $("#txbCityName").val();

    var checkCityName = htmlTagsValidation(cityName);

    if (checkCityName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateCity';
        var countryID = $("#ddlCountriesForAddEditCity").val();
        var cityID = $("#cityID").val();

        var cityData = {
            "ID": cityID,
            "Name": cityName,
            "CountryID": countryID
        };
        var requestPerameters = {
            "city": cityData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'City updated successfully.') {
                toastr.success(response);
                GetAllCities();
                $("#AddEditCityModal").modal("hide");
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

AddDepartment = function () {

    var departmentName = $("#txbDepartmentName").val();

    var checkDepartmentName = htmlTagsValidation(departmentName);

    if (checkDepartmentName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateDepartment';

        var departmentData = {
            "Name": departmentName
        };
        var requestPerameters = {
            "department": departmentData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Department name inserted successfully.') {
                toastr.success(response);
                GetAllDepartments();
                $("#AddEditDepartmentModal").modal("hide");
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

UpdateDepartment = function () {

    var departmentName = $("#txbDepartmentName").val();

    var checkDepartmentName = htmlTagsValidation(departmentName);

    if (checkDepartmentName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdateDepartment';
        var departmentID = $("#departmentID").val();

        var departmentData = {
            "ID": departmentID,
            "Name": departmentName
        };
        var requestPerameters = {
            "department": departmentData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Department name updated successfully.') {
                toastr.success(response);
                GetAllDepartments();
                $("#AddEditDepartmentModal").modal("hide");
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

AddPracticeGroup = function () {

    var practicegroupName = $("#txbPracticeGroupName").val();

    var checkPracticeGroupName = htmlTagsValidation(practicegroupName);

    if (checkPracticeGroupName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdatePracticeGroup';

        var practicegroupData = {
            "Name": practicegroupName
        };
        var requestPerameters = {
            "practicegroup": practicegroupData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Practice Group name inserted successfully.') {
                toastr.success(response);
                GetAllPracticeGroups();
                $("#AddEditPracticeGroupModal").modal("hide");
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

UpdatePracticeGroup = function () {

    var practicegroupName = $("#txbPracticeGroupName").val();

    var checkPracticeGroupName = htmlTagsValidation(practicegroupName);

    if (checkPracticeGroupName == false) {

        var url = 'SystemSettings.aspx/AddOrUpdatePracticeGroup';
        var practicegroupID = $("#PracticeGroupID").val();

        var practicegroupData = {
            "ID": practicegroupID,
            "Name": practicegroupName
        };
        var requestPerameters = {
            "practicegroup": practicegroupData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response === 'Practice Group name updated successfully.') {
                toastr.success(response);
                GetAllPracticeGroups();
                $("#AddEditPracticeGroupModal").modal("hide");
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

$(document).off('click', '.edit-region').on('click', '.edit-region', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var regionID = $(currentObject).attr("data-id");
        $("#regionID").val(regionID);
        $("#txbRegionName").val(currentObject.parent().prev().text());
        $("#btnAddEditRegion").text("Update");
        $("#AddEditRegionModal").modal("show");

    } catch (e) {
        console.log('Exception while trying to edit an region: ' + e.message);
    }
});

$(document).off('click', '.edit-country').on('click', '.edit-country', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var regionID = $(currentObject).attr("data-reg-id");
        var countryID = $(currentObject).attr("data-id");
        $("#ddlRegionsForAddEditCountry").val(regionID);
        $("#countryID").val(countryID);

        $('#ddlCountriesForAddEditCountry').prop('selected', false);
       
        $("#ddlCountriesForAddEditCountry option").each(function () {
            if ($(this).html() == currentObject.parent().prev().prev().text()) {
                $(this).prop('selected', true);
                return;
            }
        });

        
        //$("#ddlCountriesForAddEditCountry").text(currentObject.parent().prev().prev().text()).attr('selected', 'selected');
        //$("#txbCountryName").val(currentObject.parent().prev().prev().text());
        $("#btnAddEditCountry").text("Update");
        $("#AddEditCountryModal").modal("show");

    } catch (e) {
        console.log('Exception while trying to edit an country: ' + e.message);
    }
});

$(document).off('click', '.edit-city').on('click', '.edit-city', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var regionID = $(currentObject).attr("data-reg-id");
        var countryID = $(currentObject).attr("data-con-id");
        globalCountryID = countryID;
        var cityID = $(currentObject).attr("data-id");
        $("#ddlRegionsForAddEditCity").val(regionID);
        $("#ddlRegionsForAddEditCity").trigger("change");
        $("#cityID").val(cityID);
        $("#txbCityName").val(currentObject.parent().prev().prev().prev().text());
        $("#btnAddEditCity").text("Update");
        $("#AddEditCityModal").modal("show");
        //$("#ddlCountriesForAddEditCity").val(countryID);

    } catch (e) {
        console.log('Exception while trying to edit an city: ' + e.message);
    }
});

$(document).off('click', '.edit-department').on('click', '.edit-department', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var departmentID = $(currentObject).attr("data-id");
        $("#departmentID").val(departmentID);
        $("#txbDepartmentName").val(currentObject.parent().prev().text());
        $("#btnAddEditDepartment").text("Update");
        $("#AddEditDepartmentModal").modal("show");
    } catch (e) {
        console.log('Exception while trying to edit an department: ' + e.message);
    }
});

$(document).off('click', '.edit-practicegroup').on('click', '.edit-practicegroup', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var practicegroupID = $(currentObject).attr("data-id");
        $("#PracticeGroupID").val(practicegroupID);
        $("#txbPracticeGroupName").val(currentObject.parent().prev().text());
        $("#btnAddEditPracticeGroup").text("Update");
        $("#AddEditPracticeGroupModal").modal("show");
    } catch (e) {
        console.log('Exception while trying to edit an practice group: ' + e.message);
    }
});

$(document).off('click', '.delete-region').on('click', '.delete-region', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var regionID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All offices containing the ' + $(currentObject).parents("tr").find(".RegionName").text() + ' region, will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "regionID": regionID
            };
            var url = 'SystemSettings.aspx/DeleteRegionByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Region Deleted successfully.");
                    $('#regionsTable').DataTable()
                        .row(currentObject.parents('tr'))
                        .remove()
                        .draw();
                    GetAllCountries();
                    GetAllCountriesList();
                    GetAllCities();
                }
                else {
                    toastr.error("Failed to delete this region.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an region: ' + e.message);
    }
});

$(document).off('click', '.delete-country').on('click', '.delete-country', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var regionID = $(currentObject).attr("data-reg-id");
        var countryID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All offices containing the ' + $(currentObject).parents("tr").find(".CountryName").text() + ' country, will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "countryID": countryID,
                "regionID": regionID
            };
            var url = 'SystemSettings.aspx/DeleteCountryByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Country Deleted successfully.");
                    $('#countriesTable').DataTable()
                        .row(currentObject.parents('tr'))
                        .remove()
                        .draw();
                    GetAllCities();
                }
                else {
                    toastr.error("Failed to delete this country.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an country: ' + e.message);
    }
});

$(document).off('click', '.delete-city').on('click', '.delete-city', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var countryID = $(currentObject).attr("data-con-id");
        var cityID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All offices containing the ' + $(currentObject).parents("tr").find(".CityName").text() + ' city, will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "countryID": countryID,
                "cityID": cityID
            };
            var url = 'SystemSettings.aspx/DeleteCityByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("City Deleted successfully.");
                    $('#citiesTable').DataTable()
                        .row(currentObject.parents('tr'))
                        .remove()
                        .draw();
                }
                else {
                    toastr.error("Failed to delete this city.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an city: ' + e.message);
    }
});

$(document).off('click', '.delete-department').on('click', '.delete-department', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var departmentID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All employee profiles and risks containing the ' + $(currentObject).parents("tr").find(".DepartmentName").text() + ' department will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "departmentID": departmentID
            };
            var url = 'SystemSettings.aspx/DeleteDepartmentByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Department Deleted successfully.");
                    $('#departmentsTable').DataTable()
                        .row(currentObject.parents('tr'))
                        .remove()
                        .draw();
                }
                else {
                    toastr.error("Failed to delete this department.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an department: ' + e.message);
    }
});

$(document).off('click', '.delete-practicegroup').on('click', '.delete-practicegroup', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var practicegroupID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All employee profiles and risks containing the ' + $(currentObject).parents("tr").find(".PracticeGroup").text() + ' practice group will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "practicegroupID": practicegroupID
            };
            var url = 'SystemSettings.aspx/DeletePracticeGroupByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Practice Group Deleted successfully.");
                    $('#practiceGroupTable').DataTable()
                        .row(currentObject.parents('tr'))
                        .remove()
                        .draw();
                }
                else {
                    toastr.error("Failed to delete this Practice Group.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an practice group: ' + e.message);
    }
});
SaveCompanyDetail = function () {
    var companyName = $("#txbCompanyName").val();
    var companyAddress = $("#txbCompanyAddress").val();
    var currency = $("#ddlCurrency").val();
    if (companyName == '') {
        toastr.error("Please provide company name");
        return;
    }
    else if (companyAddress == '') {
        toastr.error("Please provide company address");
        return;
    }
    var param = { "CompanyName": companyName, "CompanyAddress": companyAddress, "Currency": currency };
    var token = $('[name=__RequestVerificationToken]').val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "SystemSettings.aspx/SaveCompanyDetail",
        headers: { '__RequestVerificationToken': token },
        data: "{companyDetail:" + JSON.stringify(param) + "}",
        success: function (data) {
            toastr.success("Settings Saved Sucessfully.");
        },
        error: function (xhr, textStatus, errorThrown) {

            if (isUserAborted(xhr)) {
                return;
            }

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

        }
    });
};
