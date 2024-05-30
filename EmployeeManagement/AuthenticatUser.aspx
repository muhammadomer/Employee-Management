<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthenticatUser.aspx.cs" Inherits="EmployeeManagement.AuthenticatUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="forgeryToken" runat="server"/>
        <div id="AFDiv" runat="server"></div>

    </form>
</body>
</html>
