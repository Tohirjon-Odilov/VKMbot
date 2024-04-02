using QuestPDF.Infrastructure;
using VKMbot;
// 6827225607:AAHMZ2m1n61NuZq7FBq1dkpklz0jJknsJQk

QuestPDF.Settings.License = LicenseType.Community;
var TOKEN = "6726534448:AAFZeQSAhl8vBlfE2dKdiN4dvEcPBaIa0QE"; //=> instagram downloade bot 
Server server = new Server(TOKEN);
try
{
    server.Run().Wait();
} catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

