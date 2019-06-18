using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParalelForEach
{
    public class HttpRequestHandler<T>
    {
        public static async Task<T> GetAsync(string url)
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

            return (T)Convert.ChangeType(responseString, typeof(T));


        }

        public static T Get(string url)
        {
            string responseString = string.Empty;
            var watch = Stopwatch.StartNew();
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));

                request.Method = "GET";
                request.Timeout = 10000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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

            return (T)Convert.ChangeType(responseString, typeof(T));


        }
    }
}
