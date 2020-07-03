using System;
using System.Collections.Generic;
using System.Linq;
using AptechSem3.Service.ModelService;
using AptechSem3.Models;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AptechSem3.Controllers
{
    [Authorize(Roles = "MANAGER")]
    //Authorize role
    public class ManagerController : Controller
    {
        // GET: Manager
        //Load List Report on the Home manager
        public ActionResult Index()
        {
            ReportService reportService = new ReportService();
            ViewBag.ReportList = reportService.findAllByDate();

            return View();
        }


        //View the Report Detail
        public ActionResult ReportDetail(DateTime created)
        {
            ReportService reportService = new ReportService();
            ResultService resultService = new ResultService();
            //find test with created date time of the report
            Session["TestId"] = reportService.findTestIdWithCreated(created);
            int testId = reportService.findTestIdWithCreated(created);
            TempData["Report"] = reportService.findByCreated(created);
            ViewBag.ResultList = resultService.FindResultByTestId(testId);
            ViewBag.ApplicationList = reportService.findApplicationWithResult(testId, "attempt");
            return View();
        }
        //Get method
        public ActionResult CreatePost(string error)
        {
            ViewBag.Error = error;
            return View();
        }
        //Post method 
        public ActionResult CreatePostDB(JOB_POST p)
        {
            PostService postService = new PostService();
            p.CREATED = DateTime.Now;
            if (DateTime.Compare(p.CREATED, p.END_DATE) < 0)
            {
                if (postService.create(p))
                {
                    return RedirectToAction("ShowPosts");
                }
                else return RedirectToAction("CreatePost", new { error = "There's something wrong. Please create again" });
            }
            else return RedirectToAction("CreatePost", new { error = "End Date must be after Now" });
        }
        //get method
        public ActionResult UpdatePost(string postId, string error)
        {
            PostService postService = new PostService();
            JOB_POST post = postService.findById(postId);
            if (DateTime.Compare(post.END_DATE, DateTime.Now) < 0)
            {
                return RedirectToAction("ShowPosts", "Manager", new { error = "Can't update post that passed the End time" });
            }
            else
            {
                TempData["Post"] = post;
                ViewBag.error = error;
                return View();
            }
        }
        //post method
        public ActionResult UpdatePostDB(JOB_POST p)
        {
            PostService postService = new PostService();
            if (DateTime.Compare(p.END_DATE, DateTime.Now) < 0)
            {
                return RedirectToAction("UpdatePost", "Manager", new { postId = p.POST_ID, error = "Can't update post that passed the End time" });
            }
            if (postService.update(p))
            {
                return RedirectToAction("ShowPosts", "Manager");
            }
            else return RedirectToAction("ShowPosts", "Manager");
        }
        //show every posts
        public ActionResult ShowPosts()
        {
            PostService postService = new PostService();
            ViewBag.PostList = postService.findAll();
            return View();
        }

        //Show all apps
        public ActionResult ShowApplications(int status, string error)
        {
            ApplicationService applicationService = new ApplicationService();
            ViewBag.ApplicationList = applicationService.findByStatus(status);
            ViewBag.error = error;
            return View();
        }

        //Approve Apps
        public ActionResult ApproveApplication(string applyId, int status)
        {
            ApplicationService applicationService = new ApplicationService();
            ResultService resultService = new ResultService();
            //Create a new Object APPLY TYPE
            JOB_APPLICATION apply = new JOB_APPLICATION();
            PostService postService = new PostService();
            //get TestId by ApplyId 
            int testId = applicationService.getTestIdByApplyId(applyId);
            //testid == 0 equal testid == null
            if (testId == 0)
            {
                //redirect page
                return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
            }
            //find the APPLY with applyId
            apply = applicationService.findById(applyId);
            //get Now DateTime to compare
            string created = DateTime.Now.ToString("ddMMyyyyHHmmss");
            UserService userService = new UserService();
            //Check Apply Existed
            if (userService.findByApplyId(applyId))
            {
                ViewBag.error = "User has Existed";
                return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
            }
            //Check same status ?
            if (apply.APPROVE_STATUS == status)
            {
                ViewBag.error = "Can't proceed the same Status";
                return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
            }
            else
            //Status == 1 -> Status == "Approved"
            if (status == 1)
            {
                RESULT result = new RESULT();
                result.APPLY_ID = Int32.Parse(applyId);
                result.TEST_ID = testId;
                
                 
                applicationService.apply(applyId, 1);
                string username = "user" + created;
                string decrypt = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(created)).Select(s => s.ToString("x2")));
                string password = decrypt.Substring(2, 10);
                USR user = new USR();
                user.APPLY_ID = Int32.Parse(applyId);
                user.ROLE = "CANDIDATE";
                user.USERNAME = username;
                user.PASSWORD = password;
                //if create false => user existed
                if (userService.create(user))
                {
                    return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
                }
                else
                {
                    ViewBag.error = "User for this application existed";
                    return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
                }
            }
            else
            //unapprove
            {
                //check delete the result 
                if (resultService.deleteResultByApply(Int32.Parse(applyId)))
                { 
                    
                }
                //unapprove the apply
                applicationService.apply(applyId, -1);
            }

            return RedirectToAction("ApplicationDetail", "Manager", new { applyId = applyId });
        }


        public ActionResult ApplicationDetail(string applyId)
        {
            ApplicationService applicationService = new ApplicationService();
            JOB_APPLICATION application = applicationService.findById(applyId);
            TestService testService = new TestService();
            ViewBag.TestList = testService.findByPost(application.POST_ID);
            ViewBag.Test = testService.findByPostSingle(application.POST_ID);
            UserService userService = new UserService();
            TempData["User"] = userService.findReportByApplyId(applyId);
            TempData["Application"] = applicationService.findById(applyId);
            return View();
        }
        //Mail the APPROVED candidates
        public ActionResult Mail(int APPLY_ID, int TEST_ID)
        {
            try
            {
                ResultService resultService = new ResultService();
                RESULT result = new RESULT();
                result.APPLY_ID = APPLY_ID;
                result.TEST_ID = TEST_ID;
                //create result before mail
                resultService.create(result);
            }
            catch (Exception)
            {
            }
            try
            {
                //Mail
                AptechSem3.Service.MailService mailService = new Service.MailService();
                ApplicationService applyService = new ApplicationService();
                JOB_APPLICATION apply = applyService.findById(APPLY_ID.ToString());
                AptechSem3.Service.UsrService userService = new AptechSem3.Service.UsrService();
                USR usr = userService.findUsrByApplyID(APPLY_ID);
                TestService testService = new TestService();
                TEST test = testService.findById(TEST_ID.ToString());
                //message
                String message = "<p>Dear " + apply.NAME + ",</p>" +
                    "<p>Thank you for applying for the position with The Webster Company.</p>" +
                    "<p>We would like to invite you to our online test for the position. Your test has been scheduled for " + test.START_TIME + " to " + test.END_TIME + ".</p>" +
                    "<p>Your account for this test:</p>" +
                    "<p>Username: " + usr.USERNAME + "</p>" + "<p>Password: " + usr.PASSWORD + "</p>" +
                    "<p>Please reply if you have any question.</p>" +
                    "<p>Sincerly,</p>" +
                    "<p>The Webster Company</p>";
                mailService.sendMail(apply.MAIL, test.TEST_NAME, message);
                ViewBag.error = "Mail success";    
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            return RedirectToAction("ApplicationDetail", "Manager", new { applyId = APPLY_ID });
        }
        //get method
        public ActionResult CreateTest(string error)
        {
            ViewBag.error = error;
            PostService postService = new PostService();
            //Create test depends on 1 post
            ViewBag.PostList = postService.getPostNotCreated();
            return View();
        }
        //post method
        public ActionResult CreateTestDB(TEST test)
        {
            TestService testService = new TestService();
            PostService postService = new PostService();
            DateTime postTime = postService.findById(test.POST_ID.ToString()).END_DATE;
            //startime early than post created time
            if (DateTime.Compare(postTime, test.START_TIME) < 0)
            {
                //endtime late than start time
                if (DateTime.Compare(test.START_TIME, test.END_TIME) < 0)
                {
                    //starttime early than now
                    if (DateTime.Compare(test.START_TIME, DateTime.Now) < 0)
                    {
                        return RedirectToAction("CreateTest", new { error = "Can't create a test with the start time has passed" });
                    }
                    //check create test
                    else if (testService.create(test))
                    {
                        //create report
                        REPORT report = new REPORT() { CREATED = test.END_TIME, PERCENT_HIGH_SCORE = 0, PERCENT_MIDDLE_SCORE = 0, PERCENT_ATTEMPT = 0, PERCENT_FAST_FINISH = 0, PERCENT_HIGH_PART_1 = 0, PERCENT_HIGH_PART_2 = 0, PERCENT_HIGH_PART_3 = 0, PERCENT_MID_PART_1 = 0, PERCENT_MID_PART_2 = 0, PERCENT_MID_PART_3 = 0 };
                        ReportService reportService = new ReportService();
                        if (reportService.create(report))
                        {
                            return RedirectToAction("ShowTests");

                        }
                        else return RedirectToAction("CreateTest", new { error = "Can't Create Report. Please create again" });
                    }
                    else return RedirectToAction("CreateTest", new { error = "Missing Questions. Please create again" });
                }
                else return RedirectToAction("CreateTest", new { error = "The End Time must after the Start Time" });
            }
            else return RedirectToAction("CreateTest", new { error = "The Start time must after the Post end time" });



        }

        public ActionResult ShowTests(string error)
        {
            TestService testService = new TestService();
            ViewBag.TestList = testService.findAll();
            ViewBag.error = error;
            return View();
        }

        //get method
        public ActionResult UpdateTest(string testId)
        {
            TestService testService = new TestService();
            TEST test = testService.findById(testId);
            //a if to make sure no one changes the test after they finished
            if (DateTime.Compare(test.END_TIME, DateTime.Now) < 0)
            {
                return RedirectToAction("ShowTests", "Manager", new { error = "Can't update Test that passed the End time" });
            }
            else
            {
                if (DateTime.Compare(test.START_TIME, DateTime.Now) < 0)
                {
                    return RedirectToAction("ShowTests", "Manager", new { error = "Can't update Test that passed the Start time" });
                }
                else
                {
                    PostService postService = new PostService();
                    ViewBag.PostList = postService.findAll();
                    TempData["Test"] = test;
                    return View();
                }
            }

        }
        //post method
        public ActionResult UpdateTestDB(TEST test)
        {
            TestService testService = new TestService();
            if ((DateTime.Compare(test.START_TIME, test.END_TIME) < 0) && (DateTime.Compare(DateTime.Now, test.START_TIME) < 0))
            {
                if (testService.update(test))
                {
                    return RedirectToAction("ShowTests", "Manager");
                }
            }
            
            return RedirectToAction("ShowTests", "Manager");
        }

        public ActionResult CreateQA(string error)
        {
            ViewBag.error = error;
            return View();
        }

        //post method
        public ActionResult CreateQADB(string CATEGORY, int POINT, string QUESTION_TEXT, string ANSWER1, string ANSWER2, string ANSWER3, string ANSWER4)
        {
            QUESTION question = new QUESTION();
            QuestionService questionService = new QuestionService();
            question.CATEGORY = CATEGORY;
            question.POINT = POINT;
            question.QUESTION1 = QUESTION_TEXT;
            if (questionService.create(question))
            {
                ANSWER answer = new ANSWER();
                AnswerService answerService = new AnswerService();
                string[] ans = new string[4] { ANSWER1, ANSWER2, ANSWER3, ANSWER4 };
                for (int i = 0; i < 4; i++)
                {
                    answer.QUESTION_ID = questionService.GetLastQuestionId();
                    answer.ANSWER1 = ans[i];
                    if (i == 0) { answer.STATUS = true; } else answer.STATUS = false;
                    if (answerService.create(answer) == false)
                    {
                        return RedirectToAction("CreateQA", "Manager", new { error = "Something wrong with the Answers please create it again" });
                    }
                }
                return RedirectToAction("CreateQA", "Manager", new { error = "Create Q&A successfully" });
            }
            else return RedirectToAction("CreateQA", "Manager", new { error = "Something wrong with the Question" });
        }
        
        public ActionResult ShowQuestions()
        {
            QuestionService questionService = new QuestionService();
            ViewBag.QuestionList = questionService.findAll();
            return View();
        }

        public ActionResult ShowAnswers(string questionId)
        {
            AnswerService answerService = new AnswerService();
            ViewBag.AnswerList = answerService.findByQuestionId(questionId);
            return View();
        }

        public ActionResult UpdateQuestion(string questionId)
        {
            QuestionService questionService = new QuestionService();
            TempData["Question"] = questionService.findById(questionId);
            return View();
        }

        public ActionResult UpdateQuestionDB(QUESTION question)
        {
            QuestionService questionService = new QuestionService();
            if (questionService.update(question))
            {
                return RedirectToAction("ShowQuestions", "Manager");
            }
            else return RedirectToAction("ShowQuestions", "Manager");
        }
        //delete question have to delete answers first
        public ActionResult DeleteQuestion(string questionId)
        {
            AnswerService answerService = new AnswerService();
            QuestionService questionService = new QuestionService();
            try
            {
                //delete answers
                foreach (var answer in answerService.findByQuestionId(questionId))
                {
                    if (answerService.deleteById(answer.ANSWER_ID.ToString()) == false)
                    {
                        return RedirectToAction("ShowQuestions", "Manager");
                    }
                }
                if (questionService.deleteById(questionId))
                {
                    return RedirectToAction("ShowQuestions", "Manager");
                }
                else throw new Exception();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
        //get method
        public ActionResult UpdateAnswer(string answerId)
        {
            AnswerService answerService = new AnswerService();
            QuestionService questionService = new QuestionService();
            ANSWER answer = answerService.findById(answerId);
            TempData["Answer"] = answer;
            TempData["Question"] = questionService.findById(answer.QUESTION_ID.ToString());
            return View();
        }
        //post method
        public ActionResult UpdateAnswerDB(ANSWER answer)
        {
            AnswerService answerService = new AnswerService();
            if (answerService.update(answer))
            {
                return RedirectToAction("ShowAnswers", "Manager", new { questionId = answer.QUESTION_ID });
            }
            else return RedirectToAction("ShowAnswers", "Manager", new { questionId = answer.QUESTION_ID });
        }

        public ActionResult ShowCandidates(string role)
        {
            UserService userService = new UserService();
            ViewBag.CandidateList = userService.findByRole(role);
            return View();
        }

        //delete USR
        public ActionResult DeleteCandidate(string username)
        {
            UserService userService = new UserService();
            if (userService.deleteById(username))
            {
                return RedirectToAction("ShowCandidates", "Manager" , new { role = "CANDIDATE"});
            }
            else return RedirectToAction("ShowCandidates", "Manager");

        }

        //Auto link 15 questions with difficulty to the choosen test
        public ActionResult GenerateQuestion(int testId)
        {
            TestService testService = new TestService();
            TEST test = testService.findById(testId.ToString());
            if (testService.addQuestions(testId) == false)
            { 
                return RedirectToAction("ShowTests", "Manager");
            }
            return RedirectToAction("ShowTests", "Manager");
        }

        public ActionResult ShowTestQuestions(string testId)
        {
            QuestionService questionService = new QuestionService();
            ViewBag.QuestionList = questionService.findByTestId(testId);
            TestService testService = new TestService();
            DateTime testDate = testService.findById(testId).START_TIME;
            if (DateTime.Compare(DateTime.Now, testDate) < 0)
            {
                ViewBag.testDate = true;
            }
            ViewBag.testId = testId;
            return View();

        }
        //update with the result of the test. Check all again then assign to the report
        public ActionResult UpdateReport(DateTime created)
        {
            ReportService reportService = new ReportService();
            ResultService resultService = new ResultService();
            TestService testService = new TestService();
            //find test
            int testId = reportService.findTestIdWithCreated(created);
            //get each sum scores
            int GK_score = testService.GetScoreFromTestAndCategory(testId, "GENERAL KNOWLEDGE");
            int M_score = testService.GetScoreFromTestAndCategory(testId, "MATHEMATICS");
            int CT_score = testService.GetScoreFromTestAndCategory(testId, "COMPUTER TECHNOLOGY");
            int sum_score = testService.GetScoreFromTestAndCategory(testId, "none");
            //list result of candidates took the test
            List<RESULT> results = resultService.FindResultByTestId(testId);
            //assign 0
            int attemp = 0, sum_high = 0, sum_middle = 0, GK_high = 0, GK_middle = 0, M_high = 0, M_middle = 0, CT_high = 0, CT_middle = 0;
            foreach (var r in results)
            {
                //didnt come to the test
                if (r.TEST_INDEX != 0)
                {
                    //Count then sum/count = average
                    attemp = attemp + 1;
                    if ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) >= ((double)sum_score * 80 / 100))
                    {
                        sum_high = sum_high + 1;
                    }
                    else if ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) >= ((double)sum_score * 50 / 100))
                    {
                        sum_middle = sum_middle + 1;
                    }
                    //GK 
                    if (r.TEST_RESULT_1 >= ((double)GK_score * 80 / 100))
                    {
                        GK_high = GK_high + 1;
                    }
                    else if (r.TEST_RESULT_1 >= ((double)GK_score * 50 / 100))
                    {
                        GK_middle = GK_middle + 1;
                    }
                    //M
                    if (r.TEST_RESULT_2 >= ((double)M_score * 80 / 100))
                    {
                        M_high = M_high + 1;
                    }
                    else if (r.TEST_RESULT_2 >= ((double)M_score * 50 / 100))
                    {
                        M_middle = M_middle + 1;
                    }
                    //CT
                    if (r.TEST_RESULT_3 >= ((double)CT_score * 80 / 100))
                    {
                        CT_high = CT_high + 1;
                    }
                    else if (r.TEST_RESULT_3 >= ((double)CT_score * 50 / 100))
                    {
                        CT_middle = CT_middle + 1;
                    }
                }
            }
            int quantity = results.Count();
            double resu = (double)sum_middle / quantity;
            REPORT report = new REPORT()
            {
                //Average
                CREATED = created,
                PERCENT_HIGH_SCORE = ((double)sum_high / quantity) * 100,
                PERCENT_MIDDLE_SCORE = ((double)sum_middle / quantity) * 100,
                PERCENT_ATTEMPT = attemp,
                PERCENT_FAST_FINISH = 0,
                PERCENT_HIGH_PART_1 = ((double)GK_high / quantity) * 100,
                PERCENT_HIGH_PART_2 = ((double)M_high / quantity) * 100,
                PERCENT_HIGH_PART_3 = ((double)CT_high / quantity) * 100,
                PERCENT_MID_PART_1 = ((double)GK_middle / quantity) * 100,
                PERCENT_MID_PART_2 = ((double)M_middle / quantity) * 100,
                PERCENT_MID_PART_3 = ((double)CT_middle / quantity) * 100
            };
            if (reportService.update(report))
            {
                return RedirectToAction("Index", "Manager");
            }
            else return RedirectToAction("Index", "Manager");

        }
        //get all report within month
        public ActionResult ReportMonth(DateTime month)
        {
            ReportService reportService = new ReportService();
            List<REPORT> reports = reportService.findReportWithMonth(month);
            ViewBag.type = "month";
            ViewBag.month = month.Month;
            ViewBag.ReportList = reports;
            return View("ReportFilter");
        }
        //get the avergage of each month of the year
        public ActionResult ReportYear(string YEAR)
        {
            string year = YEAR + "/01/01";
            ReportService reportService = new ReportService();
            DateTime y = DateTime.Parse(year);
            List<REPORT> reports = reportService.findReportWithYear(y);
            ViewBag.type = "year";
            ViewBag.year = YEAR;
            ViewBag.ReportList = reports;
            return View("ReportFilter");
        }
        //Viwe apps with conditions(approved , unapproved  / / /)
        public ActionResult ShowApplicationsWithCondition(string status)
        {
            ReportService reportService = new ReportService();
            int testId = Convert.ToInt32(Session["TestId"]);
            ViewBag.ApplicationList = reportService.findApplicationWithResult(testId, status);
            return View("ShowApplications");
        }
        //anyone pass 80* can go to HR
        public ActionResult ShowStatusCandidates(int status)
        {
            ApplicationService applicationService = new ApplicationService();
            switch (status)
            {
                case -2:
                    ViewBag.ApplicationList = applicationService.findCustomApplicationsWithCondition(-2);
                    return View();
                case 2:
                    ViewBag.ApplicationList = applicationService.findCustomApplicationsWithCondition(2);
                    return View();
                default:
                    ViewBag.ApplicationList = applicationService.findAllCustomApplications(); ;
                    return View();

            }


        }
        //remove 1 req question the test has
        public ActionResult RemoveQuestion(int questionId, int testId)
        {
            TestService testService = new TestService();
            if (testService.RemoveQuestionFromTest(questionId, testId))
            {
                return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
            }
            else return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });



        }
        //create 1 single question to the test
        public ActionResult CreateQAToTest(string error, string testId)
        {
            ViewBag.testId = testId;
            return View();
        }

        //post meethod
        public ActionResult CreateQAToTestDB(string CATEGORY, int POINT, string QUESTION_TEXT, string testId, string ANSWER1, string ANSWER2, string ANSWER3, string ANSWER4)
        {
            QUESTION question = new QUESTION();
            QuestionService questionService = new QuestionService();
            question.CATEGORY = CATEGORY;
            question.POINT = POINT;
            question.QUESTION1 = QUESTION_TEXT;
            if (questionService.create(question))
            {
                ANSWER answer = new ANSWER();
                AnswerService answerService = new AnswerService();
                string[] ans = new string[4] { ANSWER1, ANSWER2, ANSWER3, ANSWER4 };
                for (int i = 0; i < 4; i++)
                {
                    answer.QUESTION_ID = questionService.GetLastQuestionId();
                    answer.ANSWER1 = ans[i];
                    if (i == 0) { answer.STATUS = true; } else answer.STATUS = false;
                    if (answerService.create(answer) == false)
                    {
                        return RedirectToAction("CreateQAToTest", "Manager", new { error = "Something wrong with the Answers please create it again", testId = testId });
                    }
                }
                TestService testService = new TestService();
                if (testService.AddQuestionToTest(questionService.GetLastQuestionId(), Int32.Parse(testId)))
                {
                    return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
                }
                return RedirectToAction("CreateQAToTest", "Manager", new { error = "Create Q&A successfully", testId = testId });
            }
            else return RedirectToAction("CreateQAToTest", "Manager", new { error = "Something wrong with the Question", testId = testId });
        }
        //eremove all questions
        public ActionResult RemoveQuestions(int testId)
        {
            TestService testService = new TestService();
            if (testService.RemoveQuestionsFromTest(testId))
            {
                return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
            }
            else return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
        }
        //add random 15 questions
        public ActionResult AddQuestions(int testId)
        {
            TestService testService = new TestService();
            if (testService.addQuestions(testId))
            {
                return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
            }
            else return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
        }

        //admin wants something particular
        public ActionResult AddExistedQuestion(int testId)
        {
            QuestionService questionService = new QuestionService();
            List<QUESTION> questions = questionService.GetQuestionsExceptTestQ(testId);
            ViewBag.QuestionList = questions;
            ViewBag.testId = testId;
            return View();
        }
        //add not existed question 
        public ActionResult AddExistedQuestionToTest(int questionId, int testId)
        {
            QuestionService questionService = new QuestionService();
            TestService testService = new TestService();
            if (testService.AddQuestionToTest(questionId, testId))
            { 
                return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
            } else
            return RedirectToAction("ShowTestQuestions", "Manager", new { testId = testId });
        }
        //change mail
        public ActionResult ChangeMail()
        {
            AptechSem3.Service.MailService service = new Service.MailService();
            var task = Task.Run(() => service.changeMail());
            if (task.Wait(TimeSpan.FromSeconds(60)))
                return RedirectToAction("Index", "Manager");
            else
                return RedirectToAction("Index", "Manager");
        }
    }
}