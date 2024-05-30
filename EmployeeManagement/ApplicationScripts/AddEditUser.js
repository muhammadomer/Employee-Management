var selectedUserData = null;
var offices = null;
//var validationExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}/;
var inc = 0;
var uploadImage = false;
var removeImage = false;

$(document).ready(function () {
    //$("#users").addClass("active");
    $("#region").attr('disabled', true);
    $("#office").prop('disabled', true);
    $("#country").prop('disabled', true);
    $("#state").prop('disabled', true);
    $("#city").prop('disabled', true);
    $("#gpsPostal").prop('disabled', true);
    $("#mailPostal").prop('disabled', true);
    $("#addressLine1").prop('disabled', true);
    $("#addressLine2").prop('disabled', true);
    $("#telephone").prop('disabled', true);
    $('#practiceGroup').attr('disabled', true);
    $('#divPC').hide();

    //$("#userTypeSwitch").bootstrapSwitch('state', false);
    //$('.multibutton5 ul:nth-child(2)').click();

    $("#LockAccountSwitch").bootstrapSwitch('state', false);
    $("#applicationPermissionsSection").show();
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
    };
    options.ui = {
        //showErrors: true,
        //showStatus: true
    };
    $('#password').pwstrength(options);
    $(".mainDiv").css("height", $(".userDataDiv").height() - 40);
    LoadForm(function () {
    });
    GetAllLocationNames();
    $('#applicationTable').on('click', '.allowPermissions', function () {

        if ($("#" + $(this).attr('id') + "").is(":checked")) {
            // it is checked
            $("#application_" + $(this).attr('id') + "").removeClass("disabled");
        }
        else {
            $("#application_" + $(this).attr('id') + "").addClass("disabled");
        }
    });
    /////////// PopUp Ok Button Event /////////
    $("#btn_CentralDataRepositoryPermissionsPopup").on("click", function () {
        if ($('#userModules option:selected').length == 0) {
            toastr.error("Please define the modules that this employee is permitted to access.");
            return false;
        }
        else {
            Edit_User();
            $("#CentralDataRepositoryPermissionsModal").modal("hide");
        }
    });
    $("#btn_DentonsMitigatePermissionsPopup").on("click", function () {

        if ($('#RiskManagerModulesPermissions option:selected').length == 0) {
            toastr.error("Please define the modules that this employee is permitted to access.");
            return false;
        }
        else if ($("#RiskManagerUserLevelPermissions").val() == 0) {
            toastr.error("Please select user level access.");
        }
        else {
            Edit_User();
            $("#DentonsMitigatePermissionsModal").modal("hide");
        }
    });
    $("#btn_BusinessCardPermissionsPopup").on("click", function () {
        if ($('#BusinessCardModulesPermissions option:selected').length == 0) {
            toastr.error("Please define this users permissions for the business card module.");
            return false;
        }
        else {
            Edit_User();
            $("#BusinessCardPermissionsModal").modal("hide");
        }
    });

    $("#btn_DAC6PermissionsPopup").on("click", function () {
        if ($("#DAC6UserLevelPermissions").val() == 0) {
            toastr.error("Please select user level access.");
        }
        else if ($('#DAC6ModulesPermissions option:selected').length == 0) {
            toastr.error("Please define this users permissions for the DAC 6 module.");
            return false;
        }
        else {
            Edit_User();
            $("#DAC6PermissionsModal").modal("hide");
        }
    });

    $("#btn_TrainingCoursesPermissionsPopup").on("click", function () {
        if ($("#TrainingCoursesUserLevelPermissions").val() > 0) {
            Edit_User();
            $("#DAC6PermissionsModal").modal("hide");
        }
        else {
            toastr.error("Please select user level access.");
        }
    });

    //////////////// Change department to Practice Group ////////////////////
    $('#department').on('change', function (e) {

        var DepartmentId = $(this).val();

        if (DepartmentId != 0) {

            var Department = $('#department option:selected').text();
            Department = Department.trim().toLowerCase();
            if (Department != 'practice group') {
                $('#practiceGroup').val('');
                $('#practiceGroup').attr('disabled', true);
                $('#divPC').hide();
            }
            else {
                $('#practiceGroup').attr('disabled', false);
                $('#divPC').show();
            }
        }
        else {
            $('#practiceGroup').val('');
            $('#practiceGroup').attr('disabled', true);
            $('#divPC').hide();
        }

    });
    //////////////// User Add Buttons Events ////////////////////
    $("#btn_AddEditUser").on("click", function () {
        var btnUser = $("#btn_AddEditUser").text();
        if (UserFormValidation()) {
            if (btnUser == "Add") {
                Add_User();
            }
            else if (btnUser == "Update") {
                Edit_User(false);
            }
        }
    });

    $("#removeProfileImage").click(function () {
        $("#logo").attr("src", "images/user.png");
        $("#removeProfileImage").addClass("disabled");
        removeImage = true;
        uploadImage = false;
    });

    $("#uploadProfileImage").click(function () {
        $.FileDialog({ multiple: false }).on('files.bs.filedialog', function (ev) {

            var files = ev.files;
            var data = new FormData();
            if (files.length > 0) {

                var image = "";

                for (var i = 0; i < files.length; i++) {
                    if (files[i].name.toLowerCase().endsWith(".jpg") || files[i].name.toLowerCase().endsWith(".png") || files[i].name.toLowerCase().endsWith(".gif")) {
                        data.append(files[i].name, files[i]);
                        image = files[i].name;
                    }
                    else {
                        toastr.error("Please upload only image file");
                        return;
                    }
                }

                $('#loadUserImageModal .modal-body').empty();

                var imageToBeCropped = "<div class='row'>" +
                    "<div class='col-md-12' >" +
                    "<div class='img-container'>" +
                    "<img id='image' src='images/user.jpg' />" +
                    //"<img id='image' src='images/userImage2.jpg' />" +
                    "</div>" +
                    "</div>" +
                    "</div>";

                $('#loadUserImageModal .modal-body').append(imageToBeCropped);

                var btnCropped = "<button type='button' onclick='getCropedImage()' class='btn btn-primary purple'>Upload</button>";

                $('#loadUserImageModal .modal-footer').append(btnCropped);
                $('#loadUserImageModal #uploadButton').remove();

                $("#ajaxLoader").modal("show");

                var token = $('[name=__RequestVerificationToken]').val();

                $.ajax({
                    url: "ProfileImageUploader.ashx",
                    type: "POST",
                    headers: { '__RequestVerificationToken': token },
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        //debugger;
                        if (result != "2") {
                            $("#image").attr("src", "");
                            $("#image").attr("src", result);
                            $("#removeProfileImage").removeClass("disabled");
                            $("#ajaxLoader").modal("hide");
                        }
                        else {
                            $("#ajaxLoader").modal("hide");
                            toastr.error("Please provide jpg, jpeg and png images only.");
                        }
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
                    },
                    complete: function (result) {
                        console.log(result);
                        var $image = $('#image');
                        var options = {
                            aspectRatio: 1 / 1,
                            preview: '.img-preview'
                        };
                        $image.on({
                            ready: function (e) {

                            },
                            cropstart: function (e) {

                            },
                            cropmove: function (e) {

                            },
                            cropend: function (e) {

                            },
                            crop: function (e) {

                            },
                            zoom: function (e) {

                            }
                        }).cropper(options);

                    }
                });
            }
            else {
                toastr.error("Please select file");
                return;
            }

        }).on('cancel.bs.filedialog', function (ev) {
            //alert("Cancelled!");
        });
    });

    if ($("#application_1").attr("data-id") != 3) {

        var group5 = $('.multibutton5').multiButton({
            join: true,
            onClick: function (el, value) {
                //$('span#multibutton5-info').text(value);
                $("#hdn_multibutton5").val(value);

                if (value == 1 || value == 3) {
                    $("#applicationPermissionsSection").hide();
                    $("#reset-password").show();
                    $("#AccountStatus").show();
                    $("#AccountLocked").show();
                }
                else if (value == 2) {
                    $("#applicationPermissionsSection").show();
                    $("#reset-password").show();
                    $("#AccountStatus").show();
                    $("#AccountLocked").show();
                }
                //else if (value == 3) {
                //    $("#applicationPermissionsSection").hide();
                //    $("#reset-password").hide();
                //    $("#AccountStatus").hide();
                //    $("#AccountLocked").hide();
                //}
            }
        });
        $(".allowPermissions").removeAttr("disabled");
    }
    else {
        $('.multibutton5').multiButton({});
        $('.multibutton5 :nth-child(3)').addClass("active").css("opacity", "0.5");
        $("#LockAccountSwitch").bootstrapSwitch('disabled', true);
        $(".multibutton5 li").css("cursor", "no-drop");
        $(".multibutton5 li").unbind("click");
    }
    //$('#userTypeSwitch').on('switchChange.bootstrapSwitch', function (event, state) {
    //    var isSuperAdmin = $('#userTypeSwitch').bootstrapSwitch('state');
    //    if (isSuperAdmin) {
    //        $("#applicationPermissionsSection").hide();
    //    }
    //    else {
    //        $("#applicationPermissionsSection").show();
    //    }
    //});

    //if ($('#userTypeSwitch').bootstrapSwitch('state')) {
    //    $("#applicationPermissionsSection").hide();
    //}
    //else {
    //    $("#applicationPermissionsSection").show();
    //}

    $('#locationNames').on('change', function () {
        var object = null;
        for (var i = 0; i < offices.length; i++) {
            if (offices[i].ID == this.value) {
                object = offices[i];
                break;
            }
        }
        // print
        if (object !== null) {
            $("#region").val(object.RegionName);
            $("#office").val(object.Office);
            $("#country").val(object.CountryName);
            $("#state").val(object.State);
            $("#city").val(object.CityName);
            $("#gpsPostal").val(object.GPSPostal);
            $("#mailPostal").val(object.MailPostal);
            $("#addressLine1").val(object.AddressLine1);
            $("#addressLine2").val(object.AddressLine2);
            $("#telephone").val(object.Telephone);
        }
        else {
            $("#region").val('');
            $("#office").val('');
            $("#country").val('');
            $("#state").val('');
            $("#city").val('');
            $("#gpsPostal").val('');
            $("#mailPostal").val('');
            $("#addressLine1").val('');
            $("#addressLine2").val('');
            $("#telephone").val('');
        }

    });
    $('#allowCard').click(function () {
        if ($(this).is(':checked')) {
            ShowConfirmationDialog('Business card', 'Ticking this box will generate new business cards for this employee. Are you sure you want to continue?', function () {

            }, function () {
                $("#allowCard").prop("checked", false);
            }, 'Continue');
            //if (confirm("Are you sure?")) {
            //    alert("Ok");
            //}
            //else {
            //    $("#allowCard").prop("checked", false);
            //}
        }
    });
    $("#RiskManagerModulesPermissions").change(function () {
        var userPermissions = $('#RiskManagerModulesPermissions option:selected');
        $("#RiskManagerBoardsListPermissions option:selected").removeAttr("selected");
        $("#RiskManagerBoardsListPermissions").multiselect('refresh');
        $('#RiskManagerBoardsListPermissions').multiselect("disable", true);
        $("#RiskManagerBoardsListPermissions").parent().parent().addClass("hidden");
        $(userPermissions).each(function (index, userPermissions) {

            if ($(this).val() == "1") {
                $('#RiskManagerBoardsListPermissions').multiselect("enable", true);
                $("#RiskManagerBoardsListPermissions").multiselect('refresh');
                $("#RiskManagerBoardsListPermissions").parent().parent().removeClass("hidden");
            }
        });
    });
    $('#email').on('keypress', function (e) {
        if (e.which == 32)
            return false;
    });

    $('.multibutton5 :nth-child(3)').trigger('click');

});

