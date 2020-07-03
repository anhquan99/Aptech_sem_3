using AptechSem3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechSem3.Controllers
{
    public class bgController : Controller
    {
        // GET: bg
        public ActionResult Index()
        {
            UsrService.deletebackgroundUsrNotAvailable();
            return View();
        }
    }
}