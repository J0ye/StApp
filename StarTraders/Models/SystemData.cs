using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StApp
{
    public class SystemData
    {
        public Dictionary<string, dynamic> data { get; set; }

        public string Symbol => data["symbol"];
        public string SectorSymbol => data["sectorSymbol"];
        public string Type => data["type"];
        public int X => data["x"];
        public int Y => data["y"];
        /// <summary>
        /// Gets the list of waypoints associated with the system.
        /// Each waypoint is represented as a dictionary of dynamic properties.
        /// </summary>
        public List<Dictionary<string, dynamic>> Waypoints => data["waypoints"].ToObject<List<Dictionary<string, dynamic>>>();

        /// <summary>
        /// Gets the list of factions associated with the system.
        /// Each faction is represented as a dynamic object.
        /// </summary>
        public List<dynamic> Factions => data["factions"].ToObject<List<dynamic>>();

        public async Task<List<Dictionary<string, dynamic>>> GetWaypointsByTrait(string token, WaypointTrait trait)
        {
            var data = await JsonHelper.MakeRequest(token, "systems/" + Symbol + "/waypoints?traits=" + trait, true);
            return data["data"].ToObject<List<Dictionary<string, dynamic>>>();
        }

        public async Task<Dictionary<string, dynamic>> GetSystem(string symbol, string token)
        {
            return await JsonHelper.MakeRequest(token, "systems/" + symbol);
        }

        public async Task<SystemData> GetSystem(string symbol, string token, bool asObject = true)
        {
            var systemData = await JsonHelper.MakeRequest(token, "systems/" + symbol);
            return new SystemData { data = systemData };
        }
    }
}