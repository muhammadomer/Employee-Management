var inc = 0;
var uploadImage = false;
var removeImage = false;

$(document).ready(function () {
    setHeartbeat();
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

    $('#newPassword').pwstrength(options);
    GetUserProfile();
    GetAllUserApplications();

    $("#ShowChangePasswordModalPopup").on("click", function () {
        $("#ModalChangePassword").modal("show");
    });

    $("#btnChangePassword").on("click", function () {
        var oldPassword = $("#oldPassword").val();
        var newPassword = $("#newPassword").val();
        var confirmNewPassword = $("#confirmNewPassword").val();

        var checkOldPassword = htmlTagsValidation(oldPassword);
        var checkNewPassword = htmlTagsValidation(newPassword);
        var checkConfirmNewPassword = htmlTagsValidation(confirmNewPassword);

        if (checkOldPassword == false && checkNewPassword == false && checkConfirmNewPassword == false) {

            var passwordValidation = validateUserPasswordWithConfirmPassword(newPassword, confirmNewPassword, 'NULL');
            if (oldPassword == '') {
                toastr.error("Please provide Old Password.");
                return false;
            }
            else if (newPassword == '') {
                toastr.error("Please provide New Password.");
                return false;
            }
            else if (confirmNewPassword == '') {
                toastr.error("Please provide Confirm Password");
                return false;
            }
            else if (newPassword != confirmNewPassword) {
                toastr.error("The passwords you supplied do not match.");
                return false;
            }
            else if (passwordValidation != 'true') {
                toastr.error(passwordValidation);
                return false;
            }
            var changePasswordURL = "Login.aspx/ChangePassword";
            var requestPerameters = {
                "oldPassword": oldPassword,
                "newPassword": newPassword,
                "confirmNewPassword": confirmNewPassword
            };
            AjaxPostRequestWithRequestPerameters(changePasswordURL, requestPerameters, function (response) {
                if (response == "Password changed sucessfully") {
                    toastr.success(response);
                    $("#ModalChangePassword").modal("hide");
                }
                else {
                    toastr.error(response);
                }
            });

        }
        else {
            toastr.error(specialCharacterError);
        }
        
    });

    $("#logout").on("click", function () {
        var url = 'Applications.aspx/LogOut';
        AjaxPostRequestWithoutRequestPerameters(url, function (response) {
            if (response != null) {
                window.location.href = response;
            }
            else {
                window.location.href = "Login.aspx";
            }
        });
    });

    $('#ModalChangePassword').on('hidden.bs.modal', function () {
        $("#oldPassword").val('');
        $("#newPassword").val('');
        $("#confirmNewPassword").val('');
    });
    $(".btnEnableAuthentication").on("click", function () {
        $("#ajaxLoader").modal("show");
        if ($(".btnEnableAuthentication").text().trim() == "Disable Authenticator") {
            var url = 'ManageAccount.aspx/DisableGoogleAuthenticator';
            AjaxPostRequestWithoutRequestPerameters(url, function (response) {
                if (response) {
                    $("#ajaxLoader").modal("hide");
                    $(".btnEnableAuthentication").text("Enable Authenticator");
                    toastr.success("2FA Disabled successfully");
                }
                else {
                    $("#ajaxLoader").modal("hide");
                    toastr.error("Unable to Disable 2FA");
                }
            });
        }
        else {
            var url = 'ManageAccount.aspx/EnableGoogleAuthenticator';
            AjaxPostRequestWithoutRequestPerameters(url, function (response) {
                if (response != null) {
                    if (response[0] != '2') {
                        $("#SecretKey").val(response[0]);
                        $("#BarcodeUrl").val(response[1]);
                        $("#sharedKey").text(response[2]);
                        $("#qrcode").empty();
                        var qrcode = new QRCode("qrcode", {
                            text: response[1],
                            width: 180,
                            height: 180,
                            colorDark: "#000",
                            colorLight: "#ffffff",
                            correctLevel: QRCode.CorrectLevel.H
                        });
                        $("#Code").focus();
                        $("#ajaxLoader").modal("hide");
                        $("#EableAuthenticator").modal("show");
                    }
                    else {
                        $("#ajaxLoader").modal("hide");
                        toastr.error("2FA is already enabled");
                    }
                }
            });
        }
    });
    $("#btnVerifyCode").on("click", function () {
        var secretKey = $("#SecretKey").val();
        var code = $("#txtCode").val();
        if (code == "") {
            toastr.error("Please Provide Verification Code");
            return false;
        }
        $("#ajaxLoader").modal("show");
        var url = 'ManageAccount.aspx/EnableGoogleAuthenticatorWithVerificationCode';
        var requestPerameters = {
            "Code": code,
            "SecretKey": secretKey
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response != null) {
                $("#qrcode").empty();
                $("#txtCode").val('');
                $("#ajaxLoader").modal("hide");
                $('#EableAuthenticator').modal('hide');
                toastr.success("2FA enabled successfully. ");
                $('.btnEnableAuthentication').text("Disable Authenticator");
            }
            else {
                $("#ajaxLoader").modal("hide");
                toastr.error("Invalid 2FA Code");
            }
        });
    });

    $("#ShowChangeProfileModalPopup").on("click", function () {
        GetUserProfile();
        $("#ModalChangeProfile").modal("show");
    });

    GetProfileImage();
});

