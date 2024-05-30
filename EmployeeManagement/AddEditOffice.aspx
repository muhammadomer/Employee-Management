<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="AddEditOffice.aspx.cs" Inherits="EmployeeManagement.AddEditOffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row">
        <div class="col-md-12">
            <h3 style="font-weight: bold" id="addEditHeading">Manage Office</h3>
            <hr />
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <input type="hidden" id="locationID" />
                                <label class="control-label">Office</label><span class="required">*</span>
                                <input type="text" id="locationName" class="form-control spinner" tabindex="1" placeholder="Office" maxlength="250">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Region</label><span class="required">*</span>
                                <select id="ddlRegion" class="form-control spinner" tabindex="2"></select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Country</label><span class="required">*</span>
                                <select id="ddlCountry" class="form-control spinner" tabindex="3"></select>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">State/County/Province</label>
                                <%--<span class="required">*</span>--%>
                                <input type="text" id="state" class="form-control spinner" tabindex="4" placeholder="State/County/Province" maxlength="250" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">City</label><span class="required">*</span>
                                <select id="ddlCity" class="form-control spinner" tabindex="5" ></select>
                            </div>
                        </div>
                    </div>
                    <div class="row hidden">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Office</label><span class="required">*</span>
                                <input type="text" id="office" class="form-control spinner" value="Office" tabindex="6" placeholder="Office Name" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">GPS Postal/Zip Code</label>
                                <%--<span class="required">*</span>--%>
                                <input type="text" id="gpsPostal" class="form-control spinner" tabindex="7" placeholder="GPS Postal/Zip Code" maxlength="250" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Mail Postal/Zip Code</label><span class="required">*</span>
                                <input type="text" id="mailPostal" class="form-control spinner" tabindex="8" placeholder="Mail Postal/Zip Code" maxlength="250" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Address Line 1</label><span class="required">*</span>
                                <input type="text" id="addressLine1" class="form-control spinner" tabindex="9" placeholder="Address Line 1" maxlength="500" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Address Line 2</label>
                                <input type="text" id="addressLine2" class="form-control spinner" tabindex="10" placeholder="Address Line 2" maxlength="500" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="display:none">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Longitude</label><span class="required">*</span>
                                <input type="text" id="longitude" class="form-control spinner" tabindex="9" placeholder="Longitude">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Latitude</label><span class="required">*</span>
                                <input type="text" id="latitude" class="form-control spinner" tabindex="10" placeholder="Latitude">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">Telephone <span style="font-weight:600">(e.g. +441234567890)</span></label><span class="required">*</span>
                        <input type="text" id="telephone" class="form-control spinner" tabindex="11" maxlength="18" placeholder="Telephone" onkeypress='return isNumberKey(event);' />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <div class="form-actions pull-right" style="margin-top: 10px; margin-bottom: 10px;">
                        <a class="btn default" href="Offices.aspx">Cancel</a>
                        <a class="btn purple" id="btn_AddEditLocation"></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="ApplicationScripts/AddEditLocation.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
    <script src="ApplicationScripts/ValidationFields.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
</asp:Content>
