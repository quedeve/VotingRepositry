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

    $.ajax({
        url: URLAPI + "Category/GetAll",
        contentType: 'application/json',
        dataType: 'json',
        type: "GET",
        success: function (response) {

            if (response.isSuccess) {
                $.each(response.listCategory, function (key, entry) {
                    $("#dropdownCategory").append($('<option></option>').attr('value', entry.categoryId).text(entry.categoryName));
                })
            } else {
                messageBox("Error!", response.message, "error");
            }
        },
        error: function (jqXHR, text, errorThrown) {
            console.log(jqXHR + " " + text + " " + errorThrown);
        }
    });

    $('#ModalEdit').on('show.bs.modal', function (e) {
        resetModal();


        $('#VotingID').val(e.relatedTarget.getAttribute('data-votingID'));
        $('#VotingName').val(e.relatedTarget.getAttribute('data-votingName'));
        $('#dropdownCategory').val(e.relatedTarget.getAttribute('data-categoryID'));
        $('#DueDate').val(FormatStringDate(e.relatedTarget.getAttribute('data-dueDate')));
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
                var currentDate = new Date();
                var dueDate = new Date($('#DueDate').val());
                if (currentDate >= dueDate) {
                    return messageBox("Error!", "Due Date must greater than Current Date", "error");
                }
                var values = {
                    VotingID: parseInt($('#VotingID').val()),
                    VotingName: $('#VotingName').val(),
                    VotingDescription: $('#Description').val(),
                    CategoryID: parseInt($('#dropdownCategory').val()),
                    DueDateString: $('#DueDate').val()
                };

                $.ajax({
                    url: URLAPI + "Voting/Add",
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
                        //if (!response.isLogin) {
                        //    $.when(swal("Good job!", response.message, "error")).then(function (e) {
                        //        window.location.href = "/Views/login.html"
                        //    });
                        //}
                        if (response.isSuccess) {
                            $("#ModalEdit").modal('hide');
                            $(document.body).removeClass('modal-open');
                            $('.modal-backdrop').remove();
                            messageBox("Good job!", "Data Saved!", "success");
                            $('#VotingTable').DataTable().ajax.reload();
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
    $('#VotingTable').dataTable({
        language: {
            processing: "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        processing: true,
        serverSide: true,
        initComplete: function () {
            $(this.api().table().container()).find("input").parent().wrap("<form>").parent().attr("autocomplete", "off");
        },
        ajax: {
            url: URLAPI + "Voting/GetAllPagination",
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie("Token"));
            },
            dataSrc: function (json) {
                
                json.draw = json.draw;
                json.recordsTotal = json.recordsTotal;
                json.recordsFiltered = json.recordsFiltered;
                json.data = json.listVoting;

                var return_data = json;
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
                return return_data.data;
            }
        },
        responsive: true,
        columns: [
            {
                data: "votingName",
                width: "15%"
            },
            {
                data: "votingDescription",
                width: "10%"
            },
            {
                mRender: function (a, e, t) {
                    return FormatStringDate(t.created);
                },
                width:"10%"
            },
            {
                data: "supportersCount",
                width : "10%"
            },
            {
                mRender: function (a, e, t) {
                    return FormatStringDate(t.dueDate);
                },
                width: "10%"
            },
            {
                data: "categoryName",
                width: "10%"
            },
            {
                mRender: function (a, e, t) {

                    return '<a class="btn btn-xs btn-success waves-effect waves-themed" data-dueDate="' + t.dueDate+'" data-votingID="'+t.votingId+'" data-description="' + t.votingDescription + '" data-votingName="' + t.votingName + '" data-categoryID="' + t.categoryId + '" data-toggle="modal" data-target="#ModalEdit" > <i class="fas fa-edit"></i></a > '
                        + '<a class="btn btn-xs btn-danger waves-effect waves-themed"   data-votingID="' + t.votingId + '" data-toggle="modal" data-target="#ModalDelete"><i class="fas fa-trash-alt"></i></a>';
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
        $('#VotingID').val(e.relatedTarget.getAttribute('data-votingID'));

    });


    $('#btn-delete').on('click', function (e) {

        var userData = {
            VotingID: parseInt($("#VotingID").val())
        }
        $.ajax({
            url: URLAPI + "Voting/Delete",
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
                    $('#VotingTable').DataTable().ajax.reload();
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
