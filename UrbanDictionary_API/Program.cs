using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Telegram.Bot;

namespace UrbanDictionaryAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            TelegramBotClient bot = new TelegramBotClient("1823731962:AAEGXcBo69K9RL7nRY_3w8MhIJQ7E__Cj4Q");

            bot.OnMessage += (s, arg) =>
            {
                var query = arg.Message.Text;
                var url = $"http://api.urbandictionary.com/v0/define?term={query}";
                var request = WebRequest.Create(url);

                var response = request.GetResponse();
                var httpStatusCode = (response as HttpWebResponse).StatusCode;

                if (httpStatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(httpStatusCode);
                    return;
                }

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    var militaryBase = JsonConvert.DeserializeObject<Root>(result);

                    try
                    {
                        bot.SendTextMessageAsync(arg.Message.Chat.Id, militaryBase.list[0].definition);
                        Console.WriteLine($"Right - {query}");
                    }

                    catch (System.ArgumentOutOfRangeException)
                    {
                        bot.SendTextMessageAsync(arg.Message.Chat.Id, $"Wrong word: {query}");
                        Console.WriteLine($"Wrong - {query}");
                    }
                }
            };

            bot.StartReceiving();

            Console.ReadKey();
        }
    }
}