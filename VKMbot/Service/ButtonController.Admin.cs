using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot
{
    public partial class ButtonController
    {
        public static ReplyKeyboardMarkup AdminKeyboardMarkup = new(new[]
            {
                new KeyboardButton("Reklama jo'natish"),
                new KeyboardButton("User to pdf")
            }
        ){ ResizeKeyboard = true };
    }
}