function getCropedImage() {
    uploadImage = true;
    inc++;

    $('#image').cropper("getCroppedCanvas").toBlob((blob) => {

        const formData = new FormData();
        formData.append('croppedImage', blob, 'CroppedImage' + inc + '.png');

        var token = $('[name=__RequestVerificationToken]').val();

        $.ajax({
            url: "ProfileImageUploader.ashx",
            headers: { '__RequestVerificationToken': token },
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                console.log("result:" + result);
                if (result != null && result == "E1") {
                    $("#ajaxLoader").modal("hide");
                    toastr.error("Only jpg, jpeg and png images allowed.");
                }
                else if (result != "" && result != null) {
                    $("#logo").attr("src", "");
                    $("#logo").attr("src", result);
                    $("#removeProfileImage").removeClass("disabled");
                    $("#ajaxLoader").modal("hide");
                }
                else {
                    $("#ajaxLoader").modal("hide");
                    toastr.error("Please provide jpg, jpeg and png images only.");
                }
                removeImage = false;
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
            },
            complete: function (result) {
                $("#image").cropper("destroy");
                $('#UserProfileImageModal').modal("hide");
            }
        });

    });
}

$(function () {
    $("#upldUsrImage").on('click', function (e) {
        e.preventDefault();
        $("#uploadUsrImage").val('');
        $("#uploadUsrImage:hidden").trigger('click');
    });
});

