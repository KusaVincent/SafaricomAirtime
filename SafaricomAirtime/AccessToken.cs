using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SafaricomAirtime
{
    internal class AccessToken
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task<string> GetNewAccessToken()
        {
            try
            {
                string consumerKey = ConfigurationManager.GetKey("ConsumerKey");
                string consumerSecret = ConfigurationManager.GetKey("ConsumerSecret");
                string tokenUrl = ConfigurationManager.GetKey("SafaricomTokenUrl");

                string credential = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));

                if (tokenUrl == null)
                {
                    throw new InvalidOperationException("SafaricomTokenUrl environment variable is not set.");
                }

                using (var content = new StringContent("{}", Encoding.UTF8, "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credential}");
                    HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await HttpClient.PostAsync(tokenUrl, content);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic? accessToken = JsonConvert.DeserializeObject(responseBody);

                    if (accessToken?.access_token != null)
                    {
                        return accessToken.access_token;
                    }

                    Console.WriteLine("The 'access_token' property is null or undefined in the response.");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during token retrieval: {ex.Message}");
                throw;
            }
        }
    }
}