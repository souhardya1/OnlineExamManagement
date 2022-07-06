using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineExamManagement.Models;
namespace OnlineExamManagement.Controllers
{
    public class TeacherQuestionApiController : ApiController
    {
        MyDbContext db = new MyDbContext();

        [HttpGet]
        public IHttpActionResult GetQuestions()
        {
            var queslst = db.Questions.ToList();
            return Ok(queslst);
        }

        [HttpGet]
        public IHttpActionResult GetQuestions(int id)
        {

            var quesrow = db.Questions.Where(x => x.Id == id).FirstOrDefault();
            if (quesrow != null)
            {
                return Ok(quesrow);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateQuestion(Question q)
        {
            db.Questions.Add(q);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditQuestion(Question q)
        {
            var quesrow = db.Questions.Where(x => x.Id == q.Id).FirstOrDefault();
            if (quesrow != null)
            {
                quesrow.Id = q.Id;
                quesrow.CourseRefId = q.CourseRefId;
                quesrow.Ques = q.Ques;
                quesrow.OptionA = q.OptionA;
                quesrow.OptionB = q.OptionB;
                quesrow.OptionC = q.OptionC;
                quesrow.OptionD = q.OptionD;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete]
        public IHttpActionResult DeleteQuestion(int id)
        {
            var quesrow = db.Questions.Where(x => x.Id == id).FirstOrDefault();
            if (quesrow != null)
            {
                db.Entry(quesrow).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
