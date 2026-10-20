using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Middleware;
using RaceDayAPI.Services;
using System.Reflection;

// Set up the web application builder
var builder = WebApplication.CreateBuilder(args);

// Add controllers to the DI container
builder.Services.AddControllers();

// Set up session management - we need this for auth
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Session expires after 30 minutes of inactivity
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Connect to the SQL Server database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register the auth service so controllers can use it
builder.Services.AddScoped<IAuthService, AuthService>();

// Set up Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RaceDay API",
        Version = "v1",
        Description = "Event management system for South African road running, walking, and cycling events"
    });

    // Pull in XML comments from the code for better Swagger docs
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Enable CORS so the frontend can talk to this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Build the app
var app = builder.Build();

// Set up the middleware pipeline
if (app.Environment.IsDevelopment())
{
    // Only show Swagger in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RaceDay API v1");
    });
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowAll");

// Enable session support
app.UseSession();

// Use our custom authentication middleware to check sessions
app.UseMiddleware<SessionAuthMiddleware>();

// Standard authorization
app.UseAuthorization();

// Map the controller routes
app.MapControllers();

// Start the application
app.Run();
