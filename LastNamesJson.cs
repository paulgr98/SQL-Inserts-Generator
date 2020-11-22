using Newtonsoft.Json;

namespace SQL_Inserts_Generator
{
    class LastNamesJson
    {
        [JsonProperty("attributes")]
        public LastNames LastNames { get; set; }

        public override string ToString()
        {
            return LastNames.LastName;
        }
    }
}
