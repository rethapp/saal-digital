using saaldigital.Web;
using saaldigital.Web.Components;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//when connecting between containers, inside Docker we must use the internal port not the exposed one!!!
builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://" + 
    builder.Configuration["ApiEndpointHost"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
