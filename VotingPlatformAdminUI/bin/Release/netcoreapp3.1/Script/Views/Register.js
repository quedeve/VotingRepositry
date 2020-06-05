$(document).ready(function () {

    $.ajax({
        url: URLAPI + "Gender/GetAll",
        contentType: 'application/json',
        dataType: 'json',
        type: "GET",
        success: function (response) {

            if (response.isSuccess) {
                $.each(response.listGender, function (key, entry) {
                    $("#dropdownGender").append($('<option></option>').attr('value', entry.genderId).text(entry.name));
                })
            } else {
                messageBox("Error!", response.message, "error");
            }
        },
        error: function (jqXHR, text, errorThrown) {
            console.log(jqXHR + " " + text + " " + errorThrown);
        }
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
                if (!validateEmail($('#Email').val())) {
                    $('#invalidEmail').show().empty();
                    $('#invalidEmail').show().append("Your Email Format is not Valid");
                    //return swal("error", "Your Email Format is not Valid");
                    return Swal.fire({
                        title: 'Error!',
                        text: 'Your Email Format is not Valid',
                        icon: 'error',
                        confirmButtonText: 'Ok'
                    });
                }
                if (!validatePassword($('#Password').val())) {
                    $('#invalidPassword').show().empty();
                    $('#invalidPassword').show().append("Password must be combination upper case, lower case, number and at least 8 characters.");
                    return messageBox("error", "Password must be combination upper case, lower case, number and at least 8 characters.","error");
                    
                }
                if ($('#Password').val() != $('#RePassword').val()) {
                    return messageBox("Error!", "Your Password and RePassword  not match","error");
                }

                var values = {
                    Email: $('#Email').val(),
                    Password: $('#Password').val(),
                    FirstName: $('#FirstName').val(),
                    LastName: $('#LastName').val(),
                    Age: parseInt($('#Age').val()),
                    Gender: parseInt($('#dropdownGender').val()),
                    Role: "Admin"
                };

                $.ajax({
                    url: URLAPI + "UserProfile/Register",
                    contentType: 'application/json',
                    dataType: 'json',
                    type: "POST",
                    data: JSON.stringify(values),
                    success: function (response) {
                        if (response.isSuccess) {


                            $.when(Swal.fire({
                                title: 'Success',
                                text: 'Data has been saved',
                                icon: 'sucsess',
                                confirmButtonText: 'Ok'
                            })).then(function (e) {
                                window.location.href = "/Views/Login.html"
                            });
                        } else {
                            messageBox("Opps!", response.message, "error");
                        }
                    },
                    error: function (jqXHR, text, errorThrown) {
                        console.log(jqXHR + " " + text + " " + errorThrown);
                        messageBox("Opps!", response.message, "error");
                    }
                });
            }
            form.classList.add('was-validated');
        });
    });

    //End Validation

});