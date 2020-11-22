using Newtonsoft.Json;

namespace SQL_Inserts_Generator
{
    public class NamesJson
    {
        [JsonProperty("attributes")]
        public Names Names { get; set; }

        public override string ToString()
        {
            return Names.FirstName;
        }
    }
}
