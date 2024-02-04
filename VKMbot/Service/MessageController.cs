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
    public static List<long> ADMIN_ID { get; set;}
    public static string FilePath { get; set; }


    public MessageController()
    {
        ADMIN_ID = new List<long>() { 1633746526 };
        var direct = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
        FilePath = Path.Combine(direct.FullName, "Assets/");
    }
    public async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, bool isEnter, List<Contact> list)
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
        }
        else
        {
            Contact(botClient, update, isEnter).Wait();
        }
    }

    private async Task ContactAsyncFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
        }
        else if (messageText.StartsWith("https://www.instagram.com"))
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
                    }
                    else if (item.type == "photo")
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
            }
            catch (Exception ex)
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
        }
        else if (messageText.StartsWith("https://"))
        {
            await MyChatAction.Typing(botClient, update, cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Iltimos instagram link kiriting!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        }
        else if (messageText == "Reklama jo'natish")
        {
            foreach (var user in list)
            {
                await MyChatAction.SendingPicture(botClient, update, cancellationToken);
                //await botClient.SendTextMessageAsync(
                //    chatId: user.UserId,
                //    text: "https://t.me/Asadulloh_Tojiev",
                //    cancellationToken: cancellationToken
                //);

                await botClient.SendPhotoAsync(
                    chatId: user.UserId,
                    photo: InputFile.FromUri("https://www.pexels.com/photo/cable-car-in-narrow-old-town-street-19560870/"),
                    caption: "Reklama",
                    cancellationToken: cancellationToken
                );
            }
        }
        else if (messageText == "User to pdf")
        {
            await SendPdf.SendAllUsers2(botClient, update, cancellationToken, FilePath);
            //IronPdf.License.IsValidLicense("IRONSUITE.SHAHANSHOH819.GMAIL.COM.17684-2C4E16D18D-DGHHMUQ-HK4XMOWKF75W-O4LXWBRK3MOJ-2MQNMVAUBAGG-GVHG64RZWDRP-HFBUCWC7JIEG-UPQ2JMHM5FIO-UPPYQB-TF7FQ66HK2GLUA-DEPLOYMENT.TRIAL-I72N5S.TRIAL.EXPIRES.04.MAR.2024");

            //string text = IO.File.ReadAllText(FilePath + ".json");
            //ChromePdfRenderer renderer = new ChromePdfRenderer();
            //PdfDocument pdf = renderer.RenderHtmlAsPdf(text);
            //pdf.SaveAs(FilePath + ".pdf");

            //await using Stream stream = System.IO.File.OpenRead(FilePath + ".pdf");
            //await botClient.SendDocumentAsync(
            //    chatId: message.Chat.Id,
            //    document: InputFile.FromStream(stream: stream, fileName: $"datas.pdf"),
            //    caption: "Foydanaluvchilar ma'lumotlari"
            //    );
            //stream.Dispose();
        }
        else if (messageText.StartsWith("https://")) 
        { 
            await MyChatAction.Typing(botClient, update, cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Siz yaroqsiz link tashladingiz bloklanasiz!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await SendMusic(botClient, update, cancellationToken, messageText);
            //RootMusic body = JsonConvert.DeserializeObject<RootMusic>(ApiMusic.Run(messageText, 1, 1).Result);

            //await MyChatAction.Uploading(botClient, update, cancellationToken);

            //using (HttpClient client = new HttpClient())
            //{
            //    HttpResponseMessage response = await client.GetAsync(body.tracks.hits[0].track.hub.actions[1].uri);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        byte[] videoContent = await response.Content.ReadAsByteArrayAsync();

            //        await botClient.SendAudioAsync(
            //           chatId: update.Message.Chat.Id,
            //           audio: InputFile.FromStream(new MemoryStream(videoContent)),
            //           cancellationToken: cancellationToken);
            //    }
            //}
        }
            }

    private async Task Admin(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await MyChatAction.Typing(botClient, update, cancellationToken);
        if (ADMIN_ID.Any(item => item == message.Chat.Id)) { 
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Salom siz adminsiz.\nSiz uchun barcha imkoniyatlar ochiq.",
                replyToMessageId: message.MessageId, 
                replyMarkup: ButtonController.AdminKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await User(botClient, update, cancellationToken); 
        }
    }

    private async Task User(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync
        (
            chatId: message.Chat.Id,
            text: $"Salom {message.Chat.FirstName}.\nTabriklayman, siz muvaffaqiyatli ro'yhatdan o'tdingiz.\n\nSizdagi imkoniyatlar:\n1. Instagram'dan reels yokida rasm'ni yuklash.\n2. YouTube'dan video yuklash. \n3. Music'lar ro'yhatini ko'rish va ularni yuklab olish.",
            replyToMessageId: message.MessageId,
            replyMarkup: new ReplyKeyboardRemove()
        );
    }

    public async Task OtherMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //if(update.CallbackQuery.Data != null)
        //{
        //    int index = Convert.ToInt16(update.CallbackQuery.Data);
        //    await CatchMusic(botClient, update, cancellationToken, index);
        //    //await botClient.SendTextMessageAsync(
        //    //     chatId: update.CallbackQuery.From.Id,
        //    //     text: $"{a}",
        //    //     cancellationToken: cancellationToken);
        //}
        await MyChatAction.Typing(botClient, update, cancellationToken);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Siz boshqa narsa kiritib bug 🐞 chiqaradigan \nkimsalardek harakat qilyapsiz! 😡 \nIltimos instagram link tashlang!!!",
            replyToMessageId: update.Message.MessageId,
            cancellationToken: cancellationToken
        );
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
