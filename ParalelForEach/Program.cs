using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParalelForEach
{
    class Program
    {
        private static readonly List<string> ResultList = new List<string>();
        private static readonly List<Task<string>> Tasks = new List<Task<string>>();
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            Process().Wait();
            watch.Stop();
            Console.WriteLine("Totally " + watch.ElapsedMilliseconds + " milliseconds passed!");
            Console.ReadKey();
        }

        public static async Task Process()
        {
            var requestUrlList = new List<string>
            {
                "http://hurriyet.com.tr",
                "http://hurriyet.com.tr",
                "http://hurriyet.com.tr",
                "http://hurriyet.com.tr",
                "http://hurriyet.com.tr",

            };
            
            foreach (var requestUrl in requestUrlList)
            {
                Tasks.Add(GetGoogle(requestUrl));
            }

            //Task.WaitAll();
            await Task.WhenAll(Tasks);

            foreach (var task in Tasks)
            {
                ResultList.Add(task.Result);

            }

        }
        public static async Task<string> GetGoogle(string url)
        {
            string responseString = string.Empty;
            var watch = Stopwatch.StartNew();
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));

                request.Method = "GET";
                request.Timeout = 10000;

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                using (Stream stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream ?? throw new InvalidOperationException("Response stream is null!"), Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }
                watch.Stop();
                Console.WriteLine("Resolved after " + watch.ElapsedMilliseconds + " milliseconds passed!");
            }
            catch (Exception e)
            {
                watch.Stop();
                Console.WriteLine("After " + watch.ElapsedMilliseconds + " milliseconds request timeout!");
            }

            return responseString;


        }
    }
}
