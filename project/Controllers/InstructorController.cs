using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using project.ModelViews;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using project.Repository;

#region old way

//namespace project.Controllers
//{
//    public class InstructorController : Controller
//    {
//        private readonly ProjectContext _context;

//        public InstructorController(ProjectContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult ShowAllInstructors(int page = 1, int pageSize = 6)
//        {
//            var totalInstructors = _context.instructors.Count();
//            var totalPages = (int)Math.Ceiling((double)totalInstructors / pageSize);

//            var insList = _context.instructors
//                .Include(i => i.Department)
//                .Include(i => i.Course)
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            ViewBag.CurrentPage = page;
//            ViewBag.TotalPages = totalPages;
//            ViewBag.PageSize = pageSize;
//            ViewBag.TotalInstructors = totalInstructors;

//            return View("ShowAllInstructors", insList);
//        }

//        public IActionResult Details(int id)
//        {
//            var ins = _context.instructors
//                .Include(i => i.Department)
//                .Include(i => i.Course)
//                .FirstOrDefault(i => i.Id == id);

//            if (ins == null)
//            {
//                return NotFound();
//            }

//            return View("Details", ins);
//        }


//        public IActionResult New()
//        {
//            var vm = new NewInstructorVM
//            {
//                Courses = _context.Courses.ToList(),
//                Departments = _context.Departments.ToList()
//            };

//            return View("New", vm);
//        }
//        [HttpPost]


//        public IActionResult SaveNew(string InstructorName, float Salary, string Address, string Image, int Departments, int Courses)
//        {
//            if (string.IsNullOrEmpty(InstructorName) || Salary <= 0 || string.IsNullOrEmpty(Address) || Departments <= 0 || Courses <= 0)
//            {
//                ModelState.AddModelError("", "Please fill all fields correctly.");
//                var vm = new NewInstructorVM
//                {
//                    InstructorName = InstructorName,
//                    Salary = Salary,
//                    Address = Address,
//                    Image = Image,
//                    Departments = _context.Departments.ToList(),
//                    Courses = _context.Courses.ToList()
//                };

//                return View("New", vm);
//            }
//            var instructor = new Instructor
//            {
//                InstructorName = InstructorName,
//                Salary = Salary,
//                Address = Address,
//                Image = Image,
//                DepartmentId = Departments,
//                CourseId = Courses
//            };

//            _context.instructors.Add(instructor);
//            _context.SaveChanges();

//            return RedirectToAction("ShowAllInstructors");
//        }
//        ////////-----------------------------------------------------------------------------------////

//        public IActionResult Edit(int id)
//        {
//            var instructor = _context.instructors.FirstOrDefault(i => i.Id == id);

//            var vm = new NewInstructorVM
//            {
//                Id = instructor.Id,
//                InstructorName = instructor.InstructorName,
//                Image = instructor.Image,
//                Salary = instructor.Salary,
//                Address = instructor.Address,
//                Courses = _context.Courses.ToList(),
//                Departments = _context.Departments.ToList()
//            };
//            return View("Edit", vm);
//        }

//        [HttpPost]
//        public IActionResult SaveEdit(int Id, string InstructorName, float Salary, string Address, string Image, int Departments, int Courses)
//        {
//            if (string.IsNullOrEmpty(InstructorName) || Salary <= 0 || string.IsNullOrEmpty(Address) || Departments <= 0 || Courses <= 0)
//            {
//                ModelState.AddModelError("", "Please fill all fields correctly.");
//                var vm = new NewInstructorVM
//                {
//                    Id = Id,
//                    InstructorName = InstructorName,
//                    Salary = Salary,
//                    Address = Address,
//                    Image = Image,
//                    Departments = _context.Departments.ToList(),
//                    Courses = _context.Courses.ToList()
//                };
//                return View("Edit", vm);
//            }

//            var instructor = _context.instructors.FirstOrDefault(i => i.Id == Id);


//            instructor.InstructorName = InstructorName;
//            instructor.Salary = Salary;
//            instructor.Address = Address;
//            instructor.Image = Image;
//            instructor.DepartmentId = Departments;
//            instructor.CourseId = Courses;

//            _context.instructors.Update(instructor);
//            _context.SaveChanges();
//            return RedirectToAction("ShowAllInstructors");
//        }

//        public IActionResult Delete(int id)
//        {
//            var instructor = _context.instructors.FirstOrDefault(i => i.Id == id);
//            if (instructor == null)
//                return NotFound();

