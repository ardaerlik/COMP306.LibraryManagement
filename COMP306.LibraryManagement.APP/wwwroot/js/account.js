function Login(_email, _password) {
    const postData = {
        email: _email,
        password: _password
    };
    
    $.ajax({
        type: 'POST',
        async: true,
        url: "Login/Login",
        datatype: 'json',
        cache: false,
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=UTF-8',
        success: function (_data) {
            if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                window.location.href = '/Home';
            }
            else {
                window.location.href = '/Login';
                console.error("error: ", _data.exceptionMessage);
            }
        },
        error: function (xhr, errorType, exception) {
            console.error("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function Register(_fName, _lName, _email, _password) {
    const postData = {
        email: _email,
        password: _password,
        name: _fName,
        surname: _lName,
        roles: [2]
    };

    $.ajax({
        type: 'POST',
        async: true,
        url: "Register/Register",
        datatype: 'json',
        cache: false,
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=UTF-8',
        success: function (_data) {
            if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                window.location.href = '/Home';
            }
            else {
                window.location.href = '/Register';
                console.error("error: ", _data.exceptionMessage);
            }
        },
        error: function (xhr, errorType, exception) {
            console.error("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}
