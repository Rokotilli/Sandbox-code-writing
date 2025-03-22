using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TestProjectForDCT.Services;
using TestProjectForDCT.ViewModels;
using TestProjectForDCT.Views;

namespace TestProjectForDCT;

public partial class App : Application
{
    private readonly IHost host;

    public App()
    {
        host = InitialHost();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = host.Services.GetRequiredService<MainWindow>();

        mainWindow.Show();
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

                services.AddHttpClient(config.HackerEarth.httpClientName, client =>
                {
                    client.BaseAddress = new Uri(config.HackerEarth.url);
                    client.DefaultRequestHeaders.Add("client-secret", config.HackerEarth.client_secret);
                });

                services.AddHttpClient(config.LeetCode.httpClientName, client =>
                {
                    client.BaseAddress = new Uri(config.LeetCode.url);
                });

                services.AddTransient<MainWindow>();
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<SandBoxViewModel>();
                services.AddTransient<HomeView>();
                services.AddTransient<SandBoxView>();
                services.AddTransient<LeetCodeProblemsView>();
                services.AddTransient<LeetCodeProblemsViewModel>();
                services.AddTransient<DetailsProblemView>();
                services.AddTransient<DetailsProblemViewModel>();
                services.AddTransient<EnteringPersonalDataView>();
                services.AddTransient<EnteringPersonalDataViewModel>();

                services.AddScoped<CodeEvaluationService>();
                services.AddScoped<LeetCodeService>();
            })
            .Build();

        return host;
    }
}