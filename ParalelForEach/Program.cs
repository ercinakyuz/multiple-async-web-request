using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParalelForEach
{
    class Program
    {
        private static readonly List<string> ResultList = new List<string>();
        private static readonly List<Task<string>> Tasks = new List<Task<string>>();

        private static readonly string[] RequestUrlArray =
        {
            "http://hurriyet.com.tr",
            "http://hurriyet.com.tr",
            "http://hurriyet.com.tr",
            "http://hurriyet.com.tr",
            "http://hurriyet.com.tr",
        };
        static async Task Main(string[] args)
        {
            await HandleProcess(ProcessType.WhenAll, true);
            await HandleProcess(ProcessType.WaitAll, true);
            await HandleProcess(ProcessType.Parallel, true);

            await HandleProcess(ProcessType.WhenAll);
            await HandleProcess(ProcessType.WaitAll);
            await HandleProcess(ProcessType.Parallel);

            Console.ReadKey();
        }

        public static async Task HandleProcess(ProcessType processType, bool haveToReadResponse = false)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var watch = Stopwatch.StartNew();
            if (processType == ProcessType.Parallel)
            {
                ProcessParallel(haveToReadResponse);
            }
            else
            {
                await Process(processType, haveToReadResponse);
            }

            watch.Stop();
            Console.WriteLine($"Process {processType}: Totally {watch.ElapsedMilliseconds} milliseconds passed!");
            Console.WriteLine("----------------------------------------------------------------");

        }

        public static void ProcessParallel(bool haveToReadResponse)
        {
            Parallel.ForEach(RequestUrlArray, (requestUrl) =>
            {
                if (haveToReadResponse)
                {
                    ResultList.Add(HttpRequestHandler<string>.Get(requestUrl));
                }
                else
                {
                    HttpRequestHandler<string>.Get(requestUrl);
                }

            });

        }

        public static async Task Process(ProcessType processType, bool haveToReadResponse)
        {

            Parallel.ForEach(RequestUrlArray, (requestUrl) =>
            {
                Tasks.Add(HttpRequestHandler<string>.GetAsync(requestUrl));
            });

            switch (processType)
            {
                case ProcessType.WaitAll:
                    Task.WaitAll();
                    break;
                case ProcessType.WhenAll:
                    await Task.WhenAll();
                    break;
            }

            if (haveToReadResponse)
            {
                Parallel.ForEach(Tasks, (task) =>
                {
                    ResultList.Add(task.Result);
                });
            }


        }

    }
}
