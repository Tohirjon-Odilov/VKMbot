using Newtonsoft.Json;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace VKMbot
{
    public class SendYoutube
    {

        public static async Task EssentialFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var url = "";
            string linkY = update.Message.Text;
            var uri = new Uri(linkY);
            var query = HttpUtility.ParseQueryString(uri.Query);
            if (query.AllKeys.Contains("v"))
            {
                url = query["v"];
            }
            else
            {
                url = uri.Segments.Last();
            }

            RootYoutube YoutubeVideoDownload = JsonConvert.DeserializeObject<RootYoutube>(YoutubeClass.RunApi(url).Result);

            await botClient.SendChatActionAsync(
                chatId: update.Message.Chat.Id,
                chatAction: ChatAction.UploadDocument,
                cancellationToken: cancellationToken);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(YoutubeVideoDownload.formats[1].url);
                if (response.IsSuccessStatusCode)
                {
                    byte[] videoContent = await response.Content.ReadAsByteArrayAsync();

                    //using (var stream = new FileStream(@"../../../Assets/datas.pdf", FileMode.Open))
                    //{
                        await botClient.SendVideoAsync(
                       chatId: update.Message.Chat.Id,
                       video: new InputOnlineFile(new MemoryStream(videoContent)),
                       caption: "Formati : " + YoutubeVideoDownload.formats[1].qualityLabel + "\ntitle : " + YoutubeVideoDownload.title + "\nSeconds : " + YoutubeVideoDownload.lengthSeconds,
                       supportsStreaming: true,
                       cancellationToken: cancellationToken);
                    //};
                }
            }

        }
    }
}
