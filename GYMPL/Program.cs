using GYMBLL;
using GYMBLL.Services.Classes;
using GYMBLL.Services.Interfaces;
using GYMDAL.Data.Contexts;
using GYMDAL.Data.DataSeed;
using GYMDAL.Entities;
using GYMDAL.Repositories.Classes;
using GYMDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
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
            builder.Services.AddScoped < ISessionRepository , SessionRepository>();
            builder.Services.AddScoped<IAnalyticService, AnalyticService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase= true;
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Accessdenied";
                options.LoginPath = "/Account/Login";
            });



            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingaProfile()));
            

            var app = builder.Build();

            #region Seed Data

            using var scope = app.Services.CreateScope();
            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager =scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            GymDataSeed.DataSeed(gymDbContext);
            IdentityDataSeeding.SeedData(roleManager, userManager);
            
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
