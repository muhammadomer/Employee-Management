<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="~/Licensing.aspx.cs" Inherits="EmployeeManagement.Licensing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <style type="text/css">
        .FileUploadClass {
            background-color: Yellow;
            font-size: 5px;            
            cursor: pointer;
        }



         .FileUploadClass > div {
            width: 100% !important;
        }

            .FileUploadClass input {
                background-color: #6E2D91 !important;
                border: Dashed 2px #000000;
                width: 100% !important;
                cursor: pointer;
            }

                .FileUploadClass input[button] {
                    background-color: Yellow;
                    border: Dashed 2px #000000;
                    cursor: pointer;
                }

.table > thead:first-child > tr:first-child > th {
    background-color: #fff;
    color: #333;
    font-size: 12px;
    padding-top: 10px;
    padding-bottom: 10px;
    font-weight: bold;
}
    </style>

    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <div class="container-fluid" style="padding: 0px;">

        <div class="row" id="licSection" runat="server">
            
            <div class="col-md-6">
                <div class="panel panel-default" style="min-height: 210px;">
                    <div class="panel-heading">
                        <div class="caption">
                            <h3 class="panel-title">License Code</h3>
                        </div>
                    </div>
                    <div class="panel-body" id="">
                    
                        <asp:DropDownList ID="cboLicense" runat="server" Width="50%" Height="36px" Style="border: 1px solid #e5e5e5;">
                        </asp:DropDownList>

                        <div id="trQuantity">
                            <asp:DropDownList ID="cboQuantity" runat="server" Width="50%" Height="36px" Style="border: 1px solid #e5e5e5; margin-top:20px;">
                            </asp:DropDownList>
                        </div>
                        

                        <br />

                        <button type="button" class="btn purple" data-dismiss="modal" onclick="AddLicense()" style="width: 50%; margin-top: 20px;">Add License Code</button>

                        <div id="divGridLicense">
                            <table id='tblLicenses' class='table table-striped table-hover table-bordered' style="margin-top:20px;">
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        
                        <div id="divGenerateLicense">
                            <asp:Button ID="btnCreateLicenseKey" ClientIDMode="Static" runat="server" Text="Generate License Code" OnClick="btnCreateLicenseKey_Click" Width="50%" CssClass="btn purple" OnClientClick="DownloadFileComplete();" />
                        </div>
                        
                    </div>
                </div>
            </div>
            
            <div class="col-md-6">
                <div class="panel panel-default" style="min-height: 210px;">
                    <div class="panel-heading">
                        <div class="caption">
                            <h3 class="panel-title">License Key</h3>
                        </div>
                    </div>
                    <div class="panel-body">
                    
                        <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" OnClientUploadError="uploadError"
                            OnClientUploadStarted="StartUpload" OnClientUploadComplete="UploadComplete" CompleteBackColor="Lime"
                            UploaderStyle="Modern" BackColor="Red" ThrobberID="Throbber" OnUploadedComplete="AsyncFileUpload1_UploadedComplete"
                            UploadingBackColor="#66CCFF" CssClass="FileUploadClass" Style="position: relative; background-color:red; z-index: 2; bottom: -44px;
                            height: 34px;opacity: 0; filter: alpha(opacity=0); cursor: pointer" />
                        
                        <asp:Button ID="BtnFsAttch" runat="server" Text="Apply License Key" Style="position: relative; top:70px; cursor: pointer" CssClass="btn purple" Width="100%" Visible="false"/>
                        <div title="test" class="btn purple"  Style="position: relative; top:10px; cursor: pointer; width:100%;">Apply License Key</div>
                        

                        <div class=" col-xs-12 col-sm-12 col-md-3" style="visibility: hidden;">
                            <asp:Label ID="lblStatus" runat="server" class="form-control Label"></asp:Label>
                            <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" class="form-control Button" Text="Show"></asp:Button>
                        </div>

                        <%--<a href="#" class="btn btn-default purple" id="" style="width:30%;">Apply License Key
                        </a>
                        <input type="file" id="uploadUsrImage" onchange="readURL(this);" style="display:none;"/>--%>

                    </div>
                </div>
            </div>
        
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default" style="min-height: 150px;">
                    <div class="panel-heading">
                        <div class="caption">
                            <h3 class="panel-title">Available Licenses</h3>
                        </div>
                    </div>
                    <div class="panel-body" id="">
                    
                        <table id='tblAvailableLicenses' class='table table-striped table-hover table-bordered' style="margin-bottom:0px;">
                            <tbody>
                            </tbody>
                        </table>

                    </div>
                </div>
            </div>
            
        </div>

        <%--<div class="row" style="margin-top: 0%; width: 100%;">
            
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" id="divGenerateLicenseCode" style="display: none;">
                    <div style="border-style: ridge; border-color: #4d5b69; border-width: 1px 1px 1px 1px; height: auto;">
                        <div class="row" style="width: 100%; background-color: #4d5b69; text-align: left; vertical-align: middle; line-height: 40px;">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="">
                                <span style="font-size: 18px; color: #fffeff;">License Code</span>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 20px; margin-bottom: 22px; text-align: right;">
                            <div class=" col-xs-12 col-sm-12 col-md-12">
                                <asp:DropDownList ID="cboLicense" runat="server" Width="100%" Height="36px" Style="border: 1px solid #e5e5e5;">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 20px; margin-bottom: 22px; text-align: right; display: none" id="trQuantity">
                            <div class=" col-xs-12 col-sm-12 col-md-7">
                                <asp:DropDownList ID="cboQuantity" runat="server" Width="100%" Height="36px" Style="border: 1px solid #e5e5e5;">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-5"></div>
                        </div>



                        <div class="row" style="margin-top: 10px; margin-bottom: 5px; text-align: right;">
                            <div class=" col-xs-12 col-sm-12 col-md-7">
                                <button type="button" class="btnFlat" data-dismiss="modal" onclick="AddLicense()" style="width: 100%;">Add License Code</button>
                            </div>
                            <div class="col-md-5"></div>
                        </div>
                        <div class="row" style="margin-top: 5px; margin-bottom: 5px" id="divGridLicense">
                            <div class=" col-xs-12 col-sm-12 col-md-12">
                                <table id='tblLicenses' class='table table-striped table-hover table-bordered'>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 5px; margin-bottom: 10px; text-align: right; display: none;" id="divGenerateLicense">
                            <div class=" col-xs-12 col-sm-12 col-md-7">
                                <asp:Button ID="btnCreateLicenseKey" ClientIDMode="Static" runat="server" Text="Generate License Code" OnClick="btnCreateLicenseKey_Click" Width="100%" CssClass="btnFlat" OnClientClick="DownloadFileComplete();" />
                            </div>
                            <div class="col-md-5"></div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" id="divApplyLicenseCode" style="display: none;">
                    <div style="border-style: ridge; border-color: #4d5b69; border-width: 1px 1px 1px 1px;">
                        <div class="row" style="width: 100%; background-color: #4d5b69; text-align: left; vertical-align: middle; line-height: 40px;">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="">
                                <span style="font-size: 18px; color: #fffeff;">License Key</span>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 25px;padding-bottom:5px">
                            <div class="col-xs-12 col-sm-12 col-md-6" style="cursor: pointer;">
                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" OnClientUploadError="uploadError"
                                    OnClientUploadStarted="StartUpload" OnClientUploadComplete="UploadComplete" CompleteBackColor="Lime"
                                    UploaderStyle="Modern" BackColor="Red" ThrobberID="Throbber" OnUploadedComplete="AsyncFileUpload1_UploadedComplete"
                                    UploadingBackColor="#66CCFF" CssClass="FileUploadClass" Style="position: absolute; left: -10px; z-index: 2; opacity: 0; filter: alpha(opacity=0); cursor: pointer" />
                                <asp:Button ID="BtnFsAttch" runat="server" Text="Apply License Key" Style="position: absolute; top: -5px; left: 10px; z-index: 1; cursor: pointer" CssClass="btnFlat" Width="100%" />
                                <asp:Label ID="lblStatus" runat="server" class="form-control Label"></asp:Label>
                            </div>
                            <div class=" col-xs-12 col-sm-12 col-md-3" style="visibility: hidden;">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" class="form-control Button" Text="Show"></asp:Button>
                            </div>
                            <div class=" col-xs-12 col-sm-12 col-md-3"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="divAvaliableLicenses" class="col-xs-12 col-sm-12 col-md-6 col-lg-6" style="margin-top: 10px; display: none;">

                    <div style="border-style: ridge; border-color: #4d5b69; border-width: 1px 1px 1px 1px;">
                        <div class="row" style="width: 100%; background-color: #4d5b69; text-align: left; vertical-align: middle; line-height: 40px;">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="">
                                <span style="font-size: 18px; color: #fffeff;">Available Licenses</span>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 0%; width: 100%;">
                            <div id="grdContainer" class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="padding: 10px;">
                                <table id='tblAvailableLicenses' class='table table-striped table-hover table-bordered'>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>--%>
    
    </div>
    <script>

        function notifyMe(msgType, msg) {
            $().toastmessage(msgType, msg);
        }

        function DownloadFileComplete()
        {
            setTimeout(function () {
                $("#divGridLicense").hide();
                $("#divGenerateLicense").hide();
                $("#trQuantity").hide();
                document.getElementById('<%=cboLicense.ClientID %>').selectedIndex = 0;
                document.getElementById('<%=cboQuantity.ClientID %>').selectedIndex = 0;
            }, 1000);
        }

        function btnFileUpload_Click()
        {
            document.getElementById('<%=AsyncFileUpload1.ClientID %>').click();
        }

        function uploadError(sender, args) {
            document.getElementById('<%=lblStatus.ClientID %>').innerText = args.get_fileName(),
	        "<span style='color:red;'>" + args.get_errorMessage() + "</span>";
        }

        function StartUpload(sender, args) {
            var filename = args.get_fileName();
            var stringCheck = ".lic";
            var foundIt = (filename.lastIndexOf(stringCheck) === filename.length - stringCheck.length) > 0;
            if (foundIt) {
                document.getElementById('<%=lblStatus.ClientID %>').innerText = 'Uploading Started.';
            }  
        }

        function UploadComplete(sender, args) {
            
            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            var text = "Size of " + filename + " is " + args.get_length() + " bytes";
            if (contentType.length > 0) {
                text += " and content type is '" + contentType + "'.";
            }
            document.getElementById('<%=lblStatus.ClientID %>').innerText = text;
            
            __doPostBack('<%= btnShow.UniqueID %>', '');
        }

        function OnFieldNameChange() {
            var dropdownIndex = document.getElementById('<%=cboLicense.ClientID %>').selectedIndex;
             var dropdownValue = document.getElementById('<%=cboLicense.ClientID %>')[dropdownIndex].text;


             //if (dropdownValue.indexOf("Server") != -1 || dropdownValue.indexOf("---") != -1) {
             //    $("#trQuantity").hide();
             //}
             //else {
             //    $("#trQuantity").show();
            //}
            if (dropdownValue.indexOf("bundle") != -1 ) {
                $("#trQuantity").show();
            }
            else {
                $("#trQuantity").hide();
                
            }
        }

        function GetAvailableLicenses() {
            $('#tblAvailableLicenses').empty();
            $.ajax(
            {
                type: "POST",
                url: "Licensing.aspx/GetAvailableLicenses",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var trHTML = '<thead><tr><th>LICENSE</th><th>DESCRIPTION</th><th>STATUS</th></tr></thead><tbody></tbody>';
                    $('#tblAvailableLicenses').append(trHTML);
                    $('#divAvaliableLicenses').show();
                    if (data.d.length > 0) {
                        for (var i = 0; i < data.d.length; i++) {
                            var trHTML = '';
                            trHTML += '<tr><td>' + data.d[i].License + '</td>';
                            trHTML += '<td>' + data.d[i].Description + '</td>';
                            trHTML += '<td>' + data.d[i].Status + '</td></tr>';
                            $('#tblAvailableLicenses').append(trHTML);


                        }
                    }
                }
            });
        }

        function AddLicense() {

             var dropdownIndex = document.getElementById('<%=cboLicense.ClientID %>').selectedIndex;
             var dropdownValue = document.getElementById('<%=cboLicense.ClientID %>')[dropdownIndex].text;

            if ($("#<%=cboLicense.ClientID%>").get(0).selectedIndex == 0) {
                notifyMe('showErrorToast', 'Please select license.');
                return;
            }
            else if (dropdownValue.indexOf("vBOARD Client") != -1  && $("#<%=cboQuantity.ClientID%>").get(0).selectedIndex == 0) {
                notifyMe('showErrorToast', 'Please select quantity.');
                return;
            }

            var Lic = $("#<%=cboLicense.ClientID%>").val();
            var Quantity = $("#<%=cboQuantity.ClientID%>").val();
            var LicText = $('#<%=cboLicense.ClientID %> option:selected').text()
            var Param = { "Lic": Lic, "Quantity": Quantity, "LicText": LicText };
            $('#tblLicenses').empty();
            $.ajax(
            {
                type: "POST",
                url: "Licensing.aspx/AddLicenses",
                data: JSON.stringify(Param),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        var trHTML = '<thead><tr><th>MODULE</th><th>QUANTITY</th><th>TIME PERIOD</th></tr></thead><tbody></tbody>';
                        $('#tblLicenses').append(trHTML);
                        for (var i = 0; i < data.d.length; i++) {
                            var trHTML = '';
                            trHTML += '<tr><td>' + data.d[i].Module + '</td>';
                            trHTML += '<td>' + data.d[i].Quantity + '</td>';
                            trHTML += '<td>' + data.d[i].TimePeriod + '</td></tr>';
                            $('#tblLicenses').append(trHTML);
                            $('#divGenerateLicense').show();
                            $('#divGridLicense').show();

                        }
                    }
                }
            });
        }

        $(document).ready(function () {
            GetAvailableLicenses();
            $("#linkLicensing").addClass("active open");

            $("#divGenerateLicense").hide();
            $("#divApplyLicenseCode").show();

        });

        function openFileDialog() {
            //alert('a');
            $("#ctl00_MainContent_AsyncFileUpload1_ctl02").click();
        }

    </script>

</asp:Content>