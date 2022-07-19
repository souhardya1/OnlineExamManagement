using OnlineExamManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            try
            {
                var stud = db.Students.Where(x => x.Id == id).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine("Inside try block");
                if (stud != null)
                {
                    if (stud.Marks1 == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inside if marks1");
                        stud.Marks1 = Marks.Obtained;
                        stud.Total += Marks.Obtained;
                        db.SaveChanges();
                    }
                    else if (stud.Marks2 == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inside if marks2");
                        stud.Marks2 = Marks.Obtained;
                        stud.Total += Marks.Obtained;
                        db.SaveChanges();
                    }
                    else if (stud.Marks3 == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inside if marks3");
                        stud.Marks3 = Marks.Obtained;
                        stud.Total += Marks.Obtained;
                        db.SaveChanges();
                    }
                    else if (stud.Marks4 == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inside if marks4");
                        System.Diagnostics.Debug.WriteLine(stud.Password);

                        stud.Password = stud.Password;
                        stud.Marks4 = Marks.Obtained;
                        stud.Total += Marks.Obtained;
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
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                    }
                }
                return null;
            }
            
        }
    }
}