GetAllUserApplications = function () {
    var url = 'Applications.aspx/GetAllUserApplication';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        if (response != null) {
            var applications = '';
            var divVal = '';
            for (var i = 0; i < response.length; i++) {
                if (response[i].UserID != '') {

                    var LabelText = response[i].Name;
                    var cursorStyle = "";
                    var URLLink = "ManageApplication.aspx?appID=" + response[i].ID;

                    try {
                        if (response[i].LicenseText != '') {
                            LabelText += "<br/><span style='font-size:10px;'>(" + response[i].LicenseText + ")</span>";
                            cursorStyle = " style='cursor:default;' ";
                        }
                        //applications += "<a href='" + URLLink + "' target='_blank'><div class='tile bg-purple-studio'><div class='tile-body'><i class='" + response[i].Icon + "'></i></div><div class='tile-object'><div class='name'>" + LabelText + "</div><div class='number'></div></div></div></a>";
                        divVal = "<div class='tile bg-purple-studio' " + cursorStyle + "><div class='tile-body'><i class='" + response[i].Icon + "'></i></div><div class='tile-object'><div class='name'>" + LabelText + "</div><div class='number'></div></div></div>";
                        //if (response[i].LicenseText == '' || response[i].LicenseText.indexOf('Trial') >= 0) {
                        if (true) {
                            applications += "<a href='" + URLLink + "' target='_blank'>" + divVal + "</a>";
                        }
                        else {
                            applications += "<div>" + divVal + "</div>";
                        }
                    } catch{ }


                    //Incase of compendium
                    try {
                        var appID = response[i].ID;
                        $("#app" + appID).removeClass().addClass("appLogoImages").attr('onClick', 'OpenApplication(' + appID + ');');
                    } catch{ }



                }
                //else {
                //    applications += "<a href='#'><div class='tile bg-purple-studio'><div class='tile-body disable-tile'><i class='" + response[i].Icon + "' ></i></div><div class='tile-object'><div class='name disable-name'>" + response[i].Name + "</div><div class='number'></div></div></div></a>";
                //}
            }
            $("#applicationsTiles").append(applications);
        }
    });
};

GetProfileImage = function () {
    var url = 'Employees.aspx/GetProfileImage';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {
        
        if (response != "" && response != null) {
            $("#menuProfileImage").attr("src", response);
        }
        else {
            $("#menuProfileImage").attr("src", "images/user.png");
        }
    });
    
};

GetUserProfile = function () {
    var url = 'Applications.aspx/GetUserProfile';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {
        $('#usrID').val(response.ID);
        $('#usrFName').val(response.First_Name);
        $('#usrLastName').val(response.Last_Name);
        $('#usrDirectNum').val(response.Direct_Number);
        $('#usrMobileNum').val(response.Mobile_Number);
        $('#usrFaxNum').val(response.Fax_Number);
        $("#usrImage").attr("src", response.ProfileImage);
        $(".userName").html(response.First_Name + " " + response.Last_Name);
    });
};

$("#removeProfileImage").click(function () {
    $("#usrImage").attr("src", "images/user.png");
    $("#removeProfileImage").addClass("disabled");
    removeImage = true;
    uploadImage = false;
});

