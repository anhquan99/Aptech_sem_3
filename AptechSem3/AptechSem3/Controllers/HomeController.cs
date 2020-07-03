using AptechSem3.Models;
using AptechSem3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AptechSem3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult JobPost()
        {
            JobPostService service = new JobPostService();
            ViewBag.List = service.findAll();
            return View();
        }
        
        //login and logout
        public ActionResult Login()
        {
            if (Session["username"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult postLogin(String username, String password)
        {
            if (Session["username"] == null)
            {
                UsrService service = new UsrService();
                USR user = service.findById(username);
                if(user == null) return RedirectToAction("Login", "Home");
                AccessService loginService = new AccessService();
                String role = loginService.Login(username, password);
                FormsAuthentication.SetAuthCookie(username, true);
                Session["username"] = username;
                if (role == "CANDIDATE")
                {
                    return RedirectToAction("Index", "Test");
                }
                else if (role == "MANAGER")
                {
                    
                    return RedirectToAction("Index", "Manager");
                }
                else return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            if(Session["username"] != null)
            {
                Session["username"] = null;
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }

        
    }
}