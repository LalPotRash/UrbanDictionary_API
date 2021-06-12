using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace UrbanDictionaryAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = Console.ReadLine();
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
                Console.WriteLine(militaryBase.list[0].definition);
            }
        }
    }
}
