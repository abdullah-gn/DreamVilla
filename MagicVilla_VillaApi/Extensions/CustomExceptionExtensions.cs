﻿using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace DreamVilla_VillaApi.Extensions
{
	public static class CustomExceptionExtensions
	{
		public static void ErrorHandle(this IApplicationBuilder app, bool isDevelopment)
		{
			app.UseExceptionHandler(error =>

	error.Run(async context =>
	{
		context.Response.StatusCode = 500;
		context.Response.ContentType = "application/json";
		var feature = context.Features.Get<IExceptionHandlerFeature>();
		if (feature != null)
		{
			if (isDevelopment)
			{
				if (feature.Error is BadImageFormatException badImageFormatException)
				{
					await context.Response.WriteAsync(JsonConvert.SerializeObject(new
					{
						// If you have custom exception where you are passing status code then you can pass here

						StatusCode = 776,
						ErrorMessage = "Custom error message for the status code",
					}));
				}
				else
				{
					await context.Response.WriteAsync(JsonConvert.SerializeObject(new
					{
						StatusCode = context.Response.StatusCode,
						ErrorMessage = feature.Error.Message,
						StackTrace = feature.Error.StackTrace
					}));
				}

			}
			else
			{
				await context.Response.WriteAsync(JsonConvert.SerializeObject(new
				{
					StatusCode = context.Response.StatusCode,
					ErrorMessage = "Custom Error Message from pipeline"
				}));
			}

		}
	}
));

		}
	}
}
