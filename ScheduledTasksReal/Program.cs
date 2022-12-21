using Microsoft.EntityFrameworkCore;
using ScheduledTasks;
using Web.API.DbContexts;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory).
              AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContextFactory<BusinessContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("sqlConnection")));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
