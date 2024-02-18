namespace saaldigital.Web;

public class WeatherApiClient(HttpClient httpClient)
{

    public async Task<WeatherForecast[]> GetWeatherAsync()
    {
        //EXPL: in case of null we return an empty array
        return await httpClient.GetFromJsonAsync<WeatherForecast[]>("/Weatherforecast") ?? [];
    }
}

//EXPL: here we use the record value type intead of the class (reference type) as it is immutable and lightweight, so in this case is ok
public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
