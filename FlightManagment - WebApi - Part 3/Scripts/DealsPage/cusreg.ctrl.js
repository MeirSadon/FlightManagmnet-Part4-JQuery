module.controller('cusRegCtrl', CusRegCtrl)


function CusRegCtrl($scope, $http) {

    $scope.form = {
        fullName: '',
        email: '',
        phoneNumber: '',
        password: '',
        agree: false
    }

    $scope.tryWhenInvalid = false;


    $scope.onSubmit = function (formIsValid) {
        if (formIsValid) {
            $scope.tryWhenInvalid = false;

            const customer = {
                First_Name: $scope.form.fullName.split(' ')[0],
                Last_Name: $scope.form.fullName.split(' ')[1],
                User_Name: $scope.form.email,
                Password: $scope.form.password,
                Address: null,
                Phone_No: $scope.form.phoneNumber,
                Credit_Card_Number: null,
                };
                $http.post("https://localhost:44368/api/search/create/customer", JSON.stringify(customer))
                .then(
                    (resp) => {
                        
                    },
                    // error
                    (err) => {
                        alert('error')
                        console.log(err)
                    }
                )
        }
        if (!formIsValid) {
            $scope.tryWhenInvalid = true;
            }
    }


    // Watch For Full Name Input.
    $scope.$watch('form.fullName', (newFullName) => {
        elementIsValid(newFullName, /^[a-zA-Z]+ [a-zA-Z]+$/, $scope.regForm.name, $("#fullname-validate"))
    })

    // Watch For Email Input.
    $scope.$watch('form.email', (newEmail) => {
        elementIsValid(newEmail, /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/, $scope.regForm.email, $("#email-validate"))
    })

    // Watch For Phone Number Input.
    $scope.$watch('form.phoneNumber', (newNumber) => {
        elementIsValid(newNumber, /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{3,6}$/im, $scope.regForm.phNumber, $("#ph-number-validate"))
    })

    // Watch For Password Input.
    $scope.$watch('form.password', (newPassword) => {
        elementIsValid(newPassword, /^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[#?!@$%^&*\-_]).{8,}$/, $scope.regForm.pass, $("#pass-validate"))
    })

    function elementIsValid(elm, reg, nameFromHtml, elmFromHtml) {
        if (!nameFromHtml.$pristine) {
            if (reg.test(String(elm))) {
                elmFromHtml.removeClass('alert-validate')
                nameFromHtml.$valid = true
            } else {
                elmFromHtml.addClass('alert-validate')
                nameFromHtml.$valid = false
            }
        }
    }
    $scope.$watch('tryWhenInvalid', (newVal) => {
        if (newVal) {
            if (!$scope.regForm.name.$valid)
                $("#fullname-validate").addClass('alert-validate')
            if (!$scope.regForm.email.$valid)
                $("#email-validate").addClass('alert-validate')
            if (!$scope.regForm.phNumber.$valid)
                $("#ph-number-validate").addClass('alert-validate')
            if (!$scope.regForm.pass.$valid)
                $("#pass-validate").addClass('alert-validate')
            if (!$scope.regForm.termsAgree.checked)
                $("#agree-validate").addClass('alert-validate-agree')
        } else {
            $("#fullname-validate").removeClass('alert-validate')
            $("#email-validate").removeClass('alert-validate')
            $("#ph-number-validate").removeClass('alert-validate')
            $("#pass-validate").removeClass('alert-validate')
            $("#agree-validate").removeClass('alert-validate-agree')
        }
    })
}