using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Interface For Anonymous User (And Over).
    public interface IAnonymousUserFacade
    {
        long CreateNewCustomer(Customer customer);

        AirlineCompany GetAirlineById(int id);
        AirlineCompany GetAirlineByAirlineName(string airlineName);
        IList<AirlineCompany> GetAllAirlineCompanies();

        Flight GetFlightById(int id);
        Dictionary<Flight, int> GetAllFlightsVacancy();
        IList<Flight> GetFlightsByFilters(string fromCountry, string toCountry, string flightNumber, string byCompany , string depInHours, string landInHours, string flightDurationByHours, string fromDepDate, string upToDepDate, string fromLandDate, string upToLandDate, bool onlyVacancy = true);
        IList<Flight> GetFlightsByOriginCountry(int countryCode);
        IList<Flight> GetFlightsByDestinationCountry(int countryCode);
        IList<Flight> GetFlightsByFromDepartureDate(DateTime departureDate);
        IList<Flight> GetFlightsByUpToLandingDate(DateTime landingDate);
        IList<Flight> GetAllFlights();

        Country GetCountryById(int id);
        Country GetCountryByName(string userName);
        IList<Country> GetAllCountries();
    }
}
