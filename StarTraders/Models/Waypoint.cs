using System;
using System.Collections.Generic;
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
        public static async Task<Dictionary<string, dynamic>> GetWaypoint(string token, string systemSymbol, string waypointSymbol)
        {
            return await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints/" + waypointSymbol);
        }

        public static async Task<Dictionary<string, dynamic>> GetWaypointsByTrait(string token, string systemSymbol, WaypointTrait trait)
        {
            return await JsonHelper.MakeRequest(token, "systems/" + systemSymbol + "/waypoints?trait=" + trait);
        }
    }
}