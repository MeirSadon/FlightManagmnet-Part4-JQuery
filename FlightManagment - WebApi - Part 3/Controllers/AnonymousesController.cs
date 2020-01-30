using FlightManagment___Basic___Part_1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace FlightManagment___WebApi___Part_3
{
    /// <summary>
    /// Controller For All Functions Of Anonymous Facade.
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/search")]
    public class AnonymousController : ApiController
    {
        private AnonymousUserFacade facade = new AnonymousUserFacade();
        private const string DEFAULT_DATE = "2000-01-01 00:00:00.000";
        private ControllersCenter controllersCenter = new ControllersCenter();

        #region Get Company By Id.
        /// <summary>
        /// Get Company By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("companies/{id}", Name = "GetCompanyById")]
        [HttpGet]
        public IHttpActionResult GetCompanyById([FromUri]int id)
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                AirlineCompany company = facade.GetAirlineById(id);
                return GetSuccessResponse(company, "No Company With The Received Id Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Company By Company Name.
        /// <summary>
        /// Get Company By Company Name.
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("company/byname", Name = "GetCompanyByAirlineName")]
        [HttpGet]
        public IHttpActionResult GetCompanyByCompanyName([FromUri]string companyName)
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                AirlineCompany company = facade.GetAirlineByAirlineName(companyName);
                return GetSuccessResponse(company, "No Company With The Received UserName Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Companies.
        /// <summary>
        /// Get All Companies.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<AirlineCompany>))]
        [Route("companies", Name = "GetAllCompanies")]
        [HttpGet]
        public IHttpActionResult GetAllCompanies()
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                IList<AirlineCompany> companies = facade.GetAllAirlineCompanies();
                if (companies.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Company List Is Empty.");
                return Content(HttpStatusCode.OK, companies);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Flight By Id.
        /// <summary>
        /// Get Flight By Id.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("flights/{id}", Name = "GetFlightById")]
        [HttpGet]
        public IHttpActionResult GetFlightById([FromUri]int id)
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                Flight flight = facade.GetFlightById(id);
                return GetSuccessResponse(flight, "No Flight With The Received ID Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights Vacancy.
        /// <summary>
        /// Get All Flights Vacancy.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Dictionary<Flight, int>))]
        [Route("flights/vacancy", Name = "GetAllFlightsVacancy")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                Dictionary<Flight, int> vacancyFlights = facade.GetAllFlightsVacancy();
                if (vacancyFlights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But Currently, Has No Flights With Tickets To Buy... Please Try Later.");
                return Content(HttpStatusCode.OK, vacancyFlights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Flights By Filters.
        /// <summary>
        /// Function That Get All Flights That Matches The Filters.
        /// /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights/byfilters", Name = "GetFlightsByFilters")]
        [HttpGet]
        public IHttpActionResult GetFlightsByFilters([FromUri]string fromCountry = "", [FromUri]string toCountry = "", [FromUri]string flightNumber = "",[FromUri]string byCompany = "", [FromUri]string depInHours = "", [FromUri]string landInHours = "", [FromUri]string flightDurationByHours = "", [FromUri]string fromDepDate = "", [FromUri]string upToDepDate = "", [FromUri]string fromLandDate = "", [FromUri]string upToLandDate = "", [FromUri]bool onlyVacancy = true)
        {
            //return StatusCode(HttpStatusCode.NotFound);
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
        {
            IList<Flight> resultFlightsSearch = facade.GetFlightsByFilters(fromCountry, toCountry, flightNumber, byCompany, depInHours, landInHours, flightDurationByHours, fromDepDate, upToDepDate, fromLandDate, upToLandDate, onlyVacancy);
            if (resultFlightsSearch.Count < 1)
                return Content(HttpStatusCode.NoContent, "No Flight Found Matching Sent Parameters.");
            return Content(HttpStatusCode.OK, resultFlightsSearch);
        });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights.
        /// <summary>
        /// Get All Flights.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights", Name = "GetAllFlights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                IList<Flight> flights = facade.GetAllFlights();
                if (flights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But Currently, Has No Flights... Please Try Later.");
                return Content(HttpStatusCode.OK, flights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Country By Id.
        /// <summary>
        /// Get Country By Id.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("countries/{id}", Name = "GetCountryById")]
        [HttpGet]
        public IHttpActionResult GetCountryById([FromUri]int id)
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                Country country = facade.GetCountryById(id);
                return GetSuccessResponse(country, "No Country Found With ID That Recived...");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Country By Name.
        /// <summary>
        /// Get Country By Name.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("countries/byname", Name = "GetCountryByName")]
        [HttpGet]
        public IHttpActionResult GetCountryByName([FromUri]string name = "")
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                Country country = facade.GetCountryByName(name);
                return GetSuccessResponse(country, "No Country Found With Name That Recived...");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Countries.
        /// <summary>
        /// Get All Countries.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Country>))]
        [Route("countries", Name = "GetAllCountries")]
        [HttpGet]
        public IHttpActionResult GetAllCountries()
        {
            IHttpActionResult result = controllersCenter.ExecuteSafe(() =>
            {
                IList<Country> countries = facade.GetAllCountries();
                if (countries.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But No Countries Have Been Added To The Site Yet.");
                return Content(HttpStatusCode.OK, countries);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get The Search Result For Single Instance.
        /// <summary>
        ///  Function That Return Success Message(200 Family) With Difference Ways For Single Instance Search.
        /// </summary>
        /// <param name="poco" name="notFoundResponse"></param>
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult GetSuccessResponse(IPoco poco, string notFoundResponse)
        {
            IHttpActionResult successResponse;
            if (poco.GetHashCode() == 0)
                successResponse = Content(HttpStatusCode.NoContent, notFoundResponse);
            else
                successResponse = Content(HttpStatusCode.OK, poco);
            return successResponse;
        }
        #endregion` 
        
        
       
        #region Function To Remove All Unmatches Flights On Current Search.
        ///// <summary>
        /////  Function For Search Flight By Few Parameters(FromCountry, ToCountry, FromDate, ToDate, FlightDuration)
        ///// </summary>
        ///// <param name="allFlights" name="fromCountry" name="toCountry" name="fromDate" name="ToDate" name="hoursOfFlight"></param>
        ///// <returns>List</returns>
        //private List<Flight> FindMatchesFlightsByFilters(int fromCountry, int toCountry, DateTime fromDate, DateTime toDate, double hoursOfFlight)
        //{
        //    List<Flight> matchesFlights = new List<Flight>();
        //    string myFilters = "";
        //    DateTime dt = DateTime.Parse(DEFAULT_DATE);
        //
        //    Action<string> addAnd = (filter) =>
        //    {
        //        if (myFilters != "")
        //            myFilters += "And";
        //        myFilters += filter;
        //    };
        //    if (fromCountry != 0)
        //    {
        //        addAnd.Invoke($" Origin_Country_Code = {fromCountry} ");
        //    }
        //    if (toCountry != 0)
        //    {
        //        addAnd.Invoke($" Destination_Country_Code = {toCountry} ");
        //    }
        //    if (fromDate != dt)
        //    {
        //        addAnd.Invoke($" Departure_Time >= {fromDate} ");
        //    }
        //    if (toDate != dt)
        //    {
        //        addAnd.Invoke($" Departure_Time <= {toDate} ");
        //    }
        //    if (hoursOfFlight != 0)
        //    {
        //        addAnd.Invoke($" DATEADD(hour, {hoursOfFlight}, Departure_Time) >= Landing_Time ");
        //    }
        //
        //    using(SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
        //    {
        //        conn.Open();
        //        using(SqlCommand cmd = new SqlCommand(myFilters == "" ?  $"Select * from Flights" : $"Select * from Flights where {myFilters}",conn))
        //        {
        //            using(SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    matchesFlights.Add(new Flight
        //                    {
        //                        AirlineCompany_Id = (long)reader["AirlineCompany_Id"],
        //                        Id = (long)reader["Id"],
        //                        Departure_Time = (DateTime)reader["Departure_Time"],
        //                        Landing_Time = (DateTime)reader["Landing_Time"],
        //                        Origin_Country_Code = (long)reader["Origin_Country_Code"],
        //                        Destination_Country_Code = (long)reader["Destination_Country_Code"],
        //                        Remaining_Tickets = (int)reader["Remaining_Tickets"]
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return matchesFlights;
        //}
        #endregion
    }
}