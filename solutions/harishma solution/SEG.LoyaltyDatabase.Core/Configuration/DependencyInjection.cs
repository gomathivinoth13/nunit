using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.AutoRegisterDi;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyDatabase.Core.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SEG.LoyaltyDatabase.Core.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("LoyaltyDatabase");
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(dbConnectionString));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            var registered = services.RegisterAssemblyPublicNonGenericClasses()
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
            return services;
        }
    }
}
