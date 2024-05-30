<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Login.aspx.cs" Inherits="EmployeeManagement.Login" %>

<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml">
    <head runat="server">
    <title><% if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 0)
               {  %>
        Employee Management 
        <% }
    else
    { %>
        MITIGATE PRO
                <% }%> - Login</title>
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
    <link href="css/style.bundle.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
        <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700%7CRoboto:300,400,500,600,700" media="all"/>
         <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto:300,300i,400,400i,500,500i,700,700i,900,900i" media="all" />
<style>
    body {
    margin: 0;
    padding: 0;
    background-color: #f2f3f8;
    height: 100vh;
}

.login-container {
    width: 793px;
    margin: 106px auto 132px;
    /*margin: 217px auto 145px;*/
    position: relative;
    z-index: 5;
}

#login-box {
    margin: 0 auto;
    display: block;
    /* width: 712px; */
    /* margin: 7% auto; */
    width: 100%;
    float: left;
    float: left;
    width: 100%;
    position: relative;
    padding: 0 0 30px;
    visibility: visible;
}

.w-373 {
    width: 373px;
}

.login-logo {
    font-size: 35px;
    text-align: center;
    margin-bottom: 25px;
    font-weight: 300;
}

.login-box-body {
    background: #fff;
    padding: 28px 38px 33px 27px;
    border-top: 0;
    color: #666;
    border: 1px solid #a9a9a9;
    border-radius: 5px  !important;
}

.sidenav {
    display:block;
    padding-top: 80px !important;

}

    .sidenav h2 {
        padding-bottom: 40px;
        margin: auto;
        display: block;
        font-family: 'Poppins', sans-serif;
        font-size: 34px;
        font-weight: 500;
        font-stretch: normal;
        font-style: normal;
        line-height: 31px;
        letter-spacing: normal;
        text-align: center;
        color: #262626;
    }

.sidenav-logo img {
    margin: auto;
    display: block;
}

p.message {
     font-family: 'Roboto', sans-serif;
    font-size: 14px;
    font-weight: normal;
    font-stretch: normal;

    font-style: normal;
    line-height: 1.43;
    letter-spacing: normal;
    text-align: center;
    color: #585858;
}

.sign-message {
    display: block;
    margin: auto;
    text-align: center;
    padding: 15px 0;
    width: 205px;
}

.form-label {
    font-family: "Poppins";
    font-size: 16px;
    font-stretch: normal;
    font-style: normal;
    line-height: 10px;
    letter-spacing: normal;
    text-align: left;
    color: #262626;
    font-weight: normal;
    padding-bottom: 4px;
    margin-left: -3px;
}

.Sign-in-Button {
    border-radius: 4px !important;
    background-color: #00529b;
  
    width: 160px;
}

.visit-website {
     font-family: 'Roboto', sans-serif;
    font-weight: normal;
    color: #272528;
}

.btn-theme, .theme-btn {
    background-color: #00529b;
    border-color: #146936;
}

    .btn-theme:hover, .theme-btn:hover {
        background-color: #00529b;
        border-color: #083d1d;
    }

.footer.login-footer {
    width: 100%;
}

.footer-link {
    /* width: 100px; */
    height: 13px;
    margin: 0px 13px;
    font-family: 'Roboto', sans-serif;
    font-weight: normal;
    font-stretch: normal;
    font-style: normal;
    line-height: 0.63;
    letter-spacing: normal;
    text-align: center;
    color: #343335;
    font-size: 13px;
    /* height: 21px; */
    border-right: 1px solid;
    padding-right: 27px;
    /* float: right; */
    text-align: center;
    line-height: 100%;
    display: block;
}

.footer-content {
    padding-top: 26px;
    /* width: 1000px; */
    /* width: 850px; */
    display:flex;
    margin: 0 auto;
   /*  vertical-align: middle; */
    align-items: center;
    justify-content: center;
    /* flex-wrap: nowrap; */
  
}

.footer-container {
    height: 100% !important;
    min-height: 81px;
    margin: 11px 0 0;
    background: #f2f3f8;
    /* position: inherit; */
    /* bottom: 0; */
    /* left: 0; */
    /* right: 0; */
}

.language-list {
    max-width: 238px;
    width: 100%;
    padding-left: 16px;
}


.sidenav-logo {
    padding-bottom: 20px;
}

