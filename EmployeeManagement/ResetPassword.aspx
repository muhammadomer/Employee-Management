<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="EmployeeManagement.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml" style="height: -webkit-fill-available;">
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
    <link href="assets/global/plugins/font-awesome/css/font-awesome.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" type="text/css" />
    <%--<link href="css/login1.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />--%>
    <link href="css/bootstrap.min.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/uniform.default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/components.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/plugins.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/layout.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/custom.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/default.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <script src="Scripts/jquery-3.3.1.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/bootstrap.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/toastr.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="Scripts/jquery.validate.min.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/AjaxCommonMethods.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
     <link href="css/style.bundle.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
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
    padding-top: 45px !important;

}

    .sidenav h2 {
        padding-bottom: 30px;
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
    padding: 5px 0;
    width: 225px;
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
    max-height: 313px;
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
margin-bottom:-10px;}

    </style>
</head>
<body >


     <form id="frmResetPassword" class="login-form" runat="server">
        <asp:HiddenField ID="forgeryToken" runat="server"/>
                <div id="AFDiv" runat="server"></div>

         <div class="login-container">
              <div class="login-logo">
                <a href="#">
                    <img src="images/mp-logo.png" alt="logo" />
                </a>
            </div>
        <div id="login-box">

            <div class="login-box-body">
                <div class=" row">
                    <div class="col" style="flex: 0 0 40%;max-width: 40%;padding-right: 19px;padding-left: 13px;">
                        <div class="pt-1 sidenav"   >
                         <h2>Reset Password</h2>   
                            <div class="sidenav-logo">
                                <img src="Images/padlock.png" class="Padlock"/>
                            </div>
                            <div >
                                <p class="message">
                                    Please provide your details. An email will be sent to you containing a link to reset your password.
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
                               <% if (ConfigurationManager.AppSettings["onPrem"] == "0"){%>
                                <div class="form-group pt-1">
                                    <label class="form-label" for="UserName">
                                        Account
                                    </label>
                                    <input autocomplete="off" class="form-control w-373" placeholder="Account ID" type="text" data-validate="true" data-msg-selector=".username-error" data-required="true" data-message="Username is required" id="txbAccountId" name="UserName" value="" />
                                    <span class="validation-error username-error"></span>
                                </div>
                             <%}%>   
                             <button id="btnResetPassword" class="mt-2 Sign-in-Button btn btn-16 btn-height  m-btn--wide-normal"><span style="color:white">Reset</span></button>
                            <div class="pt-4">
                                <span class="d-block mt-1 pt-2 visit-website">Visit the Ghost Digital website for more details.<a href="http://www.ghost-digital.com" target="_blank"  class="pl-2 " style="color: #007ba0" >Click Here</a></span>
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
               <img src="images/divider-1.jpg" class="footer-divider" />
                       <img src="images/dentons-logo.png"     class="Dentons-Logo"/>
                
                </div> 
                 </div>
               </div>
         </form>

   
  
    <script>
        $(document).ready(function () {
            $('#txbUserName').focus();
            $("#btnResetPassword").on("click", function () {
                var userName = $("#txbUserName").val();
                if (userName == '') {
                    toastr.error("Please provide Username");
                    return false;
                }

                var accountID = "";

                <% if (ConfigurationManager.AppSettings["onPrem"] == "0")
        {%>
                 accountID = $("#txbAccountId").val();
                 if (userName == '') {
                     toastr.error("Please provide Account Id");
                     return false;
                 }
                <%}%>

                 $("#btnResetPassword").addClass("disabled");
                 $("#btnResetPassword").text("Please Wait...");
                 var requestPerameters = {
                     "userName": userName,
                     "accountID": accountID
                 };
                 var url = 'ResetPassword.aspx/SendResetPasswordLink';
                 AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                     if (response == 'link sent') {
                         toastr.success("Reset password email has been sent successfully.");
                         $("#btnResetPassword").removeClass("disabled");
                         $("#btnResetPassword").text("Reset Password");
                     }
                     else {
                         toastr.error(response);
                         $("#btnResetPassword").removeClass("disabled");
                         $("#btnResetPassword").text("Reset Password");
                     }
                 });
             });
         });
    </script>
</body>
</html>
