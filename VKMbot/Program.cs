﻿using VKMbot;
// 6827225607:AAHMZ2m1n61NuZq7FBq1dkpklz0jJknsJQk
var token = "6717154729:AAH972TZtseKqSOieDA6XjujMPsdlAnWv9s"; //=> instagram downloade bot 
Server server = new Server(token);
try
{
    server.Run().Wait();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}