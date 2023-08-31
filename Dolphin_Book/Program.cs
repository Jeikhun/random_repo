
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddHttpContextAccessor();
//builder.Services.Register(builder.Configuration);
builder.Services.AddDbContext<DolphinDbContext>(opt => {


    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 8;

    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
})
    .AddEntityFrameworkStores<DolphinDbContext>()

    .AddDefaultTokenProviders();


builder.Services.AddHttpContextAccessor();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//	options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
//	{
//		if (context.HttpContext.Request.Path.Value.StartsWith("/admin") || context.HttpContext.Request.Path.Value.StartsWith("/Admin"))
//		{
//			var redirectPath = new Uri(context.RedirectUri);
//			context.Response.Redirect("/admin/account/login" + redirectPath.Query);
//		}
//		else
//		{
//			var redirectPath = new Uri(context.RedirectUri);
//			context.Response.Redirect("/account/login" + redirectPath.Query);
//		}
//		return Task.CompletedTask;
//	};
//});




var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=account}/{action=login}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    await DbInitializer.SeedAsync(roleManager, userManager);
}


app.Run();
