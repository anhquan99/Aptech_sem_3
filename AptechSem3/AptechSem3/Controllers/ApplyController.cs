using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechSem3.Controllers
{
    public class ApplyController : Controller
    {
        public ActionResult ApplyForId(int id)
        {
            ViewBag.postId = id;
            return View();
        }
        [HttpPost]
        public ActionResult ApplyForm(String PERSONAL_ID, String PHONE, int postId)
        {
            ViewBag.postId = postId;
            ViewBag.phone = PHONE;
            ViewBag.person_id = PERSONAL_ID;
            return View();
        }
        [HttpPost]
        public ActionResult Apply(JOB_APPLICATION apply)
        {
            return View();
        }
    }
}