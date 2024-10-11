
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace DreamVilla_VillaApi.Middlewares
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _requestDelegate;

		public CustomExceptionMiddleware(RequestDelegate requestDelegate)
		{
			_requestDelegate = requestDelegate;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _requestDelegate(context);
			}
			catch (Exception ex)
			{
				await ProcessException(context, ex);
			}
		}

		private async Task ProcessException(HttpContext context, Exception ex)
		{
			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";


			if (ex is BadImageFormatException badImageFormatException)
			{
				await context.Response.WriteAsync(JsonConvert.SerializeObject(new
				{
					// If you have custom exception where you are passing status code then you can pass here

					StatusCode = 776,
					ErrorMessage = "Custom error message From custom Middleware",
				}));
			}
			else
			{
				await context.Response.WriteAsync(JsonConvert.SerializeObject(new
				{
					StatusCode = context.Response.StatusCode,
					ErrorMessage = "New Hello from custom Middleware"
				}));
			}
		}
	}
}
