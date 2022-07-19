using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamManagement.Models
{
    public class ExamStudent
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ExamCode { get; set; }



    }
}