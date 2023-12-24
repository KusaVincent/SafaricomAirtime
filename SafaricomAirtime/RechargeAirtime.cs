using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SafaricomAirtime
{
    internal class RechargeAirtime
    {
        public async Task MakeRechargeRequest(string accessToken, int amount, string receiverMsisdn)
        {
            try
            {
                string servicePin = ConfigurationManager.GetKey("ServicePin");
                string senderMsisdn = ConfigurationManager.GetKey("SenderMsisdn");
                string apiUrl = ConfigurationManager.GetKey("SafaricomRechargeUrl");

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    var requestData = new
                    {
                        senderMsisdn,
                        amount,
                        servicePin,
                        receiverMsisdn
                    };

                    var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the request: {ex.Message}");
                throw;
            }
        }
    }
}