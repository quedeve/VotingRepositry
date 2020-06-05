$(document).ready(function () {
    var fullName = getCookie('FullName');
    if (fullName != null && fullName != "") {
        $('#LoginMenu').empty();
        $('#LoginMenu').append(LoginMenu(fullName)); 
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

   
    $('#VotingTable').dataTable({
        searching: false,
        language: {
            processing: "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        initComplete: function () {
            $(this.api().table().container()).find("input").parent().wrap("<form>").parent().attr("autocomplete", "off");
        },
        ajax: {
            url: URLAPI + "Voting/GetAllClientSide",
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
                width: "15%"
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

                    return '<a class="btn btn-success waves-effect waves-themed  id="' + t.votingId + '" data-toggle="modal" data-target="#ModalVote" data-id="' + t.votingId + '" >Vote</a > ';
                        
                },
                width: "10%"
            }
        ]
    });


    $('#btn-search').on('click', function (e) {

        $('#VotingTable').dataTable({
            searching: false,
            language: {
                processing: "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            destroy: true,
            processing: true,
            serverSide: true,
            initComplete: function () {
                $(this.api().table().container()).find("input").parent().wrap("<form>").parent().attr("autocomplete", "off");
            },
            ajax: {
                url: URLAPI + "Voting/GetAllClientSide?CategoryID=" + $('#dropdownCategory').val() + "&VotingName=" + $('#VotingName').val(),
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
                    width: "15%"
                },
                {
                    mRender: function (a, e, t) {
                        return FormatStringDate(t.created);
                    },
                    width: "10%"
                },
                {
                    data: "supportersCount",
                    width: "10%"
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

                        return '<a class="btn btn-success waves-effect waves-themed  id="' + t.votingId + '" data-toggle="modal" data-target="#ModalVote" data-id="' + t.votingId + '" >Vote</a > ';

                    },
                    width: "10%"
                }
            ]
        });

    });


    $('#ModalVote').on('show.bs.modal', function (e) {
        resetModal();
        $('#VotingID').val(e.relatedTarget.getAttribute('data-id'));
        
    });


    $('#btn-vote').on('click', function (e) {

        var userData = {
            VotingID: parseInt($("#VotingID").val())
        }
        $.ajax({
            url: URLAPI + "UserVote/Add",
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
                    messageBox("Good job!", "Vote Sucess!", "success");
                    $('#VotingTable').DataTable().ajax.reload();
                    $("#ModalVote").modal('hide');
                    $(document.body).removeClass('modal-open');
                    $('.modal-backdrop').remove();
                } else {

                    messageBox("Opps!", response.message, "error");
                    $("#ModalVote").modal('hide');
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
