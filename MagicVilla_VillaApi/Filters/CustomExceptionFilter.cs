using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace DreamVilla_VillaApi.Filters
{
	public class CustomExceptionFilter : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is FileNotFoundException)
			{
				context.Result = new ObjectResult("File not found but handled in filter")
				{
					StatusCode = 503
				};
				//To end exception hanlding to continued 
				context.ExceptionHandled = true;
			}
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			
		}
	}
}
