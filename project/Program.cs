using Microsoft.EntityFrameworkCore;
using project.Models;
using project.Repository;

namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

          

            //services to the container.
            builder.Services.AddControllersWithViews();

            //  Distributed Memory Cache (required for session)
            builder.Services.AddDistributedMemoryCache();

            //dependency injection lab day 6
            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<ISearchRepository, SearchRepository>();
            builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();





            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            //request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

   
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

