namespace saaldigital.Web;

public class WeatherApiClient(HttpClient httpClient)
{

    public async Task<WeatherForecast[]> GetWeatherAsync()
    {
        return await httpClient.GetFromJsonAsync<WeatherForecast[]>("/Weatherforecast") ?? [];
    }
}

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
