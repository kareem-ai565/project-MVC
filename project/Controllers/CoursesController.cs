using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using project.ModelViews;

namespace project.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ProjectContext _context;

        public CoursesController(ProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllCourses(int page = 1, int pageSize = 6)
        {
            var totalCourses = _context.Courses.Count();
            var totalPages = (int)Math.Ceiling((double)totalCourses / pageSize);

            var crsList = _context.Courses
                .Include(c => c.Department)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseVM
                {
                    Id = c.Id,
                    CourseName = c.CourseName,
                    Degree = c.Degree,
                    MinDegree = c.MinDegree,
                    Hours = c.Hours,
                    DepartmentName = c.Department.Name,
                    Pic = $"{c.CourseName}.jpg"
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalInstructors = totalCourses;

            return View("ShowAllCourses", crsList);
        }
        public IActionResult AddCourse()
        {
            var vm = new CourseVM
            {
                Departments = _context.Departments.ToList()
            };

            return View("AddCourse", vm);
        }
        [HttpPost]
        public IActionResult SaveCourse(string CourseName, int? Degree, int? MinDegree, int? Hours, int DepartmentId, string? Pic)
        {
            // Basic validation
            if (string.IsNullOrEmpty(CourseName) || Degree == null || MinDegree == null || Hours == null || DepartmentId <= 0)
            {
                ModelState.AddModelError("", "Please fill all fields correctly.");

                var vm = new CourseVM
                {
                    CourseName = CourseName,
                    Degree = Degree,
                    MinDegree = MinDegree,
                    Hours = Hours,
                    Pic = Pic,
                    DepartmentId = DepartmentId,
                    Departments = _context.Departments.ToList()
                };

                return View("AddCourse", vm);
            }

            var course = new Course
            {
                CourseName = CourseName,
                Degree = Degree,
                MinDegree = MinDegree,
                Hours = Hours,
                DepartmentId = DepartmentId
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("ShowAllCourses");
        }

        public JsonResult IsCourseNameUnique(string courseName)
        {
            bool exists = _context.Courses.Any(c => c.CourseName == courseName);
            return Json(!exists);
        }

        public JsonResult IsMinDegreeValid(int minDegree, int degree)
        {
            return Json(minDegree < degree);
        }


    }
}
