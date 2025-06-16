using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Trainee
    {
        public int Id { get; set; }
        public string? TraineeName { get; set; }
        public string? Image { get; set; }

        public string? Address { get; set; }
        public int grade { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; } // fk to Department
        public Department Department { get; set; }
    }
}