$("#uploadProfileImage").click(function () {
    $.FileDialog({ multiple: false }).on('files.bs.filedialog', function (ev) {

        var files = ev.files;
        var data = new FormData();
        if (files.length > 0) {

            for (var i = 0; i < files.length; i++) {
                if (files[i].name.toLowerCase().endsWith(".jpg") || files[i].name.toLowerCase().endsWith(".png") || files[i].name.toLowerCase().endsWith(".jpeg")) {
                    data.append(files[i].name, files[i]);
                }
                else {
                    toastr.error("Only jpg, jpeg and png file types allowed");
                    return;
                }
            }
            $("#ajaxLoader").modal("show");

            var token = $('[name=__RequestVerificationToken]').val();

            $.ajax({
                url: "ProfileImageUploader.ashx",
                headers: { '__RequestVerificationToken': token },
                type: "POST",
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result != "2") {
                        $("#usrImage").attr("src", result);
                        $("#removeProfileImage").removeClass("disabled");
                        $("#ajaxLoader").modal("hide");
                    }
                    else {
                        $("#ajaxLoader").modal("hide");
                        toastr.error("Please provide jpg, jpeg and png images only.");
                    }
                    //location.reload();
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

ValidateNumber = function (number) {
    if (number != '') {
        var testPattern = new RegExp("^\\+[0-9 ]+$");
        var result = testPattern.test(number);
        return result;
    }
}

UserFormValidation = function () {

    var usrFName = $("#usrFName").val();
    var usrLastName = $("#usrLastName").val();
    var usrDirectNum = $("#usrDirectNum").val();
    var usrMobileNum = $("#usrMobileNum").val();
    var usrFaxNum = $("#usrFaxNum").val();

    if (usrFName == '') {
        toastr.error('Please provide first name.');
        return false;
    }
    else if (usrLastName == '') {
        toastr.error('Please provide last name.');
        return false;
    }
    else if (usrDirectNum.length > 18) {
        toastr.error('Direct number is too large.');
        return false;
    }
    else if (!ValidateNumber(usrDirectNum) && usrDirectNum != "") {
        toastr.error('The direct number provided is not valid.');
        return false;
    }
    else if (usrMobileNum.length > 18) {
        toastr.error('Mobile number is too large.');
        return false;
    }
    else if (!ValidateNumber(usrMobileNum) && usrMobileNum != "") {
        toastr.error('The mobile number provided is not valid.');
        return false;
    }
    else if (usrFaxNum.length > 18) {
        toastr.error('Fax number is too large.');
        return false;
    }
    else if (!ValidateNumber(usrFaxNum) && usrFaxNum != "") {
        toastr.error('The fax number provided is not valid.');
        return false;
    }

    return true;
};

Edit_User = function () {
    
    var usrID = $("#usrID").val();
    var usrFName = $("#usrFName").val();
    var usrLastName = $("#usrLastName").val();
    var usrDirectNum = $("#usrDirectNum").val();
    var usrMobileNum = $("#usrMobileNum").val();
    var usrFaxNum = $("#usrFaxNum").val();

    var checkUsrFName = htmlTagsValidation(usrFName);
    var checkUsrLastName = htmlTagsValidation(usrLastName);
    var checkUsrDirectNum = htmlTagsValidation(usrDirectNum);
    var checkUsrMobileNum = htmlTagsValidation(usrMobileNum);
    var checkUsrFaxNum = htmlTagsValidation(usrFaxNum);

    if (checkUsrFName == false && checkUsrLastName == false && checkUsrDirectNum == false && checkUsrMobileNum == false && checkUsrFaxNum == false) {

        var url = 'Applications.aspx/Update_User';

        var userData = {
            "ID": usrID,
            "First_Name": usrFName,
            "Last_Name": usrLastName,
            "Direct_Number": usrDirectNum,
            "Mobile_Number": usrMobileNum,
            "Fax_Number": usrFaxNum,
            "UploadImage": uploadImage,
            "RemoveImage": removeImage
        };
        var requestPerameters = {
            "userEntity": userData
        };
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response == 'Employee updated successfully.') {
                toastr.success(response);
                GetProfileImage();
                $("#ModalChangeProfile").modal("hide");
            }
            else {
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

$("#btnChangeProfile").on("click", function () {
    
    if (UserFormValidation()) {
            Edit_User();
    }

});

function getCropedImage() {
    uploadImage = true;
    removeImage = false;
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
                if (result != "" && result != null) {
                    $("#usrImage").attr("src", "");
                    $("#usrImage").attr("src", result);
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
                $("#image").cropper("destroy");
                //$('#loadUserImageModal').modal("hide");
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
        toastr.error("Only jpg, jpeg and png file types allowed.");
        return;
    }

}

function OpenApplication(appId) {
    var URLLink = "ManageApplication.aspx?appID=" + appId;
    window.open(URLLink, "_blank");
}