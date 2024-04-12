using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Implements;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Services.Interfaces;
using Pustok.Services.Implements;
using Pustok.Data;
using Pustok.Helpers.Implements;
using Pustok.Helpers.Interfaces;
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

		builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<IEmailService, EmailService>();
		builder.Services.AddScoped<Pustok.Areas.Admin.Services.Interfaces.IProductService, Pustok.Areas.Admin.Services.Implements.ProductService>();
		builder.Services.AddScoped<Pustok.Services.Interfaces.IShopService, Pustok.Services.Implements.ShopService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
		builder.Services.AddScoped<IFileService, FileService>();

        builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
		{
			option.User.RequireUniqueEmail = true;
			option.Password.RequireUppercase = true;
			option.Password.RequireDigit = true;
			option.Password.RequiredLength = 8;
			option.Password.RequireNonAlphanumeric = true;
            option.SignIn.RequireConfirmedEmail = true;

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
			  name: "areas",
			  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}
