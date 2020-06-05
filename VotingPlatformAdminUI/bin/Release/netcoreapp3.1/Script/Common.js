 function resetModal() {
                var $t = $('[data-dismiss=modal]'),
                    target = $t[0].href || $t.data("target") || $t.parents('.modal') || [];
                $(target)
                    .find("input,textarea,select")
                    .val('')
                    .end()
                    .find("input[type=checkbox], input[type=radio]")
                    .prop("checked", "")
                    .end();
}


function formatRupiah(input) {


    var number_string = input.toString(),
        sisa = number_string.length % 3,
        rupiah = number_string.substr(0, sisa),
        ribuan = number_string.substr(sisa).match(/\d{3}/g);

    if (ribuan) {
        separator = sisa ? '.' : '';
        rupiah += separator + ribuan.join('.');
    }
    return rupiah;
}

function formatRupiahWithSelector(input) {

    var number_without_separator = $(input).val().toString().split('.'),
        number_string = number_without_separator.join('').toString(),
        sisa = number_string.length % 3,
        rupiah = number_string.substr(0, sisa),
        ribuan = number_string.substr(sisa).match(/\d{3}/g);
    if (ribuan) {
        separator = sisa ? '.' : '';
        rupiah += separator + ribuan.join('.');
    }
    $(input).val(rupiah);
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

post_to_url = function (path, params, method) {
    method = method || "post";

    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key) && params[key] != null) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
} 


function FormatStringDate(input) {
    
    var inputSplit = input.split('T');
    var dateSplit = inputSplit[0].split('-');

    return dateSplit[0] + '-' + dateSplit[1] + '-' + dateSplit[2];

}


function ValidatePeminjaman(start, end) {

    var startDate = start.split('-');
    var endDate = end.split('-');

    return ((endDate[0] - startDate[0]) * 365) + ((endDate[1] - startDate[1]) * 30) + (endDate[2] - startDate[2]);

}



function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}



function GetMenu(input) {
    
    var menu = ''
    if (input.toLowerCase() == 'admin') {
        menu = ' <li class="nav-item ">                    <a class="nav-link" href="RentBook.html">                        <i class="fas fa-fw fa-table"></i>                        <span>BookRent</span>                    </a>                </li>                <li class="nav-item ">                    <a class="nav-link" href="BookMaintain.html" >                        <i class="fas fa-fw fa-table"></i>                        <span>BookMaintain</span>                    </a>                </li>                <li class="nav-item ">                    <a class="nav-link" href="UserProfileMaintain.html">                        <i class="fas fa-fw fa-table"></i>                        <span>User Profile Maintain</span>                    </a>                </li>'; 
    }
    else {
        menu = menu + '<li class="nav-item ">                    <a class="nav-link" href="RentBook.html">                        <i class="fas fa-fw fa-table"></i>                        <span>BookRent</span>                    </a>                </li>';
            
    }
    return menu;
}


function validateEmail(input) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(input));
}

function validatePassword(input) {
    

    return /[a-z]/.test(input) && /[0-9]/.test(input) && /[A-Z]/.test(input) && /\S{8,}$/.test(input);

}


function messageBox(title, text, icon) {
    Swal.fire({
        title: title,
        text: text,
        icon: icon,
        confirmButtonText: 'Ok'
    });
}


function messageBoxWithButton(title, text, icon, confirmButtonText) {
     Swal.fire({
        title: title,
        text: text,
        icon: icon,
        confirmButtonText: confirmButtonText
    });
}


$('#Logout').on('click', function (e) {
    var userData = {

        Token: getCookie('Token')
    }
    $.ajax({
        url: URLAPI + "UserProfile/Logout",
        type: "POST",
        dataType: 'json',
        contentType: "application/json",
        data: JSON.stringify(userData),
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie("Token"));
        },
        success: function (response) {
            if (response.isSuccess) {
                $.when(Swal.fire({
                    title: 'Good job!!',
                    text: 'Logout Success',
                    icon: 'success',
                    confirmButtonText: 'Ok'
                })).then(function (e) {
                    document.cookie = "Token=" + response.token;
                    document.cookie = "FullName=" + "";
                    window.location.href = "/Views/login.html"
                });
            } else {

                swal("Opps!", response.message, "error");
            }
        }
    });
})


