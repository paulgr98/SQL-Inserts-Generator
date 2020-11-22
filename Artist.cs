using System.Collections.Generic;

namespace SQL_Inserts_Generator
{
    public class Artist
    {
        public static string Tablename = "Artists";

        public string Name { get; set; }
        public string SqlName { get; set; } //name with '' instead of '. single ' was breaking sql code
        public List<Genre> Genres { get; set; }

        public override string ToString()
        {
            return SqlName;
        }
    }
}
