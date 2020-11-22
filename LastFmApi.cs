using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace SQL_Inserts_Generator
{
    public class LastFmApi
    {
        private const string BaseUrl = "https://ws.audioscrobbler.com/2.0";

        private readonly IRestClient Client;

        private readonly string ApiKey;

        public LastFmApi(string l_apiKey)
        {
            Client = new RestClient(BaseUrl);
            ApiKey = l_apiKey;
        }

        public string Execute(RestRequest request)
        {
            request.AddQueryParameter("api_key", ApiKey);
            var response = Client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var LastFmException = new Exception(message, response.ErrorException);
                throw LastFmException;
            }

            return response.Content;
        }

        public List<Artist> GetArtists(string genre, int limit)
        {
            try
            {
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("method", "tag.gettopartists");
                request.AddQueryParameter("tag", genre);
                request.AddQueryParameter("limit", limit.ToString());
                request.AddQueryParameter("format", "json");

                var json = Execute(request);

                JObject lvl1 = JObject.Parse(json);
                JObject lvl2 = (JObject)lvl1["topartists"];
                JArray arr = (JArray)lvl2["artist"];

                return arr.ToObject<List<Artist>>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Genre> GetGenres()
        {
            try
            {
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("method", "tag.getTopTags"); ;
                request.AddQueryParameter("format", "json");

                var json = Execute(request);

                JObject lvl1 = JObject.Parse(json);
                JObject lvl2 = (JObject)lvl1["toptags"];
                JArray arr = (JArray)lvl2["tag"];

                return arr.ToObject<List<Genre>>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Album> GetTopAlbums(Artist artist)
        {
            try
            {
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("method", "artist.gettopalbums");
                request.AddQueryParameter("artist", artist.Name);
                request.AddQueryParameter("format", "json");
                request.AddQueryParameter("limit", "3");

                var json = Execute(request);

                JObject lvl1 = JObject.Parse(json);
                JObject lvl2 = (JObject)lvl1["topalbums"];
                JArray arr = (JArray)lvl2["album"];

                return arr.ToObject<List<Album>>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Song> GetSongs(Album album)
        {
            try
            {
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("method", "album.getinfo");
                request.AddQueryParameter("artist", album.Artist.Name);
                request.AddQueryParameter("album", album.Name);
                request.AddQueryParameter("format", "json");

                var json = Execute(request);

                JObject lvl1 = JObject.Parse(json);
                JObject lvl2 = (JObject)lvl1["album"];
                JObject lvl3 = (JObject)lvl2["tracks"];
                JArray arr = (JArray)lvl3["track"];

                var res = arr.ToObject<List<Song>>();
                res.ForEach(s => s.Album = album);

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Genre> GetArtistTags(Artist artist)
        {
            try
            {
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("method", "artist.gettoptags");
                request.AddQueryParameter("artist", artist.Name);
                request.AddQueryParameter("format", "json");

                var json = Execute(request);

                JObject lvl1 = JObject.Parse(json);
                JObject lvl2 = (JObject)lvl1["toptags"];
                JArray arr = (JArray)lvl2["tag"];

                var res = arr.ToObject<List<Genre>>();

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
