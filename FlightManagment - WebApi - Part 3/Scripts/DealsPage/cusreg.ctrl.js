module.controller('cusRegCtrl', CusRegCtrl)

function CusRegCtrl($scope, $http, globalConst, apiService, dataService) {

    $scope.form = {
        fullName: '',
        email: '',
        password: '',
        address: { country: "", state: "", city: "" },
        phoneNumber: '',
        card: "",
        agree: false
    }

    $scope.allCountries = globalConst.all_countries_list
    $scope.countryStates = dataService.allCountryStates
    $scope.stateCities = dataService.allStateCities
    $scope.tryWhenInvalid = false;
    $scope.counter = 0

    $scope.onSubmit = function(formIsValid) {
        console.log(++$scope.counter);
        if (formIsValid) {
            $scope.tryWhenInvalid = false;
            const customer = {
                First_Name: $scope.form.fullName.split(' ')[0],
                Last_Name: $scope.form.fullName.split(' ')[1],
                User_Name: $scope.form.email,
                Password: $scope.form.password,
                Address: $scope.form.address.country + " " + $scope.form.address.state + " " + $scope.form.address.city,
                Phone_No: $scope.form.phoneNumber,
                Credit_Card_Number: $scope.form.card
                };
                $http.post("https://localhost:44368/api/search/create/customer", JSON.stringify(customer))
                .then(
                    (resp) => {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: `${resp.data}`,
                            showConfirmButton: true,
                            timer: 1500
                        });
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

    // Watch For Password Input.
    $scope.$watch('form.password', (newPassword) => {
        elementIsValid(newPassword, /^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[#?!@$%^&*\-_]).{8,}$/, $scope.regForm.pass, $("#pass-validate"))
    })

    // Watch For Country Input.
    $scope.$watch('form.address.country', (newCountry) => {
        $scope.updateStatesSelect(newCountry)
        if ($scope.regForm.country.$pristine) {
            if (newCountry != "Choose Country") {
                $scope.regForm.country.$valid = true
            } else {
                $scope.regForm.country.$valid = false
            }
            checkCtrStateAndCity()
        }
    })
    
    // Watch For State Input.
    $scope.$watch('form.address.state', (newState) => {
        $scope.updateCitiesSelect($scope.form.address.country, newState)
        if (!$scope.regForm.state.$pristine) {
            if (newState != "Choose State") {
                $scope.regForm.state.$valid = true
            } else {
                $scope.regForm.state.$valid = false
            }
            checkCtrStateAndCity()
        }
    })

    // Watch For City Input.
    $scope.$watch('form.address.city', (newCity) => {
        if (!$scope.regForm.city.$pristine) {
            if (newCity != "Choose City") {
                $scope.regForm.city.$valid = true
            } else {
                $scope.regForm.city.$valid = false
            }
            checkCtrStateAndCity()
        }
    })

    // Watch For Phone Number Input.
    $scope.$watch('form.phoneNumber', (newNumber) => {
        elementIsValid(newNumber, /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{3,6}$/im, $scope.regForm.phNumber, $("#ph-number-validate"))
    })

    // Function That Checked If Country And City Input Is Valid.
    function checkCtrStateAndCity() {
        if ($scope.form.address.country != "Choose Country" && $scope.form.address.state != "Choose State" && $scope.form.address.city != "Choose City") {
            $("#address-validate").removeClass('alert-validate')
        } else {
            $("#address-validate").addClass('alert-validate')
        }
    }
    // Generic Function That Check If The Input Match To Reg
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

    // Update States Options When Country Has Been Changed.
    $scope.updateStatesSelect = (newValCountry) => {
        dataService.allCountryStates = ["Choose State"]
        if (newValCountry != "Choose Country") {
            $http.get(globalConst.getStatesBasicUrl + `${newValCountry}`).then((data) => {
                const temp = data.data.details.regionalBlocs;
                dataService.allCountryStates = temp.map(s => { return s.state_name })
                if (dataService.allCountryStates.length > 0)
                    dataService.allCountryStates.unshift("Choose State")
                else
                    dataService.allCountryStates = ["Choose State"]
                $scope.countryStates = dataService.allCountryStates
            }), (err) => {
                console.log(err);
                alert('alert')
            }
        }
    }

    // Update Cities Options When State Has Been Changed.
    $scope.updateCitiesSelect = (country, newValState) => {
        console.log(`${country} + ${newValState}`);
        dataService.allStateCities = ["Choose City"]
        if (newValState != "Choose State") {
            $http.get(globalConst.getStatesBasicUrl + `${country}` + "&state=" + `${newValState}`).then((resp) => {
                console.log(resp);
                const temp = resp.data;
                console.log(temp);
                $.each(temp, function(i, city) {
                    console.log(city.city_name);
                    dataService.allStateCities.push(city.city_name)
                })
                dataService.allStateCities.pop()
                $scope.stateCities = dataService.allStateCities
            }), (err) => {
                console.log(err);
                alert('alert')
            }
        }
    }

    $scope.$watch('tryWhenInvalid', (newVal) => {
        if (newVal) {
            if (!$scope.regForm.name.$valid)
                $("#fullname-validate").addClass('alert-validate')
            if (!$scope.regForm.email.$valid)
                $("#email-validate").addClass('alert-validate')
            if (!$scope.regForm.address.country.$valid || !$scope.regForm.address.state.$valid || !$scope.regForm.address.city.$valid)
                $("#address-validate").addClass('alert-validate')
            if (!$scope.regForm.phNumber.$valid)
                $("#ph-number-validate").addClass('alert-validate')
            if (!$scope.regForm.pass.$valid)
                $("#pass-validate").addClass('alert-validate')
            if (!$scope.regForm.termsAgree.checked)
                $("#agree-validate").addClass('alert-validate-agree')
        } else {
            $("#fullname-validate").removeClass('alert-validate')
            $("#email-validate").removeClass('alert-validate')
            $("#address-validate").removeClass('alert-validate')
            $("#ph-number-validate").removeClass('alert-validate')
            $("#pass-validate").removeClass('alert-validate')
            $("#agree-validate").removeClass('alert-validate-agree')
        }
    })
}