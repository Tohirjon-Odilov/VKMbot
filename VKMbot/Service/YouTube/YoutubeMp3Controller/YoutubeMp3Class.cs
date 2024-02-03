using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InstagramAndYoutube.YoutubeController.YoutubeMp3Controller
{
    public static class YoutubeMp3Class
    {
        public static async Task<string> RunApi(string link)
        {
            string encodedUrl = WebUtility.UrlEncode(link);
            Console.WriteLine(encodedUrl);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://youtube-mp3-downloader2.p.rapidapi.com/ytmp3/ytmp3/?url={encodedUrl}"),
                Headers =
    {
        { "X-RapidAPI-Key", "16e48dfbd2msh7371fe5e63f3af1p180fe6jsnee0363ed0461" },
        { "X-RapidAPI-Host", "youtube-mp3-downloader2.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                foreach (var item in response.Content.Headers)
                {
                    Console.WriteLine(item.Value.ToList()[0]);
                }
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return body;
            }
        }   }
}
