using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechSem3.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoTest()
        {
            List<QuestionAndAnswer> list = new List<QuestionAndAnswer>();
            List<String> answ = new List<string>();
            answ.Add("A 1");
            list.Add(new QuestionAndAnswer(1, "Test Question 1", answ));
            answ.Add("A 2");
            list.Add(new QuestionAndAnswer(2, "Test Question 2", answ));
            answ.Add("A 3");
            list.Add(new QuestionAndAnswer(3, "Test Question 3", answ));
            answ.Add("A 4");
            list.Add(new QuestionAndAnswer(4, "Test Question 4", answ));
            ViewBag.Question = list;
            return View();
        }
        [HttpPost]
        public ActionResult Submit(List<int> listInt)
        {
            Console.WriteLine(listInt + " " +  listString);
            return View();
        }
    }
}