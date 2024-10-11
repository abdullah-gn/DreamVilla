using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamVilla_VillaApi.Controllers
{
	[Route("ErrorHandling")]
	[ApiController]
	[AllowAnonymous]
	[ApiVersionNeutral]
	[ApiExplorerSettings(IgnoreApi =true)]
	public class ErrorHandlingController : ControllerBase
	{
		[Route("ProcessError")]
		public IActionResult ProcessError([FromServices] IHostEnvironment hostEnvironment)
		{
			if (hostEnvironment.IsDevelopment())
			{
				//Custom details for environment problem 
				var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
				return Problem(
				instance: hostEnvironment.EnvironmentName,
				title: feature.Error.Message,
				detail: feature.Error.StackTrace
							);
			}
			else
			{
				return Problem();
			}
		}
	}
}
