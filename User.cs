using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SQL_Inserts_Generator
{
    public class User
    {
        private static readonly Random rand = new Random();

        public static string Tablename = "Users";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password_Hash { get; set; }

        //------------------- STATIC METHODS -----------------------------------

        private static string CreateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            StringBuilder res = new StringBuilder();

            while (0 < length--)
            {
                res.Append(valid[rand.Next(valid.Length)]);
            }

            return res.ToString();
        }

        private static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static string GetRandomPasswordHash(int password_length)
        {
            string pass = CreateRandomPassword(password_length);
            return CreateMD5(pass).ToLower();
        }

        public static string GetRandomBrithDate()
        {
            DateTime start = new DateTime(1980, 1, 1);
            DateTime end = new DateTime(2010, 1, 1);

            int range = (end - start).Days;

            return start.AddDays(rand.Next(range)).ToShortDateString();
        }


        public static string GetRandomCreationDate()
        {
            DateTime start = new DateTime(2015, 1, 1);
            DateTime end = DateTime.Now;

            int range = (end - start).Days;

            return start.AddDays(rand.Next(range)).ToShortDateString();
        }

        public static string GetRandomPremiumExpirationDate()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddMonths(6);

            int range = (end - start).Days;

            return start.AddDays(rand.Next(range)).ToShortDateString();
        }


        //------------------- NON STATIC METHODS -------------------------------

        public string GetRandomUsername()
        {
            StringBuilder nick = new StringBuilder();
            nick.Append(FirstName.Substring(0, 3).RemoveAccent());
            nick.Append(LastName.Substring(0, 3).RemoveAccent());

            int numb_of_digits = rand.Next(1, 5);
            for (int i = 0; i < numb_of_digits; i++)
            {
                nick.Append(rand.Next(0, 10));
            }

            return nick.ToString().ToLower();
        }

        public string GetRandomEmail()
        {
            List<string> domains = new List<string>()
            {
                "gmail.com",
                "wp.pl",
                "onet.pl",
                "o2.pl",
                "interia.pl",
                "op.pl"
            };

            return Username + '@' + domains.RandomItem();
        }
    }
}
