//using project.Models;
//using System.ComponentModel.DataAnnotations;

//namespace project.ModelViews
//{
//    public class CourseVM
//    {
//        public int Id;

//        [Display(Name = "Course Name")]
//        public string? CourseName;

//        [Display(Name = "Degree")]
//        public int? Degree;

//        [Display(Name = "Minimum Degree")]
//        public int? MinDegree;

//        [Display(Name = "Credit Hours")]
//        public int? Hours;

//        [Display(Name = "Department")]
//        public string? DepartmentName;

//        [Display(Name = "Department")]

//        public int DepartmentId;

//        public List<Department>? Departments;

//        [Display(Name = "Course Image")]
//        public string? Pic;
//    }
//}

using project.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
//using project.ValidationAttributes.project.Attributes;

namespace project.ModelViews
{
    public class CourseVM
    {
        public int Id;

        [UniqueCourseName]
        [Display(Name = "Course Name")]
        public string? CourseName;

        [Display(Name = "Degree")]
        [Range(50, 100, ErrorMessage = "Degree must be between 50 and 100.")]
        public int? Degree;

        [Display(Name = "Minimum Degree")]
        public int? MinDegree;

        [Display(Name = "Credit Hours")]
        [DivisibleBy(3, ErrorMessage = "Credit hours must be divisible by 3.")]
        public int? Hours;

        [Display(Name = "Department")]
        public string? DepartmentName;

        [Display(Name = "Department")]
        public int DepartmentId;

        public List<Department>? Departments;

        [Display(Name = "Course Image")]
        public string? Pic;
    }

    public class UniqueCourseNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var courseName = value as string;

            if (string.IsNullOrWhiteSpace(courseName))
                return new ValidationResult("Course name is required.");

            if (courseName.Length < 2 || courseName.Length > 20)
                return new ValidationResult("Course name must be between 2 and 20 characters.");

            var db = validationContext.GetService<ProjectContext>();
            if (db == null)
                throw new InvalidOperationException("Could not resolve ProjectContext from services.");

            bool exists = db.Courses.Any(c => c.CourseName == courseName);
            if (exists)
                return new ValidationResult("Course name must be unique.");

            return ValidationResult.Success;
        }
    }

    public class DivisibleByAttribute : ValidationAttribute
    {
        private readonly int _divisor;
        public DivisibleByAttribute(int divisor)
        {
            _divisor = divisor;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (value is int intValue && intValue % _divisor == 0)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"Value must be divisible by {_divisor}.");
        }
    }
}

