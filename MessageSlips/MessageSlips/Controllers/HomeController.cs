using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;
using MessageSlips.Models;

namespace MessageSlips.Controllers
{
    public class HomeController : Controller
    {
        private MessageSlipsWSGEntities db = new MessageSlipsWSGEntities();
        // GET: Home
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            User login = new User();

            foreach (var user in db.Users)
            {
                if (user.UserName == form["username"] && user.Password == form["password"])
                {
                    login = user;
                    return RedirectToAction("Index");
                }
                else if (user.UserName == form["username"] && user.Password != form["password"])
                {
                    return View("Login");
                }
            }

            return View("Login");
        }
    }
}