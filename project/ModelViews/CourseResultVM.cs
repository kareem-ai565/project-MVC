namespace project.ModelViews
{
    public class CourseResultVM
    {
        public int T_id { get; set; }
        public int c_id { get; set; }
        public string? TraineeName { get; set; }
        public string? CourseName { get; set; }
        public int? degree { get; set; }

        public string ? urlImage { get; set; }

        public int minDegree { get; set; } = 60;
        public string? status { get; set; }     
        public string? color { get; set; }

    }
}
