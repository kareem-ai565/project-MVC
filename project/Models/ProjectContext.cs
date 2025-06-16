using Microsoft.EntityFrameworkCore;

namespace project.Models
{
    public class ProjectContext :DbContext
    {
        public DbSet<Instructor> instructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<CourseResults> CourseResults { get; set; }

        //public ProjectContext() : base()
        //{
        //}
        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-CL31JEQ\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False");
        }
    }

}
