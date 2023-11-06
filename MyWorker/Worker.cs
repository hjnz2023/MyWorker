namespace MyWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly WeatherForecastService _weatherForecastService;
    private readonly Random _random = new();

    public Worker(ILogger<Worker> logger, WeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var guessNum = _random.Next(0, 4);
            if (guessNum == 3)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await _weatherForecastService.Fetch();
                }
                catch (Exception ex)
                {
                    NewRelic.Api.Agent.NewRelic.NoticeError(ex, new Dictionary<string, object> { { "guessNum", guessNum } });
                }
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
