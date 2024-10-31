using Microsoft.EntityFrameworkCore;
using ProjectApp.Core;
using ProjectApp.Core.Interfaces;
using ProjectApp.Persistance;
using Microsoft.AspNetCore.Identity;
using ProjectApp.Areas.Identity.Data;
using ProjectApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Auction service and persistence
builder.Services.AddScoped<IAuctionService, AuctionService>();

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();

// Register AuctionDbContext with MySQL
builder.Services.AddDbContext<AuctionDbContext>(options => 
    options.UseMySQL(builder.Configuration.GetConnectionString("DbConnection")));

// Register Identity with ProjectAppUser and ProjectAppContext
builder.Services.AddDbContext<ProjectAppContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityDbConnection")));

// Add Identity only once
builder.Services.AddDefaultIdentity<ProjectAppUser>(options => 
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ProjectAppContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Make sure this is added before UseAuthorization
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();