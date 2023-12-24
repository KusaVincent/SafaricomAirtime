using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SafaricomAirtime
{
    internal class AccessToken
    {
        public static async Task<string> GetNewAccessToken()
        {
            try
            {
                string consumerKey      = ConfigurationManager.GetKey("ConsumerKey");
                string consumerSecret   = ConfigurationManager.GetKey("ConsumerSecret");
                string tokenUrl         = ConfigurationManager.GetKey("SafaricomTokenUrl");

                string credential       = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credential}");
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    if (tokenUrl != null)
                    {
                        string jsonPayload = "{}";

                        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await httpClient.PostAsync(tokenUrl, content);

                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();

                        dynamic? accessToken = JsonConvert.DeserializeObject(responseBody);

                        if (accessToken != null)
                        {
                            if (accessToken.access_token != null)
                            {
                                return accessToken.access_token;
                            }
                            else
                            {
                                Console.WriteLine("The 'access_token' property is null or undefined in the response.");
                                return string.Empty;
                            }
                        }
                        else
                        {
                            Console.WriteLine("The 'accessToken' object is null or undefined in the response.");
                            return string.Empty;
                        }

                    }
                    else
                    {
                        throw new InvalidOperationException("SafaricomTokenUrl environment variable is not set.");
                    }
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