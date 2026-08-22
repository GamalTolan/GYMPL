using GYMBLL;
using GYMDAL.Data.Contexts;
using GYMDAL.Data.DataSeed;
using GYMDAL.Repositories.Classes;
using GYMDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYMPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
             {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

             });

            builder.Services.AddScoped < IUnitOfWork , UnitOfWork>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingaProfile));
            

            var app = builder.Build();

            #region Seed Data

            using var scope = app.Services.CreateScope();
            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>(); 
            GymDataSeed.DataSeed(gymDbContext);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
