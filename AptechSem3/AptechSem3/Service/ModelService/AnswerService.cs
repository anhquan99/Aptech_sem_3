using System;
using AptechSem3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class AnswerService : IService<ANSWER>
    {
        public bool create(ANSWER t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.ANSWERs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool deleteById(string answerId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(answerId);

                    var selectedAnswer = (from p in db.ANSWERs where p.ANSWER_ID == id select p).SingleOrDefault();
                    if (selectedAnswer != null)
                    {
                        db.ANSWERs.Remove(selectedAnswer);
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

        public List<ANSWER> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.ANSWERs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public ANSWER findById(string answerId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(answerId);
                    var selectedAnswer = (from p in db.ANSWERs where p.ANSWER_ID == id select p).SingleOrDefault();
                    return selectedAnswer;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool update(ANSWER t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedAnswer = (from p in db.ANSWERs
                                          where p.ANSWER_ID == t.ANSWER_ID
                                          select p).SingleOrDefault();

                    if (selectedAnswer != null)
                    {
                        selectedAnswer.QUESTION_ID = t.QUESTION_ID;
                        selectedAnswer.ANSWER1 = t.ANSWER1;
                        selectedAnswer.STATUS = t.STATUS;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public List<ANSWER> findByQuestionId(string questionId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(questionId);

                    return (from p in db.ANSWERs where p.QUESTION_ID == id select p).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}