using System.Net.Http.Json;
using Azure.Identity;
using Azure.Core;

namespace ACSAuthentication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using HttpClient client = new();

            // Key authentication
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "[Your API Key]");

            // System assigned managed identity authentication
            var credential = new ManagedIdentityCredential();
            var context = new TokenRequestContext(scopes: new[] { "https://cognitiveservices.azure.com/.default" }, tenantId: "[Your tenant id]");
            string token = "Bearer " + credential.GetToken(context).Token;
            client.DefaultRequestHeaders.Add("Authorization", token);

            string endpoint = "[Your endpoint]";
            string endpoint_text = endpoint + "/contentsafety/text:analyze?api-version=2023-04-30-preview";

            client.PostAsJsonAsync(endpoint_text, new
            {
                text = "You are an idiot"
            }).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var response = task.Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine(task.Exception?.Message);
                }
            }).Wait();
        }
    }
}