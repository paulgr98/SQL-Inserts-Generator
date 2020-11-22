using Newtonsoft.Json;

namespace SQL_Inserts_Generator
{
    public class Names
    {
        [JsonProperty("col1")]
        public string FirstName { get; set; }
    }
}
