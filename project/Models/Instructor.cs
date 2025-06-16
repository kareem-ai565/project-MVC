using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string? InstructorName { get; set; }
        public string? Image { get; set; }
        public float? Salary { get; set; }
        public string? Address { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; } //fk to Course
        public Course Course { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; } // fk to Department
        public Department Department { get; set; }
    }
}
