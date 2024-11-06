using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using EmployeeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EmployeeManagementDb"));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the Admin user
SeedAdminUser(app);

void SeedAdminUser(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var role = new IdentityRole("Admin");
    roleManager.CreateAsync(role).Wait();

    var user = userManager.FindByNameAsync("admin").Result;

    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            FullName = "Admin User"
        };

        var result = userManager.CreateAsync(user, "AdminPassword123!").Result;

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(user, "Admin").Wait();
        }
    }
}

// Configure the middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // Enable Authentication
app.UseAuthorization();   // Enable Authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
