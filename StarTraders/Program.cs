using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace StApp
{
    class Program
    {
        protected string token;

        static async Task Main(string[] args)
        {
            Program program = new Program();
            program.token = GetToken();
            var agent = await AgentData.GetAgentDetails(program.token);
            agent.PrintAgentDetails();
            /*var contract = await Contract.ShowContract(program.token);
            await contract.AcceptContract(program.token);
            var location = await Location.GetAgentHeadquarterLocation(agent, program.token);
            await location.FindWaypointWithShipyard(program.token);*/
        }

        private static string GetToken()
        {
            string token;
            try
            {
                token = System.IO.File.ReadAllText("token.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading token file: {e.Message}");
                return null;
            }
            return token;
        }
        

        public async Task<string> CreateAgent()
        {
            var client = new HttpClient();
            var requestBody = new
            {
                symbol = "Joye",
                faction = "COSMIC"
            };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync("https://api.spacetraders.io/v2/register", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                return $"Request error: {e.Message}";
            }
        }   
    }
}