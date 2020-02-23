let ajax_inprocess = false

// Send Ajax On The Sent Url And Get Matching Flights.
function getMatchingFlights(sentUrl) {
    if (ajax_inprocess == false) {
         myUrl = sentUrl
         ajax_inprocess = true
         clearTimeout(refreshPageTimer)
         refreshPageTimer = setTimeout("location.reload()", 1000 * 60 * 5)
         $.ajax({
            url: myUrl
        }).then(function (data) {
            $("#captiontable").html("").removeClass("caption-on-error-style")                        
            $("#matchesFlightsTable").empty()
            resetLocalStorage()
            takeInfoOnSucReq(data)
        }).fail(function () {
            $("#matchesFlightsTable").empty()
            if(getLocalStorage()[1].length > 1){
                printFlightsOnTable(getLocalStorage(), false)
            }else{
                $("#captiontable").html("").removeClass("caption-on-error-style")                        
                ajax_inprocess = false;
                Swal.fire('Sorry, There are no results to display.')
            }
            Swal.fire({
                icon: 'warning',
                title: 'Oops...',
                text: 'There was some problem, The results are not current.',
            })
        })
    }
}

// Update Data And Local Storage When The Request Has Been Success.
function takeInfoOnSucReq(data){
    if (data != undefined && data.length > 0) {
        data = getFlightsStatus(data)
        $.each(data, function (i, flight) {
            flight.airlineName = getCompanyNameById(flight.AirlineCompany_Id)
            flight.originCountry = getCountryNameById(flight.Origin_Country_Code)
            flight.destinationCountry = getCountryNameById(flight.Destination_Country_Code)
            switch (searchState) {
                case 1:
                    localStorage.lastDepartureResult += JSON.stringify(flight)
                    localStorage.lastDepartureTime = `${moment(new Date).format('MMMM Do YYYY, h:mm:ss a')}`
                    break;
                case 2:
                    localStorage.lastLandingResult += JSON.stringify(flight)
                    localStorage.lastLandingTime = `${moment(new Date).format('MMMM Do YYYY, h:mm:ss a')}`
                    break;
                default:
                    localStorage.lastSearchResult += JSON.stringify(flight)
                    localStorage.lastSearchTime = `${moment(new Date).format('MMMM Do YYYY, h:mm:ss a')}`
                    break;
            }
        })
            printFlightsOnTable(getLocalStorage(), true)
    }
    else { // When The Search Returned 0 Results.
        Swal.fire('Sorry, There are no results to display.')
        ajax_inprocess = false
    }
}

// Show On Table All Data In Case The Request Has Success/Failure.
function printFlightsOnTable(data, ajaxIsSuccess){
    let flights = data[1]
        flights.pop()
        const allTH = getTHForTable()
        flights = flights.map(f => {return JSON.parse(f + '}')})
        $("#captiontable").html("").removeClass("caption-on-error-style")                        
        if(ajaxIsSuccess == false){
            $("#captiontable").html(`&nbsp; Results are shown as of: ${data[0]}. &nbsp;`).addClass("caption-on-error-style")
        }
        $("#matchesFlightsTable").append(`<thead class="thead-dark"><tr>`)
        for(i = 0; i < allTH.length; i++){
            $("#matchesFlightsTable").append(`<th style="border-bottom: 3px solid white; border-top: 3px solid white; padding-top: 12px;">${allTH[i]}</th>`)
        }
        $("#matchesFlightsTable").append(`</tr ></thead>`);
                        
        $.each(flights, function(i, flight){
            flight = JSON.parse(JSON.stringify(flight))
            switch(searchState){
                case 1:
                    $("#matchesFlightsTable").append(`<tr> <td  style="border-left: 9px solid gray"> ${flight.Id} </td><td style="max-height:40px;"> <img src="../../Content/Images/Logos/${flight.airlineName}-Logo.png" alt="${flight.airlineName}" style="height:30px; width:60px; margin:3px"></img>${flight.airlineName}</td><td>${flight.originCountry}</td><td>${flight.destinationCountry}</td>
                                <td> <div class=${flight.deparStatus == "Delayed" ? "delayed-flights" : ""}>${moment(flight.Departure_Time).format('DD-MM-YYYY - HH:mm')}</div></td><td class=${flight.Remaining_Tickets <= 10 ? 'no-tickets' : ""}>${flight.Remaining_Tickets > 0 ? flight.Remaining_Tickets : '-'}</td><td class=${flight.deparStatus == "On-Time" ? 'ontime-status' : 'delayed-status'}>${flight.deparStatus}</td></tr>`)
                    break;
                case 2:
                    $("#matchesFlightsTable").append(`<tr> <td  style="border-left: 9px solid gray"> ${flight.Id} </td><td style="max-height:40px;"> <img src="../../Content/Images/Logos/${flight.airlineName}-Logo.png" alt="${flight.airlineName}" style="height:30px; width:60px; margin:3px"></img>${flight.airlineName}</td><td>${flight.originCountry}</td><td>${flight.destinationCountry}</td>
                                <td> <div class=${flight.deparStatus == "Delayed" ? "delayed-flights" : ""}>${moment(flight.Landing_Time).format('DD-MM-YYYY - HH:mm')}</div></td><td>${calculateFlightStatus(flight.Landing_Time)}</td></tr>`)
                    break;
                default:
                    $("#matchesFlightsTable").append(`<tr> <td  style="border-left: 9px solid gray"> ${flight.Id} </td><td style="max-height:40px;"> <img src="../../Content/Images/Logos/${flight.airlineName}-Logo.png" alt="${flight.airlineName}" style="height:30px; width:60px; margin:3px"></img>${flight.airlineName}</td><td>${flight.originCountry}</td><td>${flight.destinationCountry}</td>
                                <td> <div class=${flight.deparStatus == "Delayed" ? "delayed-flights" : ""}>${moment(flight.Departure_Time).format('DD-MM-YYYY - HH:mm')}</div></td><td> <div class=${flight.deparStatus == "Delayed" ? "delayed-flights" : ""}>${moment(flight.Landing_Time).format('DD-MM-YYYY - HH:mm')}</div></td></tr>`)
            }
        })
        ajax_inprocess = false;
}
                                
