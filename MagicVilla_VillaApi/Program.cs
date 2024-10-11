using DreamVilla_VillaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using DreamVilla_VillaApi;
using DreamVilla_VillaApi.Repository.IRepository;
using DreamVilla_VillaApi.Repository;
using DreamVilla_VillaApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Writers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DreamVilla_VillaApi.Filters;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using DreamVilla_VillaApi.Extensions;
using DreamVilla_VillaApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

////Serilog confugration
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/VillaLog.txt",rollingInterval: RollingInterval.Day).CreateLogger();
////Tell app that we use Serilogger instead of the default logger
//builder.Host.UseSerilog();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddApiVersioning(Options =>
{
	Options.AssumeDefaultVersionWhenUnspecified = true;
	Options.DefaultApiVersion = new ApiVersion(1, 0);
	Options.ReportApiVersions = true;
});
//SecretKey
builder.Services.AddVersionedApiExplorer(opt =>
{
	opt.GroupNameFormat = "'v'VVV";
	//add version in the url of the endpoint
	opt.SubstituteApiVersionInUrl = true;

});

var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");

builder.Services.AddControllers(/*Option =>Option.ReturnHttpNotAcceptable=true*/).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAuthentication(x =>
{


	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidIssuer = "https://magicvilla-api.com",
		ValidAudience = "https://test-magicvilla-api.com",
		//if it's a 1 sec diff for the token then it's expired and make sure there are 0 tolerance sec for the token expiry
		ClockSkew = TimeSpan.Zero
	};
});

builder.Services.AddMemoryCache();
builder.Services.AddControllers(options =>
{
	// options.CacheProfiles.Add("default30", new CacheProfile
	// {
	//     Duration = 30
	// });

	options.Filters.Add<CustomExceptionFilter>();

}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters()
  .ConfigureApiBehaviorOptions(options =>
  options.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
  {
	  // We can add for non default status code return 
	  Link = "https://Errorreference.com",
	  Title = "For more Details",
  });

//builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic Villa V2");
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic Villa V1");
	});
}

//Exception handler middleware
//app.UseExceptionHandler("/ErrorHandling/ProcessError");

// Custom Exception handling but using ExceptionHandler isnide.
//app.ErrorHandle(app.Environment.IsDevelopment());

// My custom Middlware
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplayDbMigrations();
app.Run();

void ApplayDbMigrations()
{
	using (var scope = app.Services.CreateScope())
	{
		var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		if (_dbContext.Database.GetPendingMigrations().Count() > 0)
		{
			_dbContext.Database.Migrate();
		}
	}
}