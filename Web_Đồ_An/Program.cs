using Microsoft.EntityFrameworkCore;
using Web_Đồ_An.Models.Entities;

namespace Web_Đồ_An
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			var connectionString = builder.Configuration.GetConnectionString("BanhNgotTMConnection");
			builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

			//cấu hình sử dụng session
			builder.Services.AddDistributedMemoryCache();

			//Đăng ký dịch vụ cho HttpContext
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(10);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
				options.Cookie.Name = ".Dev.Session";
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			//sử dụng session đã khai báo
			app.UseSession();


			app.MapControllerRoute(
			   name: "areas",
			   pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}