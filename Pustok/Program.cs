using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;

namespace Pustok;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddControllersWithViews();

		builder.Services.AddDbContext<PustokContext>(cfg =>
			cfg.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"))
			);

		builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
		{
			option.User.RequireUniqueEmail = true;
			option.Password.RequireUppercase = true;
			option.Password.RequireDigit = true;
			option.Password.RequiredLength = 8;
			option.Password.RequireNonAlphanumeric = true;
		}).AddEntityFrameworkStores<PustokContext>().AddDefaultTokenProviders();

		var app = builder.Build();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}
		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.MapControllerRoute(
			  name: "areas",
			  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
			);

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}
