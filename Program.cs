using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Arcade.Models;
using Arcade_.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Arcade.Hubs;

namespace Arcade_ {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddSignalR();
            string con = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;Integrated Security=True;";

            builder.Services.AddSingleton<IRepository<Game>>(sp => new GenericRepository<Game>(con));
            builder.Services.AddSingleton<IRepository<Customer>>(sp => new GenericRepository<Customer>(con));
            builder.Services.AddSingleton<IRepository<Review>>(sp => new GenericRepository<Review>(con));
            builder.Services.AddSingleton<IRepository<UsersImage>>(sp => new GenericRepository<UsersImage>(con));
            builder.Services.AddSingleton<IRepository<CustomerGame>>(sp => new GenericRepository<CustomerGame>(con));
            builder.Services.AddSingleton<IRepository<Faq>>(sp => new GenericRepository<Faq>(con));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            });


            var app = builder.Build();

            app.MapHub<CartHub>("/cartHub");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseMigrationsEndPoint();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Init}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
