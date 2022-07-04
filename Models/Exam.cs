using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamManagement.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseRefId { get; set; }
        public Course Course { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public int Active { get; set; }

        public IList<Student> Students { get; set; }
    }
}