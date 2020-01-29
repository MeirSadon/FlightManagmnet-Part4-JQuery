// Fields
let searchState = 0
let searchFlightsUrl = `https://localhost:44368/api/search/flights/byfilters?`

//Functions.

// Change Search Options (All/Only Departures/ Only Landings) - Happen When One From Menu Elements Has Clicked.
function editSearchState(state) {
    if (ajax_inprocess == false && searchState != state) {
        ajax_inprocess = true
        searchState = state
        resetFilters()
        switch(state){
            case 1: // Only Departures Search.
                flipAvailabilityOfInputs(true)
                $("#titleform").html("Search Departures Flights")
                $('#deparInHoursNmb').prop('disabled', false)
                $('#deparInHoursRng').prop('disabled', false)
                $('#deparInHoursRng').prop('min', '1')
                $('#deparInHoursNmb').prop('min', '1')
                $('#srchBtn').val("Search Departures Flights!").css({ 'font-size': '15px' })
                showSwalMessage('Departures Search Area', 'Here You Can Search Only Departures Flights.', '/Content/Images/Depar_Flight.jpg')
                break;
            case 2: // Only Landings Search.
                flipAvailabilityOfInputs(true)
                $("#titleform").html("Search Landings Flights")
                $('#landInHoursNmb').prop('disabled', false)
                $('#landInHoursRng').prop('disabled', false)
                $('#landInHoursRng').prop('min', '1')
                $('#landInHoursNmb').prop('min', '1')
                $('#srchBtn').val("Search Landings Flights!").css({ 'font-size': '15px' })
                showSwalMessage('Landing Search Area', 'Here You Can Search Only Landings Flights.', '/Content/Images/Land_Flight.jpg')
                break;
            default: //All Flights Search.
                flipAvailabilityOfInputs(false)
                $('#landInHoursRng').prop('min', '0')
                $('#landInHoursNmb').prop('min', '0')
                $('#deparInHoursRng').prop('min', '0')
                $('#deparInHoursNmb').prop('min', '0')
                $("#titleform").html("Search Your Flight")
                $('#srchBtn').val("Search Your Flight!").css({ 'font-size': '20px' })
                showSwalMessage('Main Search', 'Here You Can Search Any Flight.', '/Content/Images/Main_Flights_Search.jpg')
                break;
            }
    }
}

// Notice The User On The New Flights Search Area.
function showSwalMessage(title, text, imageUrl) {
    Swal.fire({
        title: title,
        html: '<b style="font-size: 14px;">' + text + '</b>',
        width: 450,
        imageUrl: imageUrl,
        imageWidth: 430,
        imageHeight: 300,
        imageAlt: title,
        timer: 2200
    })
    //Wait To Seconds And Send Ajax To Get Matches Flights.
    setTimeout(function () {
        ajax_inprocess = false;
        updateSearchUrl()
    }, 2500)

}

// Disable/Enable Inputs Search By Current Search-State.
function flipAvailabilityOfInputs(isItDisabled){
    $('#deparInHoursNmb').prop('disabled', isItDisabled)
    $('#deparInHoursRng').prop('disabled', isItDisabled)
    $('#landInHoursNmb').prop('disabled', isItDisabled)
    $('#landInHoursRng').prop('disabled', isItDisabled)
    $('#flightDurationNmb').prop('disabled', isItDisabled)
    $('#flightDurationRng').prop('disabled', isItDisabled)
    $('#deparFromByDate').prop('disabled', isItDisabled)
    $('#deparUpToByDate').prop('disabled', isItDisabled)
    $('#landFromByDate').prop('disabled', isItDisabled)
    $('#landUpToByDate').prop('disabled', isItDisabled)
    $('#onlyVacancyChBx').prop('disabled', isItDisabled)
}


