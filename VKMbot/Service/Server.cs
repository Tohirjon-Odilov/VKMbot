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
    public List<Contact> list { get; set; } = new List<Contact>();
    public bool IsEnter { get; set; } = false;
    public List<long> BlockList { get; set; } = new List<long>() { 5372384465, 5921666026, 5569322769 };
    public List<long> AdminList { get; set; } = new List<long>();
    public byte Status { get; set; }

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

            list = JsonConvert.DeserializeObject<List<Contact>>(dataList)!;

            #endregion
            #region callback query shu yerga kelib tushadi
            MessageController messageController = new MessageController();
            if (UpdateType.CallbackQuery == update.Type)
            {
                await messageController.CatchMusic(botClient, update, cancellationToken);
                return;
            }
            #endregion
            #region field's
            if (update.Message.Chat is not { } chat) return;

            if (update.Message is not { } message) return;

            // Foydalanuvchining chat id'sini olish
            long userId = chat.Id;

            // Kanalning username'sini o'zgartiring
            string channelUsername1 = "@dotnet_resourse";
            //string channelUsername2 = "@muhammadabdulloh_uz";

            // Foydalanuvchini tekshirish
            var chatMemberOne = await botClient.GetChatMemberAsync(channelUsername1, userId);
            //var chatMember2 = await botClient.GetChatMemberAsync(channelUsername2, userId);

            Console.WriteLine(chatMemberOne.Status.ToString());
            //Console.WriteLine(chatMember2.Status.ToString());

            Console.WriteLine($"User -> {chat.FirstName} Chat Id -> {chat.Id}\nMessage ->{update.Message.Text}\n\n");
            #endregion
            #region blacklist
            if (BlockList.Exists(i => i == message.Chat.Id))
            {
                await botClient.SendTextMessageAsync
                (
                    chatId: message.Chat.Id,
                    text: $"Hurmatli {message.Chat.Username}. Siz ko'p harakat qilganiz uchun bloklandingiz!!!",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken
                );
                return;
            }

            #endregion
            #region user'larni tekshiradi
            if (list.Exists(item => item.UserId == userId))
            {
                IsEnter = true;
            } else
            {
                IsEnter = false;
                if (update.Message.Contact is not null)
                {
                    list.Add(update.Message.Contact);

                    using (StreamWriter sw = new StreamWriter(jsonFilePath))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
                    }
                    IsEnter = true;
                }
            }
            #endregion

            #region member'larni tekshiradi
            // Agar foydalanuvchi kanalda obuna bo'lsa shunda'gina ushbu qism ishlaydi.
            switch (chatMemberOne.Status)
            {
                case ChatMemberStatus.Administrator:
                case ChatMemberStatus.Member:
                case ChatMemberStatus.Creator:
                    Status = update.Type switch
                    {
                        UpdateType.Message => await messageController.HandleMessageAsync(botClient, update, cancellationToken, IsEnter, list, Status),
                        //UpdateType.CallbackQuery => messageController.CatchMusic(botClient, update, cancellationToken), 
                        _ => await messageController.OtherMessage(botClient, update, cancellationToken),
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
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public async Task<Task> HandlePollingErrorAsync(
        ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {

        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        await Console.Out.WriteLineAsync(ErrorMessage);
        return Task.CompletedTask;
    }
}
