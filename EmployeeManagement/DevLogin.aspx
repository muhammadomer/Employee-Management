<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevLogin.aspx.cs" Inherits="EmployeeManagement.DevLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 0)
               {  %>
        Employee Management - Login
        <% }
    else
    { %>
        MITIGATE PRO - Login
                <% }%></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="css/bootstrap.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/toastr.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/index.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/uniform.default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/components.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/plugins.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/layout.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/custom.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <style>
        .form-control.form-control-solid {
            border: 2px solid;
        }

        body {
            overflow-y: hidden;
            background-color: #fff !important;
        }

        #pointer {
            text-align: unset;
            padding-bottom: 0px;
            left: 24.7%;
            top: -40px;
            position: absolute;
            background-color: #DBDBDB;
            width: 60.5%;
        }

            #pointer a {
                text-decoration: none;
                color: #817F84;
                font-size: 13px;
                padding: 5px 0px;
                float: right;
                margin-right: 25px;
            }

        #loginView {
            background-color: #ffffff;
            opacity: 0.9;
            margin-top: 25px;
            width: 310px;
            margin-left: 10px;
            padding-left: 0px;
            padding-bottom: 50px;
            padding-right: 0px;
            border-radius: 15px !important;
        }

        #welcomdiv1 {
            display: none;
        }

        #welcomdiv {
            /*width:71.666666%*/
        }

        .image-container {
        }

        .image-grey {
            display: block;
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: -1;
        }

        .image-container:after {
            position: absolute;
            content: "";
            top: 509px;
            left: 0;
            width: 100%;
            height: 100%;
            -webkit-filter: grayscale(100%);
            background: url('images/image.png') no-repeat fixed 0%;
            background-position-y: 84px;
            background-size: 100% 100%;
        }

        #imgLogo {
            text-align: unset;
            padding-bottom: 0px;
            left: 13.3%;
            top: -40px;
            position: absolute;
        }

        #test {
            background-color: white;
            margin-top: 80px;
            padding-bottom: 3px;
        }

        .form-control {
            color: black !important;
            border: solid 1px #B7C3C3 !important;
        }

        .carousel-caption .child {
            display: table-cell;
            vertical-align: middle;
            width: 650px;
        }

        .carousel-caption {
            text-align: left;
            padding-bottom: 0;
            right: 12%;
            height: 429px;
            display: table;
        }

        .modal-open {
            overflow-y: hidden !important;
        }

        @media (max-width: 1230px) {
            #imgLogo {
                left: 3.3%;
            }

            .carousel-caption {
                left: 5%;
            }
        }

        @media (max-width: 991px) {

            body {
                overflow-y: auto;
                overflow-x: hidden;
            }

            .image-container:after {
                position: unset;
            }

            #welcomdiv {
                display: none;
            }

            .image-grey {
                -webkit-filter: grayscale(100%);
            }

            #welcomdiv1 {
            }

            #loginView {
                margin: 0 auto;
                margin-top: 20px;
                margin-bottom: 20px;
            }

            .mainouter {
                width: unset !important;
            }

            .content {
                width: unset !important;
            }

            #footerDiv {
                display: none;
            }

            #imgLogo {
                left: 3% !important;
            }

            #pointer {
                width: 60%;
            }

                #pointer a {
                }

            .login-button-group {
                margin-bottom: 0px;
            }
        }

        @media screen and (max-width: 575px) {
            #pointer {
                display: none;
            }

            #imgLogo {
                text-align: center;
                padding-bottom: 0px;
                left: 13.3%;
                top: -40px;
                position: unset;
            }

            #test {
                margin-top: 2%;
            }

            .login-button-group {
                margin-bottom: 0px;
            }
        }
    </style>