// Compare Values Between Range&Number inputs.
function changeValue(from, to){
    if(to != null){
        to.val(parseInt(from.val()))
    }
    if(from.val == 0){
        to.val("")
    }

    if(from.val() < from.prop('min')){
        from.val(from.prop('min'))
    }
    if(from.val() > from.prop('max')){
        from.val(from.prop('max'))
    }
}

// Update Url Search By Selected Filters And Call The Search Function("showMatchesFlights").
function updateSearchUrl(){
    switch(searchState){
        case 1:
            searchFlightsUrl = `https://localhost:44368/api/search/flights/byfilters?`+
            `fromCountry=${$('#allOriginCountriesLst').val()}`+
            `&toCountry=${$('#allDestinationCountriesLst').val()}`+
            `&flightNumber=${$('#allFlightsNumberLst').val()}`+
            `&byCompany=${$('#allCompaniesLst').val()}`+
            `&depInHours=${$('#deparInHoursRng').val()}`+
            `&onlyVacancy=false`
            break;
            case 2:
                var today = moment().format('YYYY-MM-DDTHH:mm:ss');
                searchFlightsUrl = `https://localhost:44368/api/search/flights/byfilters?`+
                `fromCountry=${$('#allOriginCountriesLst').val()}`+
                `&toCountry=${$('#allDestinationCountriesLst').val()}`+
                `&flightNumber=${$('#allFlightsNumberLst').val()}`+
                `&byCompany=${$('#allCompaniesLst').val()}`+
                `&landInHours=${$('#landInHoursRng').val()}`+
                `&fromDepDate=${today}`+
                `&onlyVacancy=false`
                break;
            default:
                searchFlightsUrl = `https://localhost:44368/api/search/flights/byfilters?`+
                `fromCountry=${$('#allOriginCountriesLst').val()}`+
                `&toCountry=${$('#allDestinationCountriesLst').val()}`+
                `&flightNumber=${$('#allFlightsNumberLst').val()}`+
                `&byCompany=${$('#allCompaniesLst').val()}`+
                `&depInHours=${$('#deparInHoursNmb').val()}`+
                `&landInHours=${$('#landInHoursNmb').val()}`+
                `&flightDurationByHours=${$('#flightDurationNmb').val()}`+
                `&fromDepDate=${moment($('#deparFromByDate').val()).format('ll')}`+
                `&upToDepDate=${moment($('#deparUpToByDate').val()).format('ll')}`+
                `&fromlandDate=${moment($('#landFromByDate').val()).format('ll')}`+
                `&upToLandDate=${moment($('#landUpToByDate').val()).format('ll')}`+
                `&onlyVacancy=${$('#onlyVacancyChBx').prop('checked')}`
                break;
    }
    getMatchingFlights(searchFlightsUrl)
  }

// Reset Filters Search To Default Search-State Values.
function resetFilters() {
    $('#allOriginCountriesLst').val("")
    $('#allDestinationCountriesLst').val("")
    $('#allFlightsNumberLst').val("")
    $('#allCompaniesLst').val("")
    $('#deparInHoursNmb').val("0")
    $('#deparInHoursRng').val("0")
    $('#landInHoursNmb').val("0")
    $('#landInHoursRng').val("0")
    $('#flightDurationNmb').val("0")
    $('#flightDurationRng').val("0")
    $('#deparFromByDate').val("")
    $('#deparUpToByDate').val("")
    $('#landFromByDate').val("")
    $('#landUpToByDate').val("")
    switch (searchState) {
        case 1:
            $('#deparInHoursNmb').val("24")
            $('#deparInHoursRng').val("24")
            $('#onlyVacancyChBx').prop('checked', false)
            break;
        case 2:
            $('#landInHoursNmb').val("24")
            $('#landInHoursRng').val("24")
            $('#deparFromByDate').val(moment().format('YYYY-MM-DD'))
            $('#onlyVacancyChBx').prop('checked', false)
            break;
        default:
            $('#onlyVacancyChBx').prop('checked', true)
            break;
    }
}