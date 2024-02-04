using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot;

public partial class ButtonController
{
    public static List<InlineKeyboardButton> InlineKeyboardMusic(int count)
    {
        var buttons = new List<InlineKeyboardButton>();
        for (var i = 1; i <= count; i++) {
            buttons.Add(InlineKeyboardButton.WithCallbackData($"{i}", $"{i-1}"));
        }
        return buttons;
    }
}
