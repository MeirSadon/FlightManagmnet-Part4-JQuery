using FlightManagment___Basic___Part_1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace FlightManagment___WebApi___Part_3
{
    /// <summary>
    /// Center Class For All Functions Of The Controllers.
    /// </summary>
    public class ControllersCenter : ApiController
    {

        #region Execute Any Action With All Catch Cases.
        /// <summary>
        /// One Function For All Catch Cases.
        /// </summary>
        /// <param name="myFunc"></param>
        /// <returns>IHttpActionResult</returns>
        public IHttpActionResult ExecuteSafe(Func<IHttpActionResult> myFunc)
        {
            try
            {
                return myFunc.Invoke();
            }
            catch (UserNotExistException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (UserAlreadyExistException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            catch (WrongPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CentralAdministratorException ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (FlightNotMatchException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OutOfTicketsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TooLateToCancelTicketException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TicketNotMatchException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion                                                                                                                                        
    }
}