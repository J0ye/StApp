using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace StApp
{
    public class SystemData
    {
        /// <summary>
        /// Gets or sets the unique symbol identifier for the system.
        /// Example: "X1-PM3"
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the symbol of the sector this system belongs to.
        /// Example: "X1"
        /// </summary>
        public string SectorSymbol { get; set; }

        /// <summary>
        /// Gets or sets the type of celestial body represented by the system.
        /// Example: "RED_STAR"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the system in the galactic map.
        /// Example: -10723
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the system in the galactic map.
        /// Example: -857
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the list of waypoints within the system.
        /// Each waypoint can represent various celestial objects like planets, moons, or asteroids.
        /// </summary>
        public List<Waypoint> Waypoints { get; set; }

        public class Waypoint
        {
            public string Symbol { get; set; }
            public string Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public List<Orbital> Orbitals { get; set; }
            public string Orbits { get; set; } // Optional, for stations and moons
        }

        public class Orbital
        {
            public string Symbol { get; set; }
        }
    }
}