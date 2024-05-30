

$(document).ready(function () {
    $("#offices").addClass("active");
    GetAllLocations();

});
GetAllLocations = function () {
    var url = 'Offices.aspx/GetAllLocations';
    AjaxPostRequestWithoutRequestPerameters(url, function (response) {

        $("#locationTable").empty();
        $("#locationTable").append(" <thead><tr><th colspan=8 style='padding-left: 10px;'>Offices</th></tr ><tr><th>OFFICE</th><th>REGION</th><th>COUNTRY</th><th>STATE/COUNTY/PROVINCE</th><th>CITY</th><th>TELEPHONE</th> <th style='width:75px;'></th></tr ></thead > <tbody>");
        var tableHTML = '';
        if (response !== null) {
            for (var i = 0; i < response.length; i++) {
                if (response[i].Name != "Unassigned") {
                    tableHTML += "<tr><td>" + htmlEncode(response[i].Name) + "</td><td>" + htmlEncode(response[i].RegionName) + "</td><td>" + htmlEncode(response[i].CountryName) + "</td><td>" + htmlEncode(response[i].State) + "</td><td>" + htmlEncode(response[i].CityName) + "</td><td>" + htmlEncode(response[i].Telephone) + "</td><td style='text-align: right;'><a href='AddEditOffice.aspx?id=" + response[i].ID + "' ><i class='fa fa-pencil customIcon'></i></a><a href='#' class='delete-location' data-id = '" + response[i].ID + "' data-toggle='tooltip' data-placement='top' title='Delete' ><i class='fa fa-times customIcon'></i></a></td></tr >";
                }
                else {
                    tableHTML += "<tr><td class='OfficeName'>" + htmlEncode(response[i].Name) + "</td><td>" + htmlEncode(response[i].RegionName) + "</td><td>" + htmlEncode(response[i].CountryName) + "</td><td>" + htmlEncode(response[i].State) + "</td><td>" + htmlEncode(response[i].CityName) + "</td><td>" + htmlEncode(response[i].Telephone) + "</td><td></td></tr >";
                }
            }
        }
        tableHTML += "</tbody>";
        $("#locationTable").append(tableHTML);
        $('#locationTable').dataTable({
            destroy: true,
            "oLanguage": {
                "oPaginate": {
                    "sPrevious": "<i class='fa fa-angle-double-left'></i>", // This is the link to the previous page
                    "sNext": "<i class='fa fa-angle-double-right'></i>" // This is the link to the next page
                }
            },
            "pageLength": 10,
            "info": false,
            //"ordering": false,
            //"lengthChange": false
            initComplete: function () {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            }

        });
        $(".dataTables_length").empty();
        $(".dataTables_length").append("<h3 class='pageHeading' style='margin: 8px 0px 0px 0px;font-weight:bold;float:left'>Offices</h3><a href='AddEditOffice.aspx' class='btn purple' style='margin: 8px 0px 0px 10px;'>New<i style='margin-left: 10px;' class='fa fa-plus-circle'></i></a>");
    });
};





$(document).off('click', '.delete-location').on('click', '.delete-location', function (e) {
    try {
        e.preventDefault();
        currentObject = $(this);
        var locationID = $(currentObject).attr("data-id");
        ShowConfirmationDialog('Delete', 'All employee profiles and risks containing the ' + $(currentObject).parents("tr").children('td:first').text() + ' office will be set to unassigned. Do you want to continue?', function () {
            var requestPerameters = {
                "locationID": locationID
            };
            var url = 'Offices.aspx/DeleteLocationByID';
            AjaxPostRequestWithRequestPerameters(url, requestPerameters, function (response) {
                if (response) {
                    toastr.success("Office deleted successfully.");
                    currentObject.parent().parent().remove();
                }
                else {
                    toastr.error("Faild to delete this office.");
                }
            });
        }, null, 'Delete');
    } catch (e) {
        console.log('Exception while trying to delete an office: ' + e.message);
    }
});