function readURL(input) {

    if (input.files[0].name.toLowerCase().endsWith(".jpg") || input.files[0].name.toLowerCase().endsWith(".png") || input.files[0].name.toLowerCase().endsWith(".jpeg")) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            $('#UserProfileImageModal .modal-body').empty();

            var imageToBeCropped = "<div class='row'>" +
                "<div class='col-md-12' >" +
                "<div class='img-container'>" +
                "<img id='image' src='images/user.jpg' />" +
                "</div>" +
                "</div>" +
                "</div>";

            $('#UserProfileImageModal .modal-body').append(imageToBeCropped);

            reader.onload = function (e) {
                $('#image')
                    .attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);

            $("#ajaxLoader").modal("show");

            setTimeout(function () {
                var $image = $('#image');
                var options = {
                    aspectRatio: 1 / 1,
                    preview: '.img-preview'
                };
                $image.on({
                    ready: function (e) {

                    },
                    cropstart: function (e) {

                    },
                    cropmove: function (e) {

                    },
                    cropend: function (e) {

                    },
                    crop: function (e) {

                    },
                    zoom: function (e) {

                    }
                }).cropper(options);

                $("#ajaxLoader").modal("hide");
                $('#UserProfileImageModal').modal("show");
            }, 500);

        }
    }
    else {
        toastr.error("Please upload only image file");
        return;
    }

}

//////// Start Fill All Drop Downs  ////////////////
var varGetAllApplications = false;
var varGetAllPermissions = false;
var varGetAllFilesPermissions = false;
var varGetAllRepositories = false;
var varGetAllDentonsModules = false;
var varGetAllRiskManagerUserBoards = false;
var varGetAllBusinessCardPermissionsList = false;
var AllDataPopulated = false;
LoadForm = function (callback) {
    $("#ajaxLoader").modal("show");

    $('#applicationTable').dataTable({
        destroy: true,
        "oLanguage": {
            "oPaginate": {
                "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
            }
        },
        "pageLength": 5,
        "info": true,
        "bLengthChange": false,
        "searching": false,
        "bSort": false
    });

    $('#userModules').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });


    $('#userFileCategories').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });

    $('#userRepositories').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });

    $('#RiskManagerModulesPermissions').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });


    $('#RiskManagerBoardsListPermissions').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });

    //$('#RiskManagerBoardsListPermissions').multiselect("disable", true);

    $('#BusinessCardModulesPermissions').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });

    $('#DAC6ModulesPermissions').multiselect({
        buttonWidth: '100%',
        includeSelectAllOption: true
    });
    if (QueryString("id") != null) {
        $(".passwordRow").addClass("hide");
        $(".resetPasswordRow").removeClass("hide");
        $("#btn_AddEditUser").text("Update");
        GetUserInfoByID(QueryString("id"));
        //AllDataPopulated = setInterval(PopulateData, 100);

    }
    else {
        $("#btn_AddEditUser").text("Add");
        $("#ajaxLoader").modal("hide");
    }
};

GetAllLocationNames = function () {
    var url = 'Offices.aspx/GetAllLocations';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {
        offices = response;
        $("#locationNames").trigger("change");
    });
};

ShowPermissionModal = function (modalName) {
    if (QueryString("id") != null) {

        if (selectedUserData.DAC6UserLevelPermissionsList != null) {
            for (var i = 0; i < selectedUserData.DAC6UserLevelPermissionsList.length; i++) {
                $("#DAC6UserLevelPermissions option[value='" + selectedUserData.DAC6UserLevelPermissionsList[i].LevelPermissionId + "']").prop("selected", true);
            }
            checkPermissionsDAC6();
            // $("#DAC6ModulesPermissions").multiselect('refresh');
        }
        if (selectedUserData.DAC6ModulesPermissionsList != null) {
            for (var i = 0; i < selectedUserData.DAC6ModulesPermissionsList.length; i++) {
                $("#DAC6ModulesPermissions option[value='" + selectedUserData.DAC6ModulesPermissionsList[i].ID + "']").prop("selected", true);
            }
            $("#DAC6ModulesPermissions").multiselect('refresh');
        }

        $("#" + modalName + "").modal("show");
    }
    else {
        ShowConfirmationDialog('Save User', 'Please save Employee Profile before configuring permissions. Do you want to save it?', function () {
            var btnUser = $("#btn_AddEditUser").text();
            if (UserFormValidation()) {
                if (btnUser == "Add") {
                    Add_User();
                }
            }
        }, null, 'Save');
    }
};
//////// End Fill All Drop Downs  ////////////////

//////////// User CRUD Operation  ////////////////

