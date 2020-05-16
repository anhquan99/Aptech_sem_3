using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Models
{
    public class TestQuestion
    {
        public int QuestionId;
        public String Question;
        public List<String> Answer;
        public TestQuestion(int QuestionId, String Question, List<String> Answer)
        {
            this.QuestionId = QuestionId;
            this.Question = Question;
        }
        public TestQuestion(int QuestionId)
        {
            APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
            try
            {
                this.Answer = new List<string>();
                var Question = (from p in db.QUESTIONs where p.QUESTION_ID == QuestionId  select p).Single();
                this.QuestionId = Question.QUESTION_ID;
                this.Question = Question.QUESTION1;
                foreach (var i in Question.ANSWERs)
                {
                    this.Answer.Add(i.ANSWER1);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}