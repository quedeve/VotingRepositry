$(document).ready(function () {
    var token = getCookie('FullName');
    if (token != null && token != "") {
        window.location.href = "/Views/CategoryMaintain.html";
    }



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
                    return messageBox("error", "Password must be combination upper case, lower case, number and at least 8 characters.", "error");

                }
                var values = {
                    Email: $('#Email').val(),
                    Password: $('#Password').val(),
                };

                $.ajax({
                    url: URLAPI + "UserProfile/Authenticate",
                    contentType: 'application/json',
                    dataType: 'json',
                    type: "POST",
                    data: JSON.stringify(values),
                    success: function (response) {
                        if (response.isSuccess) {

                            debugger;
                            document.cookie = "Token=" + response.token;
                            document.cookie = "FullName=" + response.userProfile.firstName + " " + response.userProfile.lastName; 
                            $.when(Swal.fire({
                                title: 'Good job!!',
                                text: 'Login Success',
                                icon: 'success',
                                confirmButtonText: 'Ok'
                                 })).then(function (e) {
                                    window.location.href = "/Views/CategoryMaintain.html"
                            });
                        } else {
                            messageBox("Opps!", response.message, "error");
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

    //End Validation





});