Add_User = function () {

    var firstName = $("#firstName").val();
    var lastName = $("#lastName").val();
    var jobTitle = $("#jobTitle").val();
    var email = $("#email").val();
    var directNumber = $("#directNumber").val();
    var mobileNumber = $("#mobileNumber").val();
    var faxNumber = $("#faxNumber").val();
    var username = $("#username").val();
    var password = $("#password").val();

    var checkFirstName = htmlTagsValidation(firstName);
    var checkLastName = htmlTagsValidation(lastName);
    var checkJobTitle = htmlTagsValidation(jobTitle);
    var checkEmail = validateEmail(email);
    var checkDirectNumber = htmlTagsValidation(directNumber);
    var checkMobileNumber = htmlTagsValidation(mobileNumber);
    var checkFaxNumber = htmlTagsValidation(faxNumber);
    var checkUsername = htmlTagsValidation(username);
    var checkPassword = htmlTagsValidation(password);

    if (checkFirstName == false && checkLastName == false && checkJobTitle == false && checkEmail == true && checkDirectNumber == false && checkMobileNumber == false && checkFaxNumber == false
        && checkUsername == false && checkPassword == false) {

        $("#ajaxLoader").modal("show");

        //3516
        //var _email = email.split("@")[1];
        //email = email.split("@")[0];
        //if (_email != null && _email.trim().toLowerCase() == "ghost-digital.com") {
        //    email = email + "@ghost-digital.com"
        //}
        //else {
        //    email = email + "@dentons.com"
        //}

        var contactNumber = $("#contactNumber").val();

        var office = $("#office").val();
        var officeID = $("#locationNames").val();
        var region = $("#region").val();
        var department = $("#department").val();
        var practiceGroup = $("#practiceGroup").val();
        var country = $("#country").val();
        var state = $("#state").val();
        var city = $("#city").val();
        var gpsPostal = $("#gpsPostal").val();
        var mailPostal = $("#mailPostal").val();
        var addressLine1 = $("#addressLine1").val();
        var addressLine2 = $("#addressLine2").val();
        var telephone = $("#telephone").val();
        var allowCard = $("#allowCard").is(":checked");
        var userType = null;
        var userTypeID = null;

        var _userType = $("#hdn_multibutton5").val();
        if (_userType == 1) {
            userType = 'Super Admin';
            userTypeID = 1;
        }
        else if (_userType == 2) {
            userType = 'User';
            userTypeID = 2;
        }
        else if (_userType == 3) {
            userType = 'Data Admin';
            userTypeID = 3;
        }

        //var isSuperAdmin = $('#userTypeSwitch').bootstrapSwitch('state');
        //if (isSuperAdmin) {
        //    userType = 'Super Admin';
        //    userTypeID = 1;
        //}
        //else {
        //    userType = 'User';
        //    userTypeID = 2;
        //}

        var isDeleted = false;
        var isActive = true;
        if ($('#LockAccountSwitch').bootstrapSwitch('state')) {
            isActive = false;
        }
        isSuperAdmin = true;
        var applications = new Array();
        var modulePermissions = new Array();
        var fileTypePermissions = new Array();
        var fileRepositoryPermissions = new Array();
        var dentonsMitigateModulePermissions = new Array();
        var dentonsMitigateUserBoards = new Array();
        var businessCardModulePermissionsList = new Array();
        var dac6ModulePermissionsList = new Array();
        if (userType == "User") {
            //$("#applicationPermissionsSection").show();
            isSuperAdmin = false;
        }
        var TCLevelPermissionId = 0;
        if (!isSuperAdmin) {
            $("#applicationTable .allowPermissions").each(function (index) {
                var applicationID = '';
                var applicationName = '';
                if ($(this).is(":checked")) {
                    var applicationID = $(this).parent().prev().attr("data-id");
                    var applicationName = $('.applicationName:eq(' + index + ')').html().trim();
                    applications.push({ "ID": applicationID, "Name": applicationName });
                    if (applicationName == 'Central Data Repository') {
                        $('#userModules option:selected').each(function (index, permissions) {
                            modulePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#userFileCategories option:selected').each(function (index, permissions) {
                            fileTypePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#userRepositories option:selected').each(function (index, permissions) {
                            fileRepositoryPermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationName == 'Mitigate') {
                        $('#RiskManagerModulesPermissions option:selected').each(function (index, permissions) {
                            dentonsMitigateModulePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#RiskManagerBoardsListPermissions option:selected').each(function (index, permissions) {
                            dentonsMitigateUserBoards.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationName == 'Business Cards') {
                        $('#BusinessCardModulesPermissions option:selected').each(function (index, permissions) {
                            businessCardModulePermissionsList.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationName == 'DAC 6') {
                        $('#DAC6ModulesPermissions option:selected').each(function (index, permissions) {
                            dac6ModulePermissionsList.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                }
            });
            TCLevelPermissionId = $("#TrainingCoursesUserLevelPermissions").val();
        }
        var url = 'AddEditUser.aspx/Inser_User';
        var userData = {
            "First_Name": firstName,
            "Last_Name": lastName,
            "Email": email,
            "Contact_Number": contactNumber,
            "Job_Title": jobTitle,
            "Username": username,
            "Password": password,
            "Office": office,
            "OfficeID": officeID,
            "Region": region,
            "Department": department,
            "PracticeGroup": practiceGroup,
            "Country": country,
            "State": state,
            "City": city,
            "GPS_Postal": gpsPostal,
            "Mail_Postal": mailPostal,
            "Address_Line_1": addressLine1,
            "Address_Line_2": addressLine2,
            "Telephone": telephone,
            "Direct_Number": directNumber,
            "Mobile_Number": mobileNumber,
            "Fax_Number": faxNumber,
            "UserTypeID": userTypeID,
            "Allow_Card": allowCard,
            "IsDeleted": isDeleted,
            "IsActive": isActive,
            "IsSuperAdmin": isSuperAdmin,
            "ApplcaiotnsList": applications,
            "UploadImage": uploadImage,
            "RemoveImage": removeImage
        };
        var requestPerameters = {
            "userEntity": userData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {

            if (response.split("id")[0] == 'Employee inserted successfully.') {

                toastr.success(response.split("id")[0]);
                setTimeout(function () {
                    //$("#ajaxLoader").modal("hide");
                    document.location = "AddEditUser.aspx?id=" + encData(response.split("id")[1]);
                }, 1000);
            }
            else {
                $("#ajaxLoader").modal("hide");
                toastr.error(response);
            }
            uploadImage = false;
            removeImage = false;
        });

    }
    else {
        toastr.error(specialCharacterError);
    }

};

Edit_User = function (IsPermissions = true) {

    var firstName = $("#firstName").val();
    var lastName = $("#lastName").val();
    var jobTitle = $("#jobTitle").val();
    var email = $("#email").val();
    var directNumber = $("#directNumber").val();
    var mobileNumber = $("#mobileNumber").val();
    var faxNumber = $("#faxNumber").val();
    var username = $("#username").val();
    var password = $("#password").val();

    var checkFirstName = htmlTagsValidation(firstName);
    var checkLastName = htmlTagsValidation(lastName);
    var checkJobTitle = htmlTagsValidation(jobTitle);
    var checkEmail = validateEmail(email);
    var checkDirectNumber = htmlTagsValidation(directNumber);
    var checkMobileNumber = htmlTagsValidation(mobileNumber);
    var checkFaxNumber = htmlTagsValidation(faxNumber);
    var checkUsername = htmlTagsValidation(username);
    var checkPassword = htmlTagsValidation(password);

    if (checkFirstName == false && checkLastName == false && checkJobTitle == false && checkEmail == true
        && checkDirectNumber == false && checkMobileNumber == false && checkFaxNumber == false
        && checkUsername == false && checkPassword == false) {

        $("#ajaxLoader").modal("show");
        var userID = $("#userID").val();

        ///3516
        //var _email = email.split("@")[1];
        //email = email.split("@")[0];
        //if (_email != null && _email.trim().toLowerCase() == "ghost-digital.com") {
        //    email = email + "@ghost-digital.com"
        //}
        //else {
        //    email = email + "@dentons.com"
        //}

        var contactNumber = $("#contactNumber").val();
        var office = $("#office").val();
        var officeID = $("#locationNames").val();
        var region = $("#region").val();
        var department = $("#department").val();
        var practiceGroup = $("#practiceGroup").val();
        var country = $("#country").val();
        var state = $("#state").val();
        var city = $("#city").val();
        var gpsPostal = $("#gpsPostal").val();
        var mailPostal = $("#mailPostal").val();
        var addressLine1 = $("#addressLine1").val();
        var addressLine2 = $("#addressLine2").val();
        var telephone = $("#telephone").val();

        var allowCard = $("#allowCard").is(":checked");
        var userType = null;
        var userTypeID = null;

        var _userType = $("#hdn_multibutton5").val();
        if (_userType == 1) {
            userType = 'Super Admin';
            userTypeID = 1;
        }
        else if (_userType == 2) {
            userType = 'User';
            userTypeID = 2;
        }
        else if (_userType == 3) {
            userType = 'Data Admin';
            userTypeID = 3;
        }

        //var isAdmin = $('#userTypeSwitch').bootstrapSwitch('state');
        //if (isAdmin) {
        //    userType = 'Super Admin';
        //    userTypeID = 1;
        //}
        //else {
        //    userType = 'User';
        //    userTypeID = 2;
        //}

        var isDeleted = false;
        var isActive = true;
        if ($('#LockAccountSwitch').bootstrapSwitch('state')) {
            isActive = false;
        }
        var isSuperAdmin = true;
        var applications = new Array();
        var modulePermissions = new Array();
        var fileTypePermissions = new Array();
        var fileRepositoryPermissions = new Array();
        var dentonsMitigateModulePermissions = new Array();
        var dentonsMitigateUserBoards = new Array();
        var riskManagerLevelPermissions = new Array();
        var dac6LevelPermissions = new Array();
        var businessCardModulePermissionsList = new Array();
        var dac6ModulePermissionsList = new Array();
        if (userType == "User") {
            //$("#applicationPermissionsSection").show();
            isSuperAdmin = false;
        }
        var TCLevelPermissionId = 0;
        if (!isSuperAdmin) {
            $("#applicationTable .allowPermissions").each(function (index) {
                var applicationID = '';
                var applicationName = '';
                if ($(this).is(":checked")) {
                    var applicationID = $(this).parent().prev().attr("data-id");
                    var applicationName = $('.applicationName:eq(' + index + ')').html().trim();
                    applications.push({ "ID": applicationID, "Name": applicationName });
                    if (applicationID == 1) {
                        $('#userModules option:selected').each(function (index, permissions) {
                            modulePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#userFileCategories option:selected').each(function (index, permissions) {
                            fileTypePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#userRepositories option:selected').each(function (index, permissions) {
                            fileRepositoryPermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationID == 2) {
                        $('#RiskManagerModulesPermissions option:selected').each(function (index, permissions) {
                            dentonsMitigateModulePermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#RiskManagerBoardsListPermissions option:selected').each(function (index, permissions) {
                            dentonsMitigateUserBoards.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                        $('#RiskManagerUserLevelPermissions option:selected').each(function (index, permissions) {
                            riskManagerLevelPermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationID == 3) {
                        $('#BusinessCardModulesPermissions option:selected').each(function (index, permissions) {
                            businessCardModulePermissionsList.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                    if (applicationID == 4) {
                        $('#DAC6ModulesPermissions option:selected').each(function (index, permissions) {
                            dac6ModulePermissionsList.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });

                        $('#DAC6UserLevelPermissions option:selected').each(function (index, permissions) {
                            dac6LevelPermissions.push({ "ID": $(this).val(), "Name": $(this).text() });
                        });
                    }
                }
            });
            TCLevelPermissionId = $("#TrainingCoursesUserLevelPermissions").val();
        }
        var url = 'AddEditUser.aspx/Update_User';
        var userData = {
            "ID": userID,
            "First_Name": firstName,
            "Last_Name": lastName,
            "Email": email,
            "Contact_Number": contactNumber,
            "Job_Title": jobTitle,
            "Username": username,
            "Password": password,
            "Office": office,
            "OfficeID": officeID,
            "Region": region,
            "Department": department,
            "PracticeGroup": practiceGroup,
            "Country": country,
            "State": state,
            "City": city,
            "GPS_Postal": gpsPostal,
            "Mail_Postal": mailPostal,
            "Address_Line_1": addressLine1,
            "Address_Line_2": addressLine2,
            "Telephone": telephone,
            "Direct_Number": directNumber,
            "Mobile_Number": mobileNumber,
            "Fax_Number": faxNumber,
            "UserTypeID": userTypeID,
            "Allow_Card": allowCard,
            "IsDeleted": isDeleted,
            "IsActive": isActive,
            "IsSuperAdmin": isSuperAdmin,
            "ApplcaiotnsList": applications,
            "FileRepositoryModulePermissionsList": modulePermissions,
            "FileTypePermissionsList": fileTypePermissions,
            "FileRepositoryPermissionsList": fileRepositoryPermissions,
            "MitigateModulePermissionsList": dentonsMitigateModulePermissions,
            "MitigateBoardsList": dentonsMitigateUserBoards,
            "RiskManagerLevelPermissions": riskManagerLevelPermissions,
            "BusinessCardsModulesPermissionsList": businessCardModulePermissionsList,
            "DAC6ModulesPermissionsList": dac6ModulePermissionsList,
            "DAC6UserLevelPermissionsList": dac6LevelPermissions,
            "UploadImage": uploadImage,
            "TCLevelPermissionId": TCLevelPermissionId,
            "RemoveImage": removeImage
        };
        var requestPerameters = {
            "userEntity": userData,
            "IsPermissions": IsPermissions
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response == 'Employee updated successfully.') {
                $("#ajaxLoader").modal("hide");
                toastr.success(response);
            }
            else {
                $("#ajaxLoader").modal("hide");
                toastr.error(response);
            }
            uploadImage = false;
            removeImage = false;
        });

    }
    else {
        toastr.error(specialCharacterError);
    }


};

ClearForm = function () {
    $("#firstName").val('');
    $("#lastName").val('');
    $("#email").val('');
    $("#locationNames").val('');
    $("#contactNumber").val('');
    $("#jobTitle").val('');
    $("#username").val('');
    $("#password").val('');
    $("#office").val('');
    $("#locationNames").val('');
    $("#region").val('');
    $("#department").val('');
    $("#practiceGroup").val('');
    $("#userType").val('');

    //$("#userTypeSwitch").bootstrapSwitch('state', false);
    $('.multibutton5 :nth-child(3)').trigger('click');

    $("#country").val('');
    $("#state").val('');
    $("#city").val('');
    $("#gpsPostal").val('');
    $("#mailPostal").val('');
    $("#addressLine1").val('');
    $("#addressLine2").val('');
    $("#telephone").val('');
    $("#directNumber").val('');
    $("#mobileNumber").val('');
    $("#faxNumber").val('');
    $("#userType").trigger("change");
    $("#logo").attr("src", 'images/user.png');
    $("#removeProfileImage").addClass("disabled");
    $(".btnConfigure").addClass("disabled");
    $(".allowPermissions").prop("checked", false);
    $("#allowCard").prop("checked", false);
    $('#password').pwstrength("forceUpdate");
    $("#userModules option:selected").removeAttr("selected");
    $("#userModules").multiselect('refresh');
    $("#userFileCategories option:selected").removeAttr("selected");
    $("#userFileCategories").multiselect('refresh');

    $("#userRepositories option:selected").removeAttr("selected");
    $("#userRepositories").multiselect('refresh');

    $("#RiskManagerModulesPermissions option:selected").removeAttr("selected");
    $("#RiskManagerModulesPermissions").multiselect('refresh');

    $("#RiskManagerBoardsListPermissions option:selected").removeAttr("selected");
    $("#RiskManagerBoardsListPermissions").multiselect('refresh');

    $("#BusinessCardModulesPermissions option:selected").removeAttr("selected");
    $("#BusinessCardModulesPermissions").multiselect('refresh');

    $("#DAC6ModulesPermissions option:selected").removeAttr("selected");
    $("#DAC6ModulesPermissions").multiselect('refresh');

    uploadImage = false;
    removeImage = false;
};

UserFormValidation = function () {
    var firstName = $("#firstName").val();
    var lastName = $("#lastName").val();
    var jobTitle = $('#jobTitle').val();
    var userName = $('#username').val();
    var password = $("#password").val();
    var email = $("#email").val();
    var contactNumber = $("#contactNumber").val();
    var department = $("#department").val();
    var practiceGroup = $("#practiceGroup").val();
    //var office = $("#office").val();
    var region = $("#region").val();
    var country = $("#country").val();
    var state = $("#state").val();
    var city = $("#city").val();
    var gpsPostal = $("#gpsPostal").val();
    var mailPostal = $("#mailPostal").val();
    var addressLine1 = $("#addressLine1").val();
    var addressLine2 = $("#addressLine2").val();
    var telephone = $("#telephone").val();
    var directNumber = $("#directNumber").val();
    var mobileNumber = $("#mobileNumber").val();
    var faxNumber = $("#faxNumber").val();
    if (firstName == '') {
        toastr.error('Please provide first name.');
        return false;
    }
    else if (lastName == '') {
        toastr.error('Please provide last name.');
        return false;
    }
    else if (jobTitle == '') {
        toastr.error('Please provide job title.');
        return false;
    }
    else if (email == '') {
        toastr.error('Please provide email.');
        return false;
    }

    else if (!ValidateEmail(email)) {
        toastr.error('The email address provided is not valid.');
        return false;
    }

    //3516
    //else if (email.indexOf('@') > -1 && email.split('@')[1].trim().toLowerCase() != "ghost-digital.com") {
    //    toastr.error('Can not contain an @ symbol.');
    //    return false;
    //}
    //else if (!email.endsWith("ghost-digital.com")) {
    //    alert();
    //    if (!ValidateEmail(email + "@dentons.com")) {
    //        toastr.error('The email address provided is not valid.');
    //        return false;
    //    }
    //}
    else if (addressLine1 == '') {
        toastr.error('Please provide address 1.');
        return false;
    }
    else if (region == '') {
        toastr.error('Please provide region.');
        return false;
    }
    //else if (addressLine2 == '') {
    //    toastr.error('Please provide address 2.');
    //    return false;
    //}
    else if (country == '') {
        toastr.error('Please provide country.');
        return false;
    }
    //else if (state == '') {
    //    toastr.error('Please provide state/county/province.');
    //    return false;
    //}
    //else if (office == '') {
    //    toastr.error('Please provide office.');
    //    return false;
    //}
    else if (city == '') {
        toastr.error('Please provide city.');
        return false;
    }
    else if (department == '') {
        toastr.error('Please provide department.');
        return false;
    }
    else if (department.trim().toLowerCase() == 'practice group' && practiceGroup == '') {
        toastr.error('Please provide practice group.');
        return false;
    }
    else if (mailPostal == '') {
        toastr.error('Please provide mail postal/zip code.');
        return false;
    }
    //else if (gpsPostal == '') {
    //    toastr.error('Please provide GPS postal/zip code.');
    //    return false;
    //}
    else if (directNumber.length > 18) {
        toastr.error('Direct number is too large.');
        return false;
    }
    else if (!ValidateNumber(directNumber) && directNumber != "") {
        toastr.error('The direct number provided is not valid.');
        return false;
    }

    else if (telephone == '') {
        toastr.error('Please provide telephone number.');
        return false;
    }
    else if (telephone.length > 18) {
        toastr.error('Telephone number is too large.');
        return false;
    }
    else if (!ValidateNumber(telephone) && telephone != "") {
        toastr.error('The telephone number provided is not valid.');
        return false;
    }
    else if (mobileNumber.length > 18) {
        toastr.error('Mobile number is too large.');
        return false;
    }
    else if (!ValidateNumber(mobileNumber) && mobileNumber != "") {
        toastr.error('The mobile number provided is not valid.');
        return false;
    }
    else if (faxNumber.length > 18) {
        toastr.error('Fax number is too large.');
        return false;
    }
    else if (!ValidateNumber(faxNumber) && faxNumber != "") {
        toastr.error('The fax number provided is not valid.');
        return false;
    }
    else if (userName == '') {
        toastr.error('Please provide username.');
        return false;
    }
    else if (password == '') {
        if (QueryString("id") == null) {
            toastr.error('Please provide password.');
            return false;
        }
    }
    else if (!validationExpression.test(password)) {
        if (QueryString("id") == null) {
            toastr.error('Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.');
            return false;
        }
    }
    return true;
};

GetUserInfoByID = function (id) {

    var url = 'AddEditUser.aspx/GetUserInfoByID';
    var requestPerameters = {
        "ID": id
    };
    AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {

        if (response != null) {
            selectedUserData = response;
            $("#userID").val(response.UserID);
            $("#locationNames").val(response.OfficeID);
            $("#firstName").val(response.FirstName);
            $("#lastName").val(response.LastName);

            //3516
            //if (response.Email.split("@")[1] == "dentons.com") {
            //    $("#email").val(response.Email.split("@")[0]);
            //}
            //else {
            //    $("#email").val(response.Email);
            //}
            $("#email").val(response.Email);
            $("#contactNumber").val(response.ContacNumber);
            $("#jobTitle").val(response.JobTitle);
            $("#username").val(response.UserName);
            $("#password").val(response.Password);
            $("#password").attr("disabled", "disabled");
            $("#username").attr("disabled", "disabled");
            $("#office").val(response.Office);
            $("#region").val(response.Region);
            $("#department").val(response.Department);

            if (response.UserName == "admin")
                $(".multibutton5").hide();


            if (response.Department != 0) {

                var Department = $('#department option:selected').text();
                Department = Department.trim().toLowerCase();
                if (Department != 'practice group') {
                    $('#practiceGroup').val('');
                    $('#practiceGroup').attr('disabled', true);
                    $('#divPC').hide();
                }
                else {
                    $("#practiceGroup").val(response.PracticeGroup);
                    $('#practiceGroup').attr('disabled', false);
                    $('#divPC').show();
                }
            }
            else {
                $('#practiceGroup').val('');
                $('#practiceGroup').attr('disabled', true);
                $('#divPC').hide();
            }


            $("#country").val(response.Country);
            $("#state").val(response.State);
            $("#city").val(response.City);
            $("#gpsPostal").val(response.GPSPostal);
            $("#mailPostal").val(response.MailPostal);
            $("#addressLine1").val(response.AddressLine1);
            $("#addressLine2").val(response.AddressLine2);
            $("#telephone").val(response.Telephone);
            $("#directNumber").val(response.DirectNumber);
            $("#mobileNumber").val(response.MobileNumber);
            $("#faxNumber").val(response.FaxNumber);
            $("#LockAccountSwitch").bootstrapSwitch('state', false);
            if (!response.IsActive) {
                $("#LockAccountSwitch").bootstrapSwitch('state', true);
            }
            if (response.AllowCard) {
                $("#allowCard").prop("checked", true);
            }
            if (response.ProfileImage != "images/user.png") {
                $("#logo").attr("src", response.ProfileImage);
                $("#removeProfileImage").removeClass("disabled");
            }
            else {

                $("#logo").attr("src", "images/user.png");
                $("#removeProfileImage").addClass("disabled");
            }
            //response.UserTypeID

            if (response.UserTypeID == 1) {
                $('.multibutton5 :nth-child(1)').trigger('click');
            }
            else if (response.UserTypeID == 2) {
                $('.multibutton5 :nth-child(3)').trigger('click');
                $("#applicationPermissionsSection").show();
            }
            else if (response.UserTypeID == 3) {
                $('.multibutton5 :nth-child(2)').trigger('click');
            }

            if (selectedUserData.ApplcaiotnsList != null) {
                for (var i = 0; i < selectedUserData.ApplcaiotnsList.length; i++) {
                    if (selectedUserData.ApplcaiotnsList[i].UserID != '') {
                        $("#" + selectedUserData.ApplcaiotnsList[i].ID).prop('checked', true);
                        if ($("#application_" + selectedUserData.ApplcaiotnsList[i].ID).attr("data-id") != 3) {
                            $("#" + selectedUserData.ApplcaiotnsList[i].ID).removeAttr("disabled");
                            $("#application_" + selectedUserData.ApplcaiotnsList[i].ID).removeClass("disabled");
                        }
                    }
                }
            }

            if (selectedUserData.FileRepositoryModulePermissionsList != null) {
                for (var i = 0; i < selectedUserData.FileRepositoryModulePermissionsList.length; i++) {
                    $("#userModules option[value='" + selectedUserData.FileRepositoryModulePermissionsList[i].ID + "']").prop("selected", true);
                }
                $("#userModules").multiselect('refresh');
            }

            if (selectedUserData.FileTypePermissionsList != null) {
                for (var i = 0; i < selectedUserData.FileTypePermissionsList.length; i++) {
                    $("#userFileCategories option[value='" + selectedUserData.FileTypePermissionsList[i].ID + "']").prop("selected", true);
                }
                $("#userFileCategories").multiselect('refresh');
            }

            if (selectedUserData.FileRepositoryPermissionsList != null) {
                for (var i = 0; i < selectedUserData.FileRepositoryPermissionsList.length; i++) {
                    $("#userRepositories option[value='" + selectedUserData.FileRepositoryPermissionsList[i].ID + "']").prop("selected", true);
                }
                $("#userRepositories").multiselect('refresh');
            }

            // Set Mitigate Permissions

            if (selectedUserData.MitigateModulePermissionsList != null) {
                debugger;
                for (var i = 0; i < selectedUserData.MitigateModulePermissionsList.length; i++) {
                    if (selectedUserData.MitigateModulePermissionsList[i].ID == "1") {
                        $('#RiskManagerBoardsListPermissions').multiselect("enable", true);
                        $("#RiskManagerBoardsListPermissions").multiselect('refresh');
                        $("#RiskManagerBoardsListPermissions").parent().parent().removeClass("hidden");
                    }
                    $("#RiskManagerModulesPermissions option[value='" + selectedUserData.MitigateModulePermissionsList[i].ID + "']").prop("selected", true);

                  
                }
              

                $("#RiskManagerModulesPermissions option[value='1']").prop("selected", true);
                $('#RiskManagerModulesPermissions option[value="1"]').prop('disabled', true);
                $("#RiskManagerModulesPermissions").multiselect('refresh');
            }

            if (selectedUserData.MitigateBoardsList != null) {
                for (var i = 0; i < selectedUserData.MitigateBoardsList.length; i++) {
                    $("#RiskManagerBoardsListPermissions option[value='" + selectedUserData.MitigateBoardsList[i].ID + "']").prop("selected", true);
                }
                $("#RiskManagerBoardsListPermissions").multiselect('refresh');
            }


            $("#RiskManagerModulesPermissions").multiselect('enable');
            if (selectedUserData.RiskManagerUserLevelPermissionsList != null) {
                for (var i = 0; i < selectedUserData.RiskManagerUserLevelPermissionsList.length; i++) {
                    $("#RiskManagerUserLevelPermissions option[value='" + selectedUserData.RiskManagerUserLevelPermissionsList[i].LevelPermissionID + "']").prop("selected", true);
                    if (selectedUserData.RiskManagerUserLevelPermissionsList[i].LevelPermissionID == 1 || selectedUserData.RiskManagerUserLevelPermissionsList[i].LevelPermissionID == 6)
                        $("#RiskManagerModulesPermissions").multiselect('disable');
                }
            }

            // Set Business Card Permissions

            if (selectedUserData.BusinessCardsModulesPermissionsList != null) {
                for (var i = 0; i < selectedUserData.BusinessCardsModulesPermissionsList.length; i++) {
                    $("#BusinessCardModulesPermissions option[value='" + selectedUserData.BusinessCardsModulesPermissionsList[i].ID + "']").prop("selected", true);
                }
                $("#BusinessCardModulesPermissions").multiselect('refresh');
            }

            // Set DAC 6 Permissions
            if (selectedUserData.DAC6UserLevelPermissionsList != null) {
                for (var i = 0; i < selectedUserData.DAC6UserLevelPermissionsList.length; i++) {
                    $("#DAC6UserLevelPermissions option[value='" + selectedUserData.DAC6UserLevelPermissionsList[i].LevelPermissionId + "']").prop("selected", true);
                }
                checkPermissionsDAC6();
                // $("#DAC6ModulesPermissions").multiselect('refresh');
            }
            if (selectedUserData.DAC6ModulesPermissionsList != null) {
                for (var i = 0; i < selectedUserData.DAC6ModulesPermissionsList.length; i++) {
                    $("#DAC6ModulesPermissions option[value='" + selectedUserData.DAC6ModulesPermissionsList[i].ID + "']").prop("selected", true);
                }
                $("#DAC6ModulesPermissions").multiselect('refresh');
            }
            $("#TrainingCoursesUserLevelPermissions").val(response.TCLevelPermissionId);
            $(".mainDiv").css("height", $(".userDataDiv").height() - 40);
            $("#ajaxLoader").modal("hide");
        }
    });
};

QueryString = function (key) {
    key = key.replace(/[*+?^$.\[\]{}()|\\\/]/g, "\\$&"); // escape RegEx control chars
    var match = location.search.match(new RegExp("[?&]" + key + "=([^&]+)(&|$)"));
    return match && decodeURIComponent(match[1].replace(/\+/g, " "));
};

$(document).off('click', '#reset-password').on('click', '#reset-password', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var userName = $("#username").val();
        ShowConfirmationDialog('Reset ', 'Are you sure you want to send Reset Password email to this user?', function () {
            //return false;
            var requestPerameters = {
                "userName": userName
            };
            var url = 'AddEditUser.aspx/SendResetPasswordLink';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response == "link sent") {
                    toastr.success("Reset password email has been sent successfully.");
                }
                else {
                    toastr.error("Password reset failed. Please check SMTP credetnials.");
                }
            });
        }, null, 'Reset');
    } catch (e) {
        console.log('Exception while trying to delete an user: ' + e.message);
    }
});

function checkPermissions() {

    $("#RiskManagerModulesPermissions option[value='5']").prop("selected", false);
    $("#RiskManagerModulesPermissions option[value='5']").prop("disabled", false);

    var accessLevel = $('#RiskManagerUserLevelPermissions').val();
    debugger;
    if (parseFloat(accessLevel) == 6) {
        $('#RiskManagerModulesPermissions option').prop('selected', false);
        $("#RiskManagerModulesPermissions option[value='1']").prop("selected", true);
        $("#RiskManagerModulesPermissions option[value='5']").prop("selected", true);
        $('#RiskManagerModulesPermissions').multiselect("disable", true);
        $("#RiskManagerModulesPermissions").multiselect('refresh');
    }
    else if (parseFloat(accessLevel) == 1 || parseFloat(accessLevel) == 2) {
        $('#RiskManagerModulesPermissions option').prop('selected', true);
        $('#RiskManagerModulesPermissions').multiselect("enable");
        $("#RiskManagerModulesPermissions").multiselect('refresh');
    }
    else if (parseFloat(accessLevel) == 3 || parseFloat(accessLevel) == 4 || parseFloat(accessLevel) == 5) {
        $("#RiskManagerModulesPermissions option[value='5']").prop("selected", false);
        $("#RiskManagerModulesPermissions option[value='5']").prop("disabled", true);
        $('#RiskManagerModulesPermissions').multiselect("enable");
        $("#RiskManagerModulesPermissions").multiselect('refresh');
    }

    else {

        $('#RiskManagerModulesPermissions option').prop('selected', false);
        $("#RiskManagerModulesPermissions option[value='1']").prop("selected", true);
        $('#RiskManagerModulesPermissions').multiselect("enable");
        $("#RiskManagerModulesPermissions").multiselect('refresh');
    }

}

function checkPermissionsDAC6(fromDropDown = 0) {

    var accessLevel = $('#DAC6UserLevelPermissions').val();

    $('#DAC6ModulesPermissions option').prop('selected', false);

    $("#DAC6ModulesPermissions option[value='1']").prop("selected", true);
    $("#DAC6ModulesPermissions option[value='1']").prop("disabled", true);
    $("#DAC6ModulesPermissions option[value='2']").prop("selected", true);
    $("#DAC6ModulesPermissions option[value='2']").prop("disabled", true);



    if (parseInt(accessLevel) != 4) {
        $("#DAC6ModulesPermissions option[value='3']").prop("selected", false);
    }
    else {
        $("#DAC6ModulesPermissions option[value='3']").prop("selected", true);

    }
    $("#DAC6ModulesPermissions option[value='3']").prop("disabled", true);


    if (fromDropDown) {
        $("#DAC6ModulesPermissions").multiselect('refresh');
    }
}