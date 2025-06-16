using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }
        public int? Degree { get; set; }
        public int? MinDegree { get; set; }
        public int? Hours { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; } //fk to Department
        public Department Department { get; set; }


    }
}
