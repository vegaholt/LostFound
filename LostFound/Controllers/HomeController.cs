using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostFound.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";

            return View();
        }

        public ActionResult Policy()
        {
            ViewBag.Message = "Privacy policy";

            return View();
        }
        public ActionResult Terms()
        {
            ViewBag.Message = "Terms of service";

            return View();
        }
    }
}