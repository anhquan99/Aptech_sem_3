using System;
using AptechSem3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class QuestionService : IService<QUESTION>
    {
        public bool create(QUESTION t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.QUESTIONs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public bool deleteById(string questionId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(questionId);

                    var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == id select p).SingleOrDefault();
                    if (selectedQuestion != null)
                    {
                        db.QUESTIONs.Remove(selectedQuestion);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new NotImplementedException();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public List<QUESTION> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.QUESTIONs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public QUESTION findById(string questionId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(questionId);

                    var selectedQuestion = (from p in db.QUESTIONs where p.QUESTION_ID == id select p).SingleOrDefault();
                    return selectedQuestion;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public bool update(QUESTION t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedQuestion = (from p in db.QUESTIONs
                                            where p.QUESTION_ID == t.QUESTION_ID
                                            select p).SingleOrDefault();

                    if (selectedQuestion != null)
                    {
                        selectedQuestion.CATEGORY = t.CATEGORY;
                        selectedQuestion.QUESTION1 = t.QUESTION1;
                        selectedQuestion.POINT = t.POINT;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        //Hien thi cau hoi trong bai test
        public List<QUESTION> findByTestId(string testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(testId);
                    List<QUESTION> list = new List<QUESTION>();
                    foreach (var i in db.FIND_QUESTION_WITH_TEST(id))
                    {
                        QUESTION question = this.findById(i.QUESTION_ID.ToString());
                        list.Add(question);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public int GetLastQuestionId()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedQuestion = (from p in db.GET_LAST_QUESTION() select p).SingleOrDefault();
                    return selectedQuestion.QUESTION_ID;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}