using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using MessageSlips.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Expression.Framework;
using System.Data.SqlClient;
using FormCollection = System.Web.Mvc.FormCollection;
using WebMatrix.WebData;
using System.Web.Security;

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
            var count = 0;
            foreach (var user in db.Users)
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    login = user;
                        count++;
                    return RedirectToAction("Dashboard");
                }

            }
            
            return View("Index");
        }

        public ActionResult GetLoginResult()
        {
            String result = "This causes too much pain";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoginMessage(FormCollection form)
        {
            MessageSlips.Models.User login = new MessageSlips.Models.User();

            bool check = false;

            foreach (var user in db.Users)
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    check = true;
                }
            }

            return Json(check, JsonRequestBehavior.AllowGet);
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
            SqlConnection con = new SqlConnection("Server=JASONDANGA73C\\SQLEXPRESS;Database=MessageSlipsWSG;Trusted_Connection = True");
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
            if (form.Keys[0] == "users")
            {
                /*int id;
                int.TryParse(form["users"], out id);*/
                string dUser = form["users"];
                User deleteUser = new User();
                deleteUser = db.Users.Find(dUser);
                if (ModelState.IsValid)
                {
                    db.Users.Remove(deleteUser);
                    db.SaveChanges();
                    return RedirectToAction("Setting");
                }
            } 
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