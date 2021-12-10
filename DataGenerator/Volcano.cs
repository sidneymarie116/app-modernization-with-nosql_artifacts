using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DataGenerator
{
    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Volcano
    {
        [JsonProperty("Volcano Name")]
        public string VolcanoName { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public Location Location { get; set; }
        public int? Elevation { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        [JsonProperty("Last Known Eruption")]
        public string LastKnownEruption { get; set; }
        public string id { get; set; }

        public static List<Volcano> GetVolcanoesFromJson()
        {
            try
            {
                using (var reader = new StreamReader("VolcanoData.json"))
                {
                    var json = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<IEnumerable<Volcano>>(json);
                    return result.Take(10).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}