using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ProjectContext _context;

        public CourseRepository(ProjectContext context)
        {
            _context = context;
        }

        public List<Course> GetAll(string? include)
        {
            IQueryable<Course> query = _context.Courses;

            if (!string.IsNullOrEmpty(include))
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Course entity)
        {
            _context.Courses.Add(entity);
        }

        public void Update(Course entity)
        {
            _context.Courses.Update(entity);
        }

        public void Delete(Course entity)
        {
            _context.Courses.Remove(entity);
        }

        public void save()
        {
            _context.SaveChanges();
        }

        // Course specific Methods
        public List<Course> GetCoursesWithPagination(int page, int pageSize, string? include = null)
        {
            IQueryable<Course> query = _context.Courses;

            if (!string.IsNullOrEmpty(include))
            {
                query = query.Include(include);
            }

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalCoursesCount()
        {
            return _context.Courses.Count();
        }

        public Course GetCourseWithDepartment(int id)
        {
            return _context.Courses
                .Include(c => c.Department)
                .FirstOrDefault(c => c.Id == id);
        }

        public bool CourseExists(int id)
        {
            return _context.Courses.Any(c => c.Id == id);
        }

        public Course GetCourseByName(string courseName)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseName == courseName);
        }

        public List<Course> GetCoursesByDepartment(int departmentId, string? include = null)
        {
            IQueryable<Course> query = _context.Courses.Where(c => c.DepartmentId == departmentId);

            if (!string.IsNullOrEmpty(include))
            {
                query = query.Include(include);
            }

            return query.ToList();
        }
    }
}