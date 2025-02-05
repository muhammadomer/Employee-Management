﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="AddEditUser.aspx.cs" Inherits="EmployeeManagement.AddEditUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/bootstrap.fd.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <link href="css/bootstrap.fd.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />

    <script src="Scripts/jquery.multibutton.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <link rel="stylesheet" href="css/jquery.multibutton.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>">

    <link href="css/cropper.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />
    <link href="css/main.css?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>" rel="stylesheet" />

    <style>
        .multibutton li.active{
            background-image: -webkit-gradient(linear, left top, left bottom, color-stop(1, #6E2D91), color-stop(0, #6E2D91));
            background-image: -webkit-linear-gradient(#6E2D91, #6E2D91);
            border: solid 1px #6E2D91;
        }

        .img-responsive {
            margin: auto;
        }

        .container1 {
            position: relative;
            width:187px;
            height: 186px;
            border: 2px solid #EDEDED;
        }

        .image {
            display: block;
            height: 182px;
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

        .container1:hover .overlay {
            opacity: 0.7;
        }

        .NumberExample{
            font-weight:600;
        }

        @media (max-width: 1700px) {

            .NumberExample {
                font-size:9px;
            }
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

        /*.bootstrap-switch-id-userTypeSwitch {
            float: right;
        }*/

        .mainDiv {
            border: 2px solid #EDEDED;
            padding: 15px;
            margin-top: 25px;
        }

            .multiselect-selected-text {
                font-size: 12px;
            }

            .multiselect-container li {
                font-size: 12px;
            }

    </style>
    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row">
        <div class="col-md-12">
            <h3 style="font-weight: bold" id="addEditHeading">Manage Employee Profile</h3>
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 userDataDiv">
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <input type="hidden" id="userID" />
                                <label class="control-label">First Name</label><span class="required">*</span>
                                <input type="text" id="firstName" class="form-control spinner" tabindex="1" placeholder="First Name" maxlength="250" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Last Name</label><span class="required">*</span>
                                <input type="text" id="lastName" class="form-control spinner" tabindex="2" placeholder="Last Name" maxlength="250" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Job Title</label><span class="required">*</span>
                                <input type="text" id="jobTitle" class="form-control spinner" tabindex="3" placeholder="Job Title" maxlength="250" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12" style="margin-top: 25px; height: 100%">
                            <div class="container1">
                                <img src="images/user.png" alt="No Image" id="logo" class="img-responsive image" />
                                <div class="overlay">
                                    <a href="#" class="icon btn btn-default purple" id="removeProfileImage">Remove
                                    </a>
                                    <%--<a href="#" class="icon1 btn btn-default purple" id="uploadProfileImage">Update
                                    </a>--%>
                                    <a href="#" class="icon1 btn btn-default purple" id="upldUsrImage">Update
                                    </a>
                                    <input type="file" id="uploadUsrImage" onchange="readURL(this);" style="display:none;"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Email</label><span class="required">*</span>
                                <input type="email" id="email" class="form-control spinner" tabindex="4" placeholder="Email" maxlength="250" />
                            </div>
                        </div>
                        <%--<div class="col-md-5" style="padding-left: 5px;">
                            <div class="form-group" style="margin-bottom: 0px; margin-top: 30px; font-weight: 100;">
                                <label>@dentons.com</label>
                            </div>
                        </div>--%>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-md-5">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Office</label>
                                <select id="locationNames" class="form-control spinner" tabindex="5">
                                    <option value=''>--- Select Office ---</option>
                                    <%if (offices != null)
                                        {
                                            foreach (var item in offices)
                                            { %>
                                    <option value="<%=item.ID %>"><%= HttpUtility.HtmlDecode(item.Name) %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="control-label">Address 1</label><span class="required">*</span>
                        <input type="text" id="addressLine1" class="form-control spinner" placeholder="Address 1">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Region</label><span class="required">*</span>
                        <input type="text" id="region" class="form-control spinner" placeholder="Region">
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="control-label">Address 2</label>
                        <input type="text" id="addressLine2" class="form-control spinner" placeholder="Address 2">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Country</label><span class="required">*</span>
                        <input type="text" id="country" class="form-control spinner" placeholder="Country">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">State/County/Province</label><%--<span class="required">*</span>--%>
                        <input type="text" id="state" class="form-control spinner" placeholder="State/County/Province">
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group hidden">
                        <label class="control-label">Office</label><span class="required">*</span>
                        <input type="text" id="office" class="form-control spinner" placeholder="Office">
                    </div>
                    <div class="form-group">
                        <label class="control-label">City</label><span class="required">*</span>
                        <input type="text" id="city" class="form-control spinner" placeholder="City">
                    </div>
                </div>
            </div>
            <div class="row">               
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Department</label><span class="required">*</span>
                        <%--<input type="text" id="department" class="form-control spinner" tabindex="6" placeholder="Department">--%>
                        <select id="department" class="form-control spinner" tabindex="6">
                            <option value=''>--- Select Department ---</option>
                            <%if (Departments != null)
                                {
                                    foreach (var item in Departments)
                                    {
                                        //if (!item.Name.Equals("Practice Group"))
                                        {
                                            %>
                                                <option value="<%=item.Name %>"><%=HttpUtility.HtmlDecode(item.Name) %> </option>
                                        <%

                                        }
                                    }
                              }%>
                        </select>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group" id="divPC">
                        <label class="control-label">Practice Group</label><span class="required"></span>
                        <%--<input type="text" id="department" class="form-control spinner" tabindex="6" placeholder="Department">--%>
                        <select id="practiceGroup" class="form-control spinner" tabindex="6">
                            <option value=''>--- Select Practice Group ---</option>
                            <%if (PracticeGroups != null)
                                {
                                    foreach (var item in PracticeGroups)
                                    {                                       
                                            %>
                                                <option value="<%=item.Name %>"><%=HttpUtility.HtmlDecode(item.Name) %> </option>
                                        <%

                                    }
                              }%>
                        </select>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Mail Postal/Zip Code</label><span class="required">*</span>
                        <input type="text" id="mailPostal" class="form-control spinner" placeholder="Mail Postal/Zip Code">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">GPS Postal/Zip Code</label><%--<span class="required">*</span>--%>
                        <input type="text" id="gpsPostal" class="form-control spinner" placeholder="GPS Postal/Zip Code">
                    </div>
                </div>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Direct Number <span class="NumberExample">(e.g. +441234567890)</span></label>
                        <input type="text" id="directNumber" class="form-control spinner" tabindex="7" placeholder="Direct Number" maxlength="18" onkeypress='return isNumberKey(event);' />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">Telephone</label><span class="required">*</span>
                        <input type="text" id="telephone" class="form-control spinner" placeholder="Telephone" maxlength="18" onkeypress='return isNumberKey(event);' />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">Mobile Number <span class="NumberExample">(e.g. +441234567890)</span></label>
                        <input type="text" id="mobileNumber" class="form-control spinner" tabindex="8" placeholder="Mobile Number" maxlength="18" onkeypress='return isNumberKey(event);' />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">Fax Number <span class="NumberExample">(e.g. +441234567890)</span></label>
                        <input type="text" id="faxNumber" class="form-control spinner" tabindex="9" placeholder="Fax Number" maxlength="18" onkeypress='return isNumberKey(event);' />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group hidden">
                        <label class="control-label">Contact Number <span style="font-weight:600">(e.g. +441234567890)</span></label><span class="required">*</span>
                        <input type="text" id="contactNumber" class="form-control spinner" placeholder="Contact Number">
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="mainDiv">
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-6">
                                <h4 style="font-weight: bold">Permissions & Rights</h4>
                            </div>
                            <div class="col-md-6" style="text-align: right">
                                <%--<h2>5. Multiple button groups</h2>--%>

                                <ul class="multibutton5">
                                    <li rel="1">System Admin</li>
                                    <li rel="3">Data Admin</li>
                                    <li rel="2">Standard User</li>
                                </ul>
                                <%--<span id="multibutton5-info"></span>--%>
                                <input id="hdn_multibutton5" value="2" type="hidden" />
                                <%--<input type="checkbox" id="userTypeSwitch" class="make-switch" tabindex="10" checked data-on-text="Yes" data-off-text="No" data-off-color="danger" data-on-color="default">
                                <label class="control-label pull-right" style="margin-top: 6px; margin-right: 10px;">Super Admin</label>--%>
                            </div>
                        </div>
                        <hr style="margin-top: 10px; margin-bottom: 10px;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Username</label><span class="required">*</span>
                                    <input type="text" id="username" class="form-control spinner" tabindex="11" placeholder="Username">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row passwordRow">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Password</label><span class="required">*</span>
                                    <input type="password" id="password" class="form-control spinner" tabindex="12" placeholder="Password">
                                </div>
                            </div>
                        </div>
                        <div class="row resetPasswordRow hide">
                            <div class="col-md-12">
                                <%if (user.UserTypeID == 3)
                                { %>
                                <a class="btn purple disabled pull-right" style="margin-top: 26px;" id="reset-password">Reset Password</a>
                                <% }
                                else
                                {%>
                                <a class="btn purple pull-right" style="margin-top: 26px;" id="reset-password">Reset Password</a>
                                <%       }    %>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="applicationPermissionsSection">
                    <div class="col-md-12">
                        <p style="color: #565656; margin: 0 0 10px 0">Can access these Features</p>
                    </div>
                    <div class="col-md-12">
                        <div class="portlet-body">
                            <table id="applicationTable" class="table table-hover" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th colspan='3'>Permissions </th>
                                    </tr>
                                    <tr>
                                        <th>MODULE</th>
                                        <th>ENABLE</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <%if (ApplicationList != null)
                                    {
                                        foreach (var item in ApplicationList)
                                        {%>
                                <tr>
                                    <td class='applicationName' data-id='<%=item.ID %>' style='padding-left: 16px;'><%=item.Name %> </td>
                                    <td style='padding-left: 16px;'>
                                        <input class='allowPermissions' disabled="disabled" type='checkbox' id="<%=item.ID %>"></td>
                                    <td style='padding-left: 16px;'>
                                        <a class='btn purple disabled btnConfigure' data-id="<%=user.UserTypeID %>" style='padding: 1px 10px 1px 10px; border-radius: 4px !important; font-size: 12px;' href='#' id='application_<%=item.ID %>' onclick="ShowPermissionModal('<%=item.ModalName %>')">Configure
                                        </a>
                                    </td>
                                </tr>
                                <% }
                                    }%>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <!-- END SAMPLE TABLE PORTLET-->
                    </div>
                </div>
                <div class="row" id="AccountStatus">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-7">
                                <h4 style="font-weight: bold; margin-top: 20px;">Account Status</h4>
                            </div>
                        </div>
                        <hr style="margin-top: 10px; margin-bottom: 10px;" />
                    </div>
                </div>
                <div class="row" id="AccountLocked">
                    <div class="col-md-12">
                        <label class="control-label pull-left" style="margin-top: 6px; margin-right: 10px;">Account Locked</label>
                        <input type="checkbox" id="LockAccountSwitch" class="make-switch" tabindex="10" checked data-on-text="Yes" data-off-text="No" data-off-color="danger" data-on-color="default">
                    </div>
                </div>
                <%if (IsBusinessCardAllowed == 1) {  %>
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-7">
                                <h4 style="font-weight: bold; margin-top: 20px;">Business Cards</h4>
                            </div>
                        </div>
                        <hr style="margin-top: 10px; margin-bottom: 10px;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="mt-checkbox-list">
                            <label class="mt-checkbox mt-checkbox-outline">
                                Requires new business cards
                            <input type="checkbox" tabindex="13" id="allowCard">
                                <span></span>
                            </label>
                        </div>
                    </div>
                </div>
                 <% } %>
            </div>
        </div>
    </div>
    <div class="row" style="margin-bottom: 15px;">
        <div class="col-md-6"></div>
        <div class="col-md-6">
            <div class="form-actions pull-right" style="margin-top: 10px;">
                <a class="btn default" style="width:75px;" href="Employees.aspx">Cancel</a>
                <a class="btn purple" style="width:75px;" id="btn_AddEditUser"></a>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT BODY -->
    <div id="CentralDataRepositoryPermissionsModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <%--<div class="scroller" style="height: 300px; padding-right: 0px;" data-always-visible="1" data-rail-visible1="1">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Can access these Features</label><span class="required">*</span>
                                <select id="userModules" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (FileRepositoryModulePermissions != null)
                                        {
                                            foreach (var item in FileRepositoryModulePermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Can access these File Categories</label><%--<span class="required">*</span>--%>
                                <select id="userFileCategories" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (FileRepositoryFileCategoryPermissions != null)
                                        {
                                            foreach (var item in FileRepositoryFileCategoryPermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Repository Permissions</label><%--<span class="required">*</span>--%>
                                <select id="userRepositories" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (FileRepositoryRepositoryPermissions != null)
                                        {
                                            foreach (var item in FileRepositoryRepositoryPermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                    </div>
                    <%--</div>--%>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btn_CentralDataRepositoryPermissionsPopup" class="btn purple">Assign</a>
                </div>
            </div>
        </div>
    </div>
    <div id="DentonsMitigatePermissionsModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">User access levels</label><span class="required">*</span>
                                <select id="RiskManagerUserLevelPermissions" class="form-control spinner" onchange="checkPermissions()" >  <%--onchange="checkPermissions()"--%>
                                    <option value="0">--- Select Access Levels ---</option>
                                    <%if (RiskManagerUserLevelPermissions != null)
                                        {
                                            foreach (var item in RiskManagerUserLevelPermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        } %>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Can access these Features</label><span class="required">*</span>
                                <select id="RiskManagerModulesPermissions" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (RiskManagerModulePermissions != null)
                                        {
                                            foreach (var item in RiskManagerModulePermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12" style="display:none;">
                            <div class="form-group">
                                <label class="control-label">Can access these Boards</label>
                                <select id="RiskManagerBoardsListPermissions" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (RiskManagerBoardList != null)
                                        {
                                            foreach (var item in RiskManagerBoardList)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btn_DentonsMitigatePermissionsPopup" class="btn purple">Assign</a>
                </div>
            </div>
        </div>
    </div>
    <div id="BusinessCardPermissionsModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Can access these Features</label><span class="required">*</span>
                                <select id="BusinessCardModulesPermissions" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (BusinessCardsPermissions != null)
                                        {
                                            foreach (var item in BusinessCardsPermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btn_BusinessCardPermissionsPopup" class="btn purple">Assign</a>
                </div>
            </div>
        </div>
    </div>
    <div id="DAC6PermissionsModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">User access levels</label><span class="required">*</span>
                                <select id="DAC6UserLevelPermissions" class="form-control spinner" onchange="checkPermissionsDAC6(1)" >  <%--onchange="checkPermissions()"--%>
                                    <option value="0">--- Select Access Levels ---</option>
                                    <%if (DAC6UserLevelPermissions != null)
                                        {
                                            foreach (var item in DAC6UserLevelPermissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        } %>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Can access these Features</label><span class="required">*</span>
                                <select id="DAC6ModulesPermissions" multiple="multiple" class="form-control spinner" data-width="100%">
                                    <%if (DAC6Permissions != null)
                                        {
                                            foreach (var item in DAC6Permissions)
                                            { %>
                                    <option value="<%=item.ID %>"><%=item.Name %> </option>
                                    <% }
                                        }%>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btn_DAC6PermissionsPopup" class="btn purple">Assign</a>
                </div>
            </div>
        </div>
    </div>
    <div id="TrainingCoursesPermissionsModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <h4 class="modal-title">Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">User Type</label><span class="required">*</span>
                                <select id="TrainingCoursesUserLevelPermissions" class="form-control spinner" >
                                    <option value="0">--- Select Access Levels ---</option>
                                    <option value="1">Admin</option>
                                    <option value="2">User</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn default" data-dismiss="modal">Cancel</a>
                    <a href="#" id="btn_TrainingCoursesPermissionsPopup" class="btn purple">Assign</a>
                </div>
            </div>
        </div>
    </div>

    <div id="UserProfileImageModal" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog" style="width:485px;">
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
    
    <script src="ApplicationScripts/AddEditUser.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/cropper.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/main.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/ValidationFields.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>

</asp:Content>
