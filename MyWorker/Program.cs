using MyWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient("local", httpClient => {
            httpClient.BaseAddress = new Uri("http://172.17.0.3/");
        });

        services.AddSingleton<WeatherForecastService>();
        
    })
    .Build();

host.Run();
