using Arbetsprov.Models.Exceptions;
using Microsoft.AspNetCore.Http.Headers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Arbetsprov.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate Next;
		public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
		{
			Next = requestDelegate;
		}
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await Next(context).ConfigureAwait(false);
			}
			catch (NotFoundException notFoundException)
			{
				await GenerateResponseForNotFoundException(context, notFoundException).ConfigureAwait(false);
			}
			catch (Exception unhandledException)
			{
				await GenerateResponseForUnhandledException(context, unhandledException).ConfigureAwait(false);
				//Here I would add a loggin functionality if it where real project. 

				return;
			}
			
		}
		private static Task GenerateResponseForUnhandledException(HttpContext context, Exception ex)
		{
		
			var errorMessage = JsonConvert.SerializeObject(new { Message = ex.Message });

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(errorMessage);
		}
		private static Task GenerateResponseForNotFoundException(HttpContext context, Exception ex)
		{

			var errorMessage = JsonConvert.SerializeObject(new { Message = ex.Message });

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			return context.Response.WriteAsync(errorMessage);
		}
	} 
}