//            return View(instructor);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult ConfirmDelete(int id)
//        {
//            var instructor = _context.instructors.FirstOrDefault(i => i.Id == id);
//            if (instructor == null)
//                return NotFound();

//            _context.instructors.Remove(instructor); // Correct DbSet
//            _context.SaveChanges();

//            return RedirectToAction("ShowAllInstructors");
//        }



//        ////==============================================================/////















//    }
//}

#endregion

namespace project.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly ProjectContext _context; // Keep for Courses and Departments if no specific repositories

        public InstructorController(IInstructorRepository instructorRepository, ProjectContext context)
        {
            _instructorRepository = instructorRepository;
            _context = context; // Keep for accessing Courses and Departments
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllInstructors(int page = 1, int pageSize = 6)
        {
            var totalInstructors = _instructorRepository.GetTotalInstructorsCount();
            var totalPages = (int)Math.Ceiling((double)totalInstructors / pageSize);

            var insList = _instructorRepository.GetWithPagination("Department,Course", page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalInstructors = totalInstructors;

            return View("ShowAllInstructors", insList);
        }

        public IActionResult Details(int id)
        {
            var ins = _instructorRepository.GetInstructorWithRelations(id);

            if (ins == null)
            {
                return NotFound();
            }

            return View("Details", ins);
        }

        public IActionResult New()
        {
            var vm = new NewInstructorVM
            {
                Courses = _context.Courses.ToList(),
                Departments = _context.Departments.ToList()
            };

            return View("New", vm);
        }

        [HttpPost]
        public IActionResult SaveNew(string InstructorName, float Salary, string Address, string Image, int Departments, int Courses)
        {
            if (string.IsNullOrEmpty(InstructorName) || Salary <= 0 || string.IsNullOrEmpty(Address) || Departments <= 0 || Courses <= 0)
            {
                ModelState.AddModelError("", "Please fill all fields correctly.");
                var vm = new NewInstructorVM
                {
                    InstructorName = InstructorName,
                    Salary = Salary,
                    Address = Address,
                    Image = Image,
                    Departments = _context.Departments.ToList(),
                    Courses = _context.Courses.ToList()
                };

                return View("New", vm);
            }

            var instructor = new Instructor
            {
                InstructorName = InstructorName,
                Salary = Salary,
                Address = Address,
                Image = Image,
                DepartmentId = Departments,
                CourseId = Courses
            };

            _instructorRepository.Add(instructor);
            _instructorRepository.save();

            return RedirectToAction("ShowAllInstructors");
        }

        public IActionResult Edit(int id)
        {
            var instructor = _instructorRepository.GetById(id);

            if (instructor == null)
                return NotFound();

            var vm = new NewInstructorVM
            {
                Id = instructor.Id,
                InstructorName = instructor.InstructorName,
                Image = instructor.Image,
                Salary = instructor.Salary,
                Address = instructor.Address,
                Courses = _context.Courses.ToList(),
                Departments = _context.Departments.ToList()
            };
            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult SaveEdit(int Id, string InstructorName, float Salary, string Address, string Image, int Departments, int Courses)
        {
            if (string.IsNullOrEmpty(InstructorName) || Salary <= 0 || string.IsNullOrEmpty(Address) || Departments <= 0 || Courses <= 0)
            {
                ModelState.AddModelError("", "Please fill all fields correctly.");
                var vm = new NewInstructorVM
                {
                    Id = Id,
                    InstructorName = InstructorName,
                    Salary = Salary,
                    Address = Address,
                    Image = Image,
                    Departments = _context.Departments.ToList(),
                    Courses = _context.Courses.ToList()
                };
                return View("Edit", vm);
            }

            var instructor = _instructorRepository.GetById(Id);

            if (instructor == null)
                return NotFound();

            instructor.InstructorName = InstructorName;
            instructor.Salary = Salary;
            instructor.Address = Address;
            instructor.Image = Image;
            instructor.DepartmentId = Departments;
            instructor.CourseId = Courses;

            _instructorRepository.Update(instructor);
            _instructorRepository.save();

            return RedirectToAction("ShowAllInstructors");
        }

        public IActionResult Delete(int id)
        {
            var instructor = _instructorRepository.GetById(id);
            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var instructor = _instructorRepository.GetById(id);
            if (instructor == null)
                return NotFound();

            _instructorRepository.Delete(instructor);
            _instructorRepository.save();

            return RedirectToAction("ShowAllInstructors");
        }
    }
}