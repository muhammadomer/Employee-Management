var requestPerameters;
var token = $('[name=__RequestVerificationToken]').val();

$(document).ready(function () {
    $("#users").addClass("active");
    GetAllUsers();

});
GetAllUsers = function () {
    var url = 'Employees.aspx/GetAllUsers';
    //AjaxPostRequestWithoutRequestPerameters(url, function (response) {

    //$("#userTable").empty();
    //$("#userTable").append(" <thead><tr><th colspan=9 style='padding-left: 10px;'>Employee Profiles</th></tr ><tr><th>FIRST NAME</th><th>LAST NAME</th><th>USERNAME</th><th>EMAIL</th><th>MOBILE NUMBER</th><th>OFFICE</th><th>REGION</th><th>DEPARTMENT</th><th style='width:75px;'></th></tr ></thead><tbody>");
    //var tableHTML = '';
    //if (response != null) {
    //    for (var i = 0; i < response.length; i++) {
    //        if (response[i].UserID > 1) {
    //            tableHTML += "<tr><td>" + response[i].FirstName + "</td><td>" + response[i].LastName + "</td><td>" + response[i].UserName + "</td><td><a href=mailto:" + response[i].Email + " style='text-decoration: none;'>" + response[i].Email + "</a></td><td>" + response[i].MobileNumber + "</td><td>" + response[i].Office + "</td><td>" + response[i].Region + "</td><td>" + response[i].Department + "</td><td style='text-align: right;'><a href='AddEditUser.aspx?id=" + response[i].UserID + "' ><i class='fa fa-pencil customIcon'></i></a><a href='#' data-id='" + response[i].UserID + "' class='delete-user'><i class='fa fa-times customIcon'></i></a></td></tr >";
    //        }
    //        else {
    //            tableHTML += "<tr><td>" + response[i].FirstName + "</td><td>" + response[i].LastName + "</td><td>" + response[i].UserName + "</td><td><a href=mailto:" + response[i].Email + " style='text-decoration: none;'>" + response[i].Email + "</a></td><td>" + response[i].MobileNumber + "</td><td>" + response[i].Office + "</td><td>" + response[i].Region + "</td><td>" + response[i].Department + "</td><td style='text-align: right;'><a href='#' ><i class='fa fa-pencil disabledCustomIcon'></i></a><a href='#'><i class='fa fa-times disabledCustomIcon'></i></a></td></tr >";
    //        }
    //        //optionHTML += "<option value=" + response[i].split(":")[0] + ">" + response[i].split(":")[1] + "</option>";
    //    }
    //}
    //tableHTML += "</tbody>";
    //$("#userTable").append(tableHTML);
    //$('#userTable').DataTable();
    $('#userTable').dataTable({
        destroy: true,
        "oLanguage": {
            "oPaginate": {
                "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                "sNext": "<i class='fa fa-angle-double-right'></i>", // This is the link to the next page
            }
        },
        "pageLength": 10,
        "info": false,
        "aaSorting": [],
        //"ordering": false,
        //"lengthChange": false
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
        }

    });
    $(".dataTables_length").empty();
    $(".dataTables_length").append("<h3 class='pageHeading' style='margin: 8px 0px 0px 0px;font-weight:bold;float:left'>Employee Profiles</h3><a href='AddEditUser.aspx' class='btn purple' style='margin: 8px 0px 0px 10px;'>New<i style='margin-left: 10px;' class='fa fa-plus-circle'></i></a><a onclick=\"OpenImportOfficesModal()\" class='btn purple' style='margin: 8px 0px 0px 10px;'>Import</a>");
    //});
}


//***************** DANISH ****************//
OpenImportOfficesModal = function () {
    $('.customfile-button').empty();
    $('.customfile-button').append('<i class="fa fa-file-o"></i>');

    $('#ImportMattersCount').text(0);
    $("#btnImport").attr("disabled", "disabled");
    $('#ImportModalResultRow').hide();
    $('#ImportModalProgressBar').hide();
    $('#MattersUpload').val('');
    $('.customfile-feedback').text('No file selected')

    $('#OfficesImportModal').modal({
        backdrop: 'static',
        keyboard: true
    });
}

