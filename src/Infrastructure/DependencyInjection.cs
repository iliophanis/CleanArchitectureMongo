using System;
using System.Text;
using Application.Common.Interfaces;
using AspNetCore.Identity.MongoDbCore.Models;
using Infrastructure.Emails;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            var connectionString = configuration.GetSection("ConnectionString").Value;
            var databaseName = configuration.GetSection("DatabaseName").Value;
            var issuerSigningKey = Encoding.UTF8.GetBytes(configuration.GetSection("IssuerSigningKey").Value);

            // Emails
            services.AddSingleton<IEmailConfiguration>(emailConfiguration);
            services.AddSingleton<IEmailService, EmailService>();

            // Security
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<ITokenGenerator>(new TokenGenerator(issuerSigningKey));
            services.AddIdentity<ApplicationUser, MongoIdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 0;
                options.Lockout.MaxFailedAccessAttempts = 0;
            })
                .AddMongoDbStores<ApplicationUser, MongoIdentityRole, Guid>(connectionString, databaseName)
                .AddSignInManager()
                .AddDefaultTokenProviders();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(issuerSigningKey),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Persistence
            services.AddSingleton<IContext>(new Context(connectionString, databaseName));

            return services;
        }
    }
}
