using project.Models;

namespace project.Repository
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        List<Instructor> GetWithPagination(string? include, int page, int pageSize);
        int GetTotalInstructorsCount();

        Instructor GetInstructorWithRelations(int id);

        bool InstructorExists(int id);

        Instructor GetInstructorByName(string instructorName);

        List<Instructor> GetInstructorsByDepartment(int departmentId, string? include = null);
        List<Instructor> GetInstructorsByCourse(int courseId, string? include = null);
    }
}