using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Twilio.TwiML.Messaging;
namespace VKMbot;

public partial class MessageController
{
    public static RootMusic body { get; set; }
    public static async Task SendMusic(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string music)
    {
        body = JsonConvert.DeserializeObject<RootMusic>(ApiMusic.Run(music, 1, 1).Result)!;

        var name = String.Empty;
        if (body is not null) {
            var count = 0;
            foreach(var item in body.tracks.hits)
            {
                name += $"{++count}. "+item.track.share.subject + "\n";
            }
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"{name}",
                replyMarkup: new InlineKeyboardMarkup(ButtonController.InlineKeyboardMusic(body.tracks.hits, FilePath)),
                cancellationToken: cancellationToken
            );
        }
    }
    public async Task CatchMusic(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) 
    {
        int index = Convert.ToInt16(update.CallbackQuery.Data);
        List<string> links = new List<string>();
        string datas;
        using (StreamReader sw = new StreamReader(FilePath + "links.json"))
        {
             datas=sw.ReadToEnd();
        }
        links = JsonConvert.DeserializeObject<List<string>>(datas);

        await botClient.SendAudioAsync
        (
            chatId: update.CallbackQuery.From.Id,
            audio: InputFile.FromUri(links[index]),
            caption: $"Creator: @t_odilov",
            cancellationToken: cancellationToken
        );
    }
}
