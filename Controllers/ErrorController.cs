using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamManagement.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult ErrorNotFound(string msg = "An Unexpected Error Occured")
        {
            if (msg != null)
            {
                ViewBag.ExceptionE = msg;
            }
            return View();
        }
    }
}