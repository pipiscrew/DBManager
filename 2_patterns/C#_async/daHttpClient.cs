using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace asyncExample
{
    public class daHttpClient
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            HttpResponseMessage response = await client.SendAsync(request);
           
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(responseString);

            return result;
        }

        //application/x-www-form-urlencoded
        public async Task<string> PostAsync(string url, Dictionary<string, string> data)
        {
            // Create the form content
            var content = new FormUrlEncodedContent(data);

            // Create the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Send the request asynchronously
            HttpResponseMessage response = await client.SendAsync(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
