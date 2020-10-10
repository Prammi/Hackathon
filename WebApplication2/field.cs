using Newtonsoft.Json;

namespace WebApplication2
{
    public class field
    {
        [JsonProperty("System.AreaPath")]
        public string AreaPath { get; set; }

        [JsonProperty("System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty("System.CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("System.Title")]
        public string Title { get; set; }

        [JsonProperty("System.Description")]
        public string Description { get; set; }
    }
}