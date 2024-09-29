using MagicVilla_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MagicVilla_Web.Extentions
{
	public class AuthExceptionRedirection : IExceptionFilter
	{
		void IExceptionFilter.OnException(ExceptionContext context)
		{
			if (context.Exception is AuthException)
			context.Result = new RedirectToActionResult("Login", "Auth", null);
		}
	}
}
