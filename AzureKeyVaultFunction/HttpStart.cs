using AzureKeyVaultFunction.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AzureKeyVaultFunction
{
    public class HttpStart
    {
        private readonly AppConfiguration appConfiguration;
        private readonly IOptionsSnapshot<AppConfigurationSecrets> appConfigurationSecrets;

        public HttpStart(AppConfiguration appConfiguration,
            IOptionsSnapshot<AppConfigurationSecrets> appConfigurationSecrets)
        {
            this.appConfiguration = appConfiguration;
            this.appConfigurationSecrets = appConfigurationSecrets;
        }

        [FunctionName("GithubMonitor")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Our GiHub Monitor processed an action.");

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);


            // TODO - Do something with the data
            // log.LogInformation(requestBody);

            return new OkObjectResult(new 
            { 
                appConfiguration,
                appConfigurationSecrets
            });
        }
    }
}
