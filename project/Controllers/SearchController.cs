using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using System.Linq;

namespace project.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProjectContext db;

        public SearchController(ProjectContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

   
        public IActionResult Results(string query)
        {
            var results = db.instructors
                .Include(i => i.Course)
                .Include(i => i.Department)
                .Where(i => i.InstructorName.Contains(query) || i.Address.Contains(query))
                .ToList();

            return View(results);
        }
    }
}
