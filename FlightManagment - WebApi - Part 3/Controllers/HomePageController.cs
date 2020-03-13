using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlightManagment___WebApi___Part_3.Controllers
{
    public class HomePageController : Controller
    {
        public ActionResult Deals()
        {
            return new FilePathResult("~/Views/HomePage/mini/deals.html", "text/html");
        }

        public ActionResult CusReg()
        {
            return new FilePathResult("~/Views/HomePage/mini/customer-register.html", "text/html");
        }

        public ActionResult CmpReg()
        {
            return new FilePathResult("~/Views/HomePage/mini/company-register.html", "text/html");
        }
    }
}