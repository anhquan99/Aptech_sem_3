using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Models
{
    public class QuestionAndAnswer
    {
        public int QuestionId;
        public String Question;
        public List<String> Answer;
        public String Selected;
        public QuestionAndAnswer(int QuestionId, String Question, List<String> Answer)
        {
            this.QuestionId = QuestionId;
            this.Question = Question;
            this.Answer = Answer;
        }
        public QuestionAndAnswer(int QuestionId)
        {
            APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
            try
            {
                var Question = (from p in db.QUESTIONs where p.QUESTION_ID == QuestionId select p).Single();
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