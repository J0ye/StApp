using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text; // Added this line

namespace StApp
{
    class Program
    {
        protected string token = "";

        static async Task Main(string[] args)
        {
            Program program = new Program();
            //var registrationResult = await RegisterAgent("Mark37");
            program.token = GetToken();
            var agent = await AgentData.GetAgentDetails(program.token);
            if (agent != null)
            {
                Console.WriteLine("Agent data received");
            }
            else
            {
                Console.WriteLine("Agent data not received");
            }
            var location = await FindLocationWithShipyard(program.token, agent.GetSystemSymbole());
            foreach (var waypoint in Waypoint.FromJson(location))
            {
                Console.WriteLine(waypoint.Symbol);
            }

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
                token = File.ReadAllText("token.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading token file: {e.Message}");
                return null;
            }
            return token;
        }

        private static async Task<string> FindLocationWithShipyard(string token, string systemSymbol) // Updated return type
        {
            var options = new
            {
                headers = new
                {
                    Authorization = $"Bearer {token}"
                }
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", options.headers.Authorization);

                    var response = await client.GetAsync($"https://api.spacetraders.io/v2/systems/{systemSymbol}/waypoints?traits=SHIPYARD");
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody; // Return the response body
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null; // Return null in case of an error
            }
        }
        private static async Task<string> RegisterAgent(string callsign)
        {
            var url = "https://api.spacetraders.io/v2/register";
            var data = new
            {
                symbol = callsign,
                faction = "COSMIC"
            };

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgentRegisterResult.txt"); // Updated to use My Documents path
            try
            {
                using (var client = new HttpClient())
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    try
                    {
                        var response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        await File.WriteAllTextAsync(filePath, responseContent); // Ensure this line is correct
                        Console.WriteLine("New agent registered with this: " + responseContent);
                        return responseContent; // Return the response content directly
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Request error: {e.Message}");
                        return null;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"File is not accessible: {e.Message}");
                return null; // Return null if the file cannot be accessed
            }
        }

    }
}