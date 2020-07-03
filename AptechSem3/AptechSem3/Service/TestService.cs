using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class TestService
    {
        public static List<TestQuestion> getQuestionByUsernameAndIndex(String username, int index)
        {
            APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
            try
            {
                List<TestQuestion> returnList = new List<TestQuestion>();
                String category = "";
                if (index == 1)
                {
                    category = "GENERAL KNOWLEDGE";
                }
                else if (index == 2)
                {
                    category = "MATHEMATICS";
                }
                else if (index == 3)
                {
                    category = "COMPUTER TECHNOLOGY";
                }

                foreach (var i in db.FIND_QID_FROM_USERNAME(username))
                {
                    int result = i ?? 0;
                    //throw if result == 0 cause i can not be null or == 0
                    if (result == 0) throw new Exception("CAN NOT FOUND QUESTION ID");
                    //get question
                    int QuestionID = new int();
                    try
                    {
                        QuestionID = (from p in db.QUESTIONs where p.QUESTION_ID == i && p.CATEGORY == category select p).SingleOrDefault().QUESTION_ID;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    // if question not in index type continue
                    if (QuestionID == 0) continue;
                    returnList.Add(new TestQuestion(QuestionID));
                }
                
                return returnList;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static int getIndexByApplyId(int apply_id)
        {
            try
            {
                APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
                return (from p in db.RESULTs where p.APPLY_ID == apply_id select p).Single().TEST_INDEX;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool updateIndexByApplyId(int apply_id, int index)
        {
            try
            {
                APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
                RESULT result = (from p in db.RESULTs where p.APPLY_ID == apply_id select p).SingleOrDefault();
                result.TEST_INDEX = index;
                if (db.SaveChanges() == 0) throw new Exception("CAN NOT UPDATE RESULT");
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static double calculateScore(List<String> QuestionIds, List<String> AnswerLists,int apply_id,int index)
        {
            APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
            bool flag = false;
            try
            {
                RESULT result = (from p in db.RESULTs where p.APPLY_ID == apply_id select p).SingleOrDefault();
                for(int i = 0; i< QuestionIds.Count; i++)
                {
                    int parseQuestionId = int.Parse(QuestionIds[i]);
                    ANSWER answer = (from p in db.ANSWERs where p.QUESTION_ID == parseQuestionId && p.STATUS == true select p).SingleOrDefault();
                    if(answer.ANSWER1 == AnswerLists[i])
                    {
                        switch (index)
                        {
                            case 1:
                                flag = true;
                                result.TEST_RESULT_1 += answer.QUESTION.POINT;
                                break;
                            case 2:
                                flag = true;
                                result.TEST_RESULT_2 += answer.QUESTION.POINT;
                                break;
                            case 3:
                                flag = true;
                                result.TEST_RESULT_3 += answer.QUESTION.POINT;
                                break;
                        }
                    }
                }
                if(db.SaveChanges() == 0 && flag == true) throw new Exception("CAN NOT UPDATE RESULT");
                return result.TEST_RESULT_1 + result.TEST_RESULT_2 + result.TEST_RESULT_3;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static DateTime getStartTimeByApplyId(int apply_id)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var jobPost = ((from p in db.JOB_APPLICATION where p.APPLY_ID == apply_id select p).SingleOrDefault()).JOB_POST;
                    var Test = (from p in db.TESTs where p.POST_ID == jobPost.POST_ID select p).SingleOrDefault();
                    return Test.START_TIME;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static DateTime getEndTimeByApplyId(int apply_id)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var jobPost = ((from p in db.JOB_APPLICATION where p.APPLY_ID == apply_id select p).SingleOrDefault()).JOB_POST;
                    var Test = (from p in db.TESTs where p.POST_ID == jobPost.POST_ID select p).SingleOrDefault();
                    return Test.END_TIME;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static TEST getTestByApplyId(int apply_id)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var jobPost = ((from p in db.JOB_APPLICATION where p.APPLY_ID == apply_id select p).SingleOrDefault()).JOB_POST;
                    var Test = (from p in db.TESTs where p.POST_ID == jobPost.POST_ID select p).SingleOrDefault();
                    return Test;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}