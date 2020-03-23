module.service("apiService", CreateApiService)

function CreateApiService($http, dataService, globalConst) {


    this.getCountryStates = (url) => {
        if (url != globalConst.getStatesBasicUrl + "Choose Country") {
            return $.ajax({ url: url})
            }
        }

    this.getAllItems = (url) => {
        //Get All Countries.
                return $http.get(globalConst.get_all_countries_url)
                    .then((countries) => {
                        dataService.allCountries = countries.data
                    })
            //Get All Companies.
            .then(() => {
                return $http.get(globalConst.get_all_companies_url)
                    .then((companies) => {
                        dataService.allCompanies = companies.data
                    })
                })
            //Get All Flights
                    .then(() => {
                        return $http.get(url)
                            .then((flights) => {
                                dataService.matchingVacancyFlights = flights.data
                                takeInfoOnSucReq(flights.data)
                            },
                                (err) => {
                                    alert('error')
                                    console.log(err)
                                })})}

    function takeInfoOnSucReq(data) {
        
            $.each(data, function (i, flight) {
                flight.airlineName = getCompanyNameById(flight.AirlineCompany_Id)
                flight.originCountry = getCountryNameById(flight.Origin_Country_Code)
                flight.destinationCountry = getCountryNameById(flight.Destination_Country_Code)
                flight.dDate = moment(flight.Departure_Time).format('ll')
                flight.dTime = moment(flight.Departure_Time).format('LT')
                flight.subTitle = getSubTitle(flight, i)
                flight.details = `Fly With ${flight.airlineName} To ${flight.destinationCountry} Will Depart On ${flight.dDate} At ${flight.dTime}... ${flight.Remaining_Tickets} Tickets Left ....Hurry up`
            })
    }

    // Get Company-Name By Id
    function getCompanyNameById(id) {
        const company = dataService.allCompanies.find(({ Id }) => Id == id)
        return company != undefined ? company.Airline_Name : "????"
    }

    // Get Country-Name By Id
    function getCountryNameById(id) {
        const country = dataService.allCountries.find(({ Id }) => Id == id)
        return country != undefined ? country.Country_Name : "????"
    }

    function getSubTitle(flight, index) {

    this.sentencesToSubject = [
        `${flight.destinationCountry}! Visit, Understand!!`,
        `Let's Pool In ${flight.destinationCountry} At Discount Prices For a Limited Time!`,
        `In The Comming Summer Everyone In ${flight.destinationCountry}!!`,
        `${flight.destinationCountry}, Amazing Places, Nostalgologs And Exciting!`
        ];

        return this.sentencesToSubject[index % 4]
    }
}
