using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot
{
    public static partial class ButtonController
    {
        public readonly static ReplyKeyboardMarkup AdminKeyboardMarkup = new(new[]
            {
                new KeyboardButton [] { new KeyboardButton("Reklama jo'natish"), new KeyboardButton("User to pdf")},
                new KeyboardButton [] { new KeyboardButton("Advanced")}
            }
        )
        { ResizeKeyboard = true };
        public readonly static ReplyKeyboardMarkup AdminKeyboardAdvancedMarkup = new(new[]
            {
                new KeyboardButton [] { new KeyboardButton("Add admin"), new KeyboardButton("Block user")},
                new KeyboardButton [] { new KeyboardButton("Back")}
            }
        )
        { ResizeKeyboard = true };
    }
}
