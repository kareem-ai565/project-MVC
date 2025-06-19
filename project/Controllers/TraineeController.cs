using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using project.Models;
using project.ModelViews;
using System.Security.Cryptography;
using project.Repository;

#region old way
//namespace project.Controllers
//{
//    public class TraineeController : Controller
//    {
//        private readonly ProjectContext db;
//        public TraineeController(ProjectContext DB)
//        {
//            db = DB;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Result(int Tid, int cid)
//        {

//            CourseResultVM CR = new CourseResultVM();
//            var result = (from cr in db.CourseResults
//                          join t in db.Trainees on cr.TraineeId equals t.Id
//                          join c in db.Courses on cr.CourseId equals c.Id
//                          where cr.TraineeId == Tid && cr.CourseId == cid
//                          select new CourseResultVM
//                          {
//                              TraineeName = t.TraineeName,
//                              CourseName = c.CourseName,
//                              degree = t.grade,
//                              urlImage = t.Image,
//                              //CR.minDegree = c.MinDegree,


//                          }).ToList();

//            foreach (var item in result)
//            {
//                CR.T_id = Tid;
//                CR.c_id = cid;
//                CR.TraineeName = item.TraineeName;
//                CR.CourseName = item.CourseName;
//                CR.urlImage = item.urlImage;
//                CR.degree = item.degree;
//                var minDegree = CR.minDegree;
//                if (item.degree >= minDegree)
//                {
//                    CR.status = "Passed";
//                    CR.color = "green";
//                }
//                else
//                {
//                    CR.status = "Failed";
//                    CR.color = "red";
//                }
//            }
//            return View("Result", CR);
//        }

//        public IActionResult ShowAllTrainees()
//        {
//            var trainees = db.Trainees.Include(t => t.Department).ToList();
//            return View("ShowAllTrainees", trainees);
//        }

//        public IActionResult DetailsTrainee(int tid)
//        {
//            var trainee = db.Trainees
//                            .Include(t => t.Department)
//                            .FirstOrDefault(t => t.Id == tid);

//            if (trainee == null)
//                return NotFound("Trainee not found.");

//            // Join with CourseResults and Courses
//            var courseResults = (from cr in db.CourseResults
//                                 join c in db.Courses on cr.CourseId equals c.Id
//                                 where cr.TraineeId == tid
//                                 select new CourseResultVM
//                                 {
//                                     CourseName = c.CourseName,
//                                     degree = cr.Degree,
//                                     status = cr.Degree >= c.MinDegree ? "Passed" : "Failed",
//                                     color = cr.Degree >= c.MinDegree ? "green" : "red"
//                                 }).ToList();

//            var viewModel = new TraineeWithResultsVM
//            {
//                Id = trainee.Id,
//                TraineeName = trainee.TraineeName,
//                Image = trainee.Image,
//                Address = trainee.Address,
//                Grade = trainee.grade,
//                DepartmentName = trainee.Department?.Name,
//                CourseResults = courseResults
//            };

//            return View("DetailsTrainee", viewModel);
//        }


//        public IActionResult CoursesTrainees(int id)
//        {
//            var traineesInCourse = (from cr in db.CourseResults
//                                    join t in db.Trainees on cr.TraineeId equals t.Id
//                                    join c in db.Courses on cr.CourseId equals c.Id
//                                    where c.Id == id
//                                    select new CourseResultVM
//                                    {
//                                        T_id = t.Id,
//                                        c_id = c.Id,
//                                        TraineeName = t.TraineeName,
//                                        CourseName = c.CourseName,
//                                        degree = cr.Degree,
//                                        urlImage = t.Image,
//                                        status = cr.Degree >= c.MinDegree ? "Passed" : "Failed",
//                                        color = cr.Degree >= c.MinDegree ? "green" : "red"
//                                    }).ToList();

//            if (!traineesInCourse.Any())
//            {
//                return NotFound("No trainees found for this course.");
//            }

//            return View("CoursesTrainees", traineesInCourse);
//        }





