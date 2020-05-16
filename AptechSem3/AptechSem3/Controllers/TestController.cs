using AptechSem3.Models;
using AptechSem3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechSem3.Controllers
{
    //[Authorize(Roles ="CANDIDATE")]
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoTest()
        {



            //get test index
            UsrService usrService = new UsrService();
            int apply_id = usrService.getApplyIdByUsername("test-name");
            int index = TestService.getIndexByApplyId(apply_id);
            //update test index
            if (index == 0)
            {
                index++;
                TestService.updateIndexByApplyId(apply_id, index);
            }

            //variable list to save question
            List<TestQuestion> list = TestService.getQuestionByUsernameAndIndex("test-name", index);

            //send quetion and answer to web page
            ViewBag.Question = list;

            //send question id to late calculate result
            String QuestionIdList = "";
            foreach(var i in list)
            {
                //add question id to question id list
                QuestionIdList += i.QuestionId  + " ";
            }

            //send question id list to web page
            ViewBag.QuestionList = QuestionIdList;
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get test index
            UsrService usrService = new UsrService();
            int apply_id = usrService.getApplyIdByUsername("test-name");
            int index = TestService.getIndexByApplyId(apply_id);

            //get question id list 
            String result = Request.Params.Get("QuestionList").Trim();

            //convert result to list of string to iterator
            List<String> QuestionIds = result.Split(new char[] { ' ' }).ToList();
            List<String> AnswerLists= new List<string>();
            foreach(var i in QuestionIds)
            {
                AnswerLists.Add(Request.Params.Get("Q[" + i + "]"));
            }

            //calculate result score
            TestService.calculateScore(QuestionIds, AnswerLists, apply_id, index);

            //update index to 1
            index++;
            TestService.updateIndexByApplyId(apply_id, index);
            return View();
        }
    }
}