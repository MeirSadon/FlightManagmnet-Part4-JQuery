module.controller('cusRegCtrl', CusRegCtrl)


function CusRegCtrl($scope, dataService, apiService) {

    $scope.form = {
        fullName: '',
        email: '',
        password: '',
        phoneNumber: ''
    }

    $scope.$watch('form.email', (newEmail, old) => {
        console.log($scope.form.email);
        if (newEmail.match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) != null) {
            $("#email-validate").removeClass('alert-validate')
        } else {
            if ($scope.form.email.$pristine)
             $("#email-validate").addClass('alert-validate')
        }
    })

    $scope.onSubmit = (formIsValid) => {
    console.log($scope.form);
        return formIsValid////////////////////////////////////////
    }

    

}