.sidenav:before {
    content: "";
    position:absolute;
    right: 0;
    z-index: 100;
    width: 1px;
    max-height: 413px;
    height: 100%;
    background: #a9a9a9;
    top: 7px;
   
    
}

.footer {
  /* position:absolute;*/
  font-size:13px;
   bottom:0;
   width:100%;
   height:100px;   /* Height of the footer */
   background:#e6e7ec;
}
.footer-divider{
    margin:0 25px 0 25px;
}
@media (min-height: 850px) {
    .f-height {
       position:absolute;
    }
  
       

}
.margin-bottom {
margin-bottom:-10px;

}

    </style>
</head>
<body>
   <div class="">
      
       <div class="">

  <form id="Form" class="login-form" runat="server">

      <%if(ConfigurationManager.AppSettings["ServerDown"] == "false") {  %>
      <asp:HiddenField ID="forgeryToken" runat="server"/>   
                            <div id="AFDiv" runat="server"></div>
      <div class="login-container">

        <div id="login-box">
            <div class="login-logo">
                <a href="#">
                    <img src="images/mp-logo.png" alt="logo" />
                </a>
            </div>
            <div class="login-box-body">
                <div class=" row">
                    <div class="col" style="flex: 0 0 40%;max-width: 40%;padding-right: 19px;padding-left: 13px;">
                        <div class="pt-1 sidenav"   >
                            <h2>Sign In</h2>
                            <div class="sidenav-logo">
                                <img src="Images/padlock.png" class="Padlock"/>
                            </div>
                            <div class="sign-message">
                                <p class="message">
                                    Please sign in for secure access to your account
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="align- align-items-center col-7-- col d-flex  pr-0" style="flex: 0 0 60%;
        max-width: 60%;
        padding-left: 48px;
">
                        <div class="mb-3 mt-3  pt-4 w-100" style=" padding-bottom: 23px;">
   
                            <div class="form-group  ">
                                    <label class="form-label" for="UserName">
                                        Username
                                    </label>
                                    <input autocomplete="off" class="form-control w-373" placeholder="Username" type="text" data-validate="true" data-msg-selector=".username-error" data-required="true" data-message="Username is required" id="txbUserName" name="UserName" value="" />
                                    <span class="validation-error username-error"></span>
                                </div>
                                <div class="form-group margin-bottom ">
                                    <label class="form-label" for="PasswordHash">
                                        Password
                                    </label>
                                    <input autocomplete="off" class="form-control w-373" placeholder="Password" type="password" data-validate="true" data-msg-selector=".password-error" data-required="true" data-message="Password is required" id="txbPassword" name="PasswordHash" />
                                    <span class="validation-error password-error"></span>
                                   
                                   <span>  <a href="ForgotPassword.aspx" target="_blank" style="color: #007ba0;padding-top:5px; text-align:right;padding-right:30px;font-size:12px" class="center-block  " > Forgot password?</a>
                                   </span>
                                        
                                </div>
                            	   
                                  <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
                                <div class="form-group  pt-1">
                                    <label class="form-label" for="UserName">
                                        
                                        Account
                                    </label>
                                    <input autocomplete="off" class="form-control w-373" placeholder="Account ID" type="text" data-validate="true" data-msg-selector=".username-error" data-required="true" data-message="Username is required" id="txbAccountId" name="UserName" value="" />
                                    <span class="validation-error username-error"></span>
                                </div>
                             <%}%>   
                                <button id="btnLogin" class="mt-2 Sign-in-Button btn btn-16 btn-height  m-btn--wide-normal"><span style="color:white;">Sign In</span></button>
							
                            
	                        <div class="pt-4">
                                <span class="d-block mt-1 pt-2 visit-website">Visit the Ghost Digital website for more details.<a href="http://www.ghost-digital.com" target="_blank" class="pl-2 " style="color: #007ba0" >Click Here</a></span>
                            </div>
							 
              
                               
          
	
                        </div>
                    </div>
                </div>
            </div>
        </div>


          
    </div>
        <div class="footer footer-content f-height">
             <div class="container">

             
             <div class="row" style="justify-content:center;padding-top:10px">
                 <img src="images/mini-logo.png" height="18" />
                <img src="images/divider-1.jpg" class="footer-divider" />
                <span> <a href="http://www.dentons.com " target="_blank" style="color: #007ba0">www.dentons.com</a>  </span>
                <img src="images/divider-1.jpg" class="footer-divider" />
                <span >  Telephone: +971 52 126 5973 </span>
                <img src="images/divider-1.jpg" class="footer-divider" />
               <span >  Level 18, Boulevard Plaza 2 Dubai United Arab Emirates </span>
             </div>
                
               <div class="row" style="justify-content:center;padding-top:10px;padding-bottom:20px">
                   <span >   (c) copyright 2015-2022 Dentons </span>
               <img src="images/divider-1.jpg"  class="footer-divider" />
                       <img src="images/dentons-logo.png"     class="Dentons-Logo"/>
                
                </div> 
                 </div>
               </div>
      <%}
      else
      {  %> 
      <div id="loginView">
         
           <div class="login-container">

        <div id="login-box">
            <div class="login-logo">
                <a href="#">
                    <img src="images/mp-logo.png" alt="logo" />
                </a>
            </div>
            <div class="login-box-body">
                <div class=" row">
                    <div class="col" style="flex: 0 0 40%;max-width: 40%;padding-right: 19px;padding-left: 13px;">
                        <div class="pt-1 sidenav"   >
                            <h2>Sign In</h2>
                            <div class="sidenav-logo">
                                <img src="Images/padlock.png" class="Padlock"/>
                            </div>
                            <div class="sign-message">
                                <p class="message">
                                    Please sign in for secure access to your account
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="align- align-items-center col-7-- col d-flex  pr-0" style="flex: 0 0 60%;
        max-width: 60%;
        padding-left: 48px;
">
                        <div class="mb-3 mt-3  pt-4 w-100" style=" padding-bottom: 23px;">
   
                          
                     <p>MITIGATE PRO is being currently updated and is therefore
                                        offline. The software will be back online at <%=ConfigurationManager.AppSettings["ServerUpTime"]%> GMT 
                                        <%=ConfigurationManager.AppSettings["ServerUpDate"]%>.</p>
                                    <p>Sorry for any inconvenience.</p>           
                            
	
							 
              
                               
          
	
                        </div>
                    </div>
                </div>
            </div>
        </div>


          
    </div>





            <div class="footer footer-content f-height">
             <div class="container">

             
             <div class="row" style="justify-content:center;padding-top:10px">
                 <img src="images/mini-logo.png" height="18" />
                <img src="images/divider-1.jpg" class="footer-divider" />
                <span> <a href="http://www.dentons.com " target="_blank" style="color: #007ba0">www.dentons.com</a>  </span>
                <img src="images/divider-1.jpg" class="footer-divider" />
                <span >  Telephone: +971 52 126 5973 </span>
                <img src="images/divider-1.jpg" class="footer-divider" />
               <span >  Level 18, Boulevard Plaza 2 Dubai United Arab Emirates </span>
             </div>
                
               <div class="row" style="justify-content:center;padding-top:10px;padding-bottom:20px">
                   <span >   (c) copyright 2015-2022 Dentons </span>
               <img src="images/divider-1.jpg" class="footer-divider" />
                       <img src="images/dentons-logo.png"     class="Dentons-Logo"/>
                
                </div> 
                 </div>
               </div>
             

</div>

<%} %>


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
                    <a id="btnVerifyCode" class="btn blue1"><span style="color:white">Verify</span></a>
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
        $('#txbUserName').focus();
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
            var url = 'Login.aspx/Reset2FA';
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
        var url = 'Login.aspx/VerifyCodeOnLogin';
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            if (response == "3") {
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
            //else if (response == "6") {
            //    $("#ajaxLoader").modal("hide");
            //    location.href = "ManageApplication.aspx?appID=4";
            //}
            else {
                $("#ajaxLoader").modal("hide");
                if (response.trim() != "")
                    toastr.error(response);
            }
        });
    };

    LoginUser = function () {
        //debugger;
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
        var url = 'Login.aspx/LoginUser';
        AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
            //debugger;
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
                    url: 'Login.aspx/GetUserEmail',
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
                        //debugger;
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
            //else if (response == "6") {
            //    $("#ajaxLoader").modal("hide");
            //    location.href = "ManageApplication.aspx?appID=4";
            //}
            else {
                $("#ajaxLoader").modal("hide");
                if (response.trim() != "")
                    toastr.error(response);
            }
        });
    };

</script>


</html>
