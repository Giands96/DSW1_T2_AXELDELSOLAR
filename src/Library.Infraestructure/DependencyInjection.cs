using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Library.Domain.Ports.Out;
using Library.Infraestructure.Persistence.Context;
using Library.Infraestructure.Persistence.Repositories;

namespace Library.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("BD_HOST");
            var port = Environment.GetEnvironmentVariable("BD_PORT") ?? "3306";
            var database = Environment.GetEnvironmentVariable("BD_NAME");
            var user = Environment.GetEnvironmentVariable("BD_USER");
            var password = Environment.GetEnvironmentVariable("BD_PASSWORD");

            var connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};";

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0))
            )
        );

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}