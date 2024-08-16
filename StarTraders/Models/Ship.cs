
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StApp
{
    public enum ShipType
    {
        SHIP_PROBE,
        SHIP_MINING_DRONE,
        SHIP_SIPHON_DRONE,
        SHIP_INTERCEPTOR,
        SHIP_LIGHT_HAULER,
        SHIP_COMMAND_FRIGATE,
        SHIP_EXPLORER,
        SHIP_HEAVY_FREIGHTER,
        SHIP_LIGHT_SHUTTLE,
        SHIP_ORE_HOUND,
        SHIP_REFINING_FREIGHTER,
        SHIP_SURVEYOR
    }

    public class Ship
    {
        public Dictionary<string, dynamic> data { get; set; }
        public static async Task<Dictionary<string, dynamic>> DockShip(string shipId, string token)
        {
            var requestType = $"my/ships/{shipId}/dock";

            return await JsonHelper.MakeRequest(token, requestType, HttpMethod.Post, true);
        }
    }
}





