using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace OnlineExamManagement.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ques { get; set; }

        [Required]
        public Course Course { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionA { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionB { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionC { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionD { get; set; }

        [Required]
        [StringLength(100)]
        public string Correct { get; set; }


    }
}