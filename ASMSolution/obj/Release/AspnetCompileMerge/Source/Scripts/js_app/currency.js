//Load Data in Table when documents is ready
$(document).ready(function ()
{
    loadData();
    //alert('load data');
    //$('#tbody').html('<tr><td>ID</td><td>ID</td><td>ID</td><td>ID</td><td>ID</td><td>ID</td></tr>');

});


//Load Data function
function loadData()
{
    $.ajax({
        url: "/currency/List",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function(result) {
            var html = '';
            //console.log(result);
            $.each(result, function(key, item) {
                html += '<tr>';
                //html += '<td>' + item.curreny_id + '</td>';
                html += '<td>' + item.curreny_code + '</td>';
                html += '<td>' + item.currency_name + '</td>';
                html += '<td>' + item.fl_active + '</td>';
                html += '<td>' + item.rec_modified_by + '</td>'; //' + item.rec_modified_date.toLocaleDateString() + '
                html += '<td>' + item.rec_modified_by + '</td>';
                //html += '<td><a href="#" onclick="return getByCode(' + item.code + ')">Edit</a> | <a href="#" onclick="Delele(' + item.curreny_id + ')">Delete</a></td>';
                html += '<td><a href="#" onclick="return getById(' + item.curreny_id + ')">Edit</a> | <a href="#" onclick="Delele(' + item.curreny_id + ')">Delete</a></td>';

                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function(errormessage) {
            alert(errormessage.responseText);
        }
    });

}

//Add Data Function
function Add()
{
    var res = validate();
    if (res == false)
    {
        return false;
    }
    var currencyObj = {
        curreny_id: $('#curreny_id').val(),
        curreny_code: $('#curreny_code').val(),
        currency_name: $('#currency_name').val(),
        fl_active: $('#fl_active').val()
    };

    $.ajax({
        url: "/currency/Add",
        data: JSON.stringify(currencyObj),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID
function getById(id) {
    //alert(id)

    $('#curreny_id').css('border-color', 'lightgrey');
    $('#curreny_code').css('border-color', 'lightgrey');
    $('#currency_name').css('border-color', 'lightgrey');
    $('#fl_active').css('border-color', 'lightgrey');

    $.ajax({
        url: "/currency/getById/" + id,
        typr: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            $('#curreny_id').val(result.curreny_id);
            $('#curreny_code').val(result.curreny_code);
            $('#currency_name').val(result.currency_name);
            $('#fl_active').val(result.fl_active);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}


//Function for getting the Data Based upon Employee ID
function getByCode(code)
{
    $('#curreny_id').css('border-color', 'lightgrey');
    $('#curreny_code').css('border-color', 'lightgrey');
    $('#currency_name').css('border-color', 'lightgrey');
    $('#fl_active').css('border-color', 'lightgrey');
    $('#rec_modified_date').css('border-color', 'lightgrey');
    $('#rec_modified_by').css('border-color', 'lightgrey');

    $.ajax({
        url: "/country/GetByCode/" + code,
        typr: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function(result) {
            $('#curreny_id').val(result.curreny_id);
            $('#curreny_code').val(result.curreny_code);
            $('#currency_name').val(result.currency_name);
            $('#fl_active').val(result.fl_active);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function(errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }

    var currencyObj = {
        curreny_id: $('#curreny_id').val(),
        curreny_code: $('#curreny_code').val(),
        currency_name: $('#currency_name').val(),
        fl_active: $('#fl_active').val()
    };

    $.ajax({
        url: "/currency/Update",
        data: JSON.stringify(currencyObj),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#curreny_id').val("");
            $('#curreny_code').val("");
            $('#currency_name').val("");
            $('#fl_active').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record
function Delele(id)
{
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/currency/PackDelete/" + id,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBox()
{
    $('#curreny_id').val("0");
    $('#curreny_code').val("");
    $('#currency_name').val("");
    $('#fl_active').val("");

    $('#btnUpdate').hide();
    $('#btnAdd').show();

    $('#curreny_code').css('border-color', 'lightgrey');
    $('#currency_name').css('border-color', 'lightgrey');
}

//Valdidation using jquery
function validate()
{
    var isValid = true;
    if ($('#curreny_code').val().trim() == "") {
        $('#curreny_code').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#curreny_code').css('border-color', 'lightgrey');
    }

    if ($('#currency_name').val().trim() == "") {
        $('#currency_name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#currency_name').css('border-color', 'lightgrey');
    }

    //if ($('#fl_active').val().trim() == "") {
    //    $('#fl_active').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#fl_active').css('border-color', 'lightgrey');
    //}

    return isValid;
}