using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Repositories;
using ExpenseTrackerCrudWebAPI.Uow;
using ExpenseTrackerCrudWebAPI.Filters;
using ExpenseTrackerCrudWebAPI.Middleware;

using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text;
using AutoWrapper;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set logging level
    .WriteTo.Console() // Log to Console
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to file
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog instead of default logging


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


var keyString = builder.Configuration["Jwt:Key"];
Console.WriteLine("JWT KEY from config: " + (keyString ?? "[NULL]"));

if (string.IsNullOrEmpty(keyString))
{
    throw new Exception("JWT Key is missing or empty in configuration");
}

var key = Encoding.UTF8.GetBytes(keyString);


builder.Services.AddDbContext<ExpenseTrackerDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExpenseTrackerDB")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ExpenseTrackerDBContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

//Api Versioning


builder.Services.AddApiVersioning(options =>


{


    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version = v1.0


    options.AssumeDefaultVersionWhenUnspecified = true;


    options.ReportApiVersions = true; // Adds API-Supported and API-Depricated headers

    // Choose one of these versioning strategies


    options.ApiVersionReader = ApiVersionReader.Combine(


        new QueryStringApiVersionReader("api-version"),            // /api/values?api-version=1.0


        new HeaderApiVersionReader("X-Version"),                   // Custom header


        new MediaTypeApiVersionReader("ver")                       // Accept: application/json; ver=1.0


    );


});


builder.Services.AddCors(corsoptions =>
{
    corsoptions.AddPolicy("MyPolicy", policyoptions =>
    {
        policyoptions.AllowAnyHeader()
                     .AllowAnyOrigin()
                     .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense Tracker API", Version = "v1" });

    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by space and JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<ISavingGoalsRepository, SavingGoalsRepository>();
builder.Services.AddScoped<ISourceRepository, SourceRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<ISavingGoalsService, SavingGoalsService>();

builder.Services.AddScoped<ISourceService, SourceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IBudgetService, BudgetService>();


builder.Services.AddAutoMapper(typeof(Program));



var app = builder.Build();
app.UseApiResponseAndExceptionWrapper();



app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
