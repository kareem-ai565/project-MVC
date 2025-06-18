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
        //[HttpPost]
        //public IActionResult SaveCourse(string CourseName, int? Degree, int? MinDegree, int? Hours, int DepartmentId, string? Pic)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Please fill all fields correctly.");

        //        var course = new CourseVM
        //        {
        //            CourseName = CourseName,
        //            Degree = Degree,
        //            MinDegree = MinDegree,
        //            Hours = Hours,
        //            Pic = Pic,
        //            DepartmentId = DepartmentId,
        //            Departments = _context.Departments.ToList()
        //        };

        //        return View("AddCourse", course);
        //    }

        //    var course = new Course
        //    {
        //        CourseName = CourseName,
        //        Degree = Degree,
        //        MinDegree = MinDegree,
        //        Hours = Hours,
        //        DepartmentId = DepartmentId
        //    };

        //    _context.Courses.Add(course);
        //    _context.SaveChanges();

        //    return RedirectToAction("ShowAllCourses");
        //}

        [HttpPost]
        public IActionResult SaveCourse(CourseVM course)
        {
            if (!ModelState.IsValid)
            {
                // Reload Departments list if validation fails
                course.Departments = _context.Departments.ToList();
                return View("AddCourse", course);
            }

            var newCourse = new Course
            {
                CourseName = course.CourseName,
                Degree = course.Degree,
                MinDegree = course.MinDegree,
                Hours = course.Hours,
                DepartmentId = course.DepartmentId,
                // Pic: handle separately if you're saving uploaded files
            };

            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            return RedirectToAction("ShowAllCourses");
        }

        public IActionResult Details(int id)
        {
            var course = _context.Courses
                .Include(c => c.Department)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                return NotFound();

            var vm = new CourseVM
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Degree = course.Degree,
                MinDegree = course.MinDegree,
                Hours = course.Hours,
                Pic = $"{course.CourseName}.jpg",


                DepartmentId = course.DepartmentId,
                DepartmentName = course.Department?.Name
            };

            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();

            return View(course); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("ShowAllCourses");
        }



        [AcceptVerbs("GET")]
        public IActionResult ValidateMinDegree(int? MinDegree, int? Degree)
        {
            if (MinDegree >= Degree)
            {
                return Json("Minimum degree must be less than degree.");
            }
            return Json(true);
        }












    }
}