// Get Table Heads By Current Search-State                                
function getTHForTable(){
    switch (searchState){
        case 1:
            return [`Flight No'`,`Airline-Company`,`From`,`To`,`Departure-On`,`Tickets`,`Status`]
        case 2:
            return [`Flight No'`,`Airline-Company`,`From`,`To`,`Landing-On`,`Status`]
        default:
        return [`Flight No'`,`Airline-Company`,`From`,`To`,`Departure-On`,`Landing-On`]
    }
}

// Get Company-Name By Id
function getCompanyNameById(id) {
    const company = allCompanies.find(({ Id }) => Id == id)
    return company != undefined ? company.Airline_Name : "????"
}

// Get Country-Name By Id
function getCountryNameById(id) {
    const country = allCountries.find(({ Id }) => Id == id)
    return country != undefined ? country.Country_Name : "????"
}

// Reset Local Storage When New Data Received From The Server.
function resetLocalStorage() {
    switch (searchState) {
    case 1:
        localStorage.lastDepartureResult = []
        break;
    case 2:
        localStorage.lastLandingResult = []
        break;
    default:
        localStorage.lastSearchResult = []
        break;
    }
}

// When Request Failed, Get Compatible Local Storage.
function getLocalStorage() {
    switch (searchState) {
        case 1:
            return [localStorage.lastDepartureTime, JSON.parse(JSON.stringify(localStorage.lastDepartureResult)).split('}')]
        case 2:
            return [localStorage.lastLandingTime, JSON.parse(JSON.stringify(localStorage.lastLandingResult)).split('}')]
        default:
            return [localStorage.lastSearchTime, JSON.parse(JSON.stringify(localStorage.lastSearchResult)).split('}')]
    }
}

// Format Flight Date For The Table.
function formatDate(date) {
    return moment(date).format('DD-MM-YYYY - HH:mm');
    date = date.replace("T", '-')
    date = date.replace(':', '-')
    const parts = date.split('-')
    time = parts[4].split(':')
    return parts[0] +'-'+ parts[1] +'-'+ parts[2] +"&nbsp;&nbsp; "+ parts[3] +':'+ time[0]
}

// Random Flights Status(20%)
function getFlightsStatus(data){
    data.map(f => f.deparStatus = "On-Time")
    for(i = 0; i < data.length * 0.20; i++){
        let randomNumber = parseInt(Math.random() * data.length)
        for(x = 0; x < data.length; x++){
            if(x == randomNumber){
                randomHours = Math.floor(Math.random() * (5)+1); 
                randomMinutes = Math.round(10 * Math.floor(Math.random() * ((60)/10)));
                data[x] = delayDepAndLandTime(data[x], randomHours, randomMinutes)
            }
        }
    }
    return data
}

// Change The Takeoff/Landing Time/Status For The Selected Flight To Be Delayed.
function delayDepAndLandTime(flight, hours, minutes) {
    flight.deparStatus = "Delayed"
    let newDepTime = new Date(flight.Departure_Time)
    newDepTime.setHours(newDepTime.getHours() + hours)
    newDepTime.setMinutes(newDepTime.getMinutes() + minutes)
    flight.Departure_Time = newDepTime
    let newLandTime = new Date(flight.Landing_Time)
    newLandTime.setHours(newLandTime.getHours() + hours)
    newLandTime.setMinutes(newLandTime.getMinutes() + minutes)
    flight.Landing_Time = newLandTime
    return flight
}

// Calculate Your Landing Time With The Current Time And Get a Matching Landing Status.
function calculateFlightStatus(time){
    let now = moment(new Date()).format()
    time = moment(time).format()
    if(now > time){
        return "Landed"
    }
    if(now < time){
        if(moment(time).subtract(15, 'minutes').format() < now)
            return "Landing"
        if(moment(time).subtract(2, 'hours').format() < now)
            return "Final"
    }
    return "Not Final"
}