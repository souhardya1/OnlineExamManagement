using OnlineExamManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineExamManagement.Controllers
{
    public class StudentApiController : ApiController
    {
        MyDbContext db = new MyDbContext();

        [HttpGet]
        public IHttpActionResult GetStudentDetail()
        {
            var studentlst = db.Students.ToList();

            return Ok(studentlst);
        }

        [HttpGet]
        public IHttpActionResult GetStudentDetail(int id)
        {
            var studentDetail = db.Students.Where(x => x.Id == id).FirstOrDefault();
            if(studentDetail != null)
            {
                return Ok(studentDetail);
            }
            else
            {
                return NotFound();
            }

            
        }

        [HttpPost]
        public IHttpActionResult StudentInsert(Student s)
        {
            db.Students.Add(s);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditStudent(int id)
        {
            var stud = db.Students.Where(x => x.Id ==id).FirstOrDefault();
            if (stud != null)
            {
                if (stud.Marks1 == null)
                {
                    stud.Marks1 = Marks.Obtained;
                    db.SaveChanges();
                }
                else if (stud.Marks2 == null)
                {
                    stud.Marks2 = Marks.Obtained;
                    db.SaveChanges();
                }
                else if (stud.Marks3 == null)
                {
                    stud.Marks3 = Marks.Obtained;
                    db.SaveChanges();
                }
                else if (stud.Marks4 == null)
                {
                    stud.Marks4 = Marks.Obtained;
                    db.SaveChanges();
                }
                else
                {
                    return BadRequest();
                }
                return Ok();

            }
            else
            {
                return NotFound();
            } 
        }
    }
}
