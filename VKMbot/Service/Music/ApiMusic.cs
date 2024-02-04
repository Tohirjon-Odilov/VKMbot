using Newtonsoft.Json;
using System.Net;

namespace VKMbot
{
    public class ApiMusic
    {
        public static async Task<string> Run(string music, byte offset, byte limit)
        {
            string encodedMusic = WebUtility.UrlEncode(music);
            Console.WriteLine(encodedMusic);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://shazam.p.rapidapi.com/search?term={encodedMusic}&locale=en-US&offset=1&limit=100"),
                Headers =
                {
                    { "X-RapidAPI-Key", "f927051de5msh33c089150223b61p1e384ajsn0b24f85919fd" },
                    { "X-RapidAPI-Host", "shazam.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}
