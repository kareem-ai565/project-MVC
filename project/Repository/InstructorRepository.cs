using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Repository
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly ProjectContext _context;

        public InstructorRepository(ProjectContext context)
        {
            _context = context;
        }

        // Generic Repository Methods
        public List<Instructor> GetAll(string? include)
        {
          
            var instructors = _context.instructors.ToList();

        

            return instructors;
        }

        public Instructor GetById(int id)
        {
            var instructors = _context.instructors.ToList();
            return instructors.FirstOrDefault(i => i.Id == id);
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

       
        public List<Instructor> GetWithPagination(string? include, int page, int pageSize)
        {
        
            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalInstructorsCount()
        {
            return _context.instructors.ToList().Count;

        }

        public Instructor GetInstructorWithRelations(int id)
        {
          
            var instructors = _context.instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .ToList();

            return instructors.FirstOrDefault(i => i.Id == id);
        }

        public bool InstructorExists(int id)
        {
            var instructors = _context.instructors.ToList();
           return _context.instructors.Any(i => i.Id == id);
        }

        public Instructor GetInstructorByName(string instructorName)
        {
            var instructors = _context.instructors.ToList();
            return instructors.FirstOrDefault(i => i.InstructorName == instructorName);
        }

        public List<Instructor> GetInstructorsByDepartment(int departmentId, string? include = null)
        {
            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.DepartmentId == departmentId)
                .ToList();
        }

        public List<Instructor> GetInstructorsByCourse(int courseId, string? include = null)
        {
            var allInstructors = _context.instructors.ToList();

            return allInstructors
                .Where(i => i.CourseId == courseId)
                .ToList();
        }
    }
}