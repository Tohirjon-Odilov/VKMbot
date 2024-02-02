// token =>   6717154729:AAH972TZtseKqSOieDA6XjujMPsdlAnWv9s

using VKMbot;
try
{
    var TOKEN = "6717154729:AAH972TZtseKqSOieDA6XjujMPsdlAnWv9s";
    var send = new Service(TOKEN);
    await send.Run();
}
catch(Exception  e){
    Console.WriteLine(e);
}

