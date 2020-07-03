using System;
using System.Collections.Generic;
using AptechSem3.Models;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AptechSem3.Service.ModelService
{
    public class TestService : IService<TEST>
    {
        public bool create(TEST t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.TESTs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    if (this.addQuestions(this.GetLastTestId()) == false)
                    {
                        throw new ExecutionEngineException();
                    }
                    
                    return true;
                }
            }
            catch (ExecutionEngineException ex)
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int tempId = this.GetLastTestId();
                    var temp = (from p in db.TESTs where p.TEST_ID == tempId select p).SingleOrDefault();
                    if(temp != null)
                    {
                        db.TESTs.Remove(temp);
                        db.SaveChanges();
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public bool deleteById(string testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(testId);
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == id select p).SingleOrDefault();
                    if (selectedTest != null)
                    {
                        db.TESTs.Remove(selectedTest);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new NotImplementedException();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public List<TEST> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.TESTs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public TEST findById(string testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(testId);

                    var selectedTest = (from p in db.TESTs where p.TEST_ID == id select p).SingleOrDefault();
                    return selectedTest;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool update(TEST t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedTest = (from p in db.TESTs
                                        where p.TEST_ID == t.TEST_ID
                                        select p).SingleOrDefault();

                    if (selectedTest != null)
                    {
                        selectedTest.POST_ID = t.POST_ID;
                        selectedTest.START_TIME = t.START_TIME;
                        selectedTest.END_TIME = t.END_TIME;
                        selectedTest.TEST_NAME = t.TEST_NAME;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public List<QUESTION> getQfromT(int id)
        {
            try
            {
                List<QUESTION> questions = new List<QUESTION>();
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())

                {
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == id select p).SingleOrDefault();
                    if (selectedTest == null) throw new Exception();
                    foreach (var q in selectedTest.QUESTIONs)
                    {
                        questions.Add(q);
                    }
                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool addQuestions(int testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    QuestionService questionService = new QuestionService();
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == testId select p).SingleOrDefault();
                    if (selectedTest == null) return false;
                    var Q_GK_5 = (from p in db.GET_2_RANDOM_QUESTIONS("GENERAL KNOWLEDGE", 5) select p).ToList();
                    var Q_GK_10 = (from p in db.GET_2_RANDOM_QUESTIONS("GENERAL KNOWLEDGE", 10) select p).ToList();
                    var Q_GK_15 = (from p in db.GET_1_RANDOM_QUESTION("GENERAL KNOWLEDGE", 15) select p).ToList();
                    var Q_M_5 = (from p in db.GET_2_RANDOM_QUESTIONS("MATHEMATICS", 5) select p).ToList();
                    var Q_M_10 = (from p in db.GET_2_RANDOM_QUESTIONS("MATHEMATICS", 10) select p).ToList();
                    var Q_M_15 = (from p in db.GET_1_RANDOM_QUESTION("MATHEMATICS", 15) select p).ToList();
                    var Q_CT_5 = (from p in db.GET_2_RANDOM_QUESTIONS("COMPUTER TECHNOLOGY", 5) select p).ToList();
                    var Q_CT_10 = (from p in db.GET_2_RANDOM_QUESTIONS("COMPUTER TECHNOLOGY", 10) select p).ToList();
                    var Q_CT_15 = (from p in db.GET_1_RANDOM_QUESTION("COMPUTER TECHNOLOGY", 15) select p).ToList();
                    if ((Q_GK_5.Count == 2) && (Q_GK_10.Count == 2) && (Q_GK_15.Count == 1) && (Q_M_5.Count == 2) && (Q_M_10.Count == 2) && (Q_M_15.Count == 1) && (Q_CT_5.Count == 2) && (Q_CT_10.Count == 2) && (Q_CT_15.Count == 1))
                    {

                        foreach (var q in Q_GK_5)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);

                        }
                        if (db.SaveChanges() == 0) throw new Exception();

                        foreach (var q in Q_GK_10)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_GK_15)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_M_5)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_M_10)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_M_15)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_CT_5)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_CT_10)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                        foreach (var q in Q_CT_15)
                        {
                            var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == q.QUESTION_ID select p).SingleOrDefault();
                            selectedTest.QUESTIONs.Add(selectedQuestion);
                        }
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetLastTestId()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedTest = (from p in db.GET_LAST_TEST() select p).SingleOrDefault();
                    return selectedTest.TEST_ID;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }


        public int GetScoreFromTestAndCategory(int testId, string category)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int score = 0;
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == testId select p).SingleOrDefault();
                    foreach (var q in selectedTest.QUESTIONs)
                    {
                        if ((q.CATEGORY == category) || (category == "none"))
                        {
                            score = score + q.POINT;
                        }
                    }
                    return score;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool RemoveQuestionFromTest(int questionId, int testId)
        {

            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == testId select p).SingleOrDefault();
                    foreach (var q in selectedTest.QUESTIONs)
                    {
                        if (q.QUESTION_ID == questionId) 
                        {
                            selectedTest.QUESTIONs.Remove(q);
                            if (db.SaveChanges() == 0) throw new Exception();
                            else return true;
                        }

                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool AddQuestionToTest(int questionId, int testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == testId select p).SingleOrDefault();
                    QuestionService questionService = new QuestionService();
                    var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == questionId select p).SingleOrDefault();
                    selectedTest.QUESTIONs.Add(selectedQuestion);
                    if (db.SaveChanges() == 0) throw new Exception();
                    else return true;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool RemoveQuestionsFromTest(int testId)
        {

            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedTest = (from p in db.TESTs where p.TEST_ID == testId select p).SingleOrDefault();
                    List<QUESTION> questions = selectedTest.QUESTIONs.ToList();
                    foreach (var q in questions)
                    {
                            selectedTest.QUESTIONs.Remove(q);
                            if (db.SaveChanges() == 0) throw new Exception();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public List<TEST> findByPost(int postId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.TESTs where p.POST_ID == postId select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public TEST findByPostSingle(int postId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.TESTs where p.POST_ID == postId select p).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}