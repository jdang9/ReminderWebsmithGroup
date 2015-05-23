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
using Microsoft.Ajax.Utilities;

namespace MessageSlips.Controllers
{
    public class HomeController : Controller
    {
        
        private MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();
        private static MessageSlips.Models.User _userlogin = new MessageSlips.Models.User();
        private static MessageSlips.Models.User _useradmin = new MessageSlips.Models.User();
        public static bool CurrentAdmin;
        public static String CurrentUserName;

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
                    _userlogin = user;
                    if (user.admin == true)
                    {
                        CurrentAdmin = true;
                    }
                    else
                    {
                        CurrentAdmin = false;
                    }
                    CurrentUserName = user.firstName + " " + user.lastName;
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

        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(FormCollection form)
        {
            MessageSlip mSlip = new MessageSlip();
            DateTime mDate;
            TimeSpan mTime;
            DateTime.TryParse(form["mDate"], out mDate);
            TimeSpan.TryParse(form["mTime"], out mTime);
            String id = "";
            mSlip.sender = form["mSender"];
            mSlip.receiver = form["mReceiver"];
            mSlip.categories = form["mCategories"];
            mSlip.date = mDate;
            mSlip.time = mTime;
            mSlip.phoneNum = form["mTel"];
            mSlip.message = form["mMessage"];
            mSlip.email = form["mEmail"];
            mSlip.other = form["mOther"];
            foreach (var user in db.Users)
            {
                if (form["mReceiver"] == user.userName)
                {
                    id = user.userName;
                }
            }
            mSlip.userName = id;
            if (ModelState.IsValid)
            {
                db.MessageSlips.Add(mSlip);
                db.SaveChanges();
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
            MessageSlips.Models.User login = new MessageSlips.Models.User();
            foreach (var user in db.Users)
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    login = user;
                }

            }

            if (form.Keys[0] == "users")
            {
                string dUser = form["users"];
                User deleteUser = new User();
                MessageSlip deleteMessage = new MessageSlip();
                deleteUser = db.Users.Find(dUser);
                if (ModelState.IsValid)
                {
                    foreach (var dMess in db.MessageSlips)
                    {
                        var mID = dMess.mID;
                        if (dMess.userName == deleteUser.userName)
                        {
                            deleteMessage = db.MessageSlips.Find(mID);
                            db.MessageSlips.Remove(deleteMessage);
                        }
                    }
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
                bool.TryParse(form["adminStat"], out admin);
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