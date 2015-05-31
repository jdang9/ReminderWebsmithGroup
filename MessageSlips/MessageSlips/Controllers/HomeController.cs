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
using System.Xml.XPath;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Validation;

namespace MessageSlips.Controllers
{
    public class HomeController : Controller
    {
        
        private MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();
        private static MessageSlips.Models.User _userlogin = new MessageSlips.Models.User();
        public static bool CurrentAdmin;
        public static string CurrentUserName;
        public static string CurrentLogin = _userlogin.userName;
        public static int currentMessageID;
        public static int currentUserID = _userlogin.userID;

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
                        CurrentLogin = user.userName;
                        //currentUserID = user.userID;
                    }
                    else
                    {
                        CurrentAdmin = false;
                        CurrentLogin = user.userName;
                        //currentUserID = user.userID;
                    }
                    CurrentUserName = user.firstName + " " + user.lastName;
                    return RedirectToAction("Dashboard");
                }
            }
            
            return View("Index");
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
            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(FormCollection form)
        {
            MessageSlip message = new MessageSlip();
            MessageSlipsViewModel msMessageSlipsViewModel = new MessageSlipsViewModel();
            List<MessageSlipsViewModel> result = new List<MessageSlipsViewModel>();
            if (form.Keys[0] == "users")
            {

                string selectedUser = form["users"];

                    foreach (var slip in db.MessageSlips)
                    {                       
                        if (selectedUser == slip.userName)
                        {
                            //DateTime dateOnly = slip.date.Date;
                            //string timeOnly = slip.date.ToString("0:hh\\:mm");
                            string timeOnly = String.Format("{0:hh}:{0:mm}", slip.time);
                            msMessageSlipsViewModel.MessageId = slip.mID;
                            msMessageSlipsViewModel.Sender = slip.sender;
                            msMessageSlipsViewModel.Receiver = slip.receiver;
                            msMessageSlipsViewModel.Categories = slip.categories;
                            msMessageSlipsViewModel.Date = slip.date.ToString("MM/dd/yyyy");
                            msMessageSlipsViewModel.Time = timeOnly;
                            msMessageSlipsViewModel.Phone = slip.phoneNum;
                            msMessageSlipsViewModel.Message = slip.message;
                            msMessageSlipsViewModel.Email = slip.email;
                            msMessageSlipsViewModel.Other = slip.other;
                            msMessageSlipsViewModel.Username = slip.userName;
                            result.Add(msMessageSlipsViewModel);
                            msMessageSlipsViewModel = new MessageSlipsViewModel();
                        }
                    }
            }
            return View(result);
        }

        public ActionResult Edit(int id)
        {

                MessageSlipsViewModel mdb = new MessageSlipsViewModel();
                var d = mdb.GetMessage(id);
                if (Request.IsAjaxRequest())
                {
                    MessageSlip mSlip = new MessageSlip();
                }

            return View(d);
        }

        public ActionResult Done(int id)
        {
            using (db = new MessageSlipsWSGEntities())
            {
                var d = db.MessageSlips.Find(id);
                db.MessageSlips.Remove(d);
                db.SaveChanges();
            }

            return RedirectToAction("Dashboard");
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

            if (form.Keys[0] == "users")
            {
                MessageSlips.Models.MessageSlip newSlip = new MessageSlip();
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
                if (!string.IsNullOrEmpty(form["admin"]))
                {
                    string stringAdmin = form["admin"];
                    admin = Convert.ToBoolean(stringAdmin);
                    newUser.admin = admin;
                }
                //bool.TryParse(form["admin"], out admin);
                newUser.userName = form["newUsername"];
                newUser.password = form["newPassword"];
                newUser.email = form["newEmail"];
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

        [HttpGet]
        public ActionResult EditPopup(int id)
        {
            currentMessageID = id;
            MessageSlips.Models.MessageSlip mSlip = new MessageSlips.Models.MessageSlip();
            mSlip = db.MessageSlips.Find(id);
            return View(mSlip);
        }
        public ActionResult EditPopup(FormCollection form, int id)
        {
            currentMessageID = id;
            MessageSlips.Models.MessageSlip mSlip = new MessageSlips.Models.MessageSlip();
            mSlip.sender = form["mSender"];
            mSlip.categories = form["mCategories"];
            mSlip.phoneNum = form["mTel"];
            mSlip.message = form["mMessage"];
            mSlip.email = form["mEmail"];
            mSlip.other = form["mOther"];
            var original = db.MessageSlips.Find(id);
            if (original != null)
            {
                original.sender = mSlip.sender;
                original.categories = mSlip.categories;
                original.phoneNum = mSlip.phoneNum;
                original.message = mSlip.message;
                original.email = mSlip.email;
                original.other = mSlip.other;
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Client()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Client(FormCollection form)
        {
            MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();
            MessageSlips.Models.User login = new MessageSlips.Models.User();
            foreach (var user in db.Users.ToList())
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    login = user;
                }
            }

            Models.CurrentClient newClient = new Models.CurrentClient();
            newClient.clientName = form["newClient"];
            if (ModelState.IsValid)
            {
                db.CurrentClients.Add(newClient);
                db.SaveChanges();
            }
            
            
           
            return View();
        }

        [HttpPost]
        public ActionResult DeleteClient(FormCollection form)
        {
            if (form.Keys[0] == "clients")
            {
                string dClient = form["clients"];
                CurrentClient deleteClient = new CurrentClient();
                deleteClient = db.CurrentClients.Find(dClient);
                if (ModelState.IsValid)
                {
                    db.CurrentClients.Remove(deleteClient);
                    db.SaveChanges();
                    return RedirectToAction("Client");
                }
            }
            return View();
        }
    }
}