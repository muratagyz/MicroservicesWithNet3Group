using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace Migration.WorkerService
{
    public class Worker(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
               var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
               await dbContext.Database.MigrateAsync(stoppingToken);
            }
        }
    }
}
