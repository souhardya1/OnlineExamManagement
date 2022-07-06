using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace OnlineExamManagement.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " Exam Code is required")]
        [Display(Name = "Exam Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = " Course Type is required")]
        [ForeignKey("Course")]
        [Display(Name = "Course Type")]
        public int CourseRefId { get; set; }
        public Course Course { get; set; }

        [Required(ErrorMessage = " Last Submission Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Last Submission Date")]
        [Column(TypeName = "Date")]
        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = " Active Status is required")]
        [Display(Name = "Is it Active?")]
        public int Active { get; set; }

        public IList<Student> Students { get; set; }
    }
}