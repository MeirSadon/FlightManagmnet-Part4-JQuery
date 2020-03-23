//['$scope', '$http', 'apiService', 'dataService']
module.controller('dealsCtrl', DealsCtrl)


function DealsCtrl($scope, dataService, apiService) {

    // Get All Matching Flights From ApiService(Ajax)
    $scope.matchingVacancyFlights = dataService.matchingVacancyFlights;
    apiService.getAllItems(dataService.matchingVacancyFlightsUrl).then(() => {
        $scope.allCountries = dataService.allCountries;
        $scope.matchingVacancyFlights = dataService.matchingVacancyFlights
    })

    $scope.getFlightDetails = ($index) => {
        const currentFlight = $scope.allFlights[$index]
        return `The flight to ${$scope.allCountries.find(({ Id }) => Id == currentFlight.Destination_Country_Code).Country_Name} will depart on ${moment(currentFlight.Departure_Time).format('ll')}
                    at ${moment(currentFlight.Departure_Time).format('LT')}... ${currentFlight.Remaining_Tickets} tickets left ....Hurry up!`
    }
}











































//module.controller('dealsCtrl', ['$scope','apiService', 'dataService', function ($scope, $index, apiService, dataService) {

//    $scope.matchingVacancyFlights = dataService.matchingVacancyFlights;
//    apiService.getMatchingFlights(dataService.matchingVacancyFlightsUrl)

//    $scope.allCountries = dataService.allCountries;
//    apiService.getAllCountries()


//    $scope.getCountryNameById = ($index) => {
//        console.log($scope.allCountries);
//        console.log($index);
//        console.log($scope.allCountries.find(({ Id }) => Id == $scope.allFlights[$index].Destination_Country_Code));
//        const country = $scope.allCountries.find(({ Id }) => Id == $scope.allFlights[$index].Destination_Country_Code)
//        return country != undefined ? country.Country_Name : "????"
//    }

//    $scope.getSubTitle = () => {
//        const country = $scope.allCountries.find(({ Id }) => Id == $scope.allFlights[$index].Destination_Country_Code)
//        const countryName = country.Country_Name;
//        let sentencesToSubject = [
//            `${countryName}! Visit, Understand!!`,
//            `Let's Pool In ${countryName} At Discount Prices For a Limited Time!`,
//            `In The Coming Summer Everyone In ${countryName}!!`,
//            `${countryName}, Amazing Places, Nostalgologs And Exciting!`
//        ];
//        return sentencesToSubject[$index % 4]
//    }

//    $scope.getDepartureDate = ($index) => {
//        return $scope.allFlights[$index].Departure_Time
//    }

//    $scope.getFlightDetails = ($index) => {
//        const currentFlight = $scope.allFlights[$index]
//        return `The flight to ${$scope.allCountries.find(({ Id }) => Id == currentFlight.Destination_Country_Code).Country_Name} will depart on ${moment(currentFlight.Departure_Time).format('ll')}
//                    at ${moment(currentFlight.Departure_Time).format('LT')}... ${currentFlight.Remaining_Tickets} tickets left ....Hurry up!`
//    }
//}]);



