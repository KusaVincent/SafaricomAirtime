using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SafaricomAirtime
{
    internal class RechargeAirtime
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task MakeRechargeRequest(string accessToken, int amount, string receiverMsisdn)
        {
            try
            {
                string servicePin = ConfigurationManager.GetKey("ServicePin");
                string senderMsisdn = ConfigurationManager.GetKey("SenderMsisdn");
                string apiUrl = ConfigurationManager.GetKey("SafaricomRechargeUrl");

                var requestData = new
                {
                    senderMsisdn,
                    amount,
                    servicePin,
                    receiverMsisdn
                };

                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                HttpResponseMessage response = await HttpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseBody}");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                Console.WriteLine("Unauthorized access. Please check your access token.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the request: {ex.Message}");
                throw;
            }
        }
    }
}