using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StApp
{
    public class AgentData
    {
        public Dictionary<string, dynamic> data;

        public string GetSystemSymbole()
        {
            // Get the value from data["headquarters"]
            string headquarters = data["headquarters"];
            // Find the index of the second '-' sign
            int firstDashIndex = headquarters.IndexOf('-');
            int secondDashIndex = headquarters.IndexOf('-', firstDashIndex + 1);
            // Return substring up to the second '-' sign
            return secondDashIndex != -1 ? headquarters.Substring(0, secondDashIndex) : headquarters;
        }

        public void LogAgentData()
        {
            foreach (var key in data.Keys)
            {
                Console.WriteLine($"{key}: {data[key]}");
            }
        }

        public async Task<Dictionary<string, dynamic>> GetSystem(string token)
        {
            Console.WriteLine("Fetching system: " + GetSystemSymbole());
            try
            {
                return await JsonHelper.MakeRequest(token, $"systems/{GetSystemSymbole()}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null; // Return null if there's an error
            }
        }

        public static async Task<AgentData> GetAgentDetails(string token)
        {
            try
            {
                var data = await JsonHelper.MakeRequest(token, "my/agent");
                return new AgentData
                {
                    // Convert JObject to Dictionary<string, dynamic>
                    data = data["data"].ToObject<Dictionary<string, dynamic>>()
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                PrintTokenDetails(token);
                return null;
            }
        }

        private static void PrintTokenDetails(string token)
        {
            int tokenLength = token.Length;
            string lastTenChars = tokenLength > 10 ? token.Substring(tokenLength - 10) : token;
            Console.WriteLine($"Token length: {tokenLength}, Last 10 characters: {lastTenChars}");
        }

    }
}