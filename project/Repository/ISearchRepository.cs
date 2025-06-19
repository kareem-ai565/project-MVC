using project.Models;

namespace project.Repository
{
    public interface ISearchRepository : IRepository<Instructor>
    {
        // Search methods for instructors
        List<Instructor> SearchInstructors(string query, string? include = null);
        List<Instructor> SearchInstructorsByName(string name, string? include = null);
        List<Instructor> SearchInstructorsByAddress(string address, string? include = null);

        List<Instructor> SearchInstructorsAdvanced(string? name, string? address, int? departmentId, int? courseId, string? include = null);

        List<Instructor> SearchInstructorsWithPagination(string query, int page, int pageSize, string? include = null);
        int GetSearchResultsCount(string query);
        List<Course> SearchCourses(string query, string? include = null);
        List<Department> SearchDepartments(string query, string? include = null);
    }
}