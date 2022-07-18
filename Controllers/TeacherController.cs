using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamManagement.Models;
using System.Web.Security;
using System.Net.Http;

namespace OnlineExamManagement.Controllers
{
    [HandleError]
    [NoDirectAccess]
    public class TeacherController : Controller
    {
        MyDbContext db = new MyDbContext();
        // GET: Teacher
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Teacher t)
        {
            try
            {
                if (IsValid(t))
                {
                    FormsAuthentication.SetAuthCookie(t.Email.ToString(), false);
                    Session["TeacherEmail"] = t.Email.ToString();

                    Teacher teac = db.Teachers.Where(x => t.Email == t.Email).FirstOrDefault();


                    Session["userId"] = teac.Id;

                    //System.Diagnostics.Debug.WriteLine(teac.Id);
                    //System.Diagnostics.Debug.WriteLine(t.Email);

                    return RedirectToAction("LandingPage");
                }
                else
                {
                    ViewBag.ErrorMessage = "Email ID and Passwords Incorrect. please register";
                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound","Error",new { msg = e.Message });
            }
        }



        public bool IsValid(Teacher t)
        {
            var cred = db.Teachers.Where(
                        x => x.Email == t.Email && x.Password == t.Password).
                        FirstOrDefault();
            if (cred != null)
            {
                return (t.Email == cred.Email && t.Password == cred.Password);
            }

            else
            {
                return false;
            }
        }


        [Authorize]
        public ActionResult LandingPage()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewExam()
        {
            try
            {
                List<Exam> examlst = new List<Exam>();
                string url = "https://localhost:44301/api/TeacherExamApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<List<Exam>>();
                    display.Wait();
                    examlst = display.Result;
                    return View(examlst);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch Exams" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }             
        }


        [Authorize]
        public ActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateExam(Exam e)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherExamApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.PostAsJsonAsync<Exam>(url, e);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Exam Created Successfully";
                    return RedirectToAction("ViewExam");
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to Create Exam" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = ex.Message });
            }         
        }


        [Authorize]
        public ActionResult EditExam(int id)
        {

            try
            {
                Exam e = null; ;
                string url = "https://localhost:44301/api/TeacherExamApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Exam>();
                    display.Wait();
                    e = display.Result;
                    return View(e);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch Exam with Id" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }       
        }


        [HttpPost]
        public ActionResult EditExam(Exam e)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherExamApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.PutAsJsonAsync<Exam>(url, e);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Exam Edited Successfully";
                    return RedirectToAction("ViewExam");
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to Edit Exam with Id" });
                }
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = ex.Message });
            }          
        }


        [Authorize]
        public ActionResult DeleteExam(int id)
        {
            try
            {
                Exam e = null; ;
                string url = "https://localhost:44301/api/TeacherExamApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Exam>();
                    display.Wait();
                    e = display.Result;
                    return View(e);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to find Exam with Id" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }
        }
            
        

        [HttpPost,ActionName("DeleteExam")]
        public ActionResult DeleteExamConfirmed(int id)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherExamApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.DeleteAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Exam Deleted Successfully";
                    return RedirectToAction("ViewExam");
                }

                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to Delete Exam" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }          
        }




        // Question Part
        [Authorize]
        public ActionResult ViewQuestion()
        {
            try
            {
                List<Question> queslst = new List<Question>();
                string url = "https://localhost:44301/api/TeacherQuestionApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<List<Question>>();
                    display.Wait();
                    queslst = display.Result;
                    return View(queslst);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch Quesion" });
                }           
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }
        }

        [Authorize]
        public ActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestion(Question q)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherQuestionApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.PostAsJsonAsync<Question>(url, q);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Question Created Successfully";
                    return RedirectToAction("ViewQuestion");
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to create Question" }); 
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }
        }


        [Authorize]
        public ActionResult EditQuestion(int id)
        {
            try
            {
                Question q = null; ;
                string url = "https://localhost:44301/api/TeacherQuestionApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Question>();
                    display.Wait();
                    q = display.Result;
                    return View(q);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to find Question with Id" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }
        }


        [HttpPost]
        public ActionResult EditQuestion(Question q)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherQuestionApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.PutAsJsonAsync<Question>(url, q);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Question Edited Successfully";
                    return RedirectToAction("ViewQuestion");
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to edit Question with Id" });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        [Authorize]
        public ActionResult DeleteQuestion(int id)
        {
            try
            {
                Question q = null; ;
                string url = "https://localhost:44301/api/TeacherQuestionApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Question>();
                    display.Wait();
                    q = display.Result;
                    return View(q);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to find Question with Id" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }            
        }


        [HttpPost, ActionName("DeleteQuestion")]
        public ActionResult DeleteQuestionConfirmed(int id)
        {
            try
            {
                string url = "https://localhost:44301/api/TeacherQuestionApi/" + id.ToString();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.DeleteAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Question Deleted Successfully";
                    return RedirectToAction("ViewQuestion");
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to delete Question with Id" });
                }   
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        [Authorize]
        public ActionResult LeaderBoard()
        {
            try
            {
                List<Student> studentlst = new List<Student>();
                string url = "https://localhost:44301/api/StudentApi";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<List<Student>>();
                    display.Wait();
                    studentlst = display.Result;
                    return View(studentlst);
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch Students" });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }  
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}