using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication
    .CreateBuilder(args);
//.Host
//.ConfigureWebHostDefaults(webBuilder=> 
//{
//    webBuilder.ConfigureAppConfiguration(config =>
//    {
//        config.AddAzureAppConfiguration(options =>
//        {
//            ConfigureOptions(options, new Uri("https://agsamplekey.vault.azure.net/"));
//        });
//    });
//});


builder.Services.AddRazorPages();









var o = new DefaultAzureCredentialOptions();
o.VisualStudioTenantId = "50417500-7c24-4d99-b8cb-eb581a6d5844";


SecretClient secretClient = new SecretClient(
    new Uri("https://agsamplekey.vault.azure.net/"),
    new DefaultAzureCredential(),
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


builder.Configuration.AddAzureKeyVault(
    secretClient,
    new AzureKeyVaultConfigurationOptions()
    {

        ReloadInterval = TimeSpan.FromSeconds(60)
    });



// getting secrects

Console.WriteLine(builder.Configuration.GetConnectionString("SqlConnection"));

var app = builder.Build();


app.MapGet("api/con", () =>
 {
     return Results.Ok(new
     {
         cons = builder.Configuration.GetConnectionString("SqlConnection")
     });
 });



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();



static void ConfigureOptions(AzureAppConfigurationOptions options, Uri appConfigEndpoint)
{
    var credentials = GetAzureCredentials();

    options.Connect(appConfigEndpoint, credentials);
    options.ConfigureKeyVault(kv => kv.SetCredential(credentials));
}

static TokenCredential GetAzureCredentials()
{
    var DefaultTenantId = "50417500-7c24-4d99-b8cb-eb581a6d5844";
    var isDeployed = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
    return new DefaultAzureCredential(
        new DefaultAzureCredentialOptions
        {
            // Prevent deployed instances from trying things that don't work and generally take too long
            ExcludeInteractiveBrowserCredential = isDeployed,
            ExcludeVisualStudioCodeCredential = isDeployed,
            ExcludeVisualStudioCredential = isDeployed,
            ExcludeSharedTokenCacheCredential = isDeployed,
            ExcludeAzureCliCredential = isDeployed,
            ExcludeManagedIdentityCredential = false,
            Retry =
            {
				// Reduce retries and timeouts to get faster failures
				MaxRetries = 2,
                NetworkTimeout = TimeSpan.FromSeconds(5),
                MaxDelay = TimeSpan.FromSeconds(5)
            },


            // this helps devs use the right tenant
            InteractiveBrowserTenantId = DefaultTenantId,
            SharedTokenCacheTenantId = DefaultTenantId,
            VisualStudioCodeTenantId = DefaultTenantId,
            VisualStudioTenantId = DefaultTenantId
        }
    );
}