using System.Net;

namespace VKMbot
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
                { "X-RapidAPI-Key", "f927051de5msh33c089150223b61p1e384ajsn0b24f85919fd" },
                { "X-RapidAPI-Host", "youtube-mp3-downloader2.p.rapidapi.com" },
            },};
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
        }   
    }
}
