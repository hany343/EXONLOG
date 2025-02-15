using Blazored.Toast;
using EXONLOG;
using EXONLOG.Components;
using EXONLOG.Data;
using EXONLOG.Model.Account;
using EXONLOG.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

var ProductionString = builder.Configuration.GetConnectionString("Production");
var primaryConnectionString = builder.Configuration.GetConnectionString("homeString");
var secondaryConnectionString = builder.Configuration.GetConnectionString("alexString");


string activeConnectionString;
string serverName;
if (await ConnectionHelper.TestConnectionAsync(ProductionString))
{
    activeConnectionString = ProductionString;
    serverName = "Online";
}
else if(await ConnectionHelper.TestConnectionAsync(primaryConnectionString))
{
    activeConnectionString = primaryConnectionString;
    serverName = "Home";
}
else if (await ConnectionHelper.TestConnectionAsync(secondaryConnectionString))
{
    activeConnectionString = secondaryConnectionString;
    serverName = "Alex";
}
else
{
    throw new Exception("Both connection strings failed to connect to the database.");
}


builder.Services.AddDbContext<EXONContext>(options =>
    options.UseSqlServer(activeConnectionString));

// Register the active connection string in the service container
builder.Services.AddSingleton(new ConnectionStringService
{
    ActiveConnectionString = serverName
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



// Register the MaterialService
//builder.Services.AddScoped<MaterialService>();

//// Add services to the container.
//builder.Services.AddDbContext<EXONContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("homeString")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add services to the container.OrderService
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpContextAccessor(); // Required for IHttpContextAccessor

builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OutLadingService>();

builder.Services.AddScoped<MaterialTypeService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<StockService>();

//////////////// Trans Service  /////////
///
builder.Services.AddScoped<TruckService>();
builder.Services.AddScoped<DriverService>();
builder.Services.AddScoped<TransCompanyService>();


//////////////// Inbound Service  /////////
///
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<PortService>();
builder.Services.AddScoped<BatchService>();
builder.Services.AddScoped<InLadingService>();

builder.Services.AddSingleton<FFmpegService>();



builder.Services.AddBlazoredToast();

builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseStaticFiles(); // Serve wwwroot files

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

                RoleName = "Admin" // Assuming the Role model has a MaterialName property },
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
            FullName = "Admin", // MaterialName of the user
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
