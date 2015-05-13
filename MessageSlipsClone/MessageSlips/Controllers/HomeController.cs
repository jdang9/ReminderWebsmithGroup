using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageSlips.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Message()
        {
            ViewBag.Message = "Your Message.";

            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your dashboard.";

            return View();
        }
    }
}