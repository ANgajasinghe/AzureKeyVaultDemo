using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();



ClientSecretCredential clientSecretCredential = new(
    "50417500-7c24-4d99-b8cb-eb581a6d5844",
    "82a8b8a6-d0d3-4cdc-b760-3a941273f02c",
    "qhs7Q~EZcABFDTGCJmtZ38s5LrVaRgIxh6ssa"
    );

//builder.Configuration.AddAzureKeyVault(
//    "https://agsamplekey.vault.azure.net/",
//    "82a8b8a6-d0d3-4cdc-b760-3a941273f02c",
//   "qhs7Q~EZcABFDTGCJmtZ38s5LrVaRgIxh6ssa"
//    );


SecretClient secretClient = new SecretClient(
    new Uri("https://agsamplekey.vault.azure.net/"),
    clientSecretCredential);

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
