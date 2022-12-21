using Microsoft.EntityFrameworkCore;
using Web.API.DbContexts;

namespace ScheduledTasks
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IDbContextFactory<BusinessContext> _dbContextFactory;
        IHostApplicationLifetime _applicationLifetime;
        public Worker(IDbContextFactory<BusinessContext> dbContextFactory, ILogger<Worker> logger, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _applicationLifetime = applicationLifetime;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service has started...");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    // DeActivate All Products, just to see if service works properly
                    using (var context = _dbContextFactory.CreateDbContext())
                    {
                        int count = context.Database.ExecuteSqlRaw(@"update Products set IsActive = 0");
                        _logger.LogCritical(count + " tane ürün pasife çekildi");
                    }
                    await Task.Delay(2000, stoppingToken);
                    _applicationLifetime.StopApplication();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Program has failed: " + ex.Message);
                    _applicationLifetime.StopApplication();
                }

            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Application stopped at " + DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}