using EXONLOG.Components;
using EXONLOG.Data;
using EXONLOG.Model.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EXONContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EXONContext>();

    // Ensure database is created
    dbContext.Database.EnsureCreated();

    // Seed roles
    if (!dbContext.Roles.Any())
    {
        dbContext.Roles.AddRange(
            new Role
            {

                RoleName = "Admin" // Assuming the Role model has a Name property },
            }
        );
        dbContext.SaveChanges();
    }

        // Helper function to hash passwords
  string HashPassword(string password)
{
    var passwordHasher = new PasswordHasher<User>();
    return passwordHasher.HashPassword(null, password);
}
// Seed users
if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new User
        {

            Username = "admin",
            FullName = "Admin", // Name of the user
            Email = "admin@exonlog.com", // Admin email
            PasswordHash = HashPassword("admin"), // Use a secure hashed password
            MobileNumber = "01102090157",
            RoleId = 1, // Link to the Admin role
            CreateDate = DateTime.UtcNow, // Default CreateDate
            IsActive = true // Active user
        });
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
