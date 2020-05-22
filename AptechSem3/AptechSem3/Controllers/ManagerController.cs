using System;
using System.Collections.Generic;
using System.Linq;
using AptechSem3.Service.ModelService;
using AptechSem3.Models;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace AptechSem3.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            ReportService reportService = new ReportService();
            ViewBag.ReportList = reportService.findAll();
            return View();
        }

        public ActionResult ReportDetail(DateTime created)
        {
            ReportService reportService = new ReportService();
            TempData["Report"] = reportService.findByCreated(created);
            return View();
        }
        public ActionResult CreatePost(string error)
        {
            ViewBag.Error = error;
            return View();
        }

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
        public ActionResult ShowPosts()
        {
            PostService postService = new PostService();
            ViewBag.PostList = postService.findAll();
            return View();
        }
        public ActionResult ShowApplications(int status, string error)
        {
            ApplicationService applicationService = new ApplicationService();
            ViewBag.ApplicationList = applicationService.findByStatus(status);
            ViewBag.error = error;
            return View();
        }
        public ActionResult ApproveApplication(string applyId, int status)
        {
            ApplicationService applicationService = new ApplicationService();
            JOB_APPLICATION apply = new JOB_APPLICATION();
            apply = applicationService.findById(applyId);
            string created = DateTime.Now.ToString("ddMMyyyyHHmmss");
            UserService userService = new UserService();
            if (userService.findByApplyId(applyId))
            {
                return RedirectToAction("ShowApplications", "Manager", new { status = 0, error = "User has Existed" });
            }
            if (apply.APPROVE_STATUS == status)
            {
                return RedirectToAction("ShowApplications", "Manager", new { status = 0, error = "Can't proceed the same Status" });
            }
            else
            if (status == 1)
            {

                applicationService.apply(applyId, 1);
                string username = "user" + created;
                string decrypt = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(created)).Select(s => s.ToString("x2")));
                string password = decrypt.Substring(2, 10);
                USR user = new USR();
                user.APPLY_ID = Int32.Parse(applyId);
                user.ROLE = "CANDIDATE";
                user.USERNAME = username;
                user.PASSWORD = password;
                if (userService.create(user))
                {
                    return RedirectToAction("ShowApplications", "Manager", new { status = 1 });
                }
            }
            else applicationService.apply(applyId, -1);
            return RedirectToAction("ShowApplications", "Manager", new { status = 0 });
        }
        public ActionResult ApplicationDetail(string applyId)
        {
            ApplicationService applicationService = new ApplicationService();
            TempData["Application"] = applicationService.findById(applyId);
            return View();
        }
        public ActionResult CreateTest(string error)
        {
            ViewBag.error = error;
            PostService postService = new PostService();
            ViewBag.PostList = postService.findAll();
            return View();
        }

        public ActionResult CreateTestDB(TEST test)
        {
            TestService testService = new TestService();

            if (DateTime.Compare(test.START_TIME, test.END_TIME) < 0)
            {
                if (DateTime.Compare(test.START_TIME, DateTime.Now) < 0)
                {
                    return RedirectToAction("CreateTest", new { error = "Can't create a test with the start time has passed" });
                }
                else if (testService.create(test))
                {
                    REPORT report = new REPORT() { CREATED = test.END_TIME, PERCENT_HIGH_SCORE = 0, PERCENT_MIDDLE_SCORE = 0, PERCENT_ATTEMPT = 0, PERCENT_FAST_FINISH = 0, PERCENT_HIGH_PART_1 = 0, PERCENT_HIGH_PART_2 = 0, PERCENT_HIGH_PART_3 = 0, PERCENT_MID_PART_1 = 0, PERCENT_MID_PART_2 = 0, PERCENT_MID_PART_3 = 0 };
                    ReportService reportService = new ReportService();
                    if (reportService.create(report))
                    {
                        return RedirectToAction("ShowTests");

                    }
                    else return RedirectToAction("CreateTest", new { error = "There's something wrong. Please create again" });
                }
                else return RedirectToAction("CreateTest", new { error = "There's something wrong. Please create again" });
            }
            else return RedirectToAction("CreateTest", new { error = "The End Time must after the Start Time" });




        }

        public ActionResult ShowTests(string error)
        {
            TestService testService = new TestService();
            ViewBag.TestList = testService.findAll();
            ViewBag.error = error;
            return View();
        }

        public ActionResult UpdateTest(string testId)
        {
            TestService testService = new TestService();
            TEST test = testService.findById(testId);
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

        public ActionResult UpdateTestDB(TEST test)
        {
            TestService testService = new TestService();
            if (testService.update(test))
            {
                return RedirectToAction("ShowTests", "Manager");
            }
            else return RedirectToAction("ShowTests", "Manager");
        }

        public ActionResult CreateQA(string error)
        {
            ViewBag.error = error;
            return View();
        }

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
        public ActionResult DeleteQuestion(string questionId)
        {
            AnswerService answerService = new AnswerService();
            QuestionService questionService = new QuestionService();
            try
            {
                /*foreach (var answer in answerService.findByQuestionId(questionId))
                {
                    if (answerService.deleteById(answer.ANSWER_ID.ToString()) == false)
                    {
                        return RedirectToAction("ShowQuestions", "Manager");
                    }
                }*/
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
        public ActionResult UpdateAnswer(string answerId)
        {
            AnswerService answerService = new AnswerService();
            QuestionService questionService = new QuestionService();
            ANSWER answer = answerService.findById(answerId);
            TempData["Answer"] = answer;
            TempData["Question"] = questionService.findById(answer.QUESTION_ID.ToString());
            return View();
        }

        public ActionResult UpdateAnswerDB(ANSWER answer)
        {
            AnswerService answerService = new AnswerService();
            if (answerService.update(answer))
            {
                return RedirectToAction("ShowAnswers", "Manager", new { questionId = answer.QUESTION_ID });
            }
            else return RedirectToAction("ShowAnswers", "Manager", new { questionId = answer.QUESTION_ID });
        }

        public ActionResult ShowCandidates()
        {
            UserService userService = new UserService();
            ViewBag.CandidateList = userService.findAll();
            return View();
        }

        public ActionResult DeleteCandidate(string username)
        {
            UserService userService = new UserService();
            if (userService.deleteById(username))
            {
                return RedirectToAction("ShowCandidates", "Manager");
            }
            else return RedirectToAction("ShowCandidates", "Manager");

        }

        public ActionResult GenerateQuestion(int testId)
        {
            TestService testService = new TestService();
            testService.addQuestions(testId);
            return RedirectToAction("ShowTests", "Manager");
        }

        public ActionResult ShowTestQuestions(string testId)
        {
            QuestionService questionService = new QuestionService();
            ViewBag.QuestionList = questionService.findByTestId(testId);
            return View();

        }

        public ActionResult UpdateReport(DateTime created)
        {
            ReportService reportService = new ReportService();
            ResultService resultService = new ResultService();
            TestService testService = new TestService();
            int testId = reportService.findTestIdWithCreated(created);
            int GK_score = testService.GetScoreFromTestAndCategory(testId, "GENERAL KNOWLEDGE");
            int M_score = testService.GetScoreFromTestAndCategory(testId, "MATHEMATICS");
            int CT_score = testService.GetScoreFromTestAndCategory(testId, "COMPUTER TECHNOLOGY");
            int sum_score = testService.GetScoreFromTestAndCategory(testId, "none");
            List<RESULT> results = resultService.FindResultByTestId(testId);
            int attemp = 0, sum_high = 0, sum_middle = 0, GK_high = 0, GK_middle = 0, M_high = 0, M_middle = 0, CT_high = 0, CT_middle = 0;
            foreach (var r in results)
            {
                if (r.TEST_INDEX != 0)
                {
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


    }
}