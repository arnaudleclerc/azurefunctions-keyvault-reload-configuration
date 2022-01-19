using KeyvaultReloadTest.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace KeyvaultReloadTest.API
{
    public class ConfigurationApi
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<ServiceConfiguration> _configurationOptions;
        private readonly IOptionsSnapshot<ServiceConfiguration> _configurationOptionsSnapshot;

        public ConfigurationApi(IConfigurationRoot configuration, IOptions<ServiceConfiguration> configurationOptions, IOptionsSnapshot<ServiceConfiguration> configurationSnapshotOptions)
        {
            _configuration = configuration;
            _configurationOptions = configurationOptions;
            _configurationOptionsSnapshot = configurationSnapshotOptions;
        }

        /// <summary>
        /// This returns the configuration from IConfigurationRoot. When the configuration is reloaded, it will return the current value
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName(nameof(GetConfiguration))]
        public IActionResult GetConfiguration([HttpTrigger(AuthorizationLevel.Function, Route = "configuration")] HttpRequest request)
        {
            return new OkObjectResult(_configuration[Constants.ConfKey]);
        }
        
        /// <summary>
        /// This returns the configuration from IOptions. When the configuration is reloaded, this should return the previous value.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName(nameof(GetConfigurationFromOptions))]
        public IActionResult GetConfigurationFromOptions([HttpTrigger(AuthorizationLevel.Function, Route = "configuration/options")] HttpRequest request)
        {
            return new OkObjectResult(_configurationOptions.Value.ValueFromTheKeyvault);
        }
        
        /// <summary>
        /// This returns the configuration from IOptionsSnapshot. When the configuration is reloaded, this returns the most current value.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName(nameof(GetConfigurationFromSnapshotOptions))]
        public IActionResult GetConfigurationFromSnapshotOptions([HttpTrigger(AuthorizationLevel.Function, Route = "configuration/options/snapshot")] HttpRequest request)
        {
            return new OkObjectResult(_configurationOptionsSnapshot.Value.ValueFromTheKeyvault);
        }
        
        /// <summary>
        /// This reloads the configuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName(nameof(Reload))]
        public IActionResult Reload([HttpTrigger(AuthorizationLevel.Function, "post", Route = "configuration/reload")] HttpRequest request)
        {
            _configuration.Reload();
            return new NoContentResult();
        }
    }
}