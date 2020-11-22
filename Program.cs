using System;
using System.Diagnostics;
using System.IO;

namespace SQL_Inserts_Generator
{
    public class Program
    {
        static readonly Stopwatch Timer = new Stopwatch();
        static readonly Stopwatch Alltimer = new Stopwatch();

        static void LogError(string msg)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
            Console.WriteLine();
            Console.ReadKey();
        }

        static void LogStatusAndReset(bool correct, Stopwatch sw)
        {
            sw.Stop();
            if (correct)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ERROR!");
            }
            Console.ResetColor();
            Console.WriteLine($" ({sw.ElapsedMilliseconds / 1000.0} s)");
            Timer.Reset();
        }

        static string Menu()
        {
            string choice;
            string result;

            do
            {
                Console.WriteLine("Select result directory: ");
                Console.WriteLine("1) Desktop");
                Console.WriteLine("2) Current folder");
                choice = Console.ReadLine();

                Console.Clear();

                if (choice == "1")
                {
                    result = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else if (choice == "2")
                {
                    result = Directory.GetCurrentDirectory();
                }
                else
                {
                    Console.WriteLine("Wrong choice!\n");
                    result = "";
                }
            } while (result.Length < 1);

            return result;
        }

        static void Main()
        {

            Generator gen = new Generator
            {
                ResultDirectory = Menu()
            };

            Alltimer.Start();

            try
            {
                Timer.Start();
                Console.Write($"{Artist.Tablename}: ");

                gen.GetArtists();
                gen.GenerateArtists("Artists.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write($"{Genre.Tablename}: ");

                gen.GetGenres();
                gen.GenerateGenres("Genres.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write($"{User.Tablename}: ");

                gen.GetUsers();
                gen.GenerateUsers("Users.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write("PremiumUsers: ");

                gen.GetPremiumUsers();
                gen.GeneratePremiumUsers("PremiumUsers.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write($"{Album.Tablename}: ");

                gen.GetAlbums();
                gen.GenerateAlbums("Albums.sql");   

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write($"{Song.Tablename}: ");

                gen.GetSongs();
                gen.GenerateSongs("Songs.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write("ArtistGenres: ");

                gen.GetArtistGenres();
                gen.GenerateArtistGenres("ArtistGenres.sql");

                LogStatusAndReset(true, Timer);

            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write("FavoriteAlbums: ");

                gen.GenerateFavoriteAlbums("FavoriteAlbums.sql");

                LogStatusAndReset(true, Timer);

            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write("FavoriteArtists: ");

                gen.GenerateFavoriteArtists("FavoriteArtists.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write($"{Playlist.Tablename}: ");

                gen.GetPlaylistsFromResource();
                gen.GeneratePlaylists("Playlists.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            try
            {
                Timer.Start();
                Console.Write("PlaylistSongs: ");

                gen.GeneratePlaylistSongs("PlaylistSongs.sql");

                LogStatusAndReset(true, Timer);
            }
            catch (Exception e)
            {
                LogStatusAndReset(false, Timer);
                LogError(e.Message);
            }

            Alltimer.Stop();
            Console.WriteLine($"\nProgram is done ({Alltimer.ElapsedMilliseconds / 1000.0} s)");
            Alltimer.Reset();
            Console.ReadKey();
        }
    }
}
