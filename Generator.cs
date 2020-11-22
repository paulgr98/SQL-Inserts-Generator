using ExtensionMethods;
using SQL_Inserts_Generator.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SQL_Inserts_Generator
{
    public class Generator
    {
        private static readonly Random rand = new Random();

        private readonly LastFmApi LastFm = new LastFmApi(Config.ApiKey); //ApiKey from excluded file config.cs (you will need your own key)
        private readonly DaneGovApi DaneGov = new DaneGovApi();

        public string ResultDirectory { get; set; }

        public List<Artist> Artists { get; set; }
        public List<Genre> Genres { get; set; }
        public List<User> Users { get; set; }
        public List<User> PremiumUsers { get; set; }
        public List<Album> Albums { get; set; }
        public List<Song> Songs { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Playlist> UsedPlaylists { get; set; }

        public Generator()
        {
            ResultDirectory = Directory.GetCurrentDirectory();

            Artists = new List<Artist>();
            Genres = new List<Genre>();
            Users = new List<User>();
            PremiumUsers = new List<User>();
            Albums = new List<Album>();
            Songs = new List<Song>();
            Playlists = new List<Playlist>();
            UsedPlaylists = new List<Playlist>();
        }

        //----------------------------------- ARTISTS -------------------------------------------------------------

        public void GetArtists()
        {
            List<string> genres = new List<string>()
            { "grunge", "rock", "electronic", "metal",
              "pop", "alternative", "indie rock",
              "Progressive metal", "Hip-Hop", "soul"
            };

            try
            {
                foreach (var g in genres)
                {
                    Artists.AddRange(LastFm.GetArtists(g, 10));
                }

                Artists.ForEach(a => a.Name = a.Name.Replace("&", "And"));
                Artists.ForEach(a => a.SqlName = a.Name.Replace("'", "''"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateArtists(string filename)
        {
            try
            {
                using (StreamWriter file = File.AppendText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var a in Artists)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO {Artist.Tablename} (Artist_Name) VALUES (\'{a.SqlName}\');");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- GENRES --------------------------------------------------------------

        public void GetGenres()
        {
            try
            {
                Genres.AddRange(LastFm.GetGenres());
                Genres.Remove(new Genre() { Name = "hip hop" });
                Genres.Add(new Genre() { Name = "grunge" });

                Genres.ForEach(g => g.Name = g.Name.Replace("&", "And"));
                Genres.ForEach(g => g.SqlName = g.Name.Replace("'", "''").ToLower());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateGenres(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var g in Genres)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO {Genre.Tablename} (Genre_Name) VALUES (\'{g.SqlName}\');");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- USERS ---------------------------------------------------------------

        public void GetUsers()
        {
            try
            {
                Users.AddRange(DaneGov.GetUsers(100));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateUsers(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var u in Users)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO {User.Tablename} (FirstName, LastName, Username, Email, Password_Hash, BirthDate, CreationDate) ");
                        query.Append($"VALUES (\'{u.FirstName}\', \'{u.LastName}\', ");
                        query.Append($"\'{u.Username}\', \'{u.GetRandomEmail()}\', \'{u.Password_Hash}\', ");
                        query.Append($"TO_DATE(\'{User.GetRandomBrithDate()}\', \'DD.MM.YYYY\'), ");
                        query.Append($"TO_DATE(\'{User.GetRandomCreationDate()}\', \'DD.MM.YYYY\'));");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- PREMIUM USERS -------------------------------------------------------

        public void GetPremiumUsers()
        {
            try
            {
                int numOfUsers = Users.Count;
                int min = Convert.ToInt32(numOfUsers * 0.8);
                int max = Convert.ToInt32(numOfUsers * 0.9);
                int numOfPremiumUsers = rand.Next(min, max);

                for (int i = 0; i < numOfPremiumUsers; i++)
                {
                    var current_user = Users.RandomItem();
                    if (!PremiumUsers.Contains(current_user))
                    {
                        PremiumUsers.Add(current_user);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GeneratePremiumUsers(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var pu in PremiumUsers)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO PremiumUsers (User_ID, ExpirationDate) VALUES (");
                        query.Append($"(SELECT User_ID FROM {User.Tablename} WHERE Username = \'{pu.Username}\'), ");
                        query.Append($"TO_DATE(\'{User.GetRandomPremiumExpirationDate()}\', \'DD.MM.YYYY\'));");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- ALBUMS --------------------------------------------------------------

        public void GetAlbums()
        {
            try
            {
                foreach (var a in Artists)
                {
                    Albums.AddRange(LastFm.GetTopAlbums(a));
                }

                Albums.RemoveAll(a => a.Name == "(null)");
                Albums.ForEach(a => a.Name = a.Name.Replace("&", "And"));
                Albums.ForEach(a => a.SqlName = a.Name.Replace("'", "''"));

                Albums.ForEach(aa => aa.Artist.Name = aa.Artist.Name.Replace("&", "And"));
                Albums.ForEach(aa => aa.Artist.SqlName = aa.Artist.Name.Replace("'", "''"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateAlbums(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var a in Albums)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO {Album.Tablename} (Album_Title, Artist_ID) VALUES (");
                        query.Append($"\'{a.SqlName}\', (SELECT Artist_ID FROM {Artist.Tablename} WHERE Artist_Name = \'{a.Artist.SqlName}\'));");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- SONGS ---------------------------------------------------------------

        public void GetSongs()
        {
            try
            {
                foreach (var a in Albums)
                {
                    Songs.AddRange(LastFm.GetSongs(a));
                }

                Songs.RemoveAll(s => s.Duration == 0); //remove songs with duration 0
                Songs = Songs.Distinct().ToList(); //remove duplicates
                Songs.ForEach(s => s.Name = s.Name.Replace("&", "And"));
                Songs.ForEach(s => s.SqlName = s.Name.Replace("'", "''"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateSongs(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var s in Songs)
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($"INSERT INTO {Song.Tablename} (Song_Title, Album_ID, Duration, PlayCounter) VALUES (");
                        query.Append($"\'{s.SqlName}\', (SELECT Album_ID FROM {Album.Tablename} WHERE Album_Title = \'{s.Album.SqlName}\'), ");
                        query.Append($"{s.Duration}, {rand.Next(10000, 1000000)});");
                        file.WriteLine(query);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- ARTIST GENRES -------------------------------------------------------

        public void GetArtistGenres()
        {
            try
            {
                foreach (var a in Artists)
                {
                    a.Genres = LastFm.GetArtistTags(a).Intersect(Genres).ToList();

                    a.Genres.ForEach(g => g.Name = g.Name.Replace("&", "And"));
                    a.Genres.ForEach(g => g.SqlName = g.Name.Replace("'", "''").ToLower());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateArtistGenres(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    foreach (var a in Artists)
                    {
                        foreach (var g in a.Genres)
                        {
                            StringBuilder query = new StringBuilder();
                            query.Append($"INSERT INTO ArtistGenres (Genre_ID, Artist_ID) VALUES (");
                            query.Append($"(SELECT Genre_ID FROM {Genre.Tablename} WHERE Genre_Name = \'{g.SqlName}\'), ");
                            query.Append($"(SELECT Artist_ID FROM {Artist.Tablename} WHERE Artist_Name = \'{a.SqlName}\'));");
                            file.WriteLine(query);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //----------------------------------- Favorite Things -----------------------------------------------------

        private void Favorite<T>(List<T> list, string filename, string tablename, string colname, string foreign_tablename, string foreign_colname)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    var queries = new List<string>();

                    foreach (var u in Users)
                    {
                        int number = rand.Next(5, 10);
                        for (int i = 0; i < number; i++)
                        {
                            StringBuilder query = new StringBuilder();
                            query.Append($"INSERT INTO {tablename} (User_ID, {colname}) VALUES (");
                            query.Append($"(SELECT User_ID FROM Users WHERE Username = \'{u.Username}\'), ");
                            query.Append($"(SELECT {colname} FROM {foreign_tablename} WHERE {foreign_colname} = \'{list.RandomItem()}\'));");
                            queries.Add(query.ToString());
                            queries = queries.Distinct().ToList();
                        }
                    }

                    foreach (var q in queries)
                    {
                        file.WriteLine(q);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GenerateFavoriteAlbums(string filename)
        {
            Favorite(Albums, filename, "FavoriteAlbums", "Album_ID", Album.Tablename, "Album_Title");
        }

        public void GenerateFavoriteArtists(string filename)
        {
            Favorite(Artists, filename, "FavoriteArtists", "Artist_ID", Artist.Tablename, "Artist_Name");
        }

        //----------------------------------- PLAYLISTS -----------------------------------------------------------

        public void GetPlaylistsFromResource()
        {

            try
            {
                List<string> names = new List<string>(Resources.playlist_names.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
                foreach (var n in names)
                {
                    Playlists.Add(new Playlist() { Name = n, SqlName = n.Replace("'", "''") });
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public void GeneratePlaylists(string filename)
        {
            try
            {
                var remainingPlaylists = Playlists;
                int numOfUsers = Users.Count;
                int min = numOfUsers - Convert.ToInt32(numOfUsers * 0.3);
                int max = numOfUsers - Convert.ToInt32(numOfUsers * 0.1);
                int numOfPlaylists = rand.Next(min, max);

                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    for (int i = 0; i < numOfPlaylists; i++)
                    {
                        if (remainingPlaylists.Count > 0)
                        {
                            var current_usrer = Users.RandomItem();

                            var random_playlist = remainingPlaylists.RandomItem();
                            remainingPlaylists.Remove(random_playlist);
                            UsedPlaylists.Add(random_playlist);

                            StringBuilder query = new StringBuilder();
                            query.Append($"INSERT INTO {Playlist.Tablename} (Playlist_Title, User_ID) VALUES (");
                            query.Append($"\'{random_playlist.SqlName}\', ");
                            query.Append($"(SELECT User_ID FROM {User.Tablename} WHERE Username = \'{current_usrer.Username}\'));");
                            file.WriteLine(query);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GeneratePlaylistSongs(string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText($@"{ResultDirectory}\{filename}"))
                {
                    List<Song> usedSongs = new List<Song>(); //used to eliminate duplicates

                    foreach (var p in UsedPlaylists)
                    {
                        var numOfSongs = rand.Next(5, 21); //random number of songs in playlist between 5 and 20
                        usedSongs.Clear(); //clear used songs for every playlist

                        for (int i = 0; i < numOfSongs; i++)
                        {
                            var current_song = Songs.RandomItem();

                            if (!usedSongs.Contains(current_song)) //if song was not used, add to used and generate SQL
                            {
                                usedSongs.Add(current_song);

                                StringBuilder query = new StringBuilder();
                                query.Append($"INSERT INTO PlaylistSongs (Playlist_ID, Song_ID) VALUES (");
                                query.Append($"(SELECT Playlist_ID FROM {Playlist.Tablename} WHERE Playlist_Title = \'{p.SqlName}\'), ");
                                query.Append($"(SELECT Song_ID FROM {Song.Tablename} WHERE Song_Title = \'{current_song.SqlName}\' ");
                                query.Append($"AND Duration = {current_song.Duration}));");
                                file.WriteLine(query);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
