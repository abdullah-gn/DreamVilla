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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

////Serilog confugration
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/VillaLog.txt",rollingInterval: RollingInterval.Day).CreateLogger();
////Tell app that we use Serilogger instead of the default logger
//builder.Host.UseSerilog();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection")));

builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Description =
        "JWT Authorization header using the Bearer schema .\r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the text input Below.\r\n\r\n" +
        "Example : \"Bearer 123456789abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {

        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }

    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Magic Villa Ver1",
        Description = "Magic Villa Project Api",
        TermsOfService = new Uri("https://example.come/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contacts",
            Url = new Uri("https://example.come/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://example.come/License"),
        }

    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Magic Villa Ver2",
        Description = "Magic Villa Project Api",
        TermsOfService = new Uri("https://example.come/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contacts",
            Url = new Uri("https://example.come/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://example.come/License"),
        }

    });
});

builder.Services.AddScoped<IVillaRepository,VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAuthentication(x =>
{


    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x=>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
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
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
