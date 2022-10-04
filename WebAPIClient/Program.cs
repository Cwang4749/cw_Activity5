using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebAPIClient
{
    class Book
    {
        [JsonProperty("bib_key")]
        public string Key { get; set; }

        [JsonProperty("info_url")]
        public string Info { get; set; }

        [JsonProperty("preview")]
        public string Preview { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewURL { get; set; }

        [JsonProperty("thumbnail_url")]
        public string Thumbnail { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }
        private static async Task ProcessRepositories()
        {
            while (true)
            {
                try
                {
                    //Example ISBN to try out: ISBN:9780399256752 , ISBN:9780141325491 , ISBN:9780439064873
                    Console.WriteLine("Enter an ISBN, OCLC Number, LCCN or OLID in the format: 'ISBN:0201558025'. Press enter without writing anything to quit the program");
                    var bookKeys = Console.ReadLine();

                    if (string.IsNullOrEmpty(bookKeys))
                    {
                        Console.WriteLine("Bye!");
                        break;
                    }

                    var result = await client.GetAsync("https://openlibrary.org/api/books?bibkeys=" + bookKeys.ToString() + "&format=json");
                    var resultRead = await result.Content.ReadAsStringAsync();

                    var newResult = resultRead.Substring(2, resultRead.Length - 3);
                    int position = newResult.IndexOf("{");
                    var lastResult = newResult.Substring(position);

                    var books = JsonConvert.DeserializeObject<Book>(lastResult);

                    Console.WriteLine("----");
                    Console.WriteLine("Bib Key: " + books.Key);
                    Console.WriteLine("Info URL: " + books.Info);
                    Console.WriteLine("Preview: " + books.Preview);
                    Console.WriteLine("Preview URL: " + books.PreviewURL);
                    Console.WriteLine("Thumbnail: " + books.Thumbnail);
                    Console.WriteLine("----");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error! Please enter a valid ISBNs, OCLC Numbers, LCCNs or OLID (Open Library IDs)");
                }
            }
        }
    }
}