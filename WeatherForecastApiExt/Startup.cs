using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherForecastApiExt.Contexts;
using WeatherForecastApiExt.MIddlewares;

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

			//EXPL: here we make available the WeatherDbContext for later injection...
			//the connection string with the credentials is located into the appsettings.json file
			//normally in production it would not be written there but passed as environment variable or in other sercret ways.
			services.AddDbContext<WeatherDbContext>(
				x => x.UseNpgsql(Configuration.GetConnectionString("WeatherDatabase")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
		{

			//EXPL: here we use our extension method
			app.UseExceptionHandlerMiddleware();

			//EXPL: perform the actual database creation and initialization.
			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var context = serviceScope.ServiceProvider.GetRequiredService<WeatherDbContext>();			
			context.Database.Migrate();

			logger.LogInformation("Database Migrated sucessfully");

			//EXPL: normally swagger should not be presented in production :-(
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
