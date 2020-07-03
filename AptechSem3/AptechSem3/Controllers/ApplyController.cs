using AptechSem3.Models;
using AptechSem3.Service;
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
            ViewBag.POST_ID = postId;
            ViewBag.PHONE = PHONE;
            ViewBag.PERSONAL_ID = PERSONAL_ID;
            JOB_APPLICATION application = JobApplicationService.findByPersonalIdAndPhone(PERSONAL_ID, PHONE);
            ViewBag.application = application;
            if (application == null)
            {
                return View("ApplyForm");
            }
            else {
                TempData["Application"] = application;
                return View("UpdateForm");
            }
        }
        [HttpPost]
        public ActionResult Apply(JOB_APPLICATION apply)
        {
            JobApplicationService service = new JobApplicationService();
            if (service.Create(apply))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult UpdateForm(JOB_APPLICATION apply)
        {
            JobApplicationService jobApplicationService = new JobApplicationService();
            if (jobApplicationService.update(apply))
            {
                return RedirectToAction("Index", "Home");
            }
            else return View("Apply","Apply", new { id = apply.POST_ID});
        }
    }
}