namespace SQL_Inserts_Generator
{
    public class Genre
    {
        public static string Tablename = "Genres";

        public string Name { get; set; }
        public string SqlName { get; set; } //name with '' instead of '. single ' was breaking sql code

        public override int GetHashCode() //needed for Intersect method
        {
            return Name.ToLower().GetHashCode();
        }

        public override bool Equals(object obj) //needed for Intersect method
        {
            if (obj is Genre genre)
            {
                return genre.GetHashCode() == GetHashCode();
            }

            return false;
        }
    }
}
