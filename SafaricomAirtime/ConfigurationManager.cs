using Microsoft.Extensions.Configuration;
using System.IO;

namespace SafaricomAirtime
{
    internal static class ConfigurationManager
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public static string GetKey(string key) => Configuration[key];
    }
}