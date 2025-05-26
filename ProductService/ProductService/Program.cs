using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess;
using ProductService.Dtos;
using ProductService.FluentValidation;
using ProductService.Middlewares;
using ProductService.Repositories;
using ProductService.Repositories.Interfaces;
using ProductService.Services;
using ProductService.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductServiceImpl>();
builder.Services.AddScoped<IValidator<CreateProductDto>, ProductValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention()
    .LogTo(Console.WriteLine, LogLevel.Information);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()              
    .WriteTo.Console()                       
    .WriteTo.File("logs/productservice-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
