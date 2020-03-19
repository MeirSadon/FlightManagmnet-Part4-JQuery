using System;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForCustomerFacade : TestCenter
    {
        /*  ======= All Tests =======

        1. CancelTicket          -- CancelTicketSuccessfuly + TooLateToCancelTicketWhenTryCancelTicket.
        2. GetAllMyTickets       -- CancelTicketSuccessfuly.
        3. GetAllMyFlights       -- CancelTicketSuccessfuly.
        4. PurchaseTicket        -- CancelTicketSuccessfuly.
        5. MofidyCustomerDetails -- "TestForAdminFacadeClass"(UpdateCustomer).
        6. ChangeMyPassword      -- ChangePasswordForCustomer + WrongPasswordWhenTryChangePasswordForCustomer.
        7. GetTicketById         -- GetTicketById + TicketNotMatchWhenTryGetTicketThatNotMatchToCurrentCustomer.

        ======= All Tests ======= */


        [TestMethod]
        public void CancelTicketSuccessfuly()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            customerFacade.PurchaseTicket(customerToken, flight);
            Assert.AreEqual(customerFacade.GetAllMyFlights(customerToken).Count, 1);
            customerFacade.CancelTicket(customerToken, customerFacade.GetAllMyTickets(customerToken)[0]);
            Assert.AreEqual(customerFacade.GetAllMyFlights(customerToken).Count, 0);
        }

        //Supposed To Get "TooLateToCancelTicket" Exception.
        [TestMethod]
        [ExpectedException(typeof(TooLateToCancelTicketException))]
        public void TooLateToCancelTicketWhenTryCancelTicket()
        {
            Flight flight = CreateNewFlight();
            flight.Departure_Time = DateTime.Now;
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            customerFacade.PurchaseTicket(customerToken, flight);
            customerFacade.CancelTicket(customerToken, customerFacade.GetAllMyTickets(customerToken)[0]);
        }

        // Change Password Successfuly For Customer.
        [TestMethod]
        public void ChangePasswordForCustomer()
        {
            customerFacade.ChangeMyPassword(customerToken, customerToken.User.Password, "NewPassword");
            Assert.AreEqual(customerToken.User.Password, "NewPassword");
        }

        // Supposed To Get "WrongPasswordException" When Try Change Password For Customer.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryChangePasswordForCustomer()
        {
            customerFacade.ChangeMyPassword(customerToken, "123456", "newPassword");
        }

        // Try To Get Ticket From Ticket List Of Current Customer.
        [TestMethod]
        public void GetTicketById()
        {
            Flight flight = CreateNewFlight();
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            long newId = customerFacade.PurchaseTicket(customerToken, flight);
            Ticket ticket = customerFacade.GetPurchasedTicketById(customerToken, (int)newId);
            Assert.AreEqual(ticket.Customer_Id, customerToken.User.Id);
        }

        // Try To Get Ticket From Ticket List Of Current Customer.
        [TestMethod]
        [ExpectedException(typeof(TicketNotMatchException))]
        public void TicketNotMatchWhenTryGetTicketThatNotMatchToCurrentCustomer()
        {
            Customer customer = CreateRandomCustomer();
            customer.Customer_Number = adminFacade.CreateNewCustomer(customer);
            FlyingCenterSystem.GetUserAndFacade(customer.User_Name, "123", out ILogin token, out FacadeBase facade);
            LoginToken<Customer> newToken = token as LoginToken<Customer>;
            LoggedInCustomerFacade newfacade = facade as LoggedInCustomerFacade;
            Flight flight = new Flight { AirlineCompany_Id = airlineToken.User.Id, Departure_Time = new DateTime(2020, 12, 10, 00, 00, 00), Landing_Time = new DateTime(2020, 12, 11), Origin_Country_Code = adminFacade.GetCountryByName("Israel").Id, Destination_Country_Code = adminFacade.GetCountryByName("Israel").Id, Remaining_Tickets = 100 };
            flight.Id = airlineFacade.CreateFlight(airlineToken, flight);
            long newId = customerFacade.PurchaseTicket(customerToken, flight);
            newfacade.GetPurchasedTicketById(newToken, (int)newId);
        }
    }
}
