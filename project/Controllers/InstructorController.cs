using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For Include
using project.Models;

namespace project.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ProjectContext _context;

        // Inject the database context through constructor
        public InstructorController(ProjectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllInstructors()
        {
            var insList = _context.instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .ToList();

            return View("ShowAllInstructors", insList);
        }

        public IActionResult Details(int id)
        {
            var ins = _context.instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefault(i => i.Id == id);

            if (ins == null)
            {
                return NotFound();
            }

            return View("Details", ins);
        }
    }
}
