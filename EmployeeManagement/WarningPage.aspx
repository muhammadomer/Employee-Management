<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="WarningPage.aspx.cs" Inherits="EmployeeManagement.WarningPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row" style="margin-left: 0px; margin-right: 0px;">
        <div class="col-md-12" style="padding: 0px;">
            <div class="portlet-body">
                
                <div style="text-align:center; margin-top:100px;">

                    <img src="images/warningIcon.png" style="height:100px; width:100px;" />
                    <h2>Resource not found.</h2>

                    <%--<button type="button" id="previousBtn" class="btn purple">Go to Previous Page</button>--%>

                </div>
    
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT BODY -->

    <script>

        //$(document).ready(function () {
        //    $('#previousBtn').click(function () {
        //        parent.history.back();
        //        return false;
        //    });
        //});

</script>

</asp:Content>
