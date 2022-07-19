using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagement.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Email Id is required")]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = " Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain minimum 8 characters, 1 uppercase character,1 lowercase character,1 number, 1 special character")]
        public string Password { get; set; }

        [Display(Name="Marks 1")]
        public int? Marks1 { get; set; }

        [Display(Name = "Marks 2")]
        public int? Marks2 { get; set; }

        [Display(Name = "Marks 3")]
        public int? Marks3 { get; set; }

        [Display(Name = "Marks 4")]
        public int? Marks4 { get; set; }

        [Display(Name = "Total Marks")]
        public int? Total { get; set; }
        public IList<Exam> Exams { get; set; }
    }
}