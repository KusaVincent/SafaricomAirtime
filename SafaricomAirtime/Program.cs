using System;
using System.Net;
using System.Threading.Tasks;

namespace SafaricomAirtime
{
    internal class Program
    {
        static async Task Main()
        {
            try
            {
                int amount = 1000; //amount in cents.
                string receiverMsisdn = "798888323";

                string accessToken = await AccessToken.GetNewAccessToken();

                RechargeAirtime rechargeAirtime = new RechargeAirtime();

                await rechargeAirtime.MakeRechargeRequest(accessToken, amount, receiverMsisdn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}