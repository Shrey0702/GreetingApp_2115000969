using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Context;
using Midddleware.GlobalExceptionHandling;
using Middleware.HashingAlgo;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Add global exception filter
    options.Filters.Add<GlobalExceptionFilter>();
});



// for NLOGGING
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<GreetingAppContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddControllers();
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL, GreetingRL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IHashingService, HashingService>();

// Configure the HTTP request pipeline.


// Add swagger to container
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseMiddleware<Middlerware.GlobalExceptionHandling.ExceptionHandlerMiddleware>();


app.UseSwagger();

app.UseSwaggerUI();//reponsible for the colorfulness

// Configure the HTTP request pipeline.


app.UseAuthorization();

app.MapControllers();

app.Run();
