using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SandboxTestProject.Services;
using SandboxTestProject.Services.Interfaces;
using SandboxTestProject.ViewModels;
using SandboxTestProject.ViewModels.Core.Interfaces;
using SandboxTestProject.Views;

namespace SandboxTestProject;

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
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appconfig.json", optional: false, reloadOnChange: true).AddUserSecrets<App>();
            })
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.Configure<Config>(context.Configuration);

                var config = context.Configuration.Get<Config>();

                services.AddHttpClient(config.HackerEarth.httpClientName, client =>
                {
                    client.BaseAddress = new Uri(config.HackerEarth.url);
                    client.DefaultRequestHeaders.Add("client-secret", config.HackerEarth.client_secret);
                });

                services.AddHttpClient(config.LeetCode.httpClientName, client =>
                {
                    client.BaseAddress = new Uri(config.LeetCode.url);
                });

                services.AddSingleton<SandBoxView>();
                services.AddSingleton<ISandBoxViewModel, SandBoxViewModel>();

                services.AddTransient<MainWindow>();
                services.AddTransient<HomeView>();
                services.AddTransient<LeetCodeProblemsView>();
                services.AddTransient<DetailsProblemView>();
                services.AddTransient<EnteringPersonalDataView>();

                services.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
                services.AddTransient<IHomeViewModel, HomeViewModel>();        
                services.AddTransient<ILeetCodeProblemsViewModel, LeetCodeProblemsViewModel>();
                services.AddTransient<IDetailsProblemViewModel, DetailsProblemViewModel>();
                services.AddTransient<IEnteringPersonalDataViewModel, EnteringPersonalDataViewModel>();

                services.AddScoped<ICodeEvaluationService, CodeEvaluationService>();
                services.AddScoped<ILeetCodeService, LeetCodeService>();
            })
            .Build();

        return host;
    }
}