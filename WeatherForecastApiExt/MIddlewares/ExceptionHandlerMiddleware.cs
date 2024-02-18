using System.Net;

namespace WeatherForecastApiExt.MIddlewares
{

	//EXPL: this class added as an extension method to the ApplicationBuilder, allows us to better understand what went wrong in case of errors
	public class ExceptionHandlerMiddleware
	{

		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlerMiddleware> _logger;

		public ExceptionHandlerMiddleware(RequestDelegate next,
										  ILogger<ExceptionHandlerMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"An error occured. Details: {ex.Message}");
				httpContext.Response.ContentType = "application/json";
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await httpContext.Response.WriteAsync(new
				{
					httpContext.Response.StatusCode,
					ex.Message
				}.ToString());
			}

		}
	}

	// EXPL: Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlerMiddleware>();
		}
	}

}
