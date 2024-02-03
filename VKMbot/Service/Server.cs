using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using IO = System.IO;

namespace VKMbot;

public class Server
{
    public string Token { get; set; }
    public string VideoLink { get; private set; }

    public bool IsEnter { get; set; } = false;

    public Server(string token)
    {
        Token = token;
    }
    public async Task Run()
    {
        var botClient = new TelegramBotClient($"{Token}");

        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        cts.Cancel();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            #region User'larni o'qib oladi (deserialize)
            string jsonFilePath = "../../../Assets/datas.json";
            var dataList = IO.File.ReadAllText(jsonFilePath);

            List<Contact> list = JsonConvert.DeserializeObject<List<Contact>>(dataList);
            #endregion

            #region field's
            if (update.Message is not { } message) return;
            if (message.Chat is not { } chat) return;

            // Foydalanuvchining chat id'sini olish
            long userId = chat.Id;


            // Kanalning username'sini o'zgartiring
            string channelUsername1 = "@code_en";
            //string channelUsername2 = "@muhammadabdulloh_uz";

            // Foydalanuvchini tekshirish
            var chatMemberOne = await botClient.GetChatMemberAsync(channelUsername1, userId);
            //var chatMember2 = await botClient.GetChatMemberAsync(channelUsername2, userId);

            Console.WriteLine(chatMemberOne.Status.ToString());
            //Console.WriteLine(chatMember2.Status.ToString());

            Console.WriteLine($"User -> {chat.FirstName} Chat Id -> {chat.Id}\nMessage ->{message.Text}\n\n");
            #endregion

            #region user'larni tekshiradi
            if(list.Any(item => item.UserId == userId))
            {
                IsEnter = true;
            }
            else
            {
                IsEnter = false;
                if (update.Message.Contact is not null)
                {
                    list.Add(message.Contact);

                    var data = IO.File.ReadAllText(jsonFilePath);

                    using (StreamWriter sw = new StreamWriter(jsonFilePath))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
                    }
                    IsEnter = true;
                }
            }
            #endregion

            #region member'larni tekshiradi
            // Agar foydalanuvchi kanalda obuna bo'lsa
            switch (chatMemberOne.Status)
            {
                case ChatMemberStatus.Administrator:
                case ChatMemberStatus.Member:
                case ChatMemberStatus.Creator:
                    MessageController messageController = new MessageController();
                    var handler = update.Type switch
                    {
                        UpdateType.Message => messageController.HandleMessageAsync(botClient, update, cancellationToken, IsEnter),
                        _ => messageController.OtherMessage(botClient, update, cancellationToken),
                    };
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: "Siz kanalga obuna bo'lmagansiz. Iltimos, avval kanalga obuna bo'ling.",
                        replyMarkup: ButtonController.inlineKeyboard,
                        cancellationToken: cancellationToken
                    );
                    break;
            }
            #endregion
        }
        catch (Exception ex) { Console.WriteLine(ex); 
        }
    }
    public async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
