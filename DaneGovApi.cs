using ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace SQL_Inserts_Generator
{
    public class DaneGovApi
    {
        private const string BaseUrl = "https://api.dane.gov.pl/resources";

        private readonly IRestClient Client;

        public DaneGovApi()
        {
            Client = new RestClient(BaseUrl);
        }

        public JArray Execute(RestRequest request)
        {
            var response = Client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var LastFmException = new Exception(message, response.ErrorException);
                throw LastFmException;
            }

            JObject json = JObject.Parse(response.Content);
            JArray data = (JArray)json["data"];

            return data;
        }

        private T GetData<T>(RestRequest request)
        {
            var data = Execute(request);

            return JsonConvert.DeserializeObject<T>(data.ToString());
        }

        public List<User> GetUsers(int number)
        {
            var female_names_request = new RestRequest($"21455,imiona-zenskie-nadane-dzieciom-w-polsce-w-2019-r-imie-pierwsze/data?per_page={number / 2}", Method.GET);
            var male_names_request = new RestRequest($"21454,imiona-meskie-nadane-dzieciom-w-polsce-w-2019-r-imie-pierwsze/data?per_page={number / 2}", Method.GET);

            var female_lastnames_request = new RestRequest($"22812,nazwiska-zenskie-stan-na-2020-01-22/data?per_page={number / 2}", Method.GET);
            var male_lastnames_request = new RestRequest($"22811,nazwiska-meskie-stan-na-2020-01-22/data?per_page={number / 2}", Method.GET);

            var femaleNamesJson = GetData<List<NamesJson>>(female_names_request);
            var femaleNames = femaleNamesJson.ConvertAll(x => x.ToString().ToLower().ToUpperFirstLetter());
            femaleNames.Shuffle();

            var femaleLastNamesJson = GetData<List<LastNamesJson>>(female_lastnames_request);
            var femaleLastNames = femaleLastNamesJson.ConvertAll(x => x.ToString().ToLower().ToUpperFirstLetter());
            femaleLastNames.Shuffle();

            var users = new List<User>();

            foreach (var fln in femaleLastNames)
            {
                users.Add(new User() { FirstName = femaleNames.RandomItem(), LastName = fln });
            }

            var maleNamesJson = GetData<List<NamesJson>>(male_names_request);
            var maleNames = maleNamesJson.ConvertAll(x => x.ToString().ToLower().ToUpperFirstLetter());
            maleNames.Shuffle();

            var maleLastNamesJson = GetData<List<LastNamesJson>>(male_lastnames_request);
            var maleLastNames = maleLastNamesJson.ConvertAll(x => x.ToString().ToLower().ToUpperFirstLetter());
            maleLastNames.Shuffle();

            foreach (var mln in maleLastNames)
            {
                users.Add(new User() { FirstName = maleNames.RandomItem(), LastName = mln });
            }

            users.Shuffle();
            users.Shuffle();

            foreach (var u in users)
            {
                u.Username = u.GetRandomUsername();
                u.Password_Hash = User.GetRandomPasswordHash(20);
            }

            return users;
        }
    }
}
