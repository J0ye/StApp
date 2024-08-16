using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace StApp
{
    class Program
    {
        protected string token = "";

        static async Task Main(string[] args)
        {
            Program program = new Program();
            //var registrationResult = await RegisterAgent("Sunlit");
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
            //SystemData system = await agent.GetSystem(program.token);
            //Dictionary<string, dynamic> contracts= await Contract.GetContracts(program.token);
            //var result = await Contract.AcceptContract(program.token, (string)contracts["data"][0]["id"]);
            /*var waypoints = await Waypoint.GetOptionsForShipType(program.token, agent.GetSystemSymbole(), ShipType.SHIP_MINING_DRONE);
            string symboleOfShipyard = waypoints[0].Symbol;
            Console.WriteLine("Shipyard at: " + symboleOfShipyard);
            Waypoint shipyard = await Waypoint.GetWaypoint(program.token, agent.GetSystemSymbole(), symboleOfShipyard, true);
            //Console.WriteLine("First Traits of shipyard: " + shipyard.GetTraits()[0]["symbol"]);
            var shipyardOptions = await shipyard.GetOptions(program.token, WaypointTrait.SHIPYARD);
            foreach (var option in shipyardOptions["data"]["shipTypes"])
            {
                Console.WriteLine("Ship: " + option["type"]);
            }*/

            var waypoints = await Waypoint.GetWaypointsByTrait(program.token, agent.GetSystemSymbole(), WaypointTrait.MARKETPLACE);
            foreach (var waypoint in waypoints)
            {
                Console.WriteLine("Waypoint: " + waypoint.Symbol);
                var options = await waypoint.GetOptions(program.token, WaypointTrait.MARKETPLACE); 
                if (options["data"]["imports"] != null &&
                    ((IEnumerable<dynamic>)options["data"]["imports"]).Any(import => import["symbol"] == "IRON"))
                {
                    Console.WriteLine("Waypoint: " + waypoint.Symbol);
                }
                // Check if they sell fuel
                if (options["data"]["exchange"] != null &&
                    ((IEnumerable<dynamic>)options["data"]["exchange"]).Any(exchange => exchange["symbol"] == "FUEL"))
                {
                    Console.WriteLine($"Waypoint sells fuel: {waypoint.Symbol}");
                }
            }
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

        private static async Task<Dictionary<string, dynamic>> RegisterAgent(string callsign)
        {
            var data = new
            {
                symbol = callsign,
                faction = "COSMIC"
            };

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgentRegisterResult.txt"); // Updated to use My Documents path
            try
            {
                var responseContent = await JsonHelper.MakeRequest(" ", "register", HttpMethod.Post, true, data);
                foreach (var key in responseContent.Keys)
                {
                    await File.WriteAllTextAsync(filePath, responseContent[key]);
                }
                Console.WriteLine("New agent registered with this: " + responseContent);
                return responseContent; // Return the response content directly
            }
            catch (IOException e)
            {
                Console.WriteLine($"File is not accessible: {e.Message}");
                return null; // Return null if the file cannot be accessed
            }
        }

    }
}