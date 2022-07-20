using OnlineExamManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineExamManagement.Controllers
{
    public class QuestionApiController : ApiController
    {
        MyDbContext db = new MyDbContext();

        [HttpGet]

        public IHttpActionResult GetQuestions(int id)
        {
            var examQuestions = db.Questions.Where(x => x.Course.Id  == id).ToList();
            return Ok(examQuestions);
        }
    }
}
