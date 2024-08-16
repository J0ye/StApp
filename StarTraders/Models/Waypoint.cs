using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StApp
{
    public enum WaypointTrait
    {
        UNCHARTED,
        UNDER_CONSTRUCTION,
        MARKETPLACE,
        SHIPYARD,
        OUTPOST,
        SCATTERED_SETTLEMENTS,
        SPRAWLING_CITIES,
        MEGA_STRUCTURES,
        PIRATE_BASE,
        OVERCROWDED,
        HIGH_TECH,
        CORRUPT,
        BUREAUCRATIC,
        TRADING_HUB,
        INDUSTRIAL,
        BLACK_MARKET,
        RESEARCH_FACILITY,
        MILITARY_BASE,
        SURVEILLANCE_OUTPOST,
        EXPLORATION_OUTPOST,
        MINERAL_DEPOSITS,
        COMMON_METAL_DEPOSITS,
        PRECIOUS_METAL_DEPOSITS,
        RARE_METAL_DEPOSITS,
        METHANE_POOLS,
        ICE_CRYSTALS,
        EXPLOSIVE_GASES,
        STRONG_MAGNETOSPHERE,
        VIBRANT_AURORAS,
        SALT_FLATS,
        CANYONS,
        PERPETUAL_DAYLIGHT,
        PERPETUAL_OVERCAST,
        DRY_SEABEDS,
        MAGMA_SEAS,
        SUPERVOLCANOES,
        ASH_CLOUDS,
        VAST_RUINS,
        MUTATED_FLORA,
        TERRAFORMED,
        EXTREME_TEMPERATURES,
        EXTREME_PRESSURE,
        DIVERSE_LIFE,
        SCARCE_LIFE,
        FOSSILS,
        WEAK_GRAVITY,
        STRONG_GRAVITY,
        CRUSHING_GRAVITY,
        TOXIC_ATMOSPHERE,
        CORROSIVE_ATMOSPHERE,
        BREATHABLE_ATMOSPHERE,
        THIN_ATMOSPHERE,
        JOVIAN,
        ROCKY,
        VOLCANIC,
        FROZEN,
        SWAMP,
        BARREN,
        TEMPERATE,
        JUNGLE,
        OCEAN,
        RADIOACTIVE,
        MICRO_GRAVITY_ANOMALIES,
        DEBRIS_CLUSTER,
        DEEP_CRATERS,
        SHALLOW_CRATERS,
        UNSTABLE_COMPOSITION,
        HOLLOWED_INTERIOR,
        STRIPPED
    }

    public class Waypoint
    {
        public Dictionary<string, dynamic> data { get; set; }

        public string SystemSymbol => data["systemSymbol"];
        public string Symbol => data["symbol"];
        public string Type => data["type"];
        public int X => data["x"];
        public int Y => data["y"];
        public List<Dictionary<string, dynamic>> Orbitals => data["orbitals"].ToObject<List<Dictionary<string, dynamic>>>();
        public List<Dictionary<string, dynamic>> Traits => data["traits"].ToObject<List<Dictionary<string, dynamic>>>();
        public List<Dictionary<string, dynamic>> Modifiers => data["modifiers"].ToObject<List<Dictionary<string, dynamic>>>();
        public Dictionary<string, dynamic> Chart => data["chart"].ToObject<Dictionary<string, dynamic>>();
        public Dictionary<string, dynamic> Faction => data["faction"].ToObject<Dictionary<string, dynamic>>();
        public string Orbits => data["orbits"];
        public bool IsUnderConstruction => data["isUnderConstruction"];

        
        public void LogWaypoint()
        {
            foreach (var entry in data)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        /// <summary>
        /// Asynchronously retrieves options for the waypoint based on the specified trait.
        /// </summary>
        /// <param name="token">The authentication token for API access.</param>
        /// <param name="trait">The trait to check for the waypoint.</param>
        /// <returns>A dictionary containing the options for the waypoint if the trait is present; otherwise, null.</returns>
        public async Task<Dictionary<string, dynamic>> GetOptions(string token, WaypointTrait trait)
        {
            // Check if the waypoint has the specified trait
            if (HasTrait(trait))
            {
                // Determine the endpoint based on the trait type
                string endpoint = trait == WaypointTrait.MARKETPLACE 
                    ? "market" // Use 'market' endpoint for MARKETPLACE trait
                    : trait.ToString(); // Use the trait's string representation for other traits
                
                // Make a request to the API for the waypoint options
                return await JsonHelper.MakeRequest(token, $"systems/{GetSystemSymbol()}/waypoints/{GetSymbol()}/{endpoint}");
            }
            Console.WriteLine("Error: Trait " + trait +  " not found in " + GetSymbol());
            return null;
        }

        public string GetSymbol()
        {
            return data["symbol"];
        }

        public string GetSystemSymbol()
        {
            return data["systemSymbol"];
        }

        public List<Dictionary<string, dynamic>> GetTraits()
        {
            return data["traits"].ToObject<List<Dictionary<string, dynamic>>>();
        }

        public bool HasTrait(WaypointTrait trait)
        {
            foreach (var entry in GetTraits())
            {
                if (entry["symbol"] == trait.ToString())
                    return true;
            }
            return false;
        }

        public static async Task<Dictionary<string, dynamic>> GetWaypoint(string token, string systemSymbol, string waypointSymbol)
        {
            return await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints/" + waypointSymbol);
        }

        public static async Task<Waypoint> GetWaypoint(string token, string systemSymbol, string waypointSymbol, bool returnAsWaypoint)
        {
            var data = await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints/" + waypointSymbol);
            return new Waypoint
            {
                data = data["data"].ToObject<Dictionary<string, dynamic>>()
            };
        }

        public static async Task<List<Waypoint>> GetWaypointsByTrait(string token, string systemSymbol, WaypointTrait trait)
        {
            var data = await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints?traits=" + trait);
            var waypointsData = data["data"].ToObject<List<Dictionary<string, dynamic>>>();
            var waypoints = new List<Waypoint>();

            foreach (var waypointData in waypointsData)
            {
                waypoints.Add(new Waypoint { data = waypointData });
            }

            return waypoints;
        }

        public static async Task<List<Waypoint>> GetOptionsForShipType(string token, string systemSymbol, ShipType shipType)
        {
            var waypoints = await Waypoint.GetWaypointsByTrait(token, systemSymbol, WaypointTrait.SHIPYARD);
            var waypointSymbolsWithShipType = new List<string>(); // Updated to List<string>

            foreach (var waypoint in waypoints)
            {
                var data = await waypoint.GetOptions(token, WaypointTrait.SHIPYARD);
                // Check if shipTypes contains the specified shipType
                foreach (var option in data["data"]["shipTypes"])
                {
                    if (option["type"] == shipType.ToString())
                    {
                        // Add the symbol to options if shipType is found
                        waypointSymbolsWithShipType.Add(data["data"]["symbol"].ToString()); // Updated to add string symbol
                    }
                }
            }

            // Filter waypoints based on symbols
            var filteredWaypoints = waypoints.Where(w => waypointSymbolsWithShipType.Contains(w.Symbol)).ToList(); // Filtered list

            return filteredWaypoints; // Return filtered waypoints
        }
    }
}