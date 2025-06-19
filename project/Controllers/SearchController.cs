using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using System.Linq;

#region old way

//namespace project.Controllers
//{
//    public class SearchController : Controller
//    {
//        private readonly ProjectContext db;

//        public SearchController(ProjectContext context)
//        {
//            db = context;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }


//        public IActionResult Results(string query)
//        {
//            var results = db.instructors
//                .Include(i => i.Course)
//                .Include(i => i.Department)
//                .Where(i => i.InstructorName.Contains(query) || i.Address.Contains(query))
//                .ToList();

//            return View(results);
//        }
//    }
//}

#endregion

using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Repository;

namespace project.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Results(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Instructor>());
            }

            var results = _searchRepository.SearchInstructors(query, "Course,Department");

            ViewBag.SearchQuery = query;
            ViewBag.ResultsCount = results.Count;

            return View(results);
        }

        // Additional search methods you can add
        public IActionResult SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return View("Results", new List<Instructor>());
            }

            var results = _searchRepository.SearchInstructorsByName(name, "Course,Department");

            ViewBag.SearchQuery = name;
            ViewBag.ResultsCount = results.Count;
            ViewBag.SearchType = "Name";

            return View("Results", results);
        }

        public IActionResult SearchByAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return View("Results", new List<Instructor>());
            }

            var results = _searchRepository.SearchInstructorsByAddress(address, "Course,Department");

            ViewBag.SearchQuery = address;
            ViewBag.ResultsCount = results.Count;
            ViewBag.SearchType = "Address";

            return View("Results", results);
        }

        public IActionResult AdvancedSearch(string? name, string? address, int? departmentId, int? courseId)
        {
            var results = _searchRepository.SearchInstructorsAdvanced(name, address, departmentId, courseId, "Course,Department");

            ViewBag.SearchQuery = $"Advanced Search";
            ViewBag.ResultsCount = results.Count;
            ViewBag.SearchType = "Advanced";

            return View("Results", results);
        }

        public IActionResult SearchWithPagination(string query, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Results", new List<Instructor>());
            }

            var results = _searchRepository.SearchInstructorsWithPagination(query, page, pageSize, "Course,Department");
            var totalResults = _searchRepository.GetSearchResultsCount(query);
            var totalPages = (int)Math.Ceiling((double)totalResults / pageSize);

            ViewBag.SearchQuery = query;
            ViewBag.ResultsCount = totalResults;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View("Results", results);
        }
    }
}