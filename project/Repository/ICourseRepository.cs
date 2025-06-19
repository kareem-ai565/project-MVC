using project.Models;

namespace project.Repository
{
    public interface ICourseRepository : IRepository<Course>
    {
        List<Course> GetCoursesWithPagination(int page, int pageSize, string? include = null);
        int GetTotalCoursesCount();

        Course GetCourseWithDepartment(int id);

        bool CourseExists(int id);

        Course GetCourseByName(string courseName);

        List<Course> GetCoursesByDepartment(int departmentId, string? include = null);
    }
}