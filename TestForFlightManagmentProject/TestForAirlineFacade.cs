using System;
using System.Collections.Generic;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForAirlineFacade : TestCenter
    {
        /*  ======= All Tests =======
           
            1. CreateFlight           -- GetAllTicketsForCurrentAirline + FlightIdIsNotMatchWhenTryToCreateFlight + DepartureTimeTooLateWhenTryToCreateFlight.
            2. CancelFlight           -- CancelFlightForCurrentAirline.
            3. MofidyAirlineDetails   -- "TestForAdminFacadeClass"(UpdateAirline).
            4. ChangeMyPassword       -- ChangePasswordForAirline + WrongPasswordWhenTryChangePasswordForAirline.
            5. GetSoldTicketById      -- GetSoldTicketById + TicketNotMatchWhenTryGetTicketThatSoldFromAnotherCompany.
            5. GetAllFlightsByAirline -- GetAllTicketsForCurrentAirline.
            6. GetAllTicketsByAirline -- GetAllTicketsForCurrentAirline. 
            8. UpdateFlight           -- UpdateFlightForCurrentAirline.

            ======= All Tests ======= */


        // Try To Get All Tickets For Some Airline.
        [TestMethod]
        public void GetAllTicketsForCurrentAirline()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            customerFacade.PurchaseTicket(customerToken, flight);
            IList<Ticket> tickets = airlineFacade.GetAllTicketsByAirline(airlineToken);
            Assert.AreEqual(1, tickets.Count);
        }


        // Supposed To Get Exception Of "Flight Id Is Not Match".
        [TestMethod]
        [ExpectedException(typeof(FlightNotMatchException))]
        public void FlightIdIsNotMatchWhenTryToCreateFlight()
        {
            Flight flight = CreateNewFlight();
            flight.AirlineCompany_Id = flight.AirlineCompany_Id + 1;
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
        }

        // Supposed To Get Exception Of "Departure Time Is Too Late".
        [TestMethod]
        [ExpectedException(typeof(DepartureTimeTooLateException))]
        public void DepartureTimeTooLateWhenTryToCreateFlight()
        {
            Flight flight = CreateNewFlight();
            flight.Departure_Time = flight.Landing_Time.AddHours(1);
            flight.Remaining_Tickets = 0;
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
        }

        // Try To Cancel Flight For Current Airline.
        [TestMethod]
        public void CancelFlightForCurrentAirline()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            IList<Flight> flights = airlineFacade.GetAllFlights();
            Assert.AreEqual(1, flights.Count);
            airlineFacade.CancelFlight(airlineToken, flights[0]);
            flights = airlineFacade.GetAllFlightsByAirline(airlineToken);
            Assert.AreEqual(0, flights.Count);
        }


        // Try To Update Flight For Current Airline.
        [TestMethod]
        public void UpdateFlightForCurrentAirline()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            flight.Remaining_Tickets = 555;
            flight.Landing_Time = flight.Landing_Time + TimeSpan.FromDays(1);
            airlineFacade.UpdateFlight(airlineToken, flight);
            IList<Flight> flights = airlineFacade.GetAllFlightsByAirline(airlineToken);
            Assert.AreEqual(flights[0].Remaining_Tickets, 555);
        }

        // Change Password Successfuly For Airline.
        [TestMethod]
        public void ChangePasswordForAirline()
        {
            airlineFacade.ChangeMyPassword(airlineToken, "123", "NewPassword");
            Assert.AreEqual(airlineToken.User.Password, "NewPassword");
        }

        // Supposed To Get "WrongPasswordException" When Try Change Password For Airline.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryChangePasswordForAirline()
        {
            airlineFacade.ChangeMyPassword(airlineToken, "123456", "newPassword");
        }

        // Try To Get Ticket That Sold By Current Company.
        [TestMethod]
        public void GetSoldTicketById()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            long newId = customerFacade.PurchaseTicket(customerToken, flight);
            Ticket ticket = airlineFacade.GetSoldTicketById(airlineToken, (int)newId);
            Assert.AreEqual(airlineFacade.GetFlightById((int)ticket.Flight_Id).AirlineCompany_Id, airlineToken.User.Id);
        }

        // Supposed To Get "TicketNotMatchException" When Try Get Ticket That Sold By Another Company.
        [TestMethod]
        [ExpectedException(typeof(TicketNotMatchException))]
        public void TicketNotMatchWhenTryGetTicketThatNotMatchToCurrentCustomer()
        {
            AirlineCompany airline = CreateRandomCompany();
            airline.Airline_Number = adminFacade.CreateNewAirline(adminToken, airline);
            FlyingCenterSystem.GetUserAndFacade(airline.User_Name, "123", out ILogin token, out FacadeBase facade);
            LoginToken<AirlineCompany> newToken = token as LoginToken<AirlineCompany>;
            LoggedInAirlineFacade newfacade = facade as LoggedInAirlineFacade;
            Flight flight = CreateNewFlight();
            flight.AirlineCompany_Id = newToken.User.Id;
            flight.Id = newfacade.CreateFlight(newToken, flight);
            long newId = customerFacade.PurchaseTicket(customerToken, flight);
            Ticket ticket = airlineFacade.GetSoldTicketById(airlineToken, (int)newId);
        }
    }
}
