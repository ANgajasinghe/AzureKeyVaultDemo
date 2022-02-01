using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultFunction;
using AzureKeyVaultFunction.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            builder.Services.AddOptions<AppConfigurationSecrets>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AppConfigurationSecrets").Bind(settings);
                });

        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];


            if (builtConfig["Environtment"] == "DEV")
            {
                SecretClient secretClient = new SecretClient(
                    new Uri(keyVaultEndpoint),
                    new AzureCliCredential(),
                    new SecretClientOptions
                    {
                        Retry =
                                        {
                                        Delay = TimeSpan.FromSeconds(2),
                                        MaxDelay = TimeSpan.FromSeconds(16),
                                        MaxRetries = 5,
                                        Mode = RetryMode.Exponential
                                        }
                    });




                builder.ConfigurationBuilder
                       .SetBasePath(Environment.CurrentDirectory)
                       .AddJsonFile("local.settings.json", true)
                       .AddAzureKeyVault(
                        secretClient,
                        new AzureKeyVaultConfigurationOptions()
                        {
                            ReloadInterval = TimeSpan.FromSeconds(60)
                        })
                       .AddEnvironmentVariables()
                   .Build();

            }
            else 
            {
                builder.ConfigurationBuilder
                       .SetBasePath(Environment.CurrentDirectory)
                       .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
                       .AddJsonFile("local.settings.json", true)
                       .AddEnvironmentVariables()
                   .Build();

            }



        }
    }
}
