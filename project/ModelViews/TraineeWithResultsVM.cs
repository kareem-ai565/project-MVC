namespace project.ModelViews
{
    public class TraineeWithResultsVM
    {
        public int Id { get; set; }
        public string TraineeName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int Grade { get; set; }
        public string DepartmentName { get; set; }

        public List<CourseResultVM> CourseResults { get; set; }
    }

}
