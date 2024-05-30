<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="EmployeeManagement.Employees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
      .applicationDefaultBackgroundColor {
    background-color: /*#6B2D93*/ #3f51b5 !important;
    border-color: /*#6B2D93*/ #3f51b5;
    color: white;
}

        .applicationDefaultBackgroundColor:hover {
            color: white;
        }

        .applicationDefaultColor {
            color: /*#6B2D93*/ #3f51b5 !important;
        }

        .applicationDefaultBackgroundColor2 {
            background-color: #74BB2A !important;
            border-color: #74BB2A;
            color: white;
        }

            .applicationDefaultBackgroundColor2:hover {
                color: white;
            }

        .applicationDefaultColor2 {
            color: #74BB2A !important;
        }

.applicationDefaultBackgroundColor3 {
    background-color: #599FD8 !important;
    border-color: #599FD8;
    color: white;
}

    .applicationDefaultBackgroundColor3:hover {
        color: white;
    }

.applicationDefaultColor3 {
    color: #599FD8 !important;
}
.DarkGrey, .DarkGrey:hover {
    background-color: #666666 !important;
    border-color: #666666 !important;
    color: white !important;
}

    </style>

    <!-- BEGIN PAGE CONTENT BODY -->
    <div class="row" style="margin-left: 0px; margin-right: 0px;">
        <div class="col-md-12" style="padding: 0px;">
            <div class="portlet-body">
                <table id="userTable" class="table table-hover" style="width: 100%">
                    <thead>
                        <tr>
                            <th colspan="11" style='padding-left: 10px;'>Employee Profiles</th>
                        </tr>
                        <tr>
                            <th></th>
                            <th>FIRST NAME</th>
                            <th>LAST NAME</th>
                            <th>USERNAME</th>
                            <th>EMAIL</th>
                            <th>MOBILE NUMBER</th>
                            <th>OFFICE</th>
                            <th>REGION</th>
                            <th>DEPARTMENT</th>
                            <th>Role</th>
                            <th style='width: 75px;'></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%if (users != null)
                            {
                                foreach (var item in users)
                                {%>
                        <tr>
                            <td>
                                <img src="<%= item.ProfileImage %>" style="width: 35px; height: 35px; border-radius: 25px !important;" /></td>
                            <td><%= HttpUtility.HtmlDecode(item.First_Name) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.Last_Name) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.Username) %></td>
                            <td><a href="mailto:<%= item.Email %>" style='text-decoration: none;'><%= item.Email %></a></td>
                            <td><%= HttpUtility.HtmlDecode(item.Mobile_Number) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.Office) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.Region) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.Department) %></td>
                            <td><%= HttpUtility.HtmlDecode(item.UserTypeName) %></td>
                            <% var encrypt = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.Second.ToString() + ">" + item.ID + ">" + DateTime.Now.TimeOfDay.ToString())); %>

                            <td style='text-align: right;'><a href='AddEditUser.aspx?id=<%= encrypt %>'><i class='fa fa-pencil customIcon'></i></a>
                                <%if (item.ID > 1)
                                    { %>
                                <a href='#' data-id='<%= item.ID %>' class='delete-user'><i class='fa fa-times customIcon'></i></a>
                                <% } %>
                            </td>
                        </tr>
                        <%}
                            } %>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    

    <!-- Import Offices New modal -->
<div id="OfficesImportModal" tabindex="-1" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width:550px;">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title float-left" id="OwnerModalHeading" style="display:inline-block;">Import</h4>
                <a class="float-left margin-left-10" style="margin-top:7px;font-size:11px;" href="./downloads/EmployeeManagementFile.xlsx" download="Employee Management">Download Sample</a>
            </div>
            <div class="modal-body">

                <div class="row row-top-margin">
                    <div class="col-md-3">
                        <label for="filesUpload" class="control-label">File Path</label>
                    </div>
                    <div class="col-md-9">
                        <input type="file" class="form-control-file" id="MattersUpload" />
                    </div>
                </div>
                <div class="row row-top-margin">
                    <div class="col-md-3">
                        <label for="filesUpload" class="control-label">Total Entries</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row">
                            <div class="col-xl-6 col-lg-6 col-md-6">
                                <span class="float-left margin-top-8"><b id="ImportMattersCount">549</b> Found</span>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6">
                                <button id="btnImport" class="btn applicationDefaultBackgroundColor2 float-right" disabled onclick="return OpenImportOfficesConfirmationModal();">Import</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row row-top-margin" id="ImportModalProgressBar">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9 margin-top-8">
                        <div class="col-md-4 applicationDefaultColor" style="text-align:left;padding:0px;">
                            <span>0%</span>
                        </div>
                        <div class="col-md-4 applicationDefaultColor" style="text-align:center">
                            <span>50%</span>
                        </div>
                        <div class="col-md-4 applicationDefaultColor" style="text-align:right;padding:0px;">
                            <span>100%</span>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label class="control-label" style="margin-top:3px !important;">Progress</label>
                    </div>
                    <div class="col-md-9">
                        <div id="progressbarouterdiv" class="progress">
                            <div id="progressbarinnerdiv" class="progress-bar"></div>
                        </div>
                    </div>

                </div>

                <div class="row" id="ImportModalResultRow">
                    <div class="col-md-3">
                        <label class="control-label">Results</label>
                    </div>
                    <div class="col-md-9  margin-top-8">
                        <span id="SSEResponseText"></span>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a class="btn DarkGrey" id="ImportModalViewLogBTN" href="" disabled download="Import - Log File">View Log</a>
                <button type="button" class="btn applicationDefaultBackgroundColor" id="TAXModeratorModalOkBtn" onclick="$('#OfficesImportModal').modal('hide');">Close</button>
            </div>
        </div>

    </div>
</div>

    <!-- Office Import Confirmation Modal -->
<div id="ImportConfirmModal" tabindex="-1" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width:500px;">
        <input type="hidden" id="FileImportConfirmModalRequestParams" />
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="SignOffConfirmModalHeading">Confirmation</h4>
            </div>
            <div class="modal-body">

                <div class="row">
                    <div class="col-md-12">
                        <span id="SignOffConfirmationText" class="">Are you sure you want to import this file?</span>
                    </div>
                </div>


            </div>
            <div class="modal-footer">
                <button type="button" class="btn DarkGrey" id="SignOffConfirmModalNoButton" onclick="$('#ImportConfirmModal').modal('hide');">No</button>
                <button type="button" class="btn applicationDefaultBackgroundColor" id="ImportMatterConfirmModalYesButton" onclick="$('#ImportConfirmModal').modal('hide'); ImportAfterConfirmation($('#FileImportConfirmModalRequestParams').val())">Yes</button>
            </div>
        </div>

    </div>
</div>

    <!-- END PAGE CONTENT BODY -->
   <script src="ApplicationScripts/Users.js?v=<%=ConfigurationManager.AppSettings["VersionNumber"]%>"></script>
</asp:Content>
