using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.AccessControl;

namespace Repository
{
    public static class MigrationsManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            for (int i = 0; i < 10; i++)
            {
                using (var appContext = host.Services.CreateScope()
                .ServiceProvider.GetRequiredService<RepositoryContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                        Console.WriteLine("Success migration.");
                        return host;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(
                            $"The server was not found or was not accessible." +
                            $"Sleep 1 min before retrying... #{i}/10.\r\n" +
                            $"{ex.Message}");
                        Thread.Sleep(60000);
                    }
                }
            }
            throw new Exception("Database migration failed.");
        }
        /*private static int _numberOfRetries;
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var appContext =
                scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                try
                {
                    appContext.Database.Migrate();
                }
                catch (SqlException)
                {
                    if (_numberOfRetries < 10)
                    {
                        Thread.Sleep(60000);
                        _numberOfRetries++;
                        Console.WriteLine($"The server was not found or was not accessible.Retrying... #{_numberOfRetries}");
                        MigrateDatabase(host);
                    }
                    throw;
                }
            }
            return host;
        }*/
    }
}
