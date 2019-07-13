using Newtonsoft.Json;
using System;

namespace JazugNight.Core
{
    public class Employee
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("partitionkey")]
        public string PartitionKey { get; set; } = "0";
    }
}
