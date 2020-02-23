using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Flight) For MSSQL Data Base.
    internal class FlightDAOMSSQL : IFlightDAO
    {
        readonly string formatDate = "yyyy-MM-dd HH:mm:ss";
        private const string DEFAULT_DATE = "2000-01-01 00:00:00.000";

        //====== Crud Commands ======//

        // Add New Flight.
        public long Add(Flight t)
        {
            long newId = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_Flight", conn))
                {
                    cmd.Connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AirLineCompany_Id", t.AirlineCompany_Id);
                    cmd.Parameters.AddWithValue("@Origin_Country_Code", t.Origin_Country_Code);
                    cmd.Parameters.AddWithValue("@Destination_Country_Code", t.Destination_Country_Code);
                    cmd.Parameters.AddWithValue("@Departure_Time", t.Departure_Time);
                    cmd.Parameters.AddWithValue("@Landing_Time", t.Landing_Time);
                    cmd.Parameters.AddWithValue("@Remaining_Tickets", t.Remaining_Tickets);

                    newId = (long)cmd.ExecuteScalar();
                }
            }
            return newId;
        }

        // Remove Flight.
        public bool Remove(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete from Tickets Where Flight_Id = (select TOP 1 Id from Flights where Id = {t.Id}); " +
                    $"Delete from Flights Where Id = {t.Id};", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Update Flight.
        public bool Update(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Flights set Origin_Country_Code = {t.Origin_Country_Code}, " +
                    $"Destination_Country_Code = {t.Destination_Country_Code}, Departure_Time = '{t.Departure_Time.ToString(formatDate)}' ,Landing_Time = '{t.Landing_Time.ToString(formatDate)}' ," +
                    $"Remaining_Tickets = {t.Remaining_Tickets} where Id = {t.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        //====== Search Commands ======//

        // Search Flight By Id.
        public Flight GetById(long id)
        {
            Flight flight = new Flight();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * From Flights Where Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            flight = new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            };
                        }
                    }
                }
            }
            return flight;
        }

        // Search All Flights.
        public IList<Flight> GetAll()
        {
            List<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Get_All_Flights", conn))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            var departureDate = reader["Departure_Time"];
                            var landingDate = reader["Departure_Time"];

                            flights.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flights;
        }

        // Search All Flights By Airline Company.
        public IList<Flight> GetFlightsByAirlineCompany(AirlineCompany airline)
        {
            List<Flight> flightsByAirline = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * from Flights f where f.AirlineCompany_Id = {airline.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByAirline.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByAirline;
        }

        // Search How Much Tickets Not Buy Yet From Each Flight.
        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> ticketsByFlight = new Dictionary<Flight, int>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from Flights where Remaining_Tickets > 0", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read() == true)
                    {
                        ticketsByFlight.Add(new Flight
                        {
                            Id = (long)reader["Id"],
                            AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                            Origin_Country_Code = (long)reader["Origin_Country_Code"],
                            Destination_Country_Code = (long)reader["Destination_Country_Code"],
                            Departure_Time = (DateTime)reader["Departure_Time"],
                            Landing_Time = (DateTime)reader["Landing_Time"],
                            Remaining_Tickets = (int)reader["Remaining_Tickets"],
                        }, (int)reader["Remaining_Tickets"]);
                    }
                }
            }
            return ticketsByFlight;
        }

        // Search All Flights By Customer.
        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            List<Flight> flightsByCustomer = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from flights f join tickets t on f.Id = t.Flight_Id where t.Customer_Id = {customer.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByCustomer.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByCustomer;
        }

        #region Search Flights By Filters.
        /// <summary>
        /// Function That Search Only Flights That Matches The All Filters.
        /// </summary>
        /// <param name="fromCountry"name="toCountry"name="fromDepDate"name="depInHours"name="landInHours"name="upToDepDate"name="fromLandDate"name="upToLandDate"name="flightDurationByHours"></param>
        /// <returns>IList</returns>
        public IList<Flight> GetAllFlightsByFilters(string fromCountry, string toCountry, string flightNumber, string byCompany, string depInHours, string landInHours, string flightDurationByHours, string fromDepDate, string upToDepDate, string fromLandDate, string upToLandDate, bool onlyVacancy)
        {
            List<Flight> matchesFlights = new List<Flight>();
            string myFilters = "";
            DateTime _fromDepDate = new DateTime();
            DateTime _upToDepDate = new DateTime();
            DateTime _fromLandDate = new DateTime();
            DateTime _upToLandDate = new DateTime();
            if (fromDepDate != "" && fromDepDate != "Invalid date")
                _fromDepDate = Convert.ToDateTime(fromDepDate).AddHours(2).ToUniversalTime();
            if (upToDepDate != "" && upToDepDate != "Invalid date")
                _upToDepDate = Convert.ToDateTime(upToDepDate).AddHours(2).ToUniversalTime();
            if (fromLandDate != "" && fromLandDate != "Invalid date")
                _fromLandDate = Convert.ToDateTime(fromLandDate).AddHours(2).ToUniversalTime();
            if (upToLandDate != "" && upToLandDate != "Invalid date")
                _upToLandDate = Convert.ToDateTime(upToLandDate).AddHours(2).ToUniversalTime();

            Action<string> addFilter = (filter) =>
            {
                if (myFilters != "")
                    myFilters += "And";
                myFilters += filter;
            };
            if (fromCountry != null && fromCountry != "")
            {
                addFilter.Invoke($" Origin_Country_Code = '{fromCountry}' ");
            }
            if (toCountry != null && toCountry != "")
            {
                addFilter.Invoke($" Destination_Country_Code = '{toCountry}' ");
            }
            if (flightNumber != null && flightNumber != "")
            {
                addFilter.Invoke($" Id = '{flightNumber}' ");
            }
            if (byCompany != null && byCompany != "")
            {
                addFilter.Invoke($" AirlineCompany_Id = '{byCompany}' ");
            }
            if (depInHours != null && depInHours != "" && depInHours != "0")
            {
                addFilter.Invoke($" Departure_Time BETWEEN GETDATE() AND DATEADD(hour, {depInHours}, GETDATE()) ");
            }
            if (landInHours != null && landInHours != "" && landInHours != "0")
            {
                addFilter.Invoke($" Landing_Time BETWEEN DATEADD(hour, -4, GETDATE()) AND DATEADD(hour, {landInHours}, GETDATE()) ");
            }
            if (flightDurationByHours != null && flightDurationByHours != "" && flightDurationByHours != "0")
            {
                addFilter.Invoke($" DATEADD(hour, {flightDurationByHours}, Departure_Time) >= Landing_Time ");
            }
            if (_fromDepDate != DateTime.MinValue)
            {
                addFilter.Invoke($" Departure_Time >= '{_fromDepDate.ToString(formatDate)}' ");
            }
            if (_upToDepDate != DateTime.MinValue)
            {
                addFilter.Invoke($" Departure_Time <= '{_upToDepDate.ToString(formatDate)}' ");
            }
            if (_fromLandDate != DateTime.MinValue)
            {
                addFilter.Invoke($" Landing_Time >= '{_fromLandDate.ToString(formatDate)}' ");
            }
            if (_upToLandDate != DateTime.MinValue)
            {
                addFilter.Invoke($" Landing_Time <= '{_upToLandDate.ToString(formatDate)}' ");
            }
            if (onlyVacancy == true)
            {
                addFilter.Invoke($" Remaining_Tickets > 0 ");
            }


            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(myFilters == "" ? $"Select * from Flights" : $"Select * from Flights where {myFilters}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            matchesFlights.Add(new Flight
                            {
                                AirlineCompany_Id = (long)reader["AirlineCompany_Id"],
                                Id = (long)reader["Id"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }
            return matchesFlights;
        }
        #endregion



        // Search All Flights By From Departure Time.
        public IList<Flight> GetFlightsByFromDepartureDate(DateTime departureTime)
        {
            List<Flight> flightsByDepartueTime = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Search_Flights_By_From_DT", conn))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@departureFrom", departureTime);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByDepartueTime.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByDepartueTime;
        }

        //// Search All Flights By Destination Country.
        public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
        {
            List<Flight> flightsByDestinationCountry = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * from Flights f where f.Destination_Country_Code = {countryCode}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByDestinationCountry.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByDestinationCountry;
        }

        // Search All Flights By Up To Landing Date.
        public IList<Flight> GetFlightsByUpToLandingDate(DateTime landingTime)
        {
            List<Flight> flightsByLandingTime = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Search_Flights_By_Up_To_LT", conn))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@landingUpTo", landingTime);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByLandingTime.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByLandingTime;
        }

        // Search All Flights By Origin County.
        public IList<Flight> GetFlightsByOriginCounty(int countryCode)
        {
            List<Flight> flightsByOriginCountry = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * from Flights f where f.Origin_Country_Code = {countryCode}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            flightsByOriginCountry.Add(new Flight
                            {
                                Id = (long)reader["Id"],
                                AirlineCompany_Id = (long)reader["AirLineCompany_Id"],
                                Origin_Country_Code = (long)reader["Origin_Country_Code"],
                                Destination_Country_Code = (long)reader["Destination_Country_Code"],
                                Departure_Time = (DateTime)reader["Departure_Time"],
                                Landing_Time = (DateTime)reader["Landing_Time"],
                                Remaining_Tickets = (int)reader["Remaining_Tickets"],
                            });
                        }
                    }
                }
            }
            return flightsByOriginCountry;
        }
    }
}
