using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Telegram.Bot.Types;
using Telegram.Bot;
using QuestPDF.Helpers;

namespace VKMbot
{
    public class SendPdf
    {
        public static async Task SendAllUsers2(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string FilePath)
        {
            string JsonToString = System.IO.File.ReadAllText(FilePath + ".json");
            var JsonList = JsonConvert.DeserializeObject<List<Contact>>(JsonToString);
            int n = 1;
            
            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                      .Text("Users!")
                      .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                      .PaddingVertical(1, Unit.Centimetre)
                      .Column(x =>
                      {
                          x.Spacing(10);
                          n = 1;
                          foreach (var el in JsonList)
                          {
                              x.Item().Text($"{n} - User");
                              x.Item().Text("Chat id: " + el.UserId);
                              x.Item().Text("Phone number: " + el.PhoneNumber);
                              x.Item().Text("Name: " + el.FirstName);
                              x.Item().Text("\n");
                              n++;
                          }

                      });

                    page.Footer()
                      .AlignCenter()
                      .Text(x =>
                      {
                          x.Span("Page ");
                          x.CurrentPageNumber();
                      });
                });
            })
                .GeneratePdf(FilePath + ".pdf");


            await using Stream stream = System.IO.File.OpenRead(FilePath + ".pdf");
            await botClient.SendDocumentAsync(
                chatId: update.Message.Chat.Id,
                document: InputFile.FromStream(stream: stream, fileName: $"datas.pdf"),
                caption: "Hamma foydalanuvchi haqida malumot"
                );
            stream.Dispose();
        }
    }
}
