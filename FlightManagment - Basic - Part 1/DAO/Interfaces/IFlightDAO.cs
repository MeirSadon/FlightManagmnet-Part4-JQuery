using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public interface IFlightDAO : IBasicDB<Flight>
    {
        Dictionary<Flight, int> GetAllFlightsVacancy();
        IList<Flight> GetFlightsByCustomer(Customer customer);
        IList<Flight> GetAllFlightsByFilters(string fromCountry, string toCountry, string flightNumber, string byCompany, string depInHours, string landInHours, string flightDurationByHours, string fromDepDate, string upToDepDate, string fromLandDate, string upToLandDate, bool onlyVacancy);
        IList<Flight> GetFlightsByAirlineCompany(AirlineCompany airline);
        IList<Flight> GetFlightsByOriginCounty(int countryCode);
        IList<Flight> GetFlightsByDestinationCountry(int countryCode);
        IList<Flight> GetFlightsByFromDepartureDate(DateTime date);
        IList<Flight> GetFlightsByUpToLandingDate(DateTime date);
    }
}
