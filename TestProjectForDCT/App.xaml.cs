using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TestProjectForDCT.ViewModels;

namespace TestProjectForDCT;

public partial class App : Application
{
    private readonly IHost host;

    public App()
    {
        host = InitialHost();
    }

    public IHost InitialHost()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appconfig.json", optional: false, reloadOnChange: true).AddUserSecrets<App>();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<Config>(context.Configuration);

                var config = services.BuildServiceProvider().GetRequiredService<IOptions<Config>>().Value;

                services.AddSingleton(config);

                services.AddHttpClient(config.httpClientHackerearthAPIName, client =>
                {
                    client.BaseAddress = new Uri(config.hackerearthAPIUrl);
                    client.DefaultRequestHeaders.Add("client-secret", config.client_secret);
                });

                services.AddScoped<CodeEvaluationService>();
            })
            .Build();

        return host;
    }
}