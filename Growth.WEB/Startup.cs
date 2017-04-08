using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using Growth.WEB.Authentication;
using Growth.WEB.Authentication.Middlewares;
using Growth.WEB.Filters;
using Growth.WEB.Infrastructure.DI;
using Growth.WEB.Infrastructure.Swagger;
using Growth.WEB.Models.AccountApiModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace Growth.WEB
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">Hosting environment instance</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("NLog.config");
        }

        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Configures application services
        /// </summary>
        /// <param name="services">Collection of services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyResolver.Resolve(services, Configuration);

            services
                .Configure<TokenProviderOptions>(Configuration.GetSection("TokenProviderOptions"))
                .Configure<TokenProviderOptions>(options =>
                {
                    var signingKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(options.SecretKey));
                    options.SigningCredentials =
                        new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                });

            services
                .Configure<TokenValidationParameters>(Configuration.GetSection("TokenValidationParameters"))
                .Configure<TokenValidationParameters>(options =>
                {
                    var signingKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration["TokenValidationParameters:SecretKey"]));
                    options.IssuerSigningKey = signingKey;
                });

            services.AddAutoMapper();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Growth API",
                        Version = "v1",
                        Description = "A simple api of 'Growth' project",
                        TermsOfService = "None"
                    }
                ); 

                var pathToXmlDoc = Configuration["Swagger:FileName"];
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, pathToXmlDoc);

                options.IncludeXmlComments(filePath);
                options.DescribeAllEnumsAsStrings();

                options.OperationFilter<ApplyOAuth2Security>();
                options.DocumentFilter<ApplyTokenProvider>();
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ErrorFilter));
            });
        }

        /// <summary>
        /// Configure application and sets the middlewares
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment instance</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="tokenProviderOptions">Options for JWT token provider</param>
        /// <param name="tokenValidationOptions">Options for JWT token validator</param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<TokenProviderOptions> tokenProviderOptions,
            IOptions<TokenValidationParameters> tokenValidationOptions)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            ConfigLogManager();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

            app.UseMiddleware<TokenProviderMiddleware>(tokenProviderOptions);

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationOptions.Value
            });

            app.UseMvc();
        }

        private void ConfigLogManager()
        {
            LogManager.Configuration.Variables["configDir"] = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        }
    }
}