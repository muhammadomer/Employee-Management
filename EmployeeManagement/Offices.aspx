<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="Offices.aspx.cs" Inherits="EmployeeManagement.Offices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    

    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row" style="margin-left: 0px; margin-right: 0px;">
        <div class="col-md-12" style="padding: 0px;">
            <div class="portlet-body">
                <table id="locationTable" class="table table-hover" style="width: 100%">
                </table>
            </div>
        </div>
    </div>



    <!-- END PAGE CONTENT BODY -->
    <script src="ApplicationScripts/Locations.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/ValidationFields.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
</asp:Content>