</head>
<body>
     <div class="mainouter container-fluid" id="mainContanier1" runat="server">
        <div class="content">
            <form id="Form" class="login-form" runat="server">

                <asp:HiddenField ID="forgeryToken" runat="server"/>
                <div id="AFDiv" runat="server"></div>

              
                <div class="row image-container">
                    <img src="images/Login_Background.png" alt="" class="image-grey" />
                    <div class="col-md-8" id="welcomdiv" style="padding-left: 0px;">
                        <img src="images/text-backgroud-image.png" style="width: 100%; height: 425px;" class="img-responsive text-background-image" />
                        <div class="carousel-caption">
                           
                        </div>
                    </div>

                    <div class="col-md-8" id="welcomdiv1" style="color: white; background-color: #672a86c7; padding: 30px;">
                        <div>
                            <h1 style="font-size: 55px;">Single Point</h1>
                            <h2 style="font-size: 34px;">Software Access Portal</h2>
                            <p style="font-size: 18px;">This Software Portal provides access to the Dentons software management suite. To access this portal you will need to contact your system administrator in order to obtain a username and password.</p>
                        </div>
                    </div>
                    
                    <div class="col-md-3" id="loginView" style="display:block;" >

                        <div id="ErroeMessageRow" class="row" style="text-align: center; vertical-align: middle; line-height: 10px; height: 15px; padding-top: 30px; padding-bottom: 30px;">
                            <div class="col-md-12" style="padding-left: 0px; padding-right: 0px; text-align: center">
                                <label id="lblErrorMessage" style="color: red"></label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-10">
                                <div class="form-group">
                                    <label class="label" style="color: #672A87; font-size: 27px; padding: 0;">Login</label>
                                </div>

                            </div>
                            <div class="col-md-1">
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-10">
                                <div class="form-group" style="background-color: #edeef2;">
                                    <input id="txbUserName" class="form-control form-control-solid placeholder-no-fix" type="text" autocomplete="off" placeholder="Username" name="username" />
                                    <%--<div class="input-icon">
                                       <i class="fa fa-user"></i>
									</div>--%>
                                </div>
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-10">
                                <div class="form-group" style="background-color: #edeef2;">
                                    <input id="txbPassword" class="form-control form-control-solid placeholder-no-fix" type="password" autocomplete="off" placeholder="Password" name="password" />
                                    <%--<div class="input-icon">
										<i class="fa fa-lock"></i>
									</div>--%>
                                </div>
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){
                            %>
                            <div class="row">
                                <div class="col-md-1">
                                </div>
                                <div class="col-md-10">
                                    <div class="form-group" style="background-color: #edeef2;">
                                        <input id="txbAccountId" class="form-control form-control-solid placeholder-no-fix" type="text" autocomplete="off" placeholder="Account Id" name="accountID" />
                                    </div>
                                </div>
                                <div class="col-md-1">
                                </div>
                            </div>
                        <%}%>

                        <div class="row" style="text-align: center; vertical-align: middle;">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-10 login-button-group">
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        <input type="checkbox" style="float: left; margin-top: 2px;" /><label style="color: #4F5E73; font-size: 13px;" class="lblrememberme">Remember me</label>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 text-right">
                                        <a href="ForgotPassword.aspx" style="text-decoration: none; color: #4F5E73" class="center-block">Forgot password?</a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row" style="text-align: center; vertical-align: middle; margin-top: 24px;">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-10 login-button-group">
                                <a class="btn green" id="btnLogin" style="width: 100%; background-color: #672A87">Continue</a>
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row" style="text-align: center; vertical-align: middle; margin-top: 10px;">
                            <div class="col-md-12">
                                <label id="lblCopyRights1" runat="server" style="color: #4F5E73; font-size: 11px"></label>
                                <label id="lblVersion1" runat="server" style="color: #4F5E73; font-size: 11px"></label>
                            </div>
                        </div>
                    </div>

                    
                </div>

            </form>
            <div id="overlay" class="hidden"></div>
        </div>
    </div>
    <div class="modal fade" id="verificationcodemodal" tabindex="-1" role="dialog" data-keyboard="false" data-backdrop="static" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="border-bottom: 1px solid #e5e5e5;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Two-factor authentication required</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-4" style="text-align: center">
                            <img style="max-width: 100%; max-height: 140px;" src="images/lock.png" />
                        </div>
                        <div class="col-md-8">
                            <div class="form-group">
                                <p id="lblEmailVerificationDesc"></p>
                                <label class="control-label" id="lblForVericationCode"></label>
                                <input autocomplete="off" style="width: 150px;" class="form-control" id="txtVerificationCode" placeholder="Login code" name="Code" type="text" value="" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a href="#" class="pull-left" style="margin-top: 7.5px;" id="linkResetTwoFA">Need to authenticate via email?</a>
                    <a id="btnVerifyCode" class="btn purple">Verify</a>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ajaxLoader" tabindex="-1" role="dialog" data-keyboard="false" data-backdrop="static" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <img src="images/ajax-loader.gif" alt="" class="loading" />
                </div>
            </div>
        </div>
    </div>
