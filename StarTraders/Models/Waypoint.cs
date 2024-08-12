using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace StApp
{
    public class Waypoint
    {
        public string SystemSymbol { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<string> Orbitals { get; set; }
        public List<Trait> Traits { get; set; }
        public List<string> Modifiers { get; set; }
        public Chart Chart { get; set; }
        public Faction Faction { get; set; }
        public string Orbits { get; set; }
        public bool IsUnderConstruction { get; set; }

        public static List<Waypoint> FromJson(string json)
        {
            var waypoints = JsonConvert.DeserializeObject<List<Waypoint>>(json);
            return waypoints;
        }

        // Test function for FromJsonList
        public static void TestFromJson()
        {
            string json = "[{\"SystemSymbol\":\"SYS1\",\"Symbol\":\"W1\",\"Type\":\"Type1\",\"X\":1,\"Y\":2,\"Orbitals\":[],\"Traits\":[],\"Modifiers\":[],\"Chart\":null,\"Faction\":null,\"Orbits\":\"Orbit1\",\"IsUnderConstruction\":false}]";
            var waypoints = FromJson(json);
            // Assert that the list is not null and has the expected count
            if (waypoints == null || waypoints.Count != 1)
            {
                Console.WriteLine("Test failed: Expected 1 waypoint.");
            }
            else
            {
                Console.WriteLine("Test passed: 1 waypoint found.");
            }
            // Additional assertions can be added here to check properties
        }
    }
}

public class Trait
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class Chart
{
    public string SubmittedBy { get; set; }
    public DateTime SubmittedOn { get; set; }
}

public class Faction
{
    public string Symbol { get; set; }
}

public class Meta
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}