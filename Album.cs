namespace SQL_Inserts_Generator
{
    public class Album
    {
        public static string Tablename = "Albums";

        public string Name { get; set; }
        public string SqlName { get; set; } //name with '' instead of '. single ' was breaking sql code
        public Artist Artist { get; set; }

        public override string ToString()
        {
            return SqlName;
        }
    }
}
