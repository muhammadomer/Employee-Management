﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Applications.aspx.cs" Inherits="EmployeeManagement.Applications" %>

<!DOCTYPE html>
<html>
<head>

    <meta charset="utf-8" />

    <title><% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 0)
               {  %>
        Employee Management 
        <% }
    else
    { %>
        MITIGATE PRO
                <% }%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta content="Preview page of Metronic Admin Theme #3 for " name="description" />
    <meta content="" name="author" />
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css" />
    <link href="assets/global/plugins/font-awesome/css/font-awesome.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="assets/global/plugins/simple-line-icons/simple-line-icons.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="assets/global/plugins/bootstrap/css/bootstrap.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <!-- END GLOBAL MANDATORY STYLES -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <link href="assets/global/plugins/icheck/skins/all.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-multiselect.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/toastr.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/jquery.dataTables.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <%--<link href="assets/global/plugins/bootstrap-multiselect/css/bootstrap-multiselect.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />--%>
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN THEME GLOBAL STYLES -->
    <link href="assets/global/css/components.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" id="style_components" type="text/css" />
    <link href="assets/global/css/plugins.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <!-- END THEME GLOBAL STYLES -->
    <!-- BEGIN THEME LAYOUT STYLES -->
    <link href="assets/layouts/layout3/css/layout.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="assets/layouts/layout3/css/themes/default.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" id="style_color" />
    <link href="assets/layouts/layout3/css/custom.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="css/index.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <link href="css/custom.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />

    <link href="css/cropper.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/main.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />

    <script src="Scripts/jquery.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <!--[if lt IE 9]-->
    <script src="assets/global/plugins/respond.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="assets/global/plugins/excanvas.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="assets/global/plugins/ie8.fix.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <!--[endif]-->
    <!-- BEGIN CORE PLUGINS -->

    <script src="Scripts/bootstrap.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/global/plugins/js.cookie.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/global/plugins/jquery.blockui.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="Scripts/qrcode.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <!-- END CORE PLUGINS -->
    <!-- BEGIN THEME GLOBAL SCRIPTS -->
    <script src="assets/global/scripts/app.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <!-- END THEME GLOBAL SCRIPTS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <script src="assets/global/plugins/icheck/icheck.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/pages/scripts/form-icheck.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="Scripts/toastr.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/jquery.dataTables.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <%--<script src="assets/pages/scripts/components-bootstrap-multiselect.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>--%>
    <script src="Scripts/bootstrap-multiselect.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/pwstrength-bootstrap.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN THEME LAYOUT SCRIPTS -->
    <script src="assets/layouts/layout3/scripts/layout.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/layouts/layout3/scripts/demo.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/layouts/global/scripts/quick-sidebar.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <script src="assets/layouts/global/scripts/quick-nav.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" type="text/javascript"></script>
    <!-- END THEME LAYOUT SCRIPTS -->
    <script src="ApplicationScripts/security-PasswordValidations.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/AjaxCommonMethods.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>

    <script src="ApplicationScripts/cropper.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/main.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/ValidationFields.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>

    <!-- END THEME LAYOUT STYLES -->
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />

    <script src="Scripts/bootstrap.fd.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <link href="css/bootstrap.fd.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />



    <style>
        body {
            overflow-y: hidden;
            background-color: #fff !important;
        }

        #pointer {
            text-align: unset;
            padding-bottom: 0px;
            background-color: #DBDBDB;
            margin: 18px 15px 0px 0px;
        }

            #pointer a {
                text-decoration: none;
                color: #817F84;
                font-size: 13px;
                padding: 5px 0px;
                float: right;
                margin-right: 25px;
            }

        .form-control {
            color: black !important;
            border: solid 1px #B7C3C3 !important;
        }


        .tile-body .fa {
            text-align: center !important;
            background-color: white;
            color: #672A87;
            padding: 15px;
        }

        .tile-object .name {
            margin-bottom: 0px !important;
            text-align: center
        }

        .disable-name {
            cursor: no-drop;
            opacity: 0.5;
        }

        .disable-tile {
            cursor: no-drop;
        }

        .purple {
            color: #fff;
            background-color: #6E2D91 !important;
            border-color: #6E2D91 !important;
            font-size: 13px;
        }

        .progress {
            margin-bottom: 0px !important;
        }

        @media (max-width: 991px) {

            body {
                overflow-y: auto;
                overflow-x: hidden;
            }

            .mainouter {
                width: unset !important;
            }
        }


        .overlay {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            height: 100%;
            width: 100%;
            opacity: 0;
            transition: .3s ease;
            background-color: #000;
        }

        .container1 {
            position: relative;
            width: 187px;
            height: 187px;
            margin-top: 22px;
            border: 2px solid #EDEDED;
        }

            .container1:hover .overlay {
                opacity: 0.7;
            }

        .icon {
            color: black;
            font-size: 12px;
            position: absolute;
            top: 50%;
            left: 30%;
            transform: translate(-50%, -50%);
            -ms-transform: translate(-50%, -50%);
            text-align: center;
        }

        .icon1 {
            color: black;
            font-size: 12px;
            position: absolute;
            top: 50%;
            left: 65%;
            transform: translate(-50%, -50%);
            -ms-transform: translate(-50%, -50%);
            text-align: center;
        }


    </style>
    <% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
        {  %>

    <style>
        body {
            overflow: hidden;
            background-color: #fff !important;
            background: url('images/Login/background.jpg') no-repeat fixed 0%;
            background-size: 100% 100%;
        }

        .logoTable {
        }

        .appLogoImages {
            cursor: pointer;
        }

        .logoTable td {
            /*border-right: 3px solid #da7012;*/
        }

        .imgColumn {
            padding: 0px 40px;
        }

        .disableImage{
            opacity: 0.2;
        }

        .tableColumn::after {
            line-height: 0px;
            content: ' ';
            border-right: 2px solid #da7012;
            font-size: 50px;
            position: relative;
            top: 17px;
        }

        .logoTable td > img {
            width: 100%;
        }

        .InblockDiv {
            display: inline-block;
            width: 80%;
        }

        .userName {
            padding-left:15px;
            padding-top:5px;

    position: fixed;
                       color: white;
    font-size: 28px;
    font-family: 'Bahnschrift';
    letter-spacing: -0.3px;
        }

        .logo-top-margin{
            margin-top: 220px;
        }

        @media (min-width: 1921px) {
            .InblockDiv {
                display: inline-grid;
            }
        }

        @media (max-width: 1600px) {

             .logo-top-margin{
            margin-top: 150px;
        }

            .imgColumn {
                padding: 0px 30px;
            }

            .tableColumn::after {
                font-size: 35px;
                top: 8px;
            }

            .leftLogo {
                padding-top: 61px !important;
            }

                .leftLogo img {
                    width: 250px;
                }

            #rightLogo {
                width: 130px
            }

            .portalLabel {
                font-size: 20px;
            }

            .userName{
                font-size: 20px;
                padding-top: 2px;
                padding-left:8px;
            }

            #menuProfileImage1{
                width:32px !important;
                height:32px !important;
            }
        }
    </style>
    <link href="css/otherApp.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />

    <% } %>
