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
                if(cred.Password == t.Password)
                {            
                    return true;
                }
                return false;
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

        [HttpPost]
        public ActionResult ViewExam(string searchText)
        {
            if( searchText != null)
            {
                var searchResult = db.Exams.Where(x => x.Code.Contains(searchText)).ToList();
                return View(searchResult);
            }
            return View();
        }


        [Authorize]
        public ActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateExam(Exam e)
        {
            if (ModelState.IsValid==true)
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
            else
            {
                return View();
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
            if (ModelState.IsValid == true)
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
            else
            {
                return View();
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
            if (ModelState.IsValid)
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
            else
            {
                return View();
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


        [HttpPost]
        public ActionResult ViewQuestion(string searchText)
        {
            if (searchText != null)
            {
                var searchResult = db.Questions.Where(x => x.Ques.Contains(searchText) ||  x.OptionA.Contains(searchText) ||   x.OptionB.Contains(searchText) ||  x.OptionC.Contains(searchText) || x.OptionD.Contains(searchText) ).ToList();
                return View(searchResult);
            }
            return View();
        }


        [Authorize]
        public ActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestion(Question q)
        {
            if (ModelState.IsValid == true)
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
            else
            {
                return View();
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
            if(ModelState.IsValid == true)
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
            else
            {
                return View();
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
            if (ModelState.IsValid == true)
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
            else
            {
                return View();
            }
                       
        }


        [Authorize]
        public ActionResult LeaderBoard(string sortOrder= "",string sortBy = "")
        {
            try
            {
                if (sortOrder != "" && sortBy != "")
                {
                    List<Student> stud = db.Students.ToList();
                    switch (sortBy)
                    {
                        case "Marks1":
                            {

                                if (sortOrder == "A")
                                {
                                    stud = db.Students.OrderBy(x => x.Marks1).ToList();
                                }
                                else
                                {
                                    stud = db.Students.OrderByDescending(x => x.Marks1).ToList();
                                }
                                break;
                            }

                        case "Marks2":
                            {

                                if (sortOrder == "A")
                                {
                                    stud = db.Students.OrderBy(x => x.Marks2).ToList();
                                }
                                else
                                {
                                    stud = db.Students.OrderByDescending(x => x.Marks2).ToList();
                                }
                                break;
                            }

                        case "Marks3":
                            {

                                if (sortOrder == "A")
                                {
                                    stud = db.Students.OrderBy(x => x.Marks3).ToList();
                                }
                                else
                                {
                                    stud = db.Students.OrderByDescending(x => x.Marks3).ToList();
                                }
                                break;
                            }

                        case "Marks4":
                            {

                                if (sortOrder == "A")
                                {
                                    stud = db.Students.OrderBy(x => x.Marks4).ToList();
                                }
                                else
                                {
                                    stud = db.Students.OrderByDescending(x => x.Marks4).ToList();
                                }
                                break;
                            }

                        case "Total":
                            {

                                if (sortOrder == "A")
                                {
                                    stud = db.Students.OrderBy(x => x.Total).ToList();
                                }
                                else
                                {
                                    stud = db.Students.OrderByDescending(x => x.Total).ToList();
                                }
                                break;
                            }
                    }
                    return View(stud);
                }

                else
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