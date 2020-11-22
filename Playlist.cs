namespace SQL_Inserts_Generator
{
    public class Playlist
    {
        public static string Tablename = "Playlists";

        public string Name { get; set; }
        public string SqlName { get; set; } //name with '' instead of '. single ' was breaking sql code
    }
}
