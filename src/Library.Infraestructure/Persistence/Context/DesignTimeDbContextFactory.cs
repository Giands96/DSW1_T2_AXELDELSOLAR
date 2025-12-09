using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Library.Infraestructure.Persistence.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
            if (File.Exists(envPath))
            {
                var lines = File.ReadAllLines(envPath);
                foreach (var line in lines)
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        Environment.SetEnvironmentVariable(parts[0], parts[1]);
                    }
                }
            }
            var host = Environment.GetEnvironmentVariable("BD_HOST");
            var port = Environment.GetEnvironmentVariable("BD_PORT") ?? "3306";
            var database = Environment.GetEnvironmentVariable("BD_NAME");
            var user = Environment.GetEnvironmentVariable("BD_USER");
            var password = Environment.GetEnvironmentVariable("BD_PASSWORD");

            var connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0))
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}