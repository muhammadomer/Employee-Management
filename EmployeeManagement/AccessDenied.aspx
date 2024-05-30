<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="EmployeeManagement.AccessDenied" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="forgeryToken" runat="server"/>

        <div id="AFDiv" runat="server"></div>

         <div class="application-container">
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">

                             <p style="color: #696969; font-size: 16px;">
                        User not authorzied to visit this application. Please contact administrator
                    </p>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
