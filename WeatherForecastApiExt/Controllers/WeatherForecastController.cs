using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Net.Mime;
using WeatherForecastApiExt.Contexts;
using WeatherForecastApiExt.Models;

namespace WeatherForecastApiExt.Controllers
{

    //EXPL: this is the class that allows a client ( whatever in whatever language ) to communicate with our service
    //the constructor implements the DI pattern and the methods are relative to the different HTTP methods: GET, PUT, POST etc...
    // in the original code there was only a generic Get() method that returns all the weather forecasts records.
    //I implemented all the other methods to demonstrate the versatility of the controller
    //I did not write all the full variants that are possible for getting the parameters / body content, etc... FromQuery, FromBody...
    //Also exists different method to return result...
    [ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly WeatherDbContext _weatherDbContext;

		public WeatherForecastController(ILogger<WeatherForecastController> logger,
										WeatherDbContext weatherDbContext)
		{
			_logger = logger;
			_weatherDbContext = weatherDbContext;
		}

        //EXPL: delete a forecast info
        [HttpDelete("{id}")]
        [ProducesResponseType<WeatherForecast>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var data = _weatherDbContext.WeatherForecasts.Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                _weatherDbContext.WeatherForecasts.Remove(data);
                _weatherDbContext.SaveChanges();
                return Ok();
            } 
            else
            {
                return NotFound();
            }
            
        }

        //EXPL: bulk update for temperature, for example rectify the weather type: if temp == 0, for example --> summary must be "cold"        
        [HttpPut("{temp:int}/{summary}")]
        public async Task<IActionResult> BulkUpdateAsync(int temp, string summary)
        {
            try
            {

                await _weatherDbContext.Set<WeatherForecast>()                    
                    .Where(x => x.TemperatureC == temp)
                    .ExecuteUpdateAsync(x => x.SetProperty(p => p.Summary, summary));
                
                return Ok();

            } catch (Exception ex) { 
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        

        //EXPL: update a forecast info
        [HttpPut("{id}")]
        public async Task<ActionResult<WeatherForecast>> UpdateForecast(int id, WeatherForecast forecast)
        {
            try
            {
                if (id != forecast.Id)
                    return BadRequest("Forecast ID mismatch");

                var data = _weatherDbContext.WeatherForecasts.Where(x => x.Id == forecast.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Date = forecast.Date;
                    data.TemperatureC = forecast.TemperatureC;
                    data.Summary = forecast.Summary;
                    await _weatherDbContext.SaveChangesAsync();
                    return Ok();
                } 
                else
                {
                    return NotFound($"Forecast info with Id = {id} not found");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating data {ex.Message}");
            }
        }

        //EXPL: create a new weatherforecast info
        [HttpPost]        
        public async Task<ActionResult<WeatherForecast>> CreateAsync(WeatherForecast forecast)
        {
            if (forecast.Summary.Length == 0)
            {
                return BadRequest();
            }

            _weatherDbContext.WeatherForecasts.Add(forecast);
            await _weatherDbContext.SaveChangesAsync();

            return Ok();
        }

        //EXPL: return all the records ordered descendently
        [HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			_logger.LogInformation("Loading weather forecast information");
			return _weatherDbContext.WeatherForecasts.OrderByDescending(e => e.Date).ToArray();
		}

        //EXPL: find by summary partial string
        [HttpGet("GetBySummary")]
        public IEnumerable<WeatherForecast> GetBySummaryUsingLike(string summary)
        {
            _logger.LogInformation("Loading weather forecast information by summary");
            return _weatherDbContext.WeatherForecasts.Where(f => EF.Functions.Like(f.Summary, $"%{summary}%")).ToArray();
        }

        //EXPL: find by forecast id
        [HttpGet("{id}")]
        [ProducesResponseType<WeatherForecast>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var forecast = _weatherDbContext.WeatherForecasts.Find(id);
            return forecast == null ? NotFound() : Ok(forecast);
        }
    }
	
}
