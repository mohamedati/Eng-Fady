using ERP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using ERP.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register DbContext for Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register DbContext for the application
builder.Services.AddDbContext<ErpservicesContext>(options =>
    options.UseSqlServer(connectionString));

// Add developer page exception filter for detailed errors in development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register ViewModel as a scoped service
builder.Services.AddScoped<ViewModel>();

// Configure Identity with advanced options
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
})
.AddRoles<IdentityRole>() // Enable role management
.AddDefaultTokenProviders() // Add token providers for password reset, etc.
.AddEntityFrameworkStores<ApplicationDbContext>(); // Use ApplicationDbContext for Identity storage

// Configure the application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Set the custom login path
});


// Add controllers with views and Newtonsoft.Json for JSON handling
builder.Services.AddControllersWithViews().AddNewtonsoftJson();

// Configure localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add Razor Pages with custom model binding message
builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "NULL");
    });

// Add MVC with view localization
builder.Services.AddMvc()
    .AddViewLocalization();

// Configure request localization options
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "ar", uiCulture: "ar-EG");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

var app = builder.Build();

// Enforce HTTPS
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage(); // Show detailed database errors in development
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HTTP Strict Transport Security (HSTS)
}

// Configure request localization middleware
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Serve static files (e.g., CSS, JS, images)
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages
app.MapRazorPages();

// Run the application
app.Run();