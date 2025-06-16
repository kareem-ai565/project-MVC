using project.Models;

namespace project.ModelViews
{
    public class NewInstructorVM
    {
        public int Id { get; set; }
        public string? InstructorName { get; set; }
        public string? Image { get; set; }
        public float? Salary { get; set; }
        public string? Address { get; set; }

        public List<Department> Departments { get; set; } = new List<Department>();
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
