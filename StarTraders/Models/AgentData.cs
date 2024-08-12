using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace StApp
{
    public class AgentData
    {
        public string AccountId { get; set; }
        public string Symbol { get; set; }
        public string Headquarters { get; set; }
        public int Credits { get; set; }
        public string StartingFaction { get; set; }
        public int ShipCount { get; set; }

        public void PrintAgentDetails()
        {
            Console.WriteLine($"AccountId: {AccountId}");
            Console.WriteLine($"Symbol: {Symbol}");
            Console.WriteLine($"Headquarters: {Headquarters}");
            Console.WriteLine($"Credits: {Credits}");
            Console.WriteLine($"StartingFaction: {StartingFaction}");
            Console.WriteLine($"ShipCount: {ShipCount}");
        } 

        public string GetSystemSymbole()
        {
            int lastIndex = Headquarters.LastIndexOf('-');
            string ret = lastIndex >= 0 ? Headquarters.Substring(0, lastIndex) : Headquarters;
            Console.WriteLine(ret);
            return ret;
        }

        // Add this method to convert JSON string to AgentData object
        public static AgentData FromJson(string jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            var data = jsonObject.data;

            return new AgentData
            {
                AccountId = data.accountId,
                Symbol = data.symbol,
                Headquarters = data.headquarters,
                Credits = data.credits,
                StartingFaction = data.startingFaction,
                ShipCount = data.shipCount
            };
        }

        public static async Task<AgentData> GetAgentDetails(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                var response = await client.GetAsync("https://api.spacetraders.io/v2/my/agent");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return AgentData.FromJson(responseBody);
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