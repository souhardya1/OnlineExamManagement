using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineExamManagement.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " Question is required")]
        [StringLength(100)]
        [Display(Name = "Question")]
        public string Ques { get; set; }

        [Required(ErrorMessage = " Course Type is required")]
        [ForeignKey("Course")]
        [Display(Name = "Course Type")]
        public int CourseRefId { get; set; }
        public Course Course { get; set; }

        [Required(ErrorMessage = " Given Option A is required")]
        [StringLength(100)]
        [Display(Name = "Given Option A")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = " Given Option B is required")]
        [StringLength(100)]
        [Display(Name = "Given Option B")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = " Given Option C is required")]
        [StringLength(100)]
        [Display(Name = "Given Option C")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = " Given Option D is required")]
        [StringLength(100)]
        [Display(Name = "Given Option D")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = " Correct Option is required")]
        [StringLength(100)]
        [Display(Name = "Correct Option")]
        public string Correct { get; set; }


    }
}