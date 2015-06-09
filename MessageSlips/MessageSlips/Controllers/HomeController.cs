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
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Owin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net.Http;

namespace MessageSlips.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        private MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();  //database
        private static MessageSlips.Models.User _userlogin = new MessageSlips.Models.User();    //current user
        public static bool CurrentAdmin;    //check if current user is admin
        public static string CurrentUserName;       //current user first + last name
        public static string CurrentLogin = _userlogin.userName;    //current login user
        public static int currentMessageID;     //current messageID (needed for edit and delete a message)
        public static int currentUserID = _userlogin.userID;    //current user ID

        //Index View
        public ActionResult Index()
        {
            return View();
        }

        /*
         * Index to control login
         * @para: form - getting inputs from View
         * */
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            MessageSlips.Models.User login = new MessageSlips.Models.User();
            foreach (var user in db.Users)
            {
                /*
                 * if username and password are matched
                 * return current user admin status, name, and username
                 * */
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    _userlogin = user;

                    if (user.admin == true)
                    {
                        CurrentAdmin = true;
                        CurrentLogin = user.userName;
                    }
                    else
                    {
                        CurrentAdmin = false;
                        CurrentLogin = user.userName;                       
                    }
                    CurrentUserName = user.firstName + " " + user.lastName;
                    return RedirectToAction("Dashboard");   //Go to dashboard if success
                }
                ModelState.AddModelError("SigninError", "Username or password provided is in corrected. Please try again!");    //throw error message
            }

            return View("Index"); //Stay on Index if fail
        }

        /*
         * Log out option
         * */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //Dashboard View
        public ActionResult Dashboard()
        {
            return View();
        }

        /*
         * Dashboard to control messages
         * @para: form
         * */
        [HttpPost]
        public ActionResult Dashboard(FormCollection form)
        {
            MessageSlip message = new MessageSlip();    //Single message
            MessageSlipsViewModel msMessageSlipsViewModel = new MessageSlipsViewModel();    //Message Model
            List<MessageSlipsViewModel> result = new List<MessageSlipsViewModel>(); //List of all fields in message model
            if (form.Keys[0] == "users")
            {

                string selectedUser = form["users"];    //chosen user's message

                //getting message fields from Message model
                foreach (var slip in db.MessageSlips)
                {
                    if (selectedUser == slip.userName)
                    {
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

        /*
         * Edit to edit a single message
         * */
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

        /*
         * Done to delete a message
         * */
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

        //NewMessage View
        public ActionResult NewMessage()
        {
            return View();
        }

        /*
         * NewMessage to create a message
         * @para: form - getting inputs from user
         * */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewMessage(FormCollection form)
        {
            //https://www.google.com/settings/security/lesssecureapps
            var email = "";     //user's email
            var notification = "";      //Message to send to email

            /*
             * Getting user's input and send to database
             * */
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

            //Generate message that will be send to receiver
            foreach (var user in db.Users)
            {
                if (form["mReceiver"] == user.userName)
                {
                    id = user.userName;
                    email = user.email;
                    notification = "Hi " + user.firstName + "! " + "You have a new message:" + "<br />"
                        + "From: " + form["mSender"] + "<br />"
                        + "Category: " + form["mCategories"] + "<br />"
                        + "Phone: " + form["mTel"] + "<br />"
                        + "Your Message: " + form["mMessage"] + "<br />"
                        + "Other Info: " + form["mOther"] + "<br />" + "<br /" + "<br />"
                        + "Attention: This is an automated message. Do not reply to this email! - WebsmithGroup";
                }
            }
            mSlip.userName = id;
            if (ModelState.IsValid)
            {
                db.MessageSlips.Add(mSlip);
                db.SaveChanges();

                var message = new MailMessage();
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("messageslips@gmail.com");       //defaule sender
                message.Subject = "You Missed a Call!";     //email subject
                message.Body = notification;    //email body = notification
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    //credential set up
                    var credential = new NetworkCredential
                    {
                        UserName = "messageslips@gmail.com",
                        Password = "changemerva"
                    };
                    //smtp set up
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }

            if (form.Keys[0] == "users")
            {
                MessageSlips.Models.MessageSlip newSlip = new MessageSlip();
            }
            return View();
        }

        //Setting (Add/Delete User) View
        public ActionResult Setting()
        {
            return View();
        }

        /*
         * Setting to add or delete a user
         * @para: form - getting new user's information
         * */
        [HttpPost]
        public ActionResult Setting(FormCollection form)
        {
            MessageSlips.Models.User login = new MessageSlips.Models.User();

            //Delete a user
            if (form.Keys[0] == "users")
            {
                string dUser = form["users"];
                User deleteUser = new User();
                MessageSlip deleteMessage = new MessageSlip();
                deleteUser = db.Users.Find(dUser);  //find user
                if (ModelState.IsValid)
                {
                    //Delete all remain messages of deleted user
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

            //try to make new username, if username is already taken throw catch
            try
            {
                //Add a user
                if (form["newPassword"] != form["confirmedNewPassword"]) //won't allow adding a user if passwords don't match
                {
                    ModelState.AddModelError("PasswordsValidation", "Confirmed password doesn't match! Please try again!"); //throw error message
                }
                else if (form["newPassword"] == form["confirmedNewPassword"])   //continue if matched
                {
                    Models.User newUser = new Models.User();    //new user account
                    bool admin;
                    //getting new user's info
                    if (!string.IsNullOrEmpty(form["admin"]))
                    {
                        string stringAdmin = form["admin"];
                        admin = Convert.ToBoolean(stringAdmin);
                        newUser.admin = admin;
                    }
                    newUser.userName = form["newUsername"];
                    newUser.password = form["newPassword"];
                    newUser.email = form["newEmail"];
                    newUser.firstName = form["newFirstName"];
                    newUser.lastName = form["newLastName"];
                    if (ModelState.IsValid)
                    {
                        db.Users.Add(newUser);  //add
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e) //catch showing username is taken
            {
                ModelState.AddModelError("UserExist", "Username is already taken. Please choose another username!");    //throw error message
            }
            

            return View();
        }

        //UserSetting (Account Setting) View
        public ActionResult UserSetting()
        {
            return View();
        }

        /*
         * UserSetting to change user password and set admin
         * */
        [HttpPost]
        public ActionResult UserSetting(FormCollection form)
        {
            //Change password
            foreach (var userPass in db.Users.ToList())
            {
                Models.User newUserPass = new Models.User();
                newUserPass = db.Users.Find(CurrentLogin);
                if (userPass.password == form["currentPassword"]) //current password check
                {
                    if (form["newPassword"] == form["confirmedNewPassword"])    //new passwords check
                    {
                        string newPassword = form["newPassword"];
                        newUserPass.password = newPassword;         //password changed
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordsValidation", "Confirmed password doesn't match! Please try again!");     //throw error message
                    }
                }
                else if (userPass.password != form["currentPassword"])
                {
                    ModelState.AddModelError("CurrentPasswordValidation", "Current password is incorrect! Please try again!");      //throw error message
                }
            }

            //Set admin for a user
            foreach (var user in db.Users.ToList())
            {
                if (form["setAdminUser"] == user.userName)
                {
                    var setAdminUser = db.Users.Find(form["setAdminUser"]);
                    bool admin;
                    if (!string.IsNullOrEmpty(form["setAdmin"]))
                    {
                        string stringAdmin = form["setAdmin"];
                        admin = Convert.ToBoolean(stringAdmin);
                        setAdminUser.admin = admin;     //set admin
                        db.SaveChanges();
                    }
                }
            }
            return View();
        }

        /*
         * EditPopup to view edit message
         * @para: message id
         * */
        [HttpGet]
        public ActionResult EditPopup(int id)
        {
            currentMessageID = id;
            MessageSlips.Models.MessageSlip mSlip = new MessageSlips.Models.MessageSlip();
            mSlip = db.MessageSlips.Find(id);       //find the message
            return View(mSlip);
        }

        /*
         * EditPopup to make changes to the message
         * @para: form - gather inputs, id - the message id
         * */ 
        public ActionResult EditPopup(FormCollection form, int id)
        {
            currentMessageID = id;  //current message id
            //Getting editable info
            MessageSlips.Models.MessageSlip mSlip = new MessageSlips.Models.MessageSlip();
            mSlip.sender = form["mSender"];
            mSlip.categories = form["mCategories"];
            mSlip.phoneNum = form["mTel"];
            mSlip.message = form["mMessage"];
            mSlip.email = form["mEmail"];
            mSlip.other = form["mOther"];

            var original = db.MessageSlips.Find(id);    //find original message
            //Edit orginial message
            if (original != null)
            {
                original.sender = mSlip.sender;
                original.categories = mSlip.categories;
                original.phoneNum = mSlip.phoneNum;
                original.message = mSlip.message;
                original.email = mSlip.email;
                original.other = mSlip.other;
                db.SaveChanges();   //save edit message
            }

            return View();
        }

        //Client View
        public ActionResult Client()
        {
            return View();
        }

        /*
         * Client to add new client
         * @para: form - getting new client name
         * avaibable to only admin users
         * */
        [HttpPost]
        public ActionResult Client(FormCollection form)
        {
            MessageSlips.Models.MessageSlipsWSGEntities db = new Models.MessageSlipsWSGEntities();
            MessageSlips.Models.User login = new MessageSlips.Models.User();
            //check if user is admin
            foreach (var user in db.Users.ToList())
            {
                if (user.userName == form["username"] && user.password == form["password"])
                {
                    login = user;
                }
            }

            Models.CurrentClient newClient = new Models.CurrentClient();
            newClient.clientName = form["newClient"];   //get client name
            if (ModelState.IsValid)
            {
                db.CurrentClients.Add(newClient);       //add client
                db.SaveChanges();
            }



            return View();
        }

        /*
         * Delete client
         * */
        [HttpPost]
        public ActionResult DeleteClient(FormCollection form)
        {
            if (form.Keys[0] == "clients")
            {
                string dClient = form["clients"];   //get client
                CurrentClient deleteClient = new CurrentClient();
                deleteClient = db.CurrentClients.Find(dClient);
                if (ModelState.IsValid)
                {
                    db.CurrentClients.Remove(deleteClient); //delete client
                    db.SaveChanges();
                    return RedirectToAction("Client");
                }
            }
            return View();
        }

        /*
         * Json getting para from Google Sign in
         * @para: user's name and email
         * */
        [HttpPost]
        public JsonResult ExternalLogin(string name, string email)
        {
            var success = name;     //user's name as first passing success
            var secondsuccess = email;  //user's email as second passing success
            
            var names = success.Split(' '); //split first and last name
            string firstName = names[0];
            string lastname = names[1];
            User googleUser = new User();
            //Autotamically create new user account when choosing Google sign in
            googleUser.firstName = firstName;
            googleUser.lastName = lastname;
            googleUser.admin = false;       //all users using Google sign in will be non-admin
            googleUser.email = email;
            googleUser.userName = email;
            googleUser.password = "changeme";       //default password will be forced to change

            CurrentLogin = email;
            CurrentUserName = firstName + " " + lastname;

            if (ModelState.IsValid) {
                db.Users.Add(googleUser);   //add new user
                db.SaveChanges();
            }

            return Json(new { success, secondsuccess });
        }

        //ChangePassword View
        public ActionResult ChangePassword()
        {
            return View();
        }

        /*
         * ChangePassword to force new user from Google sign in to change their password (set up a new password)
         * @para: form - getting passwords
         * */
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            foreach (var userPass in db.Users.ToList())
            {
                Models.User newUserPass = new Models.User();
                newUserPass = db.Users.Find(CurrentLogin);
                if (form["newPassword"] == form["confirmedNewPassword"])   //Passwords have to be matched 
                {
                    string newPassword = form["newPassword"];
                    newUserPass.password = newPassword;         //new password created
                    db.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("PasswordsValidation", "Confirmed password doesn't match! Please try again!");     //throw error message
            }
            return View();
        }
    }
}