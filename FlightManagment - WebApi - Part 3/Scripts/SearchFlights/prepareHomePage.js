let refreshPageTimer = window.setTimeout("location.reload();", 1000 * 60 * 5);
let allCountries = []
let allCompanies = []

// Get All Countries/Flights/Companies With Ajax And Prepare All Select List.
$(function () {
    $.ajax({
        url: `/api/search/countries` //Send Ajax To Get All Countries.
    }).then(function (countries) {
        allCountries = countries
        $.each(countries, function (i, country){
            $("#allOriginCountriesLst").append(`<option value=${country.Id}>${country.Country_Name}</option>`);
            $("#allDestinationCountriesLst").append(`<option value=${country.Id}>${country.Country_Name}</option>`);
        });
    })
    $.ajax({
        url: `/api/search/flights` //Send Ajax To Get All Flights.
    }).then(function(flights){
        $.each(flights, function (i, flight){
            $("#allFlightsNumberLst").append(`<option value=${flight.Id}>${flight.Id}</option>`);
        });
    })
    $.ajax({
        url : `/api/search/companies` // Send Ajax To Get All Companies.
    }).then(function (companies) {
        allCompanies = companies
        $.each(companies, function (i, company) {
            $("#allCompaniesLst").append(`<option value=${company.Id}>${company.Airline_Name}</option>`);
        });
    }).then(function (data) {
        getMatchingFlights(searchFlightsUrl)
    }) 
});

//Auto Refresg Page(Every 5 Minutes)
function refreshPage(time) {
    if (typeof (Storage !== "undefined")) {
        window.clearTimeout(refreshPageTimer)
        refreshPageTimer = setTimeout("location.reload();", time);
    }
}