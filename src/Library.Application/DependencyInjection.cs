using Microsoft.Extensions.DependencyInjection;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Application.Mappings;

namespace Library.Application
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(MappingProfile));

            // Registrar servicios de la capa Application (Use Cases)
            services.AddScoped<IBookService, BookService>(); 
            services.AddScoped<ILoanService, LoanService>(); 

            return services;
        }
    }
}