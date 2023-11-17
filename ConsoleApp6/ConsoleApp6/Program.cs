using System.Net.Http.Headers;
using Notion.Client;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var token = "secret_WqEJ6bhqM6yjaPVm2ICF6HNfx7rqQkJL98Tjm1Oa2p3";
        HttpClient client;
        using (client = new HttpClient())
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var clientOptions = new IRestClient
        {
            AuthToken = token
        };
        var notion = new NotionClient(clientOptions, client, client, client,client,client,client);
        
        var page = await notion.Pages.RetrieveAsync("PAGE_ID");
        foreach (var permission in page.Properties)
        {   
            var userEmail = permission.Value.Type == PropertyValueType.Email;
            var userPermission = permission.Value.Type == PropertyValueType.Status;

            Console.WriteLine($"User: {userEmail}, Permission: {userPermission}");
        }
    }
}