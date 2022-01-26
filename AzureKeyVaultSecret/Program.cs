// See https://aka.ms/new-console-template for more information



using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using System.Text;

ClientSecretCredential clientSecretCredential = new(
    "50417500-7c24-4d99-b8cb-eb581a6d5844",
    "82a8b8a6-d0d3-4cdc-b760-3a941273f02c",
    "qhs7Q~EZcABFDTGCJmtZ38s5LrVaRgIxh6ssa"
    );


SecretClient secretClient = new SecretClient(
    new Uri("https://agsamplekey.vault.azure.net/"), 
    clientSecretCredential);

var secretValue = secretClient.GetSecret("demoSecrect");

Console.WriteLine("secretValue" + secretValue.Value.Value);

Console.WriteLine("Hello, World!");
