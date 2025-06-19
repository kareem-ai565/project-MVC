using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ProjectContext _context;

        public SearchRepository(ProjectContext context)
        {
            _context = context;
        }

        public List<Instructor> GetAll(string? include)
        {
            var instructors = _context.instructors.ToList();

            if (!string.IsNullOrEmpty(include))
            {
              
            }

            return instructors;
        }

        public Instructor GetById(int id)
        {
            return _context.instructors.ToList().FirstOrDefault(i => i.Id == id);
        }

        public void Add(Instructor entity)
        {
            _context.instructors.Add(entity);
        }

        public void Update(Instructor entity)
        {
            _context.instructors.Update(entity);
        }

        public void Delete(Instructor entity)
        {
            _context.instructors.Remove(entity);
        }

        public void save()
        {
            _context.SaveChanges();
        }

        public List<Instructor> SearchInstructors(string query, string? include = null)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Instructor>();

            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.InstructorName.Contains(query) || i.Address.Contains(query))
                .ToList();
        }

        public List<Instructor> SearchInstructorsByName(string name, string? include = null)
        {
            if (string.IsNullOrEmpty(name))
                return new List<Instructor>();

            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.InstructorName.Contains(name))
                .ToList();
        }

        public List<Instructor> SearchInstructorsByAddress(string address, string? include = null)
        {
            if (string.IsNullOrEmpty(address))
                return new List<Instructor>();

            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.Address.Contains(address))
                .ToList();
        }

        public List<Instructor> SearchInstructorsAdvanced(string? name, string? address, int? departmentId, int? courseId, string? include = null)
        {
            var allInstructors = _context.instructors.ToList();

            var filtered = allInstructors.AsEnumerable();

            if (!string.IsNullOrEmpty(name))
                filtered = filtered.Where(i => i.InstructorName.Contains(name));

            if (!string.IsNullOrEmpty(address))
                filtered = filtered.Where(i => i.Address.Contains(address));

            if (departmentId.HasValue)
                filtered = filtered.Where(i => i.DepartmentId == departmentId.Value);

            if (courseId.HasValue)
                filtered = filtered.Where(i => i.CourseId == courseId.Value);

            return filtered.ToList();
        }

        public List<Instructor> SearchInstructorsWithPagination(string query, int page, int pageSize, string? include = null)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Instructor>();

            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.InstructorName.Contains(query) || i.Address.Contains(query))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetSearchResultsCount(string query)
        {
            if (string.IsNullOrEmpty(query))
                return 0;

            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.InstructorName.Contains(query) || i.Address.Contains(query))
                .Count();
        }

        public List<Course> SearchCourses(string query, string? include = null)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Course>();

            var allCourses = _context.Courses.ToList();

            return allCourses
                .Where(c => c.CourseName.Contains(query))
                .ToList();
        }

        public List<Department> SearchDepartments(string query, string? include = null)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Department>();

            var allDepartments = _context.Departments.ToList();

            return allDepartments
                .Where(d => d.Name.Contains(query))
                .ToList();
        }
    }
}