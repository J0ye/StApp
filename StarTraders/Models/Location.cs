using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace StApp
{
    public class Location
    {
        public string Symbol { get; set; }
        public string SystemSymbol { get; set; }
        public string WaypointSymbol { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string[] Orbitals { get; set; }
        public string[] Traits { get; set; }
        public string[] Modifiers { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string FactionSymbol { get; set; }
        public bool IsUnderConstruction { get; set; }

        public void PrintToConsole()
        {
            Console.WriteLine("Symbol: " + Symbol);
            Console.WriteLine("SystemSymbol: " + SystemSymbol);
            Console.WriteLine("WaypointSymbol: " + WaypointSymbol);
            Console.WriteLine("Type: " + Type);
            Console.WriteLine("X: " + X);
            Console.WriteLine("Y: " + Y);
            Console.WriteLine("Orbitals: " + string.Join(", ", Orbitals));
            Console.WriteLine("Traits: " + string.Join(", ", Traits));
            Console.WriteLine("Modifiers: " + string.Join(", ", Modifiers));
            Console.WriteLine("SubmittedBy: " + SubmittedBy);
            Console.WriteLine("SubmittedOn: " + SubmittedOn);
            Console.WriteLine("FactionSymbol: " + FactionSymbol);
            Console.WriteLine("IsUnderConstruction: " + IsUnderConstruction);
        }

        public async Task FindWaypointWithShipyard(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                var response = await client.GetAsync($"https://api.spacetraders.io/v2/systems/{SystemSymbol}/waypoints?traits=SHIPYARD");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }

        public static Location FromJson(string jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            var data = jsonObject.data;

            return new Location
            {
                Symbol = data.symbol,
                SystemSymbol = data.systemSymbol,
                WaypointSymbol = data.symbol,
                Type = data.type,
                X = data.x,
                Y = data.y,
                Orbitals = ((IEnumerable<dynamic>)data.orbitals).Select(t => (string)t.symbol).ToArray(),
                Traits = ((IEnumerable<dynamic>)data.traits).Select(t => (string)t.symbol).ToArray(),
                Modifiers = ((IEnumerable<dynamic>)data.modifiers).Select(m => (string)m.symbol).ToArray(),
                SubmittedBy = data.chart.submittedBy,
                SubmittedOn = DateTime.ParseExact((string)data.chart.submittedOn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                FactionSymbol = data.faction.symbol,
                IsUnderConstruction = data.isUnderConstruction
            };
        }

        public static async Task<string> GetShipPosition(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                var response = await client.GetAsync("https://api.spacetraders.io/v2/my/ships");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                return $"Request error: {e.Message}";
            }
        }

        public static async Task PrintStartingLocation(string token)
        {
            string systemSymbol = "X1-AJ74";
            string waypointSymbol = "X1-AJ74-A1";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                var response = await client.GetAsync($"https://api.spacetraders.io/v2/systems/{systemSymbol}/waypoints/{waypointSymbol}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                
                var location = FromJson(responseBody);
                location.PrintToConsole();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }    
    }
}