using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace VKMbot;

public class Server
{
    public string Token { get; set; }
    public string VideoLink { get; private set; }

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
            if (update.Message is not { } message) return;
            if (message.Chat is not { } chat) return;

            // Foydalanuvchining chat id'sini olish
            long userId = chat.Id;

            Console.WriteLine($"User -> {chat.FirstName} Chat Id -> {chat.Id}\nMessage ->{message.Text}\n\n");

            // Kanalning username'sini o'zgartiring
            string channelUsername1 = "@code_en";
            //string channelUsername2 = "@muhammadabdulloh_uz";

            // Foydalanuvchini tekshirish
            var chatMemberOne = await botClient.GetChatMemberAsync(channelUsername1, userId);
            //var chatMember2 = await botClient.GetChatMemberAsync(channelUsername2, userId);

            Console.WriteLine(chatMemberOne.Status.ToString());
            //Console.WriteLine(chatMember2.Status.ToString());

            // Agar foydalanuvchi kanalda obuna bo'lsa
            switch (chatMemberOne.Status)
            {
                case ChatMemberStatus.Administrator:
                case ChatMemberStatus.Member:
                case ChatMemberStatus.Creator:

                    MessageController messageController = new MessageController();
                    var handler = update.Type switch
                    {
                        UpdateType.Message => messageController.HandleMessageAsync(botClient, update, cancellationToken),
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
