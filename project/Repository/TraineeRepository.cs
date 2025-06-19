using Microsoft.EntityFrameworkCore;
using project.Models;
using project.ModelViews;



namespace project.Repository
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly ProjectContext db;

        public TraineeRepository(ProjectContext context)
        {
            db = context;
        }

        public List<Trainee> GetAll(string? include)
        {
            if (!string.IsNullOrEmpty(include))
            {
                return db.Trainees.Include(include).ToList();
            }
            return db.Trainees.ToList();
        }

        public Trainee GetById(int id)
        {
            return db.Trainees.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Trainee entity)
        {
            db.Trainees.Add(entity);
        }

        public void Update(Trainee entity)
        {
            db.Trainees.Update(entity);
        }

        public void Delete(Trainee entity)
        {
            db.Trainees.Remove(entity);
        }

        public bool Delete(int id)
        {
            var trainee = GetById(id);
            if (trainee != null)
            {
                Delete(trainee);
                save();
                return true;
            }
            return false;
        }

        public void save()
        {
            db.SaveChanges();
        }

        //  method for ShowAllTrainees action
        public List<Trainee> GetAllWithDepartments()
        {
            return db.Trainees.Include(t => t.Department).ToList();
        }

        //  method for DetailsTrainee action
        public Trainee GetByIdWithDepartment(int id)
        {
            return db.Trainees
                     .Include(t => t.Department)
                     .FirstOrDefault(t => t.Id == id);
        }

        //  method for Result action
        public List<CourseResultVM> GetTraineeCoursesWithResults(int traineeId, int courseId)
        {
            return (from cr in db.CourseResults
                    join t in db.Trainees on cr.TraineeId equals t.Id
                    join c in db.Courses on cr.CourseId equals c.Id
                    where cr.TraineeId == traineeId && cr.CourseId == courseId
                    select new CourseResultVM
                    {
                        TraineeName = t.TraineeName,
                        CourseName = c.CourseName,
                        degree = t.grade,
                        urlImage = t.Image,
                        minDegree = (int)c.MinDegree
                    }).ToList();
        }

        public List<CourseResultVM> GetTraineeDetailsWithResults(int traineeId)
        {
            return (from cr in db.CourseResults
                    join c in db.Courses on cr.CourseId equals c.Id
                    where cr.TraineeId == traineeId
                    select new CourseResultVM
                    {
                        CourseName = c.CourseName,
                        degree = cr.Degree,
                        status = cr.Degree >= c.MinDegree ? "Passed" : "Failed",
                        color = cr.Degree >= c.MinDegree ? "green" : "red"
                    }).ToList();
        }

        public List<CourseResultVM> GetTraineeInCourse(int courseId)
        {
            return (from cr in db.CourseResults
                    join t in db.Trainees on cr.TraineeId equals t.Id
                    join c in db.Courses on cr.CourseId equals c.Id
                    where c.Id == courseId
                    select new CourseResultVM
                    {
                        T_id = t.Id,
                        c_id = c.Id,
                        TraineeName = t.TraineeName,
                        CourseName = c.CourseName,
                        degree = cr.Degree,
                        urlImage = t.Image,
                        status = cr.Degree >= c.MinDegree ? "Passed" : "Failed",
                        color = cr.Degree >= c.MinDegree ? "green" : "red"
                    }).ToList();
        }

        List<CourseResultVM> ITraineeRepository.GetTraineesInCourse(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}

