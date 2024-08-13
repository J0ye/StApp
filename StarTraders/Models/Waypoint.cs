using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StApp
{
    public enum WaypointTrait
    {
        uncharted,
        under_construction,
        marketplace,
        shipyard,
        outpost,
        scattered_settlements,
        sprawling_cities,
        mega_structures,
        pirate_base,
        overcrowded,
        high_tech,
        corrupt,
        bureaucratic,
        trading_hub,
        industrial,
        black_market,
        research_facility,
        military_base,
        surveillance_outpost,
        exploration_outpost,
        mineral_deposits,
        common_metal_deposits,
        precious_metal_deposits,
        rare_metal_deposits,
        methane_pools,
        ice_crystals,
        explosive_gases,
        strong_magnetosphere,
        vibrant_auroras,
        salt_flats,
        canyons,
        perpetual_daylight,
        perpetual_overcast,
        dry_seabeds,
        magma_seas,
        supervolcanoes,
        ash_clouds,
        vast_ruins,
        mutated_flora,
        terraformed,
        extreme_temperatures,
        extreme_pressure,
        diverse_life,
        scarce_life,
        fossils,
        weak_gravity,
        strong_gravity,
        crushing_gravity,
        toxic_atmosphere,
        corrosive_atmosphere,
        breathable_atmosphere,
        thin_atmosphere,
        jovian,
        rocky,
        volcanic,
        frozen,
        swamp,
        barren,
        temperate,
        jungle,
        ocean,
        radioactive,
        micro_gravity_anomalies,
        debris_cluster,
        deep_craters,
        shallow_craters,
        unstable_composition,
        hollowed_interior,
        stripped
    }

    public class Waypoint
    {
        public Dictionary<string, dynamic> data { get; set; }

        public void LogWaypoint()
        {
            foreach (var entry in data)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        public async Task<Dictionary<string, dynamic>> GetOptions(string token, WaypointTrait trait)
        {
            if (HasTrait(trait))
            {
                return await JsonHelper.MakeRequest(token, "systems/" + GetSystemSymbol() + "/waypoints/" + GetSymbol() + "/" + trait);
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
            return data["systemsymbol"];
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

        public static async Task<List<Dictionary<string, dynamic>>> GetWaypointsByTrait(string token, string systemSymbol, WaypointTrait trait)
        {
            var data = await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints?traits=" + trait, true);
            return data["data"].ToObject<List<Dictionary<string, dynamic>>>();
        }
    }
}