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
    //public static string FilePath { get; set; }
    public static async Task SendMusic(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string music)
    {
        body = JsonConvert.DeserializeObject<RootMusic>(ApiMusic.Run(music, 1, 1).Result);

        //await botClient.SendTextMessageAsync(
        //    chatId: update.Message.Chat.Id,
        //    text: $"{body.tracks.hits.Count}",
        //    cancellationToken: cancellationToken
        //    );
        //return;
        var name = String.Empty;
        //Console.WriteLine(ButtonController.InlineKeyboardMusic(body.tracks.hits.Count));
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

        //for(var i = 0; i < links.Count; i++)
        //{
        //    if(index == )
        //}

        await botClient.SendAudioAsync(
            chatId: update.CallbackQuery.From.Id,
            audio: InputFile.FromUri(links[index]),
            caption: $"Supports: https://t.me/code_en" +
            $"         https://t.me/muhammadabdulloh_uz\n" +
            $"Creator: https://t.me/t_odilov",
            //performer: path.title,
            //title: path.share.subject,
            cancellationToken: cancellationToken);

        //return;
        //using (HttpClient client = new HttpClient())
        //{
        //    HttpResponseMessage response = await client.GetAsync(link);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        byte[] videoContent = await response.Content.ReadAsByteArrayAsync();

        //        await MyChatAction.Uploading(botClient, update, cancellationToken);
        //        await botClient.SendAudioAsync(
        //           chatId: update.CallbackQuery.From.Id,
        //           audio: InputFile.FromStream(new MemoryStream(videoContent)),
        //           caption: $"Supports: https://t.me/code_en" +
        //           $"         https://t.me/muhammadabdulloh_uz\n" +
        //           $"Creator: https://t.me/t_odilov",
        //           //performer: path.title,
        //           //title: path.share.subject,
        //           cancellationToken: cancellationToken);
        //        }
        //        //}
        //    }
        }

        //internal Task CallbackData(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
