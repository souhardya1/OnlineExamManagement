using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineExamManagement.Models;

namespace OnlineExamManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult ForceLogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                Marks.Obtained = 0;
                Marks.c = 0;
                FormsAuthentication.SignOut();               
            }
            return RedirectToAction("Index");

        }
    }
}
