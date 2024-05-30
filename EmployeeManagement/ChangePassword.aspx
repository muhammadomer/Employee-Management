<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="EmployeeManagement.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 0)
               {  %>
        Employee Management 
        <% }
    else
    { %>
        MITIGATE PRO
                <% }%></title>
    <link href="css/toastr.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/login1.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/bootstrap.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/uniform.default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/components.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/plugins.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/layout.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/custom.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <script src="Scripts/jquery.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/bootstrap.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/toastr.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/pwstrength-bootstrap.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/jquery.validate.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/AjaxCommonMethods.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/security-PasswordValidations.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script>
        $(document).ready(function () {

            $("#userFeedback").hide();
            var options = {};
            options.common = {
                minChar: 8
            };
            options.rules = {
                activated: {
                    wordTwoCharacterClasses: true,
                    wordRepetitions: true,
                    wordUppercase: true
                },
                scores: { wordUppercase: 5 }
            };
            options.ui = {
                //showErrors: true,
                //showStatus: true
            };
            $('#txbPassword').pwstrength(options);
            AdjustColumnHeight();
            $(window).resize(function () {
                AdjustColumnHeight();
            });
        });

        function AdjustColumnHeight() {
            var windowWidth = $(window).width();
            console.log('Window width ' + windowWidth);
            if (windowWidth > 972) {
                $('#changePasswordBody').height($('#safetyTips').height());
            } else {
                $('#changePasswordBody').height('auto');
            }
            if (windowWidth > 972 && windowWidth < 1030) {
                $('#buttonContainer').css('padding-top', '20%');
            }
            else if (windowWidth >= 1030 && windowWidth < 1046) {
                $('#buttonContainer').css('padding-top', '18%');
            }
            else if (windowWidth >= 1046 && windowWidth < 1086) {
                $('#buttonContainer').css('padding-top', '16%');
            }
            else if (windowWidth >= 1086 && windowWidth < 1112) {
                $('#buttonContainer').css('padding-top', '14%');
            }
            else {
                $('#buttonContainer').css('padding-top', '');
            }
        }
        function ChangePassword() {
            //debugger;
            var userPassword = $("#txbPassword").val();
            var userConfirmPassword = $("#txbConfirmPassword").val();
            var userName = $("#hiddenUserName").val();
            var userId = $("#hiddenUseId").val();
            var passwordValidation = validateUserPasswordWithConfirmPassword(userPassword, userConfirmPassword, userName);
            if (passwordValidation == "true") {
                $("#userFeedback").hide();
                var param = { "password": userPassword, "userId": userId };

                var token = $('[name=__RequestVerificationToken]').val();

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    headers: { '__RequestVerificationToken': token },
                    dataType: "json",
                    url: "ChangePassword.aspx/ResetPassword",
                    data: JSON.stringify(param),
                    success: function (data) {
                        if (data.d == "Password changed sucessfully") {
                            toastr.success("Password changed successfully.");
                            window.setTimeout(function () {
                                window.location.href = 'Login.aspx';
                            }, 3000);
                        }
                        else {
                            $("#userFeedback").html(data.d);
                            $("#userFeedback").show();
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
            }
            else {
                $("#userFeedback").html(passwordValidation);
                $("#userFeedback").show();
            }
        }

    </script>

    <style>
        .progress {
            margin-bottom: 0px !important;
        }

        #backToLogin {
            color: #4F5E73;
            text-decoration: none;
        }

            #backToLogin:hover {
                color: #4F5E73;
                text-decoration: none;
            }

        .image-grey {
            display: block;
            position: fixed;
            width: 100%;
            height: 100%;
            -webkit-filter: grayscale(100%);
            z-index: -1;
        }

        .purple {
            color: #fff;
            background-color: #6E2D91 !important;
            border-color: #6E2D91 !important;
            font-size: 13px;
        }
    </style>
         <% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
        {  %>

    <link href="css/otherApp.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />

    <% } %>