OpenImportOfficesConfirmationModal = function () {

    $('#ImportConfirmModal').modal({
        backdrop: 'static',
        keyboard: true
    });
    return false;
}

ImportAfterConfirmation = function (rr) {

    console.log(requestPerameters);

    var url = 'ImportEmployeeFile.ashx';

    $('#ImportModalLoaderIcon').show();
    $('#ImportModalResultRow').hide();

    $('#progressbarinnerdiv').attr('style', 'width: 0%');
    $('#progressbarinnerdiv').text('0%');

    $.ajax({
        url: url,
        headers: { '__RequestVerificationToken': token },
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: requestPerameters,
        success: function (result) {

            console.log("Responce aya: ", result);

            result = JSON.parse(result);

            if (result.Mode) {
                if (result.Mode == 5) {
                    $('#ImportModalResultRow').show();
                    $('#ImportModalProgressBar').show();
                    $('#btnImport').prop("disabled", true);
                    $("#ImportModalViewLogBTN").attr("disabled", true);
                    //FilesUploadResponse(result.fileName)
                    progressbar(result.fileName, result.fileGroupID);
                }
                else if (result.Mode == 2) {
                    toastr.error(result.error);
                }
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            toastr.error("Error occured")
        }
    });

}
progressbar = function (fileName, fileGroupID) {
    debugger;
    fileName = fileName.replace("&", "%26");
    //fileName = "\"" + fileName + "\"";
    var source = new EventSource('ImportEmployeeContents.ashx?fname=' + fileName);

    source.addEventListener("open", function (event) {

    }, false);


    source.addEventListener("error", function (event) {
        if (event.eventPhase == EventSource.CLOSED) {
            source.close();
        }
    }, false);

    source.addEventListener("message", function (event) {
        var data = event.data;

        console.log("data:::", data);
        if (data.split('!')[0] == "Close") {
            return;
        }

        if (data.split('!')[0] == "Last") {

            $("#ImportModalViewLogBTN").attr("href", "../upload/" + data.split('!')[1]);
            $('#ImportModalViewLogBTN').removeAttr('disabled');

            $('#PracticeGroupIDURL').val("");
            $('#OfficeIDURL').val("");
            $('#FilterByURL').val("");

            $('#btnImport').prop("disabled", false);

            GetAllUsers();

        }
        else {
            var total = parseInt(data.split('+')[0].split(':')[1]);
            var error = parseInt(data.split('+')[2].split(':')[1]);
            var success = parseInt(data.split('+')[1].split(':')[1]);

            var resultString = success + " Entries Generated,  " + error + " errors";
            var percentage = Math.ceil(((success + error) / total) * 100);

            $('#progressbarinnerdiv').attr('style', 'width: ' + (percentage.toString() + '%'));
            $('#progressbarinnerdiv').text(percentage.toString() + '%');

            if (total == (success + error)) {
                $("#SSEResponseText").text(resultString);
            }
            else {
                $("#SSEResponseText").text("In progress..");
            }

        }

    }, false);
}

//********** Files Upload Event *************//
$('#filesUpload').on('change', function () {
    $('#FileImportConfirmModalRequestParams').val('');
    var files = $(this).get(0).files;

    if (files != null && files != '' && files.length > 0) {
        requestPerameters = new FormData();
        for (var i = 0; i < files.length; i++) {
            if (files[i].name.endsWith(".xlsx"))
                requestPerameters.append(files[i].name, files[i]);
            else {
                toastr.error("Please upload a file with .xlsx extension");
                return;
            }
        }

        //$('#FileImportConfirmModalRequestParams').val(requestPerameters);

    }
    else {
        toastr.error("Please upload a file with .xlsx extension");
    }
});

$('#MattersUpload').on('change', function () {
    $('#FileImportConfirmModalRequestParams').val('');
    var files = $(this).get(0).files;

    if (files != null && files != '' && files.length > 0) {
        requestPerameters = new FormData();
        for (var i = 0; i < files.length; i++) {
            if (files[i].name.endsWith(".xlsx")) {
                requestPerameters.append(files[i].name, files[i]);

                $('.customfile-button').empty();
                $('.customfile-button').append('<i class="fa fa-file-o"></i>');
                ImportMatterFile();
            }
            else {
                $('.customfile-button').empty();

                $('.customfile-button').append('<i class="fa fa-file-o"></i>');
                $('#ImportMattersCount').text(0);
                $("#btnImport").attr("disabled", "disabled");
                toastr.error("Please upload a file with .xlsx extension");
                return;
            }
        }

        //$('#FileImportConfirmModalRequestParams').val(requestPerameters);

    }
    else {
        toastr.error("Please upload a file with .xlsx extension");
    }
});

function ImportMatterFile() {
    //var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
    /*Checks whether the file is a valid excel file*/
    if (true/*regex.test($("#MattersUpload").val().toLowerCase())*/) {
        var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
        if ($("#MattersUpload").val().toLowerCase().indexOf(".xlsx") > 0) {
            xlsxflag = true;
        }
        /*Checks whether the browser supports HTML5*/
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                /*Converts the excel data in to object*/
                if (xlsxflag) {
                    var workbook = XLSX.read(data, { type: 'binary' });
                }
                else {
                    var workbook = XLS.read(data, { type: 'binary' });
                }
                /*Gets all the sheetnames of excel in to a variable*/
                var sheet_name_list = workbook.SheetNames;

                var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/

                sheet_name_list.forEach(function (y) {
                    /*Iterate through all sheets*/
                    /*Convert the cell value to Json*/

                    if (xlsxflag) {
                        var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                    }
                    else {
                        var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                    }
                    if (exceljson.length) {
                        cnt = cnt + exceljson.length;
                    }

                });

                if (cnt > 0) {
                    $('#ImportMattersCount').text(cnt);
                    $("#btnImport").removeAttr("disabled");

                }
                else {
                    $('#ImportMattersCount').text(0);
                    $("#btnImport").attr("disabled", "disabled");
                }
            }
            if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                reader.readAsArrayBuffer($("#MattersUpload")[0].files[0]);
            }
            else {
                reader.readAsBinaryString($("#MattersUpload")[0].files[0]);
            }
        }
        else {
            alert("Sorry! Your browser does not support HTML5!");
        }
    }
    else {
        alert("Please upload a valid Excel file!");
    }
}

