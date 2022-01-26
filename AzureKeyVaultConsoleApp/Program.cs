// See https://aka.ms/new-console-template for more information

// create key with clieant credential instance

using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

ClientSecretCredential clientSecretCredential = new(
    "50417500-7c24-4d99-b8cb-eb581a6d5844", 
    "82a8b8a6-d0d3-4cdc-b760-3a941273f02c",
    "qhs7Q~EZcABFDTGCJmtZ38s5LrVaRgIxh6ssa"
    );



KeyClient keyClient = new KeyClient(
    new Uri("https://agsamplekey.vault.azure.net/"),
    clientSecretCredential);


// key that you have created in azure before.
var key = keyClient.GetKey("demoKey");

string textToEncrypt = "Some sample key";

CryptographyClient cryptographyClient = new CryptographyClient(key.Value.Id, clientSecretCredential);
var encText = cryptographyClient.Encrypt(EncryptionAlgorithm.Rsa15, Encoding.UTF8.GetBytes(textToEncrypt));

Console.WriteLine("Enc text : " + Encoding.UTF8.GetString(encText.Ciphertext));




var decriptResult = cryptographyClient.Decrypt(EncryptionAlgorithm.Rsa15, encText.Ciphertext);
Console.WriteLine("Decrpted Text : " + Encoding.UTF8.GetString(decriptResult.Plaintext));

Console.WriteLine("Hello, World!");
Console.Read();

