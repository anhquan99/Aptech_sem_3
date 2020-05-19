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
    [Authorize(Roles = "CANDIDATE")]
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
           if(Session["username"] != null)
            {
                UsrService usrService = new UsrService();
                int apply_id = usrService.getApplyIdByUsername(Session["username"].ToString());
                JOB_APPLICATION application = JobApplicationService.findByUsername(Session["username"].ToString());
                ViewBag.Application = application;
                TEST test = TestService.getTestByApplyId(apply_id);
                ViewBag.Test = test;
                //if start time is later than now and end time is ealier than now let the candidate have link to do test
                if (DateTime.Compare(DateTime.Now,test.START_TIME) > 0 && DateTime.Compare(DateTime.Now,test.END_TIME) < 0) 
                {
                    ViewBag.link = "true";
                }
                else
                {
                    ViewBag.link = "false";   
                }
                //timer
                TimeSpan timeLeft = test.END_TIME - test.START_TIME;
                ViewBag.TimeLeft = timeLeft;
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        //do test
        //remove cache from candidate browser to prvent backward page to save
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult DoTest()
        {
            if(Session["username"] != null)
            {
                //get test index
                UsrService usrService = new UsrService();
                int apply_id = usrService.getApplyIdByUsername(Session["username"].ToString());
                int index = TestService.getIndexByApplyId(apply_id);
                //check test start time and end time to do test
                if (DateTime.Compare(DateTime.Now, TestService.getStartTimeByApplyId(apply_id)) > 0 && DateTime.Compare(DateTime.Now, TestService.getEndTimeByApplyId(apply_id)) < 0)
                {
                    //update test index
                    if (index == 0)
                    {
                        index++;
                        TestService.updateIndexByApplyId(apply_id, index);
                    }
                    else if (index == 4)
                    {
                        return RedirectToAction("finished", "Test");
                    }

                    //variable list to save question
                    List<TestQuestion> list = TestService.getQuestionByUsernameAndIndex(Session["username"].ToString(), index);

                    //send quetion and answer to web page
                    ViewBag.Question = list;

                    //send question id to late calculate result
                    String QuestionIdList = "";
                    foreach (var i in list)
                    {
                        //add question id to question id list
                        QuestionIdList += i.QuestionId + " ";
                    }

                    //send question id list to web page
                    ViewBag.QuestionList = QuestionIdList;
                    //timer
                    TimeSpan timeLeft = TestService.getEndTimeByApplyId(apply_id) - DateTime.Now;
                    double total = timeLeft.TotalSeconds;
                    //set time count
                    ViewBag.TimeLeft = total;
                    return View();
                }
                return RedirectToAction("Index", "Test");
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult finished()
        {
            if(Session["username"] != null)
            {
                //get test index
                UsrService usrService = new UsrService();
                int apply_id = usrService.getApplyIdByUsername(Session["username"].ToString());
                int index = TestService.getIndexByApplyId(apply_id);
                //check if acces is authorized and user have finished test or test out of time
                if ((index == 4 || DateTime.Compare(DateTime.Now, TestService.getEndTimeByApplyId(apply_id)) > 0))
                {
                    ViewBag.Message = "You have finished your test. Please wait for our mail for the result.\nYou will be logout and this accont will be delete !";
                    Session["username"] = null;
                    FormsAuthentication.SignOut();
                }
                //if user is authorized but did not finish test yet
                else if ((DateTime.Compare(DateTime.Now, TestService.getStartTimeByApplyId(apply_id)) > 0 && DateTime.Compare(DateTime.Now, TestService.getEndTimeByApplyId(apply_id)) < 0))
                {
                    return RedirectToAction("DoTest", "Test");
                }
                //if start time is later than now
                else if (DateTime.Compare(DateTime.Now, TestService.getStartTimeByApplyId(apply_id)) < 0)
                {
                    return RedirectToAction("Index", "Test");
                }
                //else ViewBag.Message = "Please login to do your test !";
                return View();
            }
            return RedirectToAction("Login", "Home");
            
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get test index
            UsrService usrService = new UsrService();
            int apply_id = usrService.getApplyIdByUsername(Session["username"].ToString());
            int index = TestService.getIndexByApplyId(apply_id);
            if (DateTime.Compare(DateTime.Now, TestService.getStartTimeByApplyId(apply_id)) > 0 && DateTime.Compare(DateTime.Now, TestService.getEndTimeByApplyId(apply_id)) < 0)
            {
                //get question id list 
                String result = Request.Params.Get("QuestionList").Trim();

                //convert result to list of string to iterator
                List<String> QuestionIds = result.Split(new char[] { ' ' }).ToList();
                List<String> AnswerLists = new List<string>();
                foreach (var i in QuestionIds)
                {
                    AnswerLists.Add(Request.Params.Get("Q[" + i + "]"));
                }

                //calculate result score
                TestService.calculateScore(QuestionIds, AnswerLists, apply_id, index);

                //update index to 1
                index++;
                TestService.updateIndexByApplyId(apply_id, index);
                if (DateTime.Compare(DateTime.Now, TestService.getEndTimeByApplyId(apply_id)) > 0) return RedirectToAction("finished", "Test");
                return RedirectToAction("DoTest", "Test");
            }
            return RedirectToAction("finished", "Test");
            
        }
    }
}