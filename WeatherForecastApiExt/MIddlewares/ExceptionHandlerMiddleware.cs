using System.Net;

namespace WeatherForecastApiExt.MIddlewares
{
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

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlerMiddleware>();
		}
	}

}
