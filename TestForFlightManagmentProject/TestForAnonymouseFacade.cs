using System;
using System.Collections.Generic;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForAnonymouseFacade : TestCenter
    {
        /*  ======= All Tests =======

        1. CreateNewCustomer               -- "TestLogin Class" (LoginSuccesfullyAsCustomer).
        2. GetAirlineById                 -- GetAirlineById.
        3. GetAirlineByAirlineName        -- GetAirlineCompanyByCompanyName
        4. GetAllAirlineCompanies         -- GetAllArlineCompanies.
        5. GetFlightById                  -- GetFlightById.
        6. GetAllFlightsVacancy           -- GetAllFlightsVacancy.
        7. GetFlightsByDepatrureDate      -- GetFlightsByFromDepartureDate.
        8. GetFlightsByLandingDate        -- GetFlightsByUpToLandingDate.
        9. GetFlightsByOriginCountry      -- GetFlightsByOriginCountry.
        10. GetFlightsByDestinationCountry -- GetFlightsByDestinationCountry.
        11. GetAllFlights                  -- GetAllFlights.
        12.GetCountryById                 -- GetCountryById.
        13.GetCountryByName               -- GetCountryByName.
        14.GetAllCountries                -- GetAllCountries.

        ======= All Tests ======= */


        // Search Some Airline By Id.
        [TestMethod]
        public void GetAirlineById()
        {

            AirlineCompany airline = CreateRandomCompany();
            airline.Airline_Number = adminFacade.CreateNewAirline(adminToken, airline);
            Assert.AreNotEqual(adminFacade.GetAirlineById((int)airline.Id), null);
        }

        // Supposed To Get Airline By Company Name.
        [TestMethod]
        public void GetAirlineCompanyByCompanyName()
        {
            AirlineCompany airline = new AnonymousUserFacade().GetAirlineByAirlineName(airlineToken.User.Airline_Name);
            Assert.AreNotEqual(airline, null);
        }

        // Supposed To Get All Airline Companies.
        [TestMethod]
        public void GetAllArlineCompanies()
        {
            AirlineCompany airline = CreateRandomCompany();
            airline.Id = adminFacade.CreateNewAirline(adminToken, airline);
            IList<AirlineCompany> AirlineCompanies = new AnonymousUserFacade().GetAllAirlineCompanies();
            Assert.AreEqual(AirlineCompanies.Count, 2);
        }


        // Supposed To Get Flight By Id.
        [TestMethod]
        public void GetFlightById()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            Assert.AreEqual(airlineFacade.GetFlightById((int)flight.Id), flight);
        }

        // Supposed To Get All Flights With At List One Ticket.
        [TestMethod]
        public void GetAllFlightsVacancy()
        {
            Flight f1 = CreateNewFlight();
            Flight f2 = CreateNewFlight();
            f2.Remaining_Tickets = 0;
            f1.Id = airlineFacade.CreateFlight(airlineToken, f1);
            f2.Id = airlineFacade.CreateFlight(airlineToken, f2);
            Dictionary<Flight, int> TicketsByFlight = new AnonymousUserFacade().GetAllFlightsVacancy();
            Assert.AreEqual(TicketsByFlight.Count, 1);
        }

        // Supposed To Get Flight By Departure Time.
        [TestMethod]
        public void GetFlightsByFromDepartureDate()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            Assert.AreEqual(airlineFacade.GetFlightsByFromDepartureDate(flight.Departure_Time).Count, 1);
        }

        // Supposed To Get Flight By Landing Time.
        [TestMethod]
        public void GetFlightsByUpToLandingDate()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            Assert.AreEqual(airlineFacade.GetFlightsByUpToLandingDate(flight.Landing_Time)[0], flight);
        }

        // Supposed To Get Flight By Origin Country.
        [TestMethod]
        public void GetFlightsByOriginCountry()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            Assert.AreEqual(adminFacade.GetFlightsByOriginCountry((int)adminFacade.GetCountryByName("Israel").Id).Count, 1);
        }

        // Supposed To Get Flight By Destination Country.
        [TestMethod]
        public void GetFlightsByDestinationCountry()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            Assert.AreEqual(adminFacade.GetFlightsByDestinationCountry((int)adminFacade.GetCountryByName("Israel").Id).Count, 1);
        }

        // Supposed To Get All Flights.
        [TestMethod]
        public void GetAllFlights()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            IList<Flight> flights = new AnonymousUserFacade().GetAllFlights();
            Assert.AreEqual(flights.Count, 1);
        }

        // Supposed To Get Country By Id.
        [TestMethod]
        public void GetCountryById()
        {
            Country country = new Country("USA");
            country.Id = adminFacade.CreateNewCountry(adminToken, country);
            Assert.AreEqual(airlineFacade.GetCountryById((int)country.Id), country);
        }

        // Supposed To Get Country By Name.
        [TestMethod]
        public void GetCountryByName()
        {
            Country country = new Country("USA");
            country.Id = adminFacade.CreateNewCountry(adminToken, country);
            Assert.AreEqual(airlineFacade.GetCountryByName("USA"), country);
        }

        // Supposed To Get All Countries.
        [TestMethod]
        public void GetAllCountries()
        {
            Country country = new Country("USA");
            country.Id = adminFacade.CreateNewCountry(adminToken, country);
            IList<Country> countries = new AnonymousUserFacade().GetAllCountries();
            Assert.AreEqual(countries.Count, 2);
        }
    }
}