</head>
<body class="page-header-fixed page-quick-sidebar-over-content page-style-rounded">

    <%--<div class="page-header navbar navbar-fixed-top">
        <div class="page-header-inner">
            <div class="page-logo">
                <a href="#" id="link" runat="server">
                    <asp:Image ID="LogoImage" runat="server" CssClass="logo-default" />
                </a>
                <div class="menu-toggler sidebar-toggler hide">
                </div>
            </div>
            <a href="javascript:;" class="menu-toggler responsive-toggler" data-toggle="collapse" data-target=".navbar-collapse"></a>

            <div class="top-menu">
                <ul class="nav navbar-nav pull-right" style="margin-right: 0px; padding-top: 0px !important">
                    <li class="dropdown dropdown-user" id="logOutOption" runat="server">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true" style="padding-right: 0px">
                            <i class="icon-user" style="font-size: 20px"></i>
                            <input type="hidden" id="currentUserName" runat="server" />
                            <span class="username username-hide-on-mobile" id="UserName" runat="server"></span>
                            <i class="icon-angle-down"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-default">
                            <li>
                            </li>
                            <li>
                                <a onclick="LogOut()"><i class='icon-key'></i>Log Out </a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </div>--%>
    
    <form runat="server">
        <asp:HiddenField ID="forgeryToken" runat="server"/>
        <div id="AFDiv" runat="server"></div>
    </form>
    
    
    <div class="clearfix">
    </div>
    <div class="page-container" style="margin-top:0px;">
        <div class="row" style="margin: 15px 30px">
            <div class="col-md-12" style="padding: 15px;color: #fff;font-size: 20px;" id="changePasswordCaption" runat="server">
            </div>
        </div>
        <input type="hidden" value="<%= Session["UserName"] %>" id="hiddenUserName" />
        <input type="hidden" value="<%= Session["UserId"] %>" id="hiddenUseId" />
        <div class="row row-eq-height" style="margin: 15px;" id="changePasswordArea" runat="server">
            <div class="col-md-6 col-sm-12">
                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-edit"></i>Change Password
                        </div>
                    </div>
                    <div class="portlet-body" id="changePasswordBody">
                        <div class="row" style="padding-top: 10px; padding-bottom: 10px; border-bottom: 1px solid #e5e5e5; margin-left: 0px; margin-right: 0px;">
                            <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4" style="">
                                <label class="control-label">New Password</label>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8" style="height: 70px">
                                <input id="txbPassword" type="password" class="form-control" style="height: 31px;" />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px; padding-bottom: 10px; border-bottom: 1px solid #e5e5e5; margin-left: 0px; margin-right: 0px;">
                            <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4" style="">
                                <label class="control-label">Confirm Password</label>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8" style="">
                                <input id="txbConfirmPassword" type="password" class="form-control" style="height: 31px;" />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px; padding-bottom: 10px; margin-left: 0px; margin-right: 0px; height: 75px">
                            <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4" style="">
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8" style="height: 75px; color: red; display: none; padding-left: 16px" id="userFeedback">
                            </div>
                        </div>
                        <div class="row" style="padding: 14px 0 0; text-align: right; margin-left: -12px; margin-right: -12px;" id="buttonContainer">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="">
                                <button type="button" class="btn purple" onclick="ChangePassword()">Confirm</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-12">
                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-edit"></i>Password Safety Tips
                        </div>
                    </div>
                    <div class="portlet-body" id="safetyTips">
                        <ul style="line-height: 2.3em">
                            <li>Never reveal your password to anyone.
                            </li>
                            <li>Don't just use a single password, change it for every application.
                            </li>
                            <li>Create passwords that are easy to remember but hard for others to guess.
                            </li>
                            <li>Make your password a little different by adding a couple of unique letters.
                            </li>
                            <li>Never include a username in your password.
                            </li>
                            <li>Make the password at least 8 characters long.
                            </li>
                            <li>Include numbers, capital letters and symbols. 
                            </li>
                            <li>Don’t use dictionary words.
                            </li>
                            <li>Consider using a password manager.
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="page-footer">
        <div class="page-footer-inner">
        </div>
        <div style="float: right;">
            <span id="lblVersionInfo" style="color: azure" runat="server"></span>
        </div>
    </div>
</body>
</html>