//        //=================================================================////

//        public IActionResult SetSessionAndCookie(string traineeName)
//        {
//            HttpContext.Session.SetString("Trainee Name session", traineeName);

//            CookieOptions options = new CookieOptions
//            {
//                Expires = DateTimeOffset.Now.AddMinutes(30)
//            };

//            HttpContext.Response.Cookies.Append("TraineeNamecookie", traineeName, options);

//            return Content("Session and cookie set!");
//        }


//        public IActionResult GetSessionAndCookie()
//        {
//            var sessionValue = HttpContext.Session.GetString("Trainee Name session");
//            var cookieValue = HttpContext.Request.Cookies["TraineeNamecookie"];

//            if (string.IsNullOrEmpty(sessionValue))
//                sessionValue = "No session value found.";

//            if (string.IsNullOrEmpty(cookieValue))
//                cookieValue = "No cookie value found.";

//            return Content($"Data from Session: {sessionValue}\nData from Cookie: {cookieValue}");
//        }

//    }


//} 
#endregion


namespace project.Controllers
{
    public class TraineeController : Controller
    {
        private readonly ITraineeRepository traineeRepo;

        public TraineeController(ITraineeRepository traineeRepository)
        {
            traineeRepo = traineeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Result(int Tid, int cid)
        {
            CourseResultVM CR = new CourseResultVM();
            var result = traineeRepo.GetTraineeCoursesWithResults(Tid, cid);

            foreach (var item in result)
            {
                CR.T_id = Tid;
                CR.c_id = cid;
                CR.TraineeName = item.TraineeName;
                CR.CourseName = item.CourseName;
                CR.urlImage = item.urlImage;
                CR.degree = item.degree;
                CR.minDegree = item.minDegree;

                if (item.degree >= CR.minDegree)
                {
                    CR.status = "Passed";
                    CR.color = "green";
                }
                else
                {
                    CR.status = "Failed";
                    CR.color = "red";
                }
            }
            return View("Result", CR);
        }

        public IActionResult ShowAllTrainees()
        {
            var trainees = traineeRepo.GetAllWithDepartments();
            return View("ShowAllTrainees", trainees);
        }

        public IActionResult DetailsTrainee(int tid)
        {
            var trainee = traineeRepo.GetByIdWithDepartment(tid);

            if (trainee == null)
                return NotFound("Trainee not found.");

            var courseResults = traineeRepo.GetTraineeDetailsWithResults(tid);

            var viewModel = new TraineeWithResultsVM
            {
                Id = trainee.Id,
                TraineeName = trainee.TraineeName,
                Image = trainee.Image,
                Address = trainee.Address,
                Grade = trainee.grade,
                DepartmentName = trainee.Department?.Name,
                CourseResults = courseResults
            };

            return View("DetailsTrainee", viewModel);
        }

        public IActionResult CoursesTrainees(int id)
        {
            var traineesInCourse = traineeRepo.GetTraineesInCourse(id);

            if (!traineesInCourse.Any())
            {
                return NotFound("No trainees found for this course.");
            }

            return View("CoursesTrainees", traineesInCourse);
        }

        // Session and Cookie methods remain the same
        public IActionResult SetSessionAndCookie(string traineeName)
        {
            HttpContext.Session.SetString("Trainee Name session", traineeName);

            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(30)
            };

            HttpContext.Response.Cookies.Append("TraineeNamecookie", traineeName, options);

            return Content("Session and cookie set!");
        }

        public IActionResult GetSessionAndCookie()
        {
            var sessionValue = HttpContext.Session.GetString("Trainee Name session");
            var cookieValue = HttpContext.Request.Cookies["TraineeNamecookie"];

            if (string.IsNullOrEmpty(sessionValue))
                sessionValue = "No session value found.";

            if (string.IsNullOrEmpty(cookieValue))
                cookieValue = "No cookie value found.";

            return Content($"Data from Session: {sessionValue}\nData from Cookie: {cookieValue}");
        }
    }
}




