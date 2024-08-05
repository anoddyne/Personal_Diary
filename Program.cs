using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Personal_Diary;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages and logging services
builder.Services.AddRazorPages();
builder.Services.AddLogging();

// Clear existing logging providers and add console and debug logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add configuration for logging and event source logging
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddEventSourceLogger();

// Configure web encoder options
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

// Add a hosted service for updating task status
builder.Services.AddHostedService<TaskStatusUpdateService>();

// Get the database connection string from configuration
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add the PersonalDiaryDbContext to the service provider
builder.Services.AddDbContext<PersonalDiaryDbContext>(options => options.UseSqlite(connection));

// Build the application
var app = builder.Build();

// Initialize the database with seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PersonalDiaryDbContext>();
    DbInitializer.Initialize(context);
}

// Configure the application to use HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure the application to use HTTPS redirection
app.UseHttpsRedirection();

// Configure the application to serve static files
app.UseStaticFiles();

// Configure the application to use routing
app.UseRouting();

// Configure the application to use authorization
app.UseAuthorization();

// Map Razor Pages routes
app.MapRazorPages();

// Run the application
app.Run();