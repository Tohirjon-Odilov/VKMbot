using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace VKMbot;

public partial class ButtonController
{
    //public static List<string> DownloadLinks { get; set; }
    public static List<InlineKeyboardButton> InlineKeyboardMusic(List<Hit> data, string FilePath)
    {

        var buttons = new List<InlineKeyboardButton>();
        int i = 1;
        var DownloadLinks = new List<string>();

        foreach (var item in data)
        {
            DownloadLinks.Add(item.track.hub.actions[1].uri);
            buttons.Add(InlineKeyboardButton.WithCallbackData($"{i}", $"{i-1}"));
            i++;
        }
        //for (var i = 1; i <= data.Count; i++) {
        //}
        //File.WriteAllText(".json", );
        using (StreamWriter sw = new StreamWriter(FilePath + "links.json"))
        {
            sw.WriteLine(JsonConvert.SerializeObject(DownloadLinks, Formatting.Indented));
        }
        //using (StreamWriter sw = new StreamWriter())
        return buttons;
    }
}
