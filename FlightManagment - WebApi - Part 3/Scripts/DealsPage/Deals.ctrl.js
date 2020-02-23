module.controller('dealsCtrl', ['$scope', '$http', function ($scope, $http, $index) {

    $scope.allFlights = []
    $scope.allCountries = []

    $http.get(`https://localhost:44368/api/search/flights/byfilters?onlyVacancy=true`)
        .then(
            (resp) => {
                $scope.allFlights = resp.data
            },
            // error
            (err) => {
                alert('error')
                console.log(err)
            }
    )

    $http.get(`/api/search/countries`)
        .then(
            (resp) => {
                $scope.allCountries = resp.data
            },
            // error
            (err) => {
                alert('error')
                console.log(err)
            }
        )

    $scope.getCountryNameById = ($index) => {
        const country = $scope.allCountries.find(({ Id }) => Id == $scope.allFlights[$index].Destination_Country_Code)
        return country != undefined ? country.Country_Name : "????"
    }

    $scope.getSubTitle = ($index) => {
        const country = $scope.allCountries.find(({ Id }) => Id == $scope.allFlights[$index].Destination_Country_Code)
        const countryName = country.Country_Name;
        let sentencesToSubject = [
            `${countryName}! Visit, Understand!!`,
            `Let's Pool In ${countryName} At Discount Prices For a Limited Time!`,
            `In The Coming Summer Everyone In ${countryName}!!`,
            `${countryName}, Amazing Places, Nostalgologs And Exciting!`
        ];
        return sentencesToSubject[$index % 4]
    }

    $scope.getDepartureDate = ($index) => {
        return $scope.allFlights[$index].Departure_Time
    }

    $scope.getFlightDetails = ($index) => {
        const currentFlight = $scope.allFlights[$index]
        return `The flight to ${$scope.allCountries.find(({ Id }) => Id == currentFlight.Destination_Country_Code).Country_Name} will depart on ${moment(currentFlight.Departure_Time).format('ll')}
                    at ${moment(currentFlight.Departure_Time).format('LT')}... ${currentFlight.Remaining_Tickets} tickets left ....Hurry up!`
    }

    $scope.getPicturePath = ($index) => {
        return `../../Content/Images/Vacations/vacation${parseInt(Math.random() * 5)}.jpg`
    }
}]);