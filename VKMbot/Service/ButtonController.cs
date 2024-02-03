using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot;

public class ButtonController
{
    public static ReplyKeyboardMarkup AdminKeyboardMarkup = new(
    new[]
    {
        new KeyboardButton("Reklama jo'natish"),
        new KeyboardButton("User to pdf")
    }){ ResizeKeyboard = true };

    public static InlineKeyboardMarkup inlineKeyboard = new(new[]
{
        //First row. You can also add multiple rows.
        new []
        {
            InlineKeyboardButton.WithUrl(text: "Kanal 1", url: "https://t.me/muhammadabdulloh_uz"),
            InlineKeyboardButton.WithUrl(text: "Kanal 2", url: "https://t.me/code_en")
        },
    });

    public static ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(
new List<KeyboardButton[]>()
        {
            new KeyboardButton[]
            {
                new KeyboardButton("Reklama jo'natish"),
                new KeyboardButton("User ma'lumotlarini pdf shaklida jo'natish"),
            },
            //new KeyboardButton[]
            //{
            //    new KeyboardButton("Image update"),
            //    new KeyboardButton("Link update")
            //},
            //new KeyboardButton[]
            //{
            //    new KeyboardButton("⬅️"),
            //    new KeyboardButton("Save")
            //},
            //new KeyboardButton[]
            //{
            //    new KeyboardButton("Send channel")
            //}
        }){ ResizeKeyboard = true };
}
