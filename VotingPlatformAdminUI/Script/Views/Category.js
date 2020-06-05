$(document).ready(function () {

    var fullName = getCookie('FullName');
    if (fullName != null && fullName != "") {
        $('#FullName').empty();
        $('#FullName').append(fullName);
    }
    else {
        $('#FullName').empty();
        $('#FullName').append("Please Login!");
    }

    $('#ModalEdit').on('show.bs.modal', function (e) {
        resetModal();
      

        $('#CategoryID').val(e.relatedTarget.getAttribute('data-categoryID'));
        $('#CategoryName').val(e.relatedTarget.getAttribute('data-categoryName'));
        $('#Description').val(e.relatedTarget.getAttribute('data-description'));

    });
    //Validation
    var forms = document.getElementsByClassName('needs-validation');
    var button = document.getElementById("btn-save");
    // Loop over them and prevent submission
    var validation = Array.prototype.filter.call(forms, function (form) {
        button.addEventListener('click', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            } else {
           

              

                var values = {
                    CategoryID: parseInt($('#CategoryID').val()),
                    CategoryName: $('#CategoryName').val(),
                    Description: $('#Description').val(),
                };

                $.ajax({
                    url: URLAPI + "Category/Add",
                    contentType: 'application/json',
                    dataType: 'json',
                    type: "POST",
                    data: JSON.stringify(values),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie("Token"));
                    },
                    success: function (response) {
                        if (!response.isLogin) {
                            $.when(Swal.fire({
                                title: 'Opps!!',
                                text: 'Your Token is Expired, Please Login First!!!!',
                                icon: 'error',
                                confirmButtonText: 'Ok'
                            })).then(function (e) {
                                window.location.href = "/Views/login.html"
                            });
                        }
                        if (response.isSuccess) {
                            $("#ModalEdit").modal('hide');
                            $(document.body).removeClass('modal-open');
                            $('.modal-backdrop').remove();
                            messageBox("Good job!", "Data Saved!", "success");
                            $('#CategoryTable').DataTable().ajax.reload();
                        } else {
                            messageBox("Opps!", response.message, "error");
                            $("#ModalEdit").modal('hide');
                            $(document.body).removeClass('modal-open');
                            $('.modal-backdrop').remove();
                        }
                    },
                    error: function (jqXHR, text, errorThrown) {
                        console.log(jqXHR + " " + text + " " + errorThrown);
                    }
                });
            }
            form.classList.add('was-validated');
        });
    });
    $('#CategoryTable').dataTable({
        language: {
            processing: "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        processing: true,
        serverSide: true,
        initComplete: function () {
            $(this.api().table().container()).find("input").parent().wrap("<form>").parent().attr("autocomplete", "off");
        },
        ajax: {
            url: URLAPI + "Category/GetAllPagination",
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie("Token"));
            },
            dataSrc: function (json) {
                if (!json.isLogin) {
                    $.when(Swal.fire({
                        title: 'Opps!!',
                        text: 'Your Token is Expired, Please Login First!!!!',
                        icon: 'error',
                        confirmButtonText: 'Ok'
                    })).then(function (e) {
                        window.location.href = "/Views/login.html"
                    });
                }
                json.draw = json.draw;
                json.recordsTotal = json.recordsTotal;
                json.recordsFiltered = json.recordsFiltered;
                json.data = json.listCategory;

                var return_data = json;
                return return_data.data;
            }
        },
        responsive: true,
        columns: [
            {
                data: "categoryName",
                width: "15%"
            },
            {
                data: "description",
                width: "10%"
            },
            {
                mRender: function (a, e, t) {
                  
                    return '<a class="btn btn-xs btn-success waves-effect waves-themed" data-description="' + t.description + '" data-categoryName="' + t.categoryName + '" data-categoryID="' + t.categoryId + '" data-toggle="modal" data-target="#ModalEdit"><i class="fas fa-edit"></i></a> '
                        + '<a class="btn btn-xs btn-danger waves-effect waves-themed"   data-categoryID="' + t.categoryId + '" data-toggle="modal" data-target="#ModalDelete"><i class="fas fa-trash-alt"></i></a>';
                },
                width: "15%"
            }
        ]
    });


 

    //$('#ExportExcel').on('click', function (e) {

    //    post_to_url(URLAPI + "RentBook/DownloadExcel", "", "get")
    //});

    //$('#ExportPDF').on('click', function (e) {

    //    post_to_url(URLAPI + "RentBook/DownloadPDF", "", "get")
    //});

    $('#ModalDelete').on('show.bs.modal', function (e) {
        resetModal();
        $('#CategoryID').val(e.relatedTarget.getAttribute('data-categoryID'));

    });


    $('#btn-delete').on('click', function (e) {

        var userData = {
            CategoryID: parseInt($("#CategoryID").val())
        }
        $.ajax({
            url: URLAPI + "Category/Delete",
            type: "POST",
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(userData),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie("Token"));
            },
            success: function (response) {
                if (!response.isLogin) {
                    $.when(Swal.fire({
                        title: 'Opps!!',
                        text: 'Your Token is Expired, Please Login First!!!!',
                        icon: 'error',
                        confirmButtonText: 'Ok'
                    })).then(function (e) {
                        window.location.href = "/Views/login.html"
                    });
                }
                if (response.isSuccess) {
                    messageBox("Good job!", "Data Deleted!", "success");
                    $('#CategoryTable').DataTable().ajax.reload();
                    $("#ModalDelete").modal('hide');
                    $(document.body).removeClass('modal-open');
                    $('.modal-backdrop').remove();
                } else {

                    messageBox("Opps!", response.message, "error");
                    $("#ModalDelete").modal('hide');
                    $(document.body).removeClass('modal-open');
                    $('.modal-backdrop').remove();
                }
            },
            error: function (jqXHR, text, errorThrown) {
                console.log(jqXHR + " " + text + " " + errorThrown);
            }
        });
    });

});
