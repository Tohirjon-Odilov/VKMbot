using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot;

public partial class MessageController
{
    public string VideoLink { get; set; }
    public static Message message { get; set; }
    public static List<long> ADMIN_ID { get; set; }
    public static string FilePath { get; set; }
    public static byte Status { get; set; }


    public MessageController()
    {
        ADMIN_ID = new List<long>() { 1633746526 };
        var direct = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
        FilePath = Path.Combine(direct.FullName, "Assets/");
    }
    public async Task<byte> HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, bool isEnter, List<Contact> list, byte status)
    {
        message = update.Message;
        Console.WriteLine($"User Name: {message.Chat.Username}\nYou said: {message.Text}\nData: {DateTime.Now}\n");
        if (isEnter == true)
        {
            Console.WriteLine("HandleMessageAsync");
            var handler = message.Type switch
            {
                MessageType.Text => TextAsyncFunction(botClient, update, cancellationToken, list, body),
                MessageType.Contact => ContactAsyncFunction(botClient, update, cancellationToken),
                _ => OtherMessage(botClient, update, cancellationToken),
            };
        } else
        {
            Contact(botClient, update, isEnter).Wait();
        }
        return status;
    }

    public async Task ContactAsyncFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        await MyChatAction.Typing(botClient, update, cancellationToken);

        await Admin(botClient, update, cancellationToken);
    }

    public static async Task Contact(ITelegramBotClient botClient, Update update, bool isEnter)
    {
        Console.WriteLine(isEnter);
        ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup
        (
            KeyboardButton.WithRequestContact("Contact yuborish")
        );

        markup.ResizeKeyboard = true;
        await botClient.SendTextMessageAsync
        (
            chatId: message.Chat.Id,
            text: "Iltimos oldin telefon raqamingizni yuboring!",
            replyMarkup: markup
        );

    }

    public async Task TextAsyncFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<Contact> list, RootMusic musicBody)
    {
        // user'dan kelayotgan ma'lumot message'ga o'zlashadi aks holda dasturni return dsaturni to'xtadi
        if (update.Message is not { } message)
            return;

        // user'dan kelayotgan update.Message'dagi text messageText'ga o'zlashadi aks holda dasturni return dsaturni to'xtadi
        if (message.Text is not { } messageText)
            return;

        // user'dan kelayotgan update.Message'dagi chatId chatId'ga o'zlashadi istisno holatlar yo'q
        var chatId = update.Message.Chat.Id;

        Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. UserName =>  {message.Chat.Username}");

        #region mock $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //Console.WriteLine("Boshi");
        //await botClient.SendAudioAsync(
        //           chatId: chatId,
        //           audio: messageText,
        //           replyToMessageId: message.MessageId,
        //           //supportsStreaming: true,
        //           cancellationToken: cancellationToken);
        //Console.WriteLine("Oxiri");
        //return;
        #endregion 

        if (messageText == "/start")
        {
            await MyChatAction.Typing(botClient, update, cancellationToken);
            await Admin(botClient, update, cancellationToken);
        } else if (messageText.StartsWith("https://www.instagram.com"))
            try
            {
                Console.WriteLine($"Message Type: {message.Type} Username=> {message.Chat.Username} Text => {message.Text} ");
                Api root = new Api();

                IList<Root> body = JsonConvert.DeserializeObject<IList<Root>>(root.RunApi(messageText).Result)!;

                Console.WriteLine(body.Count);

                foreach (var item in body)
                {
                    Console.WriteLine($"\n{item.url}\n");
                    if (item.type == "video")
                    {
                        await MyChatAction.Uploading(botClient, update, cancellationToken);

                        await botClient.SendVideoAsync(
                           chatId: chatId,
                           video: InputFile.FromUri(item.url),
                           replyToMessageId: message.MessageId,
                           supportsStreaming: true,
                           cancellationToken: cancellationToken);
                    } else if (item.type == "photo")
                    {
                        await botClient.SendChatActionAsync(
                            chatId: update.Message.Chat.Id,
                            chatAction: ChatAction.UploadPhoto,
                            cancellationToken: cancellationToken
                        );
                        await botClient.SendPhotoAsync(
                           chatId: chatId,
                           replyToMessageId: message.MessageId,
                           photo: InputFile.FromUri(item.url),
                           cancellationToken: cancellationToken);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                await MyChatAction.Typing(botClient, update, cancellationToken);

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: ex.Message,
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken
                );
                string replasemessage = messageText.Replace("www.", "dd");
                await botClient.SendVideoAsync
                (
                       chatId: chatId,
                       video: InputFile.FromUri(messageText),
                       replyToMessageId: message.MessageId,
                       supportsStreaming: true,
                       cancellationToken: cancellationToken
                );
            }
        else if (messageText.StartsWith("https://youtube.com") || messageText.StartsWith("https://youtu.be"))
        {
            await SendYoutube.EssentialFunction(botClient, update, cancellationToken);
        } else if (messageText.StartsWith("https://"))
        {
            await MyChatAction.Typing(botClient, update, cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Iltimos instagram link kiriting!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        } else if (messageText == "Reklama jo'natish")
        {
            foreach (var user in list)
            {
                await MyChatAction.SendingPicture(botClient, update, cancellationToken);

                await botClient.SendPhotoAsync(
                    chatId: user.UserId,
                    photo: InputFile.FromUri("https://www.pexels.com/photo/cable-car-in-narrow-old-town-street-19560870/"),
                    caption: "Reklama",
                    cancellationToken: cancellationToken
                );
            }
        } else if (messageText == "User to pdf")
        {
            await SendPdf.SendAllUsers2(botClient, update, cancellationToken, FilePath);
        } else if (messageText.StartsWith("https://"))
        {
            await MyChatAction.Typing(botClient, update, cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Siz yaroqsiz link tashladingiz bloklanasiz!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        } else if (messageText == "admin")
        {
            await AdminMessage(botClient, update, cancellationToken);
        } else if (messageText == "Add admin")
        {
            Status = 1;
            await OtherMessage(botClient, update, cancellationToken, "User id kiriting:");
        } else if (messageText == "Advanced")
        {
            await botClient.SendTextMessageAsync
            (
                chatId: update.Message.Chat.Id,
                text: "Tugmalardan birini tanlang",
                replyMarkup: ButtonController.AdminKeyboardAdvancedMarkup,
                replyToMessageId: update.Message.MessageId,
                cancellationToken: cancellationToken
            );
        } else if (Status == 1)
        {
            string datas;
            Console.WriteLine(FilePath);
            using (StreamReader sw = new StreamReader(FilePath + "AdminAndBlacks.json"))
            {
                datas = sw.ReadToEnd();
            }
            Dictionary<string, List<long>> AdminsId = JsonConvert.DeserializeObject<Dictionary<string, List<long>>>(datas) ?? new Dictionary<string, List<long>>();
            AdminsId["admins"].Add(Convert.ToInt64(update.Message.Chat.Id));

            datas = JsonConvert.SerializeObject(AdminsId);

            using (StreamWriter sw = new StreamWriter(FilePath + "AdminAndBlacks.json"))
            {
                sw.WriteLine(datas);
            }
        } else
        {
            await SendMusic(botClient, update, cancellationToken, messageText);
        }
    }

    public async Task Admin(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await MyChatAction.Typing(botClient, update, cancellationToken);
        if (ADMIN_ID.Any(item => item == message.Chat.Id))
        {
            await AdminMessage(botClient, update, cancellationToken);
        } else
        {
            await User(botClient, update, cancellationToken);
        }
    }

    public async Task User(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync
        (
            chatId: message.Chat.Id,
            text: $"Salom {message.Chat.FirstName}.\nTabriklayman, siz muvaffaqiyatli ro'yhatdan o'tdingiz.\n\nSizdagi imkoniyatlar:\n1. Instagram'dan reels yokida rasm'ni yuklash.\n2. YouTube'dan video yuklash. \n3. Music'lar ro'yhatini ko'rish va ularni yuklab olish.",
            replyToMessageId: message.MessageId,
            replyMarkup: new ReplyKeyboardRemove()
        );
    }

    public async Task<byte> OtherMessage(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken, string text =
        "Siz boshqa narsa kiritib bug 🐞 chiqaradigan \nkimsalardek harakat qilyapsiz! 😡 \nIltimos instagram link tashlang!!!")
    {
        await MyChatAction.Typing(botClient, update, cancellationToken);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: text,
            replyToMessageId: update.Message.MessageId,
            cancellationToken: cancellationToken
        );
        return 0;
    }

    public static async Task EventReSendContact(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync
        (
            chatId: message.Chat.Id,
            text: $"{message.Chat.FirstName}, siz allaqachon ro'yhatdan o'tgansiz!\n Botdan bemalol foydalanishingiz mumkin.",
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken
        );
    }

}
