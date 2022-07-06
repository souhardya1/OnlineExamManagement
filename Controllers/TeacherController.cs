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
            }

            return View(examlst);
        }

        [Authorize]
        public ActionResult CreateExam()
        {
            IList<Course> CourseCategory = db.Courses.ToList();
            IEnumerable<SelectListItem> selectListCategory =
                 from c in CourseCategory
                 select new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.Id.ToString()
                 };
            ViewBag.CourseCategoryData = selectListCategory;

            List<SelectListItem> activeListCategory = new List<SelectListItem>();
            activeListCategory.Add(new SelectListItem
            {
                Text = "Yes",
                Value = "1"
            });
            activeListCategory.Add(new SelectListItem
            {
                Text = "No",
                Value = "0"
            });

            ViewBag.ActiveList = activeListCategory;
            return View();
        }

        [HttpPost]
        public ActionResult CreateExam(Exam e)
        {
            string url = "https://localhost:44301/api/TeacherExamApi";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.PostAsJsonAsync<Exam>(url, e);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["CreateSuccess"] = "<script>Exam Inserted Successfully</script>";
                return RedirectToAction("ViewExam");
            }
            return View();
        }

        [Authorize]
        public ActionResult EditExam(int id)
        {
            Exam e = null; ;
            string url = "https://localhost:44301/api/TeacherExamApi/"+id.ToString();
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
            }

            IList<Course> CourseCategory = db.Courses.ToList();
            IEnumerable<SelectListItem> selectListCategory =
                 from c in CourseCategory
                 select new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.Id.ToString()
                 };
            ViewBag.CourseData = selectListCategory;
            List<SelectListItem> activeListCategory = new List<SelectListItem>();
            activeListCategory.Add(new SelectListItem
            {
                Text = "Yes",
                Value = "1"
            });
            activeListCategory.Add(new SelectListItem
            {
                Text = "No",
                Value = "0"
            });

            ViewBag.ActiveData = activeListCategory;
            

            return View(e);
        }

        [HttpPost]
        public ActionResult EditExam(Exam e)
        {
            string url = "https://localhost:44301/api/TeacherExamApi";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.PutAsJsonAsync<Exam>(url, e);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["CreateSuccess"] = "<script>Exam Inserted Successfully</script>";
                return RedirectToAction("ViewExam");
            }
            return View();
        }

        [Authorize]      
        public ActionResult DeleteExam(int id)
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
            }
            return View(e);
        }

        [HttpPost,ActionName("DeleteExam")]
        public ActionResult DeleteExamConfirmed(int id)
        {
            string url = "https://localhost:44301/api/TeacherExamApi/"+id.ToString();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.DeleteAsync(url);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["DeleteSuccess"] = "<script>Exam Deleted Successfully</script>";
                return RedirectToAction("ViewExam");
            }

            return View();
        }

        // Question Part
        [Authorize]
        public ActionResult ViewQuestion()
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
            }

            return View(queslst);


        }

        [Authorize]
        public ActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestion(Question q)
        {
            string url = "https://localhost:44301/api/TeacherQuestionApi";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.PostAsJsonAsync<Question>(url, q);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["CreateSuccess"] = "<script>alert(Exam Inserted Successfully)</script>";
                return RedirectToAction("ViewQuestion");
            }
            return View();

        }

        [Authorize]
        public ActionResult EditQuestion(int id)
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
            }
            return View(q);
        }

        [HttpPost]
        public ActionResult EditQuestion(Question q)
        {
            string url = "https://localhost:44301/api/TeacherQuestionApi";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.PutAsJsonAsync<Question>(url, q);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["EditSuccess"] = "<script>alert('Question Edited Successfully')</script>";
                return RedirectToAction("ViewExam");
            }

            return View();
        }


        [Authorize]
        public ActionResult DeleteQuestion(int id)
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
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteQuestion")]
        public ActionResult DeleteQuestionConfirmed(int id)
        {
            string url = "https://localhost:44301/api/TeacherQuestionApi/" + id.ToString();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.DeleteAsync(url);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                ViewData["DeleteSuccess"] = "<script>alert('Exam Deleted Successfully')</script>";
                return RedirectToAction("ViewQuestion");
            }

            return View();
        }

        [Authorize]
        public ActionResult LeaderBoard()
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
            }

            return View(studentlst);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}