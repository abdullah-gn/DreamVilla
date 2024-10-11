using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DreamVilla_VillaApi
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
		{
			_apiVersionDescriptionProvider = apiVersionDescriptionProvider;
		}

		public void Configure(SwaggerGenOptions options)
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

			foreach (var descr in _apiVersionDescriptionProvider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(descr.GroupName, new OpenApiInfo
				{
					Version = descr.ApiVersion.ToString(),
					Title = $"Magic Villa {descr.ApiVersion}",
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
			}


		}
	}
}
