using Azure.Identity;
using AzureKeyVaultFunction;
using AzureKeyVaultFunction.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text.Json;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureKeyVaultFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // throw new NotImplementedException();

            

            builder.Services.AddScoped<AppConfiguration>((services) => 
            {
                var config = services.GetService<IConfiguration>();
                var data = config!.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();

                Console.WriteLine(data.Name);

                return data;

            });

            //builder.Services.AddOptions<AppConfiguration>()
            //    .Configure<IConfiguration>((settings, configuration) =>
            //    {
            //        var data = configuration.GetSection("AppConfiguration");
            //        var currentData = data.Get<AppConfiguration>();
            //        data.Bind(settings);
            //    });

            builder.Services.AddOptions<AppConfigurationSecrets>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AppConfigurationSecrets").Bind(settings);
                });


            builder.Services.AddScoped<HttpStart>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();

            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                builder.ConfigurationBuilder
                       .SetBasePath(Environment.CurrentDirectory)
                       .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
                       .AddJsonFile("local.settings.json", true)
                       .AddEnvironmentVariables()
                   .Build();
            }
            else 
            {
                builder.ConfigurationBuilder
                      .SetBasePath(Environment.CurrentDirectory)
                      .AddJsonFile("local.settings.json", true)
                      .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                      .AddEnvironmentVariables()
                      .Build();
            }

        }
    }
}
