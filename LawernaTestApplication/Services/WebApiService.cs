using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LawernaTestApplication.Services;

public static class WebApiService
{
    public static async Task<HttpResponseMessage> GetCall(string url)  
    {  
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  
        var apiUrl = App.ApiUrl + url;
        try
        {
            // Create new httpClient
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);  
            client.Timeout = TimeSpan.FromSeconds(900);  
            client.DefaultRequestHeaders.Accept.Clear();  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
            // Get response async
            var response = await client.GetAsync(apiUrl);   
            return response;
        }
        catch
        {
            throw;
        }
    }
}