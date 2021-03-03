using System;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence
            var connectionString = configuration.GetSection("ConnectionString").Value;
            var databaseName = configuration.GetSection("DatabaseName").Value;

            services.AddSingleton<IContext>(new Context(connectionString, databaseName));

            return services;
        }
    }
}
