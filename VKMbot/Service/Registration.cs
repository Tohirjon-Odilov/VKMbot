//using Twilio.Types;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;

//namespace VKMbot
//{
//    public class Registration
//    {
//        //var checkNumber = new
//        public Registration()
//        {
//var checkNumber = new Random().Next(9999);
//Console.WriteLine(checkNumber);
//            string accountSid = Environment.GetEnvironmentVariable("AC5c4fef9ad2fb254343f473130be3de2a");
//            string authToken = Environment.GetEnvironmentVariable("50dc436cc4b61430f744fe7667716dce");

//            TwilioClient.Init(accountSid, authToken);

//            var verificationCheck = VerificationCheckResource.Create(
//                to: "+998916800819",
//                code: "123456",
//                pathServiceSid: "VAXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
//            );

//            //Console.WriteLine(verificationCheck.Status);

//            //    TwilioClient.Init("AC5c4fef9ad2fb254343f473130be3de2a", "50dc436cc4b61430f744fe7667716dce");

//            //    //var call = CallResource.Create(
//            //    //    new PhoneNumber("+998916800819"),
//            //    //    from: new PhoneNumber("+998998734975"),
//            //    //    url: new Uri("https://my.twiml.here")
//            //    //);
//            //    //Console.WriteLine(call.Sid);

//            //    var message = MessageResource.Create(
//            //        new PhoneNumber("+998998734975"),
//            //        from: new PhoneNumber("+998998734975"),
//            //        body: "Hello World!"
//            //    );
//            //    Console.WriteLine(message.Sid);

//        }
//    }
//}


//email
//Random random = new Random();
//int otp = random.Next(100000, 999999);
//possword = otp.ToString();
//var email = new MimeMessage();

//email.From.Add(MailboxAddress.Parse("samadovxusan9013@gmail.com"));
//email.To.Add(MailboxAddress.Parse(emailSender));
//email.Subject = "Your verification code";
//email.Body = new TextPart(TextFormat.Html) { Text = "Your verification code is " + otp };

//var smpt = new SmtpClient();
//await smpt.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
//await smpt.AuthenticateAsync("samadovxusan9013@gmail.com", "dixaxlwqasgetqlb");
//await smpt.SendAsync(email);
//await smpt.DisconnectAsync(true);