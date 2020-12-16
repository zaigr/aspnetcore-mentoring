using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace ConsoleApiClient
{
    public static class Program
    {
        private static readonly IConfiguration Configuration = BuildConfiguration();

        private static IConfiguration BuildConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("configuration.json");

            return configBuilder.Build();
        }

        public static void Main()
        {
            Console.WriteLine("Welcome to Northwind API console client!");

            while (true)
            {
                Console.WriteLine("Enter one of the following options:");
                Console.WriteLine("  - 1 --> get all products");
                Console.WriteLine("  - 2 --> get all categories");

                Console.Write(">");
                var option = Console.ReadLine()?.Trim();
                if (option != "1" && option != "2")
                {
                    Console.WriteLine($"Option '{option}' is not defined.");
                    continue;
                }

                switch (option)
                {
                    case "1": 
                        DisplayRequestContent(Path.Combine(Configuration["ApiUrl"], "products"));
                    break;
                    case "2":
                        DisplayRequestContent(Path.Combine(Configuration["ApiUrl"], "categories"));
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported option key");
                }
            }
        }

        private static void DisplayRequestContent(string requestUrl)
        {
            var client = new HttpClient();

            var response = client.GetAsync(requestUrl).GetAwaiter().GetResult();

            Console.WriteLine($"Response code: '{response.StatusCode}'");
            Console.WriteLine($"GET {requestUrl}");
            Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}
