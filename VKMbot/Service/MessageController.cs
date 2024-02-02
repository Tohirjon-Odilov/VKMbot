using Telegram.Bot;
using Telegram.Bot.Types;

namespace VKMbot
{
    public class MessageController
    {
        public object HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public object OtherMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}