</body>
    <script src="Scripts/jquery.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
<script src="Scripts/bootstrap.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
<script src="Scripts/toastr.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
<script src="Scripts/jquery.validate.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
<script src="ApplicationScripts/AjaxCommonMethods.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>

    
<script>

    $("body").keypress(function (event) {
        if (event.which == 13) {
        }
    });

    window.onresize = function (event) {
        ChangeWindowsSize();
    };

    function ChangeWindowsSize() {
        $("#imgLogo").css("left", function () {
            $("#pointer").css("left", parseInt($("#imgLogo").css("left")) + 152);

            if (parseInt($("body").css("width")) <= 991)
                $("#pointer").css("width", parseInt($("body").css("width")) - 50 - parseInt($("#pointer").css("left")));
            else {
                var position = $("#loginView").position();
                var pointWidth = parseInt(position.left) + parseInt($("#loginView").css("width")) - parseInt($("#pointer").css("left")) - 18;

                $("#pointer").css("width", pointWidth);
            }
        });
        var screenSize = $(window).width();

        $(".image-grey").attr("src", "images/Login_Background.png");
        if (screenSize < 992) {
            $(".image-grey").attr("src", "images/image.png");
        }
    }

    $(document).ready(function () {

        var windowHeight = 75;
        $("#test").css("margin-top", windowHeight);
        $('<style> .image-container:after {top:' + (509 + windowHeight - 80) + 'px !important;background-position-y:' + (84 + windowHeight - 60) + 'px !important;} </style>').appendTo($('head'));

        ChangeWindowsSize();

        $(".image-grey").attr("src", "images/Login_Background.png");

        $("#imgLogo").css("left", function () {
            $("#pointer").css("left", parseInt($("#imgLogo").css("left")) + 152);
        });

        $("#linkResetTwoFA").on("click", function () {
            var userName = $("#txbUserName").val();
            var password = $("#txbPassword").val();
            if (userName == "") {
                toastr.error("Please Provide Username");
                return false;
            }
            else if (password == "") {
                toastr.error("Please Provide Password");
                return false;
            }
            $("#ajaxLoader").modal("show");
            var requestPerameters = {
                "userName": userName,
                "password": password,
            };
            var url = 'DevLogin.aspx/Reset2FA';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    $("#ajaxLoader").modal("hide");
                    $("#lblForVericationCode").text("Enter Email Verification Code");
                    toastr.success("Successfully Reset 2FA you enter email verification code for login");
                }
                else {
                    $("#ajaxLoader").modal("hide");
                    toastr.error("Unable to reset 2FA");
                }
            });
        });

        $("#btnVerifyCode").on("click", function () {
            VerifyCodeOnLogin();
        });

        var txbPasswordID = document.getElementById("txbPassword");
        var txbUsernamID = document.getElementById("txbUserName");
        var txbAccountID = document.getElementById("txbAccountId");
        var txtVerificationCodeID = document.getElementById("txtVerificationCode");

        txtVerificationCodeID.addEventListener("keydown", function (e) {
            if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
                VerifyCodeOnLogin();
            }
        });

        txbPasswordID.addEventListener("keydown", function (e) {
            if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
                LoginUser();
            }
        });

        txbUsernamID.addEventListener("keydown", function (e) {
            if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
                LoginUser();
            }
        });

        txbAccountID.addEventListener("keydown", function (e) {
            if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
                LoginUser();
            }
        });

    });

    $("#btnLogin").on("click", function () {
        LoginUser();
    });

    VerifyCodeOnLogin = function () {
        var verificationCode = $("#txtVerificationCode").val();
        var userName = $("#txbUserName").val();
        var password = $("#txbPassword").val();
        var accountID = "";

         <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
        accountID = $("#txbAccountId").val();
        <%}%>

        if (userName == "") {
            toastr.error("Please provide Username");
            return false;
        }
        else if (password == "") {
            toastr.error("Please provide Password");
            return false;
        }
        <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
        else if (accountID == "") {
            toastr.error("Please provide Account Id");
            return false;
        }
        <%}%>
        else if (verificationCode == "") {
            toastr.error("Please provide 2FA verification code");
            return false;
        }
        $("#ajaxLoader").modal("show");
        var requestPerameters = {
            "userName": userName,
            "password": password,
            "accountID": accountID,
            "code": verificationCode
        };
        var url = 'DevLogin.aspx/VerifyCodeOnLogin';
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response == "3") {
                $("#ajaxLoader").modal("hide");
                location.href = "Employees.aspx"
            }
            else if (response == "4") {
                $("#ajaxLoader").modal("hide");
                location.href = "Applications.aspx"
            }
            else {
                $("#ajaxLoader").modal("hide");
                toastr.error(response);
            }
        });
    };

    LoginUser = function () {

        var userName = $("#txbUserName").val();
        var password = $("#txbPassword").val();
        var accountID = "";

        <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
        accountID = $("#txbAccountId").val();
        <%}%>

        if (userName == "") {
            toastr.error("Please Provide Username");
            return false;
        }
        else if (password == "") {
            toastr.error("Please Provide Password");
            return false;
        }
        <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
        else if (accountID == "") {
            toastr.error("Please Provide Account Id");
            return false;
        }
        <%}%>

        var requestPerameters = {
            "userName": userName,
            "password": password,
            "accountID": accountID
        };
        $("#ajaxLoader").modal("show");
        var url = 'DevLogin.aspx/LoginUser';
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            $("#lblEmailVerificationDesc").text("");
            if (response == "1") {
                $("#ajaxLoader").modal("hide");
                $("#lblForVericationCode").html("Enter the 6-digit code from your <b>code generator</b> or third-party app below.");
                $("#linkResetTwoFA").removeClass("hidden");
                $("#verificationcodemodal").modal("show");
            }
            else if (response == "5") {
                $("#ajaxLoader").modal("hide");
                $("#linkResetTwoFA").addClass("hidden");
                $("#lblForVericationCode").text("Enter Email Verification Code");

                var token = $('[name=__RequestVerificationToken]').val();

                $.ajax({
                    type: "POST",
                    url: 'DevLogin.aspx/GetUserEmail',
                    headers: { '__RequestVerificationToken': token },
                    data: JSON.stringify(requestPerameters),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data != null) {
                            if (data.d != '' && data.d != null && data.d != "null") {
                                dataToReturn = data.d;
                                $("#lblEmailVerificationDesc").text("An email has been sent to " + dataToReturn + " which contains an authentication code which you will need to login.");
                            }
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
                    }
                });

                $("#verificationcodemodal").modal("show");
            }
            else if (response == "2") {
                $("#ajaxLoader").modal("hide");
                location.href = "ChangePassword.aspx"
            }
            else if (response == "3") {
                $("#ajaxLoader").modal("hide");
                location.href = "Employees.aspx"
            }
            else if (response == "4") {
                $("#ajaxLoader").modal("hide");
                location.href = "Applications.aspx"
            }
            else if (response.indexOf("6:") > -1) {

                $("#ajaxLoader").modal("hide");
                location.href = "ManageApplication.aspx?appID=" + response.split(':')[1];
            }
            else if (response == "6") {
                $("#ajaxLoader").modal("hide");
                location.href = "ManageApplication.aspx?appID=2";
            }
            else {
                $("#ajaxLoader").modal("hide");
                if (response.trim() != "")
                    toastr.error(response);
            }
            //else if (response == "6") {
            //    $("#ajaxLoader").modal("hide");
            //    location.href = "ManageApplication.aspx?appID=4";
            //}
            //else {
            //    $("#ajaxLoader").modal("hide");
            //    toastr.error(response);
            //}
        });
    };

</script>

</html>
