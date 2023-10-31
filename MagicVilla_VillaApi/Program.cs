//using Serilog;

using DreamVilla_VillaApi.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

////Serilog confugration
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/VillaLog.txt",rollingInterval: RollingInterval.Day).CreateLogger();
////Tell app that we use Serilogger instead of the default logger
//builder.Host.UseSerilog();


//Custom logger

builder.Services.AddControllers(/*Option =>Option.ReturnHttpNotAcceptable=true*/).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
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
