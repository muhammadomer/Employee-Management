<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="EmployeeManagement.SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/bootstrap.fd.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <link href="css/bootstrap.fd.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <style>
        hr {
            border-top: 1px solid #000;
        }

        .disabled-customIcon {
            opacity: 0.5;
        }

        .required {
            color: red;
        }

        /*Data table Css*/
        .dataTables_filter input {
            width: 150px;
            height: 26px;
            padding: 6px 10px;
            background-color: #fff;
            border: 1px solid #c2cad8;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }

        .dataTables_filter label {
            font-size: 13px;
            margin-top: 10px;
            margin-bottom: 0px;
        }

        .dataTables_length select {
            width: 70px;
            height: 34px;
            padding: 6px 12px;
            background-color: #fff;
            border: 1px solid #c2cad8;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }

        .dataTables_length {
            padding-bottom: 17px;
        }

        .sorting, .sorting_asc, .sorting_desc {
            padding-left: 11px !important;
        }

        .table > thead:first-child > tr:first-child > th {
            background-color: #fff !important;
            color: #333;
            font-size: 12px;
            padding-top: 10px;
            padding-bottom: 10px;
            font-weight: bold;
        }

        .table > thead:first-child > tr:nth-child(2) > th {
            background-color: #E4E4E4;
            font-size: 12px;
            font-weight: bold;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            padding: 0px;
        }

        .paginate_button, .dataTables_paginate > .disabled {
            background-color: #969696;
            color: white !important;
            border-radius: 4px !important;
        }

        table {
            border: 1px solid #e7ecf1;
            border-radius: 10px;
        }

        tr {
            border-top: 1px solid #C0C0C0;
        }

            tr + tr {
                border-top: 1px solid transparent;
            }


        .even td, .even .sorting_1 {
            background-color: #F1F1F1;
            font-size: 13px;
        }

        .odd td, .odd .sorting_1 {
            font-size: 13px;
        }

        tr td {
            font-size: 13px !important;
        }

        .dataTables_paginate a {
            font-size: 13px;
        }
        /*End Css data table*/



        .navbar-default {
            background-color: #f1f1f1;
            border-color: #f1f1f1;
        }






        .control-label {
            font-weight: bold;
        }

        .form-control {
            font-size: 12px;
        }

        .pageHeading {
            margin-top: 5px;
            margin-bottom: 10px;
        }

        .table {
            border-top: 0px;
        }

        .progress {
            margin-bottom: 0px !important;
        }

        .panel-heading {
            background-color: #e4e4e4 !important;
        }

        .SecondHeading {
            border-top: 1px solid #e7ecf1 !important;
        }

        .table-hover, .table-hover > tbody > tr > td, .table-hover > tbody > tr > th, .table-hover > tfoot > tr > td, .table-hover > tfoot > tr > th, .table-hover > thead > tr > td, .table-hover > thead > tr > th {
            border-right: 1px solid #e7ecf1;
        }
    </style>
    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Regions</h3>
                    </div>
                </div>
                <div class="panel-body" id="">

                    <table id="regionsTable" class="table table-hover" style="margin-top: 10px;">
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Countries</h3>
                    </div>
                </div>
                <div class="panel-body" id="">

                    <table id="countriesTable" class="table table-hover" style="margin-top: 10px;">
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Cities</h3>
                    </div>
                </div>
                <div class="panel-body" id="">

                    <table id="citiesTable" class="table table-hover" style="margin-top: 10px;">
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Departments</h3>
                    </div>
                </div>
                <div class="panel-body" id="">

                    <table id="departmentsTable" class="table table-hover" style="margin-top: 10px;">
                    </table>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Practice Groups</h3>
                    </div>
                </div>
                <div class="panel-body">

                    <table id="practiceGroupTable" class="table table-hover" style="margin-top: 10px;">
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 430px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>SMTP Settings</h3>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-9">
                                    <div class="form-group">
                                        <label class="control-label">Outgoing Mail Server</label><span class="required">*</span>
                                        <input type="text" id="txtMailServer" class="form-control spinner" tabindex="2">
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">SMTP Port</label><span class="required">*</span>
                                        <input type="number" id="txtSMTPPort" class="form-control spinner" tabindex="2">
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label">Mail Username</label><span class="required">*</span>
                                <input type="text" id="txtMailUsername" class="form-control spinner" tabindex="2">
                            </div>
                            <div class="form-group">
                                <label class="control-label">Mail Password</label><span class="required">*</span>
                                <input type="password" id="txtMailPassword" class="form-control spinner" tabindex="2">
                            </div>
                            <div class="form-group">
                                <label class="control-label">Enable SSL</label><span class="required">*</span>
                                <input type="checkbox" id="ckbEnableSSL" class="make-switch" tabindex="9" checked data-on-text="Yes" data-off-text="No" data-off-color="danger" data-on-color="default">
                            </div>
                        </div>
                    </div>
                    <div class="form-actions pull-right" style="margin-top: 10px;">
                        <a class="btn purple" href="#ModalTestEmail" data-toggle="modal" style="margin-left: 1px;">Test</a>
                        <a class="btn purple" id="btnSaveSMTPSettings">Save</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" style="height: 450px;">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Security Settings</h3>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label" style="width:200px">Enable 2FA Authentication<span class="required">*</span></label>
                                <input type="checkbox" id="ckbTwoFAAuthentication" class="make-switch" tabindex="9" checked data-on-text="Yes" data-off-text="No" data-off-color="danger" data-on-color="default">
                            </div>
                        </div>
                    </div>
                    <% if (Session["ManageAccount"] == null)// && Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)
                        { %>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">                                
                                <label class="control-label" style="width:200px;">Data Encryption</label>
                                <a id="btnSaveEncryptedPassword" data-toggle='modal' href='#ModalChangeEncryptedPassword' class="btn purple" style="width:160px">Change Password</a>
                                
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="caption">
                        <h3 class="panel-title"><i class="fa fa-edit panelIcon"></i>Report Settings</h3>
                    </div>
                </div>
                <div class="panel-body" id="reportSettingsPanel">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Company Name</label><span class="required">*</span>
                                <input id="txbCompanyName" type="text" class="form-control">
                            </div>
                        </div>
                         <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Currency</label><span class="required">*</span>
                                <select id="ddlCurrency" class="form-control">
                                    <%--<option value="0">---Select Currency---</option>
                                    <option value="$">Dollar</option>
                                    <option value="£">Pound</option>
                                    <option value="€">Euro</option>--%>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Company Address</label><span class="required">*</span>
                                <input id="txbCompanyAddress" type="text" class="form-control">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Logo (300 X 100)</label><span class="required">*</span>
                                <div style="height: 105px;padding: 0px; border: 1px solid rgb(222, 214, 214); padding: 1px">
                                <img class="logo-image" alt="Image Not Found" src="">
                            </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8">
                            <input type="button" id="btnImageUpload" class="btn purple btn-primary" value="Upload Logo" style="float: right; background-color: #2a3642; width: 116px">
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <button type="button" class="btn purple buttonAdjustment pull-right" onclick="SaveCompanyDetail()">Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--Change Encrypted Password-->

    <div id="ModalChangeEncryptedPassword" data-backdrop="static" class="modal fade" role="dialog" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog" id="PasswordPopup">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Change Data Encryption Password</h4>
                </div>
                <div class="modal-body">
                    <div class="row" id="oldEncryptedPasswordDiv">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Enter Old Password</label><span class="required">*</span>
                                <input type="password" id="oldEncryptedPassword" class="form-control spinner" placeholder="Enter Old Password">
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding-top: 10px;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Enter New Password</label><span class="required">*</span>
                                <input type="password" id="newEncryptedPassword" class="form-control spinner" placeholder="Enter New Password">
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding-top: 10px;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Confirm New Password</label><span class="required">*</span>
                                <input type="password" id="confirmNewEncryptedPassword" class="form-control spinner" placeholder="Confirm New Password">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <a class="btn purple" id="btnChangeEncryptedPassword">Change Password</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal For Add Edit Region -->

    <div id="AddEditRegionModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title" id="addEditHeadingForRegion">Add Region</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Region Name</label><span class="required">*</span>
                                <input type="hidden" id="regionID" />
                                <input type="text" id="txbRegionName" class="form-control spinner" tabindex="2" placeholder="Region Name">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnAddEditRegion" class="btn purple">Save</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal For Add Edit Country -->

    <div id="AddEditCountryModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title" id="addEditHeadingForCountry">Add Country</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Region Name</label><span class="required">*</span>
                                <select id="ddlRegionsForAddEditCountry" class="form-control spinner" tabindex="2">
                                </select>
                            </div>
                            <div class="form-group">
                                <label class="control-label">Country Name</label><span class="required">*</span>
                                <input type="hidden" id="countryID" />
                               
                                <select id="ddlCountriesForAddEditCountry" class="form-control spinner" tabindex="2">
                                     
                                </select>
                                <input type="text" id="txbCountryName" class="form-control spinner hidden"  tabindex="2" placeholder="Country Name" style="margin-top:15px">
                            </div>
                         
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnAddEditCountry" class="btn purple">Save</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal For Add Edit City -->

    <div id="AddEditCityModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title" id="addEditHeadingForCity">Add City</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Region Name</label><span class="required">*</span>
                                <select id="ddlRegionsForAddEditCity" class="form-control spinner" tabindex="2">
                                </select>
                            </div>
                            <div class="form-group">
                                <label class="control-label">Country Name</label><span class="required">*</span>
                                <select id="ddlCountriesForAddEditCity" class="form-control spinner" tabindex="2">
                                    <option value=''>--- Select Country ---</option>
                                </select>
                            </div>
                            <div class="form-group">
                                <label class="control-label">City Name</label><span class="required">*</span>
                                <input type="hidden" id="cityID" />
                                <input type="text" id="txbCityName" class="form-control spinner" tabindex="2" placeholder="City Name">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnAddEditCity" class="btn purple">Save</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal For Add Edit Department -->

    <div id="AddEditDepartmentModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title" id="addEditHeadingForDepartment">Add Department</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Department Name</label><span class="required">*</span>
                                <input type="hidden" id="departmentID" />
                                <input type="text" id="txbDepartmentName" class="form-control spinner" tabindex="2" placeholder="Department Name">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnAddEditDepartment" class="btn purple">Save</a>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal For Add Edit Practice Group -->

    <div id="AddEditPracticeGroupModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title" id="addEditHeadingForPracticeGroup">Add Practice Group</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Practice Group Name</label><span class="required">*</span>
                                <input type="hidden" id="PracticeGroupID" />
                                <input type="text" id="txbPracticeGroupName" class="form-control spinner" tabindex="2" placeholder="Practice Group Name">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnAddEditPracticeGroup" class="btn purple">Save</a>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal For Test Email -->

    <div id="ModalTestEmail" data-backdrop="static" class="modal fade" role="dialog" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Test SMTP Settings</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="control-label">Email</label><span class="required">*</span>
                                <input type="text" id="txbToEmail" class="form-control spinner" tabindex="2" placeholder="To Email">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btnTestSMTPSettings" class="btn purple">Test</a>
                </div>
            </div>
        </div>
    </div>

    <!-- END PAGE CONTENT BODY -->
    <script src="ApplicationScripts/Settings.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/ValidationFields.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
</asp:Content>
