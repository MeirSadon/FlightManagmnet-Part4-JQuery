using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlightManagment___WebApi___Part_3.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            return new FilePathResult("~/Views/Page/home-page.html", "text/html");
        }

        public ActionResult SearchFlight()
        {
            return new FilePathResult("~/Views/Page/search-flights.html", "text/html");
        }
    }
}