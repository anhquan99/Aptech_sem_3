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
            ViewBag.postId = postId;
            ViewBag.phone = PHONE;
            ViewBag.person_id = PERSONAL_ID;
            JOB_APPLICATION application = JobApplicationService.findByPersonalIdAndPhone(PERSONAL_ID, PHONE);
            ViewBag.application = application;
            return View();
        }
        [HttpPost]
        public ActionResult Apply(JOB_APPLICATION apply)
        {
            JOB_APPLICATION application = JobApplicationService.findByPersonalIdAndPhone(apply.PERSONAL_ID, apply.PHONE);
            JobApplicationService service = new JobApplicationService();
            if (application != null)
            {
                service.update(application);
            }
            else
            {
                service.Create(application);
            }
            return View();
        }
    }
}