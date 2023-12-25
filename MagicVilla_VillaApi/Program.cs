//using Serilog;

//using DreamVilla_VillaApi.Logging;

using AutoMapper;
using DreamVilla_VillaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using DreamVilla_VillaApi;
using DreamVilla_VillaApi.Repository.IRepository;
using DreamVilla_VillaApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

////Serilog confugration
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/VillaLog.txt",rollingInterval: RollingInterval.Day).CreateLogger();
////Tell app that we use Serilogger instead of the default logger
//builder.Host.UseSerilog();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection")));

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers(/*Option =>Option.ReturnHttpNotAcceptable=true*/).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVillaRepository,VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
//builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
