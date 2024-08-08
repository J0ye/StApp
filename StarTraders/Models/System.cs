
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace StApp
{
    public class System
    {
        public static async Task<string> GetAgentHeadquarterLocation(AgentData agentData, string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                var response = await client.GetAsync($"https://api.spacetraders.io/v2/systems/{agentData.GetSystemSymbole()}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }
    }
}