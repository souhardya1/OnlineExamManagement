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
    
    public class StudentController : Controller
    {
        MyDbContext db = new MyDbContext();
       


        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Student s)
        {
            if (s != null)
            {
                HttpClient client = new HttpClient();
                List<Exam> activeExams = new List<Exam>();
                client.BaseAddress = new Uri("https://localhost:44301/api/Studentapi");
                var response = client.PostAsJsonAsync<Student>("StudentApi", s);
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else { return View("Signup"); }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Student
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(Student s)
        {
            if (IsValid(s))
            {
                FormsAuthentication.SetAuthCookie(s.Email.ToString(), false);
                Session["studentEmail"] = s.Email.ToString();
                
                Student stud = db.Students.Where(x=>x.Email == s.Email).FirstOrDefault();


                Session["userId"] = stud.Id;
                System.Diagnostics.Debug.WriteLine(s.Id);
                //System.Diagnostics.Debug.WriteLine(s.Email);
                if (s.Marks4 == null)
                {
                    Session["isEligible"] = 1;  // yes eligible
                }
                else
                {
                    Session["isEligible"] = 0;  // no max number of exams completed

                }
                return RedirectToAction("AvailableExams");
            }
            else
            {
                ViewBag.ErrorMessage = "Email ID and Passwords Incorrect. please register";
                return View();
            }


        }

        public bool IsValid(Student s)
        {
            var cred = db.Students.Where(
                        x => x.Email == s.Email && x.Password == s.Password).
                        FirstOrDefault();
            if (cred != null)
            {
                return (s.Email == cred.Email && s.Password == cred.Password);
            }

            else
            {
                return false;
            }

        }


        [Authorize]
        public ActionResult AvailableExams()
        {
            if (Session["studentEmail"] == null)
            {

                return RedirectToAction("Login", "Student");
            }
            else
            {
                HttpClient client = new HttpClient();
                List<Exam> activeExams = new List<Exam>();
                client.BaseAddress = new Uri("https://localhost:44301/api/Examapi");
                var response = client.GetAsync("ExamApi");
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<List<Exam>>();
                    display.Wait();

                    activeExams = display.Result;
                }
                return View(activeExams);

            }
            
        }



        [Authorize]
        
        public ActionResult MainExam(int id,int qid = 0)
        {
            HttpClient client = new HttpClient();
            List<Question> examQuestions = new List<Question>();
            string apiUrl = "https://localhost:44301/api/Questionapi/"+id;
            client.BaseAddress = new Uri(apiUrl);
            var response = client.GetAsync(apiUrl);


  
            response.Wait();

            var test = response.Result;

            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Question>>();
                display.Wait();

                if (qid != 10)
                {
                    examQuestions = display.Result;
                    var questionData = examQuestions[qid];
                    var index = qid + 1;

                    Session["QuestionCourse"] = id;
                    ViewData["QuestionIndex"] = index;

                    return View(questionData);
                }
                else
                {
                    return RedirectToAction("Result");
                }
                

            }
            else
            {
                return RedirectToAction("AvailableExams");
            }
        }

        [HttpPost]
        public ActionResult MainExam(string choice)
        {
            int courseid;
            int qid;
            

            string url = Request.Url.ToString();
            if (url.Length == 42)
            {
                courseid = int.Parse(url[41].ToString());
                qid = 0;
            }
            else if(url.Length == 44)
            {
                courseid = int.Parse(url[41].ToString());
                qid = int.Parse(url[43].ToString())-1;
            }
            else if(url.Length == 45)
            {
                qid = 9;
                courseid = 1;
            }
            else
            {
                qid = 0;
                courseid = 1;
            }

            var questionlst = db.Questions.Where(x => x.Course.Id == courseid).ToList();
            var question = questionlst[qid];

            System.Diagnostics.Debug.WriteLine(url.Length);
            System.Diagnostics.Debug.WriteLine(question.Correct);
            System.Diagnostics.Debug.WriteLine(qid);
            System.Diagnostics.Debug.WriteLine(question.Correct);
            System.Diagnostics.Debug.WriteLine(choice);
            System.Diagnostics.Debug.WriteLine(Marks.Obtained);
            bool what = (question.Correct.ToLower() == choice.ToLower());
            System.Diagnostics.Debug.WriteLine(what);
            System.Diagnostics.Debug.WriteLine("hehe");





            if (question.Correct.ToLower() == choice.ToLower())
            {

                Marks.Obtained=Marks.Obtained+10;
                System.Diagnostics.Debug.WriteLine(Marks.Obtained);
            }
            return RedirectToAction("MainExam");
        }

        [Authorize]
        public ActionResult Result()
        {
            
            if (Session["studentEmail"] == null)
            {

                return RedirectToAction("Login", "Student");

            }
            else
            {

                ViewBag.marksObtained = Marks.Obtained;
                int id = int.Parse(Session["userId"].ToString());


                HttpClient client = new HttpClient();
                var url = "https://localhost:44301/api/Studentapi/"+id;
                client.BaseAddress = new Uri(url);
                var response = client.PutAsJsonAsync<int>(url,id);
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Student>();
                    display.Wait();
                }
                return View();

            }
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            if (Session["userId"] == null)
            {

                return RedirectToAction("Login", "Student");
            }
            else
            {
                int id = int.Parse(Session["userId"].ToString());

                HttpClient client = new HttpClient();
                Student studentDetails = null;
                var url = "https://localhost:44301/api/Studentapi/"+id;
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url);
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Student>();
                    display.Wait();

                    studentDetails = display.Result;
                    System.Diagnostics.Debug.WriteLine(id);
                    System.Diagnostics.Debug.WriteLine(studentDetails);
                    System.Diagnostics.Debug.WriteLine("hoho");
                }
                return View(studentDetails);

            }
        }

        public ActionResult Logout()
        {
            Marks.Obtained = 0;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}