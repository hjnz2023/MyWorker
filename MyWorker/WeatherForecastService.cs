using NewRelic.Api.Agent;

namespace MyWorker;

public class WeatherForecastService
{
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Lazy<HttpClient> _httpClient;

    public WeatherForecastService(ILogger<WeatherForecastService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _httpClient = new Lazy<HttpClient>(() => _httpClientFactory.CreateClient("local"));
    }


    [Transaction]
    public async Task Fetch()
    {
        var httpResponseMessage = await _httpClient.Value.GetAsync("weatherforecast");
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Response: {response}", await httpResponseMessage.Content.ReadAsStringAsync());
        } else {
            throw new Exception("Failed to fetch weather forecast");
        }
    }
}