FilesUploadResponse = function (fileName, fileGroupID) {

    fileName = fileName.replace("&", "%26");

    var url = 'ImportEmployeeContents.ashx?fname=' + fileName;

    //$('#ImportModalLoaderIcon').show();
    $('#ImportModalResultRow').show();


    $.ajax({
        url: url,
        headers: { '__RequestVerificationToken': token },
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        //data: { "fname": fileName },
        success: function (result) {
            debugger;
            console.log("Responce aya: ", result);

            result = JSON.parse(result);

            if (result.Mode) {

                var data = result.status;

                var total = parseInt(data.split('+')[0].split(':')[1]);
                var error = parseInt(data.split('+')[2].split(':')[1]);
                var success = parseInt(data.split('+')[1].split(':')[1]);
                var logFileName = data.split('+')[3].split(':')[1];

                var resultString = success + " Entries Generated,  " + error + " errors";

                $("#ImportModalViewLogBTN").attr("href", "../upload/" + logFileName);
                $('#ImportModalViewLogBTN').removeAttr('disabled');

                $('#PracticeGroupIDURL').val("");
                $('#OfficeIDURL').val("");
                $('#FilterByURL').val("");

                $('#btnImport').prop("disabled", false);

                $("#SSEResponseText").text(resultString);

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            toastr.error("Error occured")
        }
    });


}

//*****************************************//

//*****************************************//



$(document).off('click', '.delete-user').on('click', '.delete-user', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var userID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'Do you want to delete this Employee?', function () {
            var requestPerameters = {
                "userID": userID
            };
            var url = 'Employees.aspx/DeleteUserByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Employee deleted successfully.");
                    currentObject.parent().parent().remove();
                }
                else {
                    toastr.error("Unable to delete Employee.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an employee: ' + e.message);
    }
});