using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace VKMbot
{
    public static class SendYoutubeMp3
    {
        public static async Task EssentialFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string linkY = update.Message.Text;
            RootYoutubeMp3 YoutubeMp3Download = JsonConvert.DeserializeObject<RootYoutubeMp3>(YoutubeMp3Class.RunApi(linkY).Result);

            await botClient.SendChatActionAsync(
                chatId: update.Message.Chat.Id,
                chatAction: ChatAction.UploadDocument,
                cancellationToken: cancellationToken);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(YoutubeMp3Download.link);
                if (response.IsSuccessStatusCode)
                {
                    byte[] videoContent = await response.Content.ReadAsByteArrayAsync();

                    await botClient.SendAudioAsync(
                       chatId: update.Message.Chat.Id,
                       audio: InputFile.FromStream(new MemoryStream(videoContent)),
                       caption:"title : "+YoutubeMp3Download.title+"\nsize : "+YoutubeMp3Download.size+"\n Seconds : "+YoutubeMp3Download.length,
                       cancellationToken: cancellationToken);
                }
            }

                    
        }
    }
}
