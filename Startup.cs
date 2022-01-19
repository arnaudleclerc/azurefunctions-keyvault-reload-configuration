using System;
using Azure.Identity;
using KeyvaultReloadTest;
using KeyvaultReloadTest.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace KeyvaultReloadTest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddAzureKeyVault(
                new Uri(Environment.GetEnvironmentVariable("KeyVaultUri")),
                new DefaultAzureCredential())
                .Build();

            builder.Services.AddSingleton(configuration)
                .AddOptions<ServiceConfiguration>()
                .Configure<IConfigurationRoot>((options, config) =>
                {
                    options.ValueFromTheKeyvault = config[Constants.ConfKey];
                });
        }
    }
}