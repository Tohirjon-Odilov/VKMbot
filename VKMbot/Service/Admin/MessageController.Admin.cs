using Telegram.Bot;
using Telegram.Bot.Types;

namespace VKMbot
{
    public partial class MessageController
    {
        public async Task AdminMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) 
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Salom siz adminsiz.\nSiz uchun barcha imkoniyatlar ochiq.",
                replyToMessageId: update.Message.MessageId,
                replyMarkup: ButtonController.AdminKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
    }
}
