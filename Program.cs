using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Threading;
using System;

namespace FileReaderPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var csvFileReader = new CsvRowReader();
            var blumbergFileServiceFile = new BlumbergFileReaderService(csvFileReader);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            await blumbergFileServiceFile.LoadFile("blumberg.csv");

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Load time " + elapsedTime);

           var val =   await blumbergFileServiceFile.Read("FR9Z6D177I97");
           val = await blumbergFileServiceFile.Read("FRJ72UD7C367");

        }
    }
}