</head>
<body>
    <form id="Form" class="login-form" runat="server">

        <asp:HiddenField ID="forgeryToken" runat="server" />
        <div id="AFDiv" runat="server"></div>

        <% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 0)
            {  %>
        <div class="mainouter container" id="mainContanier1" runat="server" style="height: auto">

            <div class="row">
                <div class="col-sm-12 col-md-12" style="margin-top: 40px;">
                    <img src="images/heading-image.png" style="width: 150px;" class="img-responsive header-image" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4 col-md-3 col-lg-3">
                    <h1 class="headingStyle">Single Point</h1>
                </div>
                <div class="col-sm-6 col-md-8 col-lg-8" style="padding: 0px;">
                    <div id="pointer">
                        <a href="#;" class="" style="cursor: default">Software Access Portal</a>
                    </div>
                </div>
                <div class="col-sm-2 col-md-1 col-lg-1">
                    <div style="margin-top: 6px;">
                        <a href="#options" class="dropdown-toggle" style="padding: 4px;" data-toggle="dropdown">
                            <img id="menuProfileImage" src="" style="width: 49px; height: 49px; border-radius: 25px !important;" />
                        </a>
                        <ul class="dropdown-menu theme-panel pull-right dropdown-custom hold-on-click" style="min-width: 160px; padding: 0px;">
                            <li><a href='#ModalChangeProfile' id="ShowChangeProfileModalPopup">Profile</a></li>
                            <li><a href='#ModalChangePassword' id="ShowChangePasswordModalPopup">Change Password</a></li>
                            <li>
                                <a href='#' class="btnEnableAuthentication" runat="server" id="btnEnableAuthentication">Enable Authenticator</a>
                            </li>
                            <li><a href="#Logout" id="logout">Logout</a></li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>

        <div class="application-container">
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="tiles" id="applicationsTiles">
                            <%--<div class='tile bg-purple-studio'><div class='tile-body'><i class='fa fa-briefcase'></i></div><div class='tile-object'><div class='name'>File Management</div><div class='number'></div></div></div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <p style="color: #696969; font-size: 16px;">
                        The icons displayed above indicate which privileges have been assigned to you. If there is an application that you need but is not currently visible, please contact your system administrator for the appropriate permissions.
                    </p>
                </div>
            </div>
        </div>

        <% }
            else
            {  %>

        
        <div class="row">
            <div class="col-md-6 leftLogo" style="padding-left: 85px; padding-top: 85px;">
                <div class="row">
                    <img src="images/Login/frame.png" alt="" />
                </div>
                <div class="row" style="padding-top: 15px;">
                    <%--<span class="portalLabel">CIient Access Portal</span>--%>
                     <div class="col-md-12" style="padding:0px;">
                    <div style="margin-top: 6px;">
                        <a href="#options" class="dropdown-toggle" style="padding: 4px;" data-toggle="dropdown">
                            <label class="userName"></label>
                            <img id="menuProfileImage1" src="images/ouser.png" style="width: 49px; height: 49px; border-radius: 25px !important; float:left" />
                            
                        </a>
                        <ul class="dropdown-menu theme-panel pull-right dropdown-custom hold-on-click" style="min-width: 160px !important; width:160px; padding: 0px;left: 5px;">
                            <li><a href='#ModalChangeProfile' id="ShowChangeProfileModalPopup">Profile</a></li>
                            <li><a href='#ModalChangePassword' id="ShowChangePasswordModalPopup">Change Password</a></li>
                            <li>
                                <a href='#' class="btnEnableAuthentication" runat="server" id="btnEnableAuthentication1">Enable Authenticator</a>
                            </li>
                            <li><a href="#Logout" id="logout">Logout</a></li>
                        </ul>
                    </div>
                </div>
                </div>
            </div>
            <div class="col-md-6" style="text-align: right; padding-right: 85px;">
                <img id="rightLogo" src="images/Login/Gdd-logo.jpg" alt="" />
            </div>
        </div>

        <div class="row logo-top-margin">


            <div class="col-sm-12 col-md-12 col-lg-12" style="text-align: center;">
                <div class="InblockDiv">
                    <table class="logoTable">
                        <tr>
                            <td class="imgColumn">
                                <img id="app4" class="disableImage" src="images/Login/dac-6-logo.png" alt="" /></td>
                            <td class="tableColumn"></td>
                            <td class="imgColumn">
                                <img id="app2" class="disableImage" src="images/Login/mitigate-logo.png" alt="" /></td>
                            <td class="tableColumn"></td>
                            <td class="imgColumn">
                                <img id="app1" class="disableImage" src="images/Login/central-logo.png" alt="" /></td>
                            <td class="tableColumn"></td>
                            <td class="imgColumn">
                                <img id="app3" class="disableImage" src="images/Login/business-logo.png" alt="" /></td>
                            <td class="tableColumn"></td>
                            <td class="imgColumn">
                                <img id="app5" class="appLogoImages" src="images/Login/tc-logo.png" alt="" onclick="OpenApplication(5);" /></td>
                        </tr>
                    </table>
                </div>

            </div>
        </div>

        <%--<div class="row">
                <div class="col-sm-2 col-md-1 col-lg-1">
                    <div style="margin-top: 6px;">
                        <a href="#options" class="dropdown-toggle" style="padding: 4px;" data-toggle="dropdown">
                            <img id="menuProfileImage1" src="images/ouser.png" style="width: 49px; height: 49px; border-radius: 25px !important;" />
                        </a>
                        <ul class="dropdown-menu theme-panel pull-right dropdown-custom hold-on-click" style="min-width: 160px; padding: 0px;left: 20px;">
                            <li><a href='#ModalChangeProfile' id="ShowChangeProfileModalPopup">Profile</a></li>
                            <li><a href='#ModalChangePassword' id="ShowChangePasswordModalPopup">Change Password</a></li>
                            <li>
                                <a href='#' class="btnEnableAuthentication" runat="server" id="btnEnableAuthentication1">Enable Authenticator</a>
                            </li>
                            <li><a href="#Logout" id="logout">Logout</a></li>
                        </ul>
                    </div>
                </div>
            </div>--%>

        <% } %>

        <div id="ModalChangePassword" data-backdrop="static" class="modal fade" role="dialog" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog" id="PasswordPopup">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                        <h4 class="modal-title">Change Password</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Enter Old Password</label><span class="required">*</span>
                                    <input type="password" id="oldPassword" class="form-control spinner" placeholder="Enter Old Password">
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Enter New Password</label><span class="required">*</span>
                                    <input type="password" id="newPassword" class="form-control spinner" placeholder="Enter New Password">
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Confirm New Password</label><span class="required">*</span>
                                    <input type="password" id="confirmNewPassword" class="form-control spinner" placeholder="Confirm New Password">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <a class="btn purple" id="btnChangePassword">Change Password</a>
                    </div>
                </div>
            </div>
        </div>
        <div id="EableAuthenticator" data-backdrop="static" class="modal fade" role="dialog" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="border-bottom: 1px solid #e5e5e5;">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                        <h4 class="modal-title">Two-Factor Authentication</h4>
                    </div>
                    <div class="modal-body" style="padding-top: 0px;">
                        <div class="row" style="border-bottom: 1px solid #e5e5e5;">
                            <div class="col-md-2">
                                <img src="images/SampleImageForQRCodeScanner.png" />
                            </div>
                            <div class="col-md-10">
                                <h3 style="font-size: 18px; font-weight: 400; margin-bottom: 0; margin-top: 15px;">Set up via Third Party Authenticator
                                </h3>
                                <p style="margin-top: 10px; font-size: 13px;">Please use your authentication app (such as Microsoft Authenticator, Free OTP or Google Authenticator) to scan this QR code.</p>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 25px;">
                            <div class="col-md-6">
                                <div id="qrcode" style="width: 200px; margin: auto;"></div>
                            </div>
                            <div class="col-md-5">
                                <p style="text-align: center;">Or enter this code into your authentication app</p>
                                <h5 id="sharedKey" style="text-align: center; text-transform: uppercase; font-weight: bold;"></h5>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 25px;">
                            <div class="col-md-12">
                                <input id="SecretKey" name="SecretKey" type="hidden" value="">
                                <input id="BarcodeUrl" name="BarcodeUrl" type="hidden" value="">
                                <div class="form-group">
                                    <label class="control-label">Please enter the confirmation code you see on your authentication app</label>
                                    <input autocomplete="off" class="form-control" id="txtCode" name="Code" type="text" value="">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <a id="btnVerifyCode" style="margin-top: 10px;" class="btn purple">Verify</a>
                    </div>
                </div>
            </div>
        </div>
        <div id="ModalChangeProfile" data-backdrop="static" class="modal fade" role="dialog" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog" id="ProfilePopup">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                        <h4 class="modal-title">Profile</h4>
                    </div>
                    <div class="modal-body">

                        <input type="hidden" id="usrID" />

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label">First Name</label><span class="required">*</span>
                                    <input type="text" id="usrFName" class="form-control spinner" placeholder="Enter First Name">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Last Name</label><span class="required">*</span>
                                    <input type="text" id="usrLastName" class="form-control spinner" placeholder="Enter Last Name">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Direct Number <span style="font-weight:600">(e.g. +441234567890)</span></label>
                                    <input type="text" id="usrDirectNum" class="form-control spinner" placeholder="Enter Direct Number">
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Mobile Number <span style="font-weight:600">(e.g. +441234567890)</span></label>
                                    <input type="text" id="usrMobileNum" class="form-control spinner" placeholder="Enter Mobile Number">
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Fax Number <span style="font-weight:600">(e.g. +441234567890)</span></label>
                                    <input type="text" id="usrFaxNum" class="form-control spinner" placeholder="Enter Fax Number">
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="container1">
                                    <img src="images/user.png" alt="No Image" id="usrImage" class="img-responsive image" style="height: 100%; margin: auto;" />
                                    <div class="overlay">
                                        <a href="#" class="icon btn btn-default purple" id="removeProfileImage">Remove
                                        </a>
                                        <%--<a href="#" class="icon1 btn btn-default purple" id="uploadProfileImage">Update
                                        </a>--%>
                                        <a href="#" class="icon1 btn btn-default purple" id="upldUsrImage">Update
                                        </a>
                                        <input type="file" id="uploadUsrImage" onchange="readURL(this);" style="display: none;" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" style="width: 65px;" data-dismiss="modal">Cancel</button>
                        <a class="btn purple" style="width: 65px;" id="btnChangeProfile">Save</a>
                    </div>
                </div>
            </div>
        </div>

        <div id="UserProfileImageModal" class="modal fade" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog" style="width: 485px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                        <h4 class="modal-title">Upload Profile Image</h4>
                    </div>
                    <div class="modal-body">
                    </div>
                    <div class="modal-footer">
                        <a class="btn default" data-dismiss="modal">Cancel</a>
                        <a href="#" id="btnProfileImgUpload" class="btn purple" onclick="getCropedImage()">Upload</a>
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

    </form>
    <script src="ApplicationScripts/Application.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
</body>
</html>
