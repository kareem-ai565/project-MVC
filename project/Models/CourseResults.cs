using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class CourseResults
    {
        public int Id { get; set; }
        public int Degree { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; } //fk to Course
        public Course Course { get; set; }

        [ForeignKey("Trainee")]
        public int TraineeId { get; set; } // fk to Trainee
        public Trainee Trainee { get; set; }


    }
}
