using Microsoft.EntityFrameworkCore;
using project.Models;
using project.ModelViews;

namespace project.Repository
{
    public interface ITraineeRepository : IRepository<Trainee>
    {
        public bool Delete(int id);
        public List<Trainee> GetAllWithDepartments();
        public Trainee GetByIdWithDepartment(int id);
        public List<CourseResultVM> GetTraineeCoursesWithResults(int traineeId, int courseId);
        public List<CourseResultVM> GetTraineesInCourse(int courseId);
        public List<CourseResultVM> GetTraineeDetailsWithResults(int traineeId);
    }

}