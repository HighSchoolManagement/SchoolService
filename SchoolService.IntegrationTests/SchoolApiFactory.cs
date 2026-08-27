using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolService.Infrastructure.Persistence;


namespace SchoolService.IntegrationTests
{
    public class SchoolApiFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<SchoolDbContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<SchoolDbContext>>();
                
                _connection.Open();
                services.AddDbContext<SchoolDbContext>(options =>
                    options.UseSqlite(_connection));

                using var scope = services.BuildServiceProvider().CreateScope();


                var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                context.Database.EnsureCreated();
            });
        }
    }
}
