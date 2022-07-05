using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineExamManagement.Models;
namespace OnlineExamManagement.Controllers
{
    public class TeacherExamApiController : ApiController
    {
        MyDbContext db = new MyDbContext();

        [HttpGet]
        public IHttpActionResult GetExams()
        {
            var examlst = db.Exams.ToList();
            return Ok(examlst);
        }

        [HttpGet]
        public IHttpActionResult GetExams(int id)
        {
            var examrow = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            return Ok(examrow);
        }

        [HttpPost]
        public IHttpActionResult CreateExam(Exam e)
        {
            db.Exams.Add(e);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditExam(Exam e)
        {
            var examrow = db.Exams.Where(x => x.Id == e.Id).FirstOrDefault();
            if (examrow != null)
            {
                examrow.Id = e.Id;
                examrow.Code = e.Code;
                examrow.Date = e.Date;
                examrow.Active = e.Active;
                examrow.CourseRefId = e.CourseRefId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpDelete]
        public IHttpActionResult DeleteExam(int id)
        {
            var examrow = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            db.Entry(examrow).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Ok();
        }
    }
}
