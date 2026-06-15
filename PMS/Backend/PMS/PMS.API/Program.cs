using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.API.Middlewares;
using PMS.Application.Interfaces;
using PMS.Application.Mappings;
using PMS.Application.Services;
using PMS.Application.Validators;
using PMS.Infrastructure.Repositories;
using PMS.Infrastructure.Services;
using PMS.Persistence.Repositories;
using ProductManagementSystem.Persistence.Context;
using System.Text;
using Serilog;

//logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Swagger 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PMS API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer your_token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

//CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("Demo", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });

var app = builder.Build();
Log.Information("Application Started");

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Demo");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();