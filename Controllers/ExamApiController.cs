using OnlineExamManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineExamManagement.Controllers
{
    public class ExamApiController : ApiController
    {
        MyDbContext db = new MyDbContext();

        [HttpGet]
        public IHttpActionResult GetExams()
        {
            List<Exam> exams = db.Exams.ToList();
            return Ok(exams);
        }
    }
}
