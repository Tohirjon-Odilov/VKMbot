using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot
{
    internal class ButtonController
    {
        public static InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            //First row. You can also add multiple rows.
            new []
            {
                InlineKeyboardButton.WithUrl(text: "Kanal 1", url: "https://t.me/muhammadabdulloh_uz"),
                InlineKeyboardButton.WithUrl(text: "Kanal 2", url: "https://t.me/code_en")
            },
        });
    }
}