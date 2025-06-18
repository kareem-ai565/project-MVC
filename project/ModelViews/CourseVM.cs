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

namespace project.ModelViews
{
    public class CourseVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Course name must be between 2 and 20 characters.")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100.")]
        public int? Degree { get; set; }

        [Display(Name = "Minimum Degree")]
        public int? MinDegree { get; set; }

        [Display(Name = "Credit Hours")]
        [DivisibleByThree]
        public int? Hours { get; set; }

        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }

        public int DepartmentId { get; set; }

        public List<Department>? Departments { get; set; }

        [Display(Name = "Course Image")]
        public string? Pic { get; set; }
    }

    public class CourseNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var courseName = value as string;
            if (string.IsNullOrWhiteSpace(courseName) || courseName.Length < 2 || courseName.Length > 20)
            {
                return new ValidationResult("Course name must be between 2 and 20 characters.");
            }

            // Check uniqueness
            var context = (project.Models.ProjectContext)validationContext.GetService(typeof(project.Models.ProjectContext))!;
            var vm = (project.ModelViews.CourseVM)validationContext.ObjectInstance;

            var exists = context.Courses.Any(c => c.CourseName == courseName && c.Id != vm.Id);
            if (exists)
            {
                return new ValidationResult("Course name must be unique.");
            }

            return ValidationResult.Success;
        }
    }


    public class DegreeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int degree)
            {
                if (degree <= 50 || degree >= 100)
                {
                    return new ValidationResult("Degree must be between 50 and 100.");
                }
            }
            return ValidationResult.Success;
        }
    }
    public class DivisibleByThreeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int hours && hours % 3 != 0)
            {
                return new ValidationResult("Hours must be divisible by 3.");
            }
            return ValidationResult.Success;
        }




    }
}