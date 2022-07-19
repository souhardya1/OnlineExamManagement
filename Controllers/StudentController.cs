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
    public class StudentController : Controller
    {
        MyDbContext db = new MyDbContext();
       
        public ActionResult Signup()
        {
            try
            {
 
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }          
        }


        [HttpPost]
        public ActionResult Signup(Student s)
        {
            try
            {
                if (s != null)
                {
                    var checkmail = db.Students.Where(x => x.Email == s.Email).FirstOrDefault();
                    if (checkmail != null)
                    {
                        ViewBag.AlreadyRegistered = "This Email id is already registered. Please Log in";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        HttpClient client = new HttpClient();
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
                }
                else
                {
                    return RedirectToAction("ErrorNotFound", "Error", new {msg = "Failed to Sign up"});
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        // GET: Student
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        [HttpPost]
        public ActionResult Login(Student s)
        {
            try
            {

                    if (IsValid(s))
                    {
                        FormsAuthentication.SetAuthCookie(s.Email.ToString(), false);
                        Session["studentEmail"] = s.Email.ToString();

                        Student stud = db.Students.Where(x => x.Email == s.Email).FirstOrDefault();


                        Session["userId"] = stud.Id;
                        //System.Diagnostics.Debug.WriteLine(s.Id);
                        //System.Diagnostics.Debug.WriteLine(s.Email);
                        if (stud.Marks4 == null)
                        {
                            Session["IsEligible"] = "1";  // yes eligible
                        }
                        else
                        {
                            Session["IsEligible"] = "0";  // no max number of exams completed

                        }
                        //System.Diagnostics.Debug.WriteLine(".smarks4 is ",stud.Marks4);
                        //System.Diagnostics.Debug.WriteLine("iseligible is: ",Session["IsEligible"].ToString());

                        return RedirectToAction("AvailableExams");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Email ID and Passwords Incorrect. please register";
                        return View();
                    }

                

                
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
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
            try
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

                        List<string> givenExams = db.ExamStudents.Where(x => x.Email ==  User.Identity.Name).Select(x => x.ExamCode).ToList();
                        ViewBag.GivenExams = givenExams;


                        activeExams = display.Result;
                        return View(activeExams);
                    }
                    else
                    {
                        return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch available Exams" });
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }   
        }


        [Authorize]
        [NoDirectAccess]
        public ActionResult MainExam(int id,int qid = 0)
        {
            try
            {
                HttpClient client = new HttpClient();
                List<Question> examQuestions = new List<Question>();
                string apiUrl = "https://localhost:44301/api/Questionapi/" + id;
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
                    return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to find Questions for this Exam" });
                }
            }
            catch (Exception e)
            {   

                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        [HttpPost]
        public ActionResult MainExam(string choice)
        {
            try
            {
                int courseid;
                int qid;


                string url = Request.Url.ToString();
                if (url.Length == 42)
                {
                    courseid = int.Parse(url[41].ToString());
                    qid = 0;
                }
                else if (url.Length == 44)
                {
                    courseid = int.Parse(url[41].ToString());
                    qid = int.Parse(url[43].ToString()) - 1;
                }
                else if (url.Length == 45)
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

                //System.Diagnostics.Debug.WriteLine(url.Length);
                //System.Diagnostics.Debug.WriteLine(question.Correct);
                //System.Diagnostics.Debug.WriteLine(qid);
                //System.Diagnostics.Debug.WriteLine(question.Correct);
                //System.Diagnostics.Debug.WriteLine(choice);
                //System.Diagnostics.Debug.WriteLine(Marks.Obtained);
                //bool what = (question.Correct.ToLower() == choice.ToLower());
                //System.Diagnostics.Debug.WriteLine(what);
                //System.Diagnostics.Debug.WriteLine("hehe");

                Marks.c = Marks.c + 1;
                if (Marks.c <= 10)
                {
                    if (choice != null)
                    {
                        if (question.Correct.ToLower() == choice.ToLower())
                        {
                            Marks.Obtained = Marks.Obtained + 10;
                        }
                        //System.Diagnostics.Debug.WriteLine(Marks.Obtained);
                    }
                }
                return RedirectToAction("MainExam");
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }
        }


        [Authorize]
        [NoDirectAccess]
        public ActionResult Result()
        {
            try
            {
                if (Session["studentEmail"] == null)
                {

                    return RedirectToAction("Login", "Student");

                }
                else
                {
                    ViewBag.marksObtained = Marks.Obtained;
                    if (Marks.Obtained > 40)
                    {
                        ViewData["PassFail"] = "1";
                    }
                    else
                    {
                        ViewData["PassFail"] = "0";
                    }
                    int id = int.Parse(Session["userId"].ToString());
                    HttpClient client = new HttpClient();
                    var url = "https://localhost:44301/api/Studentapi/" + id.ToString();
                    client.BaseAddress = new Uri(url);
                    System.Diagnostics.Debug.WriteLine("result id is", id.ToString());
                    var response = client.PutAsJsonAsync<int>(url, id);
                    response.Wait();

                    var test = response.Result;
                    if (test.IsSuccessStatusCode)
                    {
                        var display = test.Content.ReadAsAsync<Student>();
                        display.Wait();
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to generate result" });
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorNotFound", "Error", new { msg = e.Message });
            }           
        }


        [Authorize]
        public ActionResult UserProfile()
        {
            try
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
                        List<string> givenExams = db.ExamStudents.Where(x => x.Email == User.Identity.Name).Select(x => x.ExamCode).ToList();
                        ViewBag.GivenExams = givenExams;


                        return View(studentDetails);
                    }
                    else
                    {
                            return RedirectToAction("ErrorNotFound", "Error", new { msg = "Failed to fetch User Details" });
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
            Marks.Obtained = 0;
            Marks.c = 0;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ErrorNotFound(string msg="An Unexpected Error Occured")
        {
            ViewBag.ExceptionE = msg;
            return View();
        }

      
        public ActionResult instructions(int id)
        {
            string email = User.Identity.Name;
            string code = @Request.Url.ToString().Substring(52, Request.Url.ToString().Length - 52);
            ExamStudent es = new ExamStudent();
            es.Email = email;
            es.ExamCode = code;

            db.ExamStudents.Add(es);
            db.SaveChanges();

            ViewData["CourseRefId"] = id;
            return View();
        }
    }
}