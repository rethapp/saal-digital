using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherForecastApiExt.MIddlewares;
using WeatherForecastApiExt.Models;

namespace WeatherForecastApiExt
{
	public class Startup
	{

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherForecastAPI", Version = "v2" });
			});
			services.AddDbContext<WeatherDbContext>(
				x => x.UseNpgsql(Configuration.GetConnectionString("WeatherDatabase")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
		{
			app.UseExceptionHandlerMiddleware();

			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var context = serviceScope.ServiceProvider.GetRequiredService<WeatherDbContext>();			
			context.Database.Migrate();

			logger.LogInformation("Database Migrated sucessfully");

			//if (env.IsDevelopment())
			//{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherForecastAPIExt v2"));
			//}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
