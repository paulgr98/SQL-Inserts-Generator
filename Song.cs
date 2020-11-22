namespace SQL_Inserts_Generator
{
    public class Song
    {
        public static string Tablename = "Songs";

        public string Name { get; set; }
        public string SqlName { get; set; } //name with '' instead of '. single ' was breaking sql code
        public int Duration { get; set; }
        public Album Album { get; set; }

        public override int GetHashCode()
        {
            int nameHash = Name.ToLower().GetHashCode();
            int durationHash = Duration.GetHashCode();

            return nameHash ^ durationHash;
        }

        public override bool Equals(object obj)
        {
            if (obj is Song song)
            {
                return (song.GetHashCode() == GetHashCode());
            }

            return false;
        }
    }
}
