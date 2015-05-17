using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using MessageSlips.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Expression.Framework;
using System.Data.SqlClient;
using FormCollection = System.Web.Mvc.FormCollection;

namespace MessageSlips.Controllers
{
    public class HomeController : Controller
    {
        private MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();
 
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            MessageSlips.Models.User login = new MessageSlips.Models.User();

            foreach (var user in db.Users)
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    login = user;
                    return RedirectToAction("Dashboard");
                }
                else if (user.userName != form["username"] && user.password != form["password"])
                {
                    return View("Index");
                }
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard goes here";

            return View();
        }

        public ActionResult NewMessage(FormCollection form)
        {
            MessageSlips.Models.MessageSlip ml = new MessageSlips.Models.MessageSlip();
            SqlConnection con = new SqlConnection("Server=JDANG-PC\\SQLEXPRESS;Database=MessageSlipsWSG;Trusted_Connection = True");
            {
                SqlCommand xp = new SqlCommand("INSERT INTO MessageSlip(sender, receiver, categories, date, time, phoneNum, message, location, other, userName, userID) VALUES(@sender, @receiver, @categories, @date, @time, @phoneNum, @message, @location, @other, @userName, @userID", con);
            }
            return View();
        }


        public ActionResult Setting()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Setting(FormCollection form)
        {
           if (form["newPassword"] != form["confirmedNewPassword"])
            {
                MessageBox.Show("Confirmed password does not match!");
                return RedirectToAction("Setting");
            }
            else if (form["newPassword"] == form["confirmedNewPassword"])
            {
                Models.User newUser = new Models.User();
                bool admin;
                bool.TryParse(form["admin"], out admin);
                newUser.userName = form["newUsername"];
                newUser.password = form["newPassword"];
                newUser.email = form["newEmail"];
                newUser.admin = admin;
                newUser.firstName = form["newFirstName"];
                newUser.lastName = form["newLastName"];
                if (ModelState.IsValid)
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
            }
            return View(); 
        }
    }
}