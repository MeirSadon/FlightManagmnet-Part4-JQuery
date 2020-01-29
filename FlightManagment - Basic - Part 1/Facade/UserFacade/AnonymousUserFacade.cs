using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Basic Facade(Without Login) With The Simple Options.
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {

        private const string DEFAULT_DATE = "2000-01-01 00:00:00.000";

        // Search Airline Company By Id.
        public AirlineCompany GetAirlineById(int id)
        {
            return _airlineDAO.GetById(id);
        }

        // Get Some Airline By Company Name.
        public AirlineCompany GetAirlineByAirlineName(string airlineName)
        {
            return _airlineDAO.GetByName(airlineName);
        }

        // Get All Airline Companies.
        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }


        // Get Some Flight By Id.
        public Flight GetFlightById(int id)
        {
            return _flightDAO.GetById(id);
        }

        // Get All Flights With At Least One Ticket.
        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightsVacancy();
        }

        #region Search Flights By Filters.
        /// <summary>
        ///  Function That Return All Flights That Matches To Sent Filters.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IList<Flight> GetFlightsByFilters(string fromCountry = "", string toCountry = "", string flightNumber = "", string byCompany = "", string depInHours = "", string landInHours = "", string flightDurationByHours = "", string fromDepDate = "", string upToDepDate = "", string fromLandDate = "", string upToLandDate = "", bool onlyVacancy = true)
        {
            return _flightDAO.GetAllFlightsByFilters(fromCountry, toCountry, flightNumber, byCompany, depInHours, landInHours, flightDurationByHours, fromDepDate, upToDepDate, fromLandDate, upToLandDate, onlyVacancy);
        }
        #endregion

        // Get All Flights By Origin Country.
        public IList<Flight> GetFlightsByOriginCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByOriginCounty(countryCode);
        }

        //Get All Flights By Destination Country.
        public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByDestinationCountry(countryCode);
        }

        //Get All Flights By Departure Time.
        public IList<Flight> GetFlightsByFromDepartureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightsByFromDepartureDate(departureDate);
        }
        
        // Get All Flights By Landing Date.
        public IList<Flight> GetFlightsByUpToLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightsByUpToLandingDate(landingDate);
        }
        

        // Get All Flights
        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }


        //Get Country By Id.
        public Country GetCountryById(int id)
        {
            return _countryDAO.GetById(id);
        }

        // Get Country By Name.
        public Country GetCountryByName(string name)
        {
            return _countryDAO.GetByName(name);
        }

        //Get All Countries.
        public IList<Country> GetAllCountries()
        {
            return _countryDAO.GetAll();
        }
    }
}
