var validationExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$£%&_?*^"/=\\'+\-`|,(){}[\]:;<>.])[A-Za-z\d~!@#$£%&_?*^"/=\\'+\-`|,(){}[\]:;<>.]{8,}/;

validateUserPassword = function (password, userName) {
    if (!validationExpression.test(password)) {
        return "Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.";
    }
    else {
        return "true";
    }

};

validateUserPasswordWithConfirmPassword = function (password, confirmPassword, userName) {
    
    //var validationExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%£*?&])[A-Za-z\d$@$!%£*?&]{8,}/;
    if (password != confirmPassword) {
        return "The password you supplied do not match";
    }
    //if (password.toLowerCase().indexOf(userName) >= 0) {
    //    return "Password cannot contain username";
    //}
    else if (!validationExpression.test(password)) {
        return "Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.";
    }
    else {
        return "true";